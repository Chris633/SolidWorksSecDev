using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksSecDev.Entity;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using ISheet = NPOI.SS.UserModel.ISheet;

namespace SolidWorksSecDev
{
    public partial class Form1 : Form
    {
        private static Form1 form1;

        private int from;

        private Form1()
        {
            //var end = "2021-06-05 00:00:00";
            //var now = DateTime.Now;
            //var today2 = new DateTime(now.Year, now.Month, now.Day); //当天的零时零分
            //if (DateTime.Parse(end) < today2)
            //{
            //    MessageBox.Show("请与开发者联系 微信:Chris633 qq:378151018 email:chris63388@outlook.com");
            //    return;
            //}

            InitializeComponent();
        }

        public static Form1 getForm1()
        {
            if (form1 == null) form1 = new Form1();
            return form1;
        }

        #region BOM表格操作
        //返回指定范围单元格
        private ICellRange<ICell> GetCellRange(ISheet ws, CellRangeAddress range)
        {
            var firstRow = range.FirstRow;
            var firstColumn = range.FirstColumn;
            var lastRow = range.LastRow;
            var lastColumn = range.LastColumn;
            var height = lastRow - firstRow + 1;
            var width = lastColumn - firstColumn + 1;
            var temp = new List<ICell>(height * width);
            for (var rowIn = firstRow; rowIn <= lastRow; rowIn++)
                for (var colIn = firstColumn; colIn <= lastColumn; colIn++)
                {
                    var row = ws.GetRow(rowIn);
                    if (row == null) row = ws.CreateRow(rowIn);
                    var cell = row.GetCell(colIn);
                    if (cell == null) cell = row.CreateCell(colIn);
                    temp.Add(cell);
                }

            return SSCellRange<ICell>.Create(firstRow, firstColumn, height, width, temp, typeof(XSSFCell));
        }
        //表头生成
        private ISheet GenerateTitle(XSSFWorkbook wb)
        {
            var st = wb.CreateSheet("bom");
            #region 【打印】页面设置
            const double CmPInch = 2.54;
            st.SetMargin(MarginType.TopMargin, 2.5d / CmPInch);
            st.SetMargin(MarginType.BottomMargin, 2.5d / CmPInch);
            st.SetMargin(MarginType.LeftMargin, 1.9d / CmPInch);
            st.SetMargin(MarginType.RightMargin, 1.9d / CmPInch);
            st.SetMargin(MarginType.HeaderMargin, 1.3d / CmPInch);
            st.SetMargin(MarginType.FooterMargin, 1.3d / CmPInch);

            st.FitToPage = true;
            st.PrintSetup.FitHeight = 10;
            st.PrintSetup.FitWidth = 1;
            st.PrintSetup.HResolution = 1200;
            st.PrintSetup.VResolution = 1200;

            st.PrintSetup.PaperSize = (short)PaperSize.A3 + 1;
            st.PrintSetup.Landscape = true;
            #endregion

            #region 列宽设置
            const int CharSize = 256;
            st.SetColumnWidth(0, 7 * CharSize);
            st.SetColumnWidth(1, 14 * CharSize);
            st.SetColumnWidth(2, 7 * CharSize);
            st.SetColumnWidth(3, 14 * CharSize);
            st.SetColumnWidth(4, 14 * CharSize);
            st.SetColumnWidth(5, 4 * CharSize);
            st.SetColumnWidth(6, 4 * CharSize);
            st.SetColumnWidth(7, 7 * CharSize);
            st.SetColumnWidth(8, 14 * CharSize);
            st.SetColumnWidth(9, 39 * CharSize);
            st.SetColumnWidth(10, 44 * CharSize);
            st.SetColumnWidth(11, 49 * CharSize);
            st.SetColumnWidth(12, 8 * CharSize);
            st.SetColumnWidth(13, 7 * CharSize);
            st.SetColumnWidth(14, 7 * CharSize);
            st.SetColumnWidth(15, 8 * CharSize);
            st.SetColumnWidth(16, 7 * CharSize);
            st.SetColumnWidth(17, 8 * CharSize);
            #endregion

            #region 每行表头
            var header = st.CreateRow(0);
            header.CreateCell(0).SetCellValue("层次");
            header.CreateCell(1).SetCellValue("父项物料代码");
            header.CreateCell(2).SetCellValue("父项数量");
            header.CreateCell(3).SetCellValue("子项物料代码");
            header.CreateCell(4).SetCellValue("原材料代码");
            header.CreateCell(5).SetCellValue("数量");
            header.CreateCell(6).SetCellValue("损耗率");
            header.CreateCell(7).SetCellValue("备注");
            header.CreateCell(8).SetCellValue("外形尺寸");
            header.CreateCell(9).SetCellValue("规格型号");
            header.CreateCell(10).SetCellValue("名称");
            header.CreateCell(11).SetCellValue("文件名称");
            header.CreateCell(12).SetCellValue("重量");
            header.CreateCell(13).SetCellValue("委外方式");
            header.CreateCell(14).SetCellValue("Bom组别");
            header.CreateCell(15).SetCellValue("计量单位");
            header.CreateCell(16).SetCellValue("车间");
            header.CreateCell(17).SetCellValue("仓库");
            #endregion           

            st.CreateFreezePane(0, 1, 0, 2);
            return st;
        }
        #endregion



        private SWEntity parseChild(Component2 comp, AssmblyEntity parentAss)
        {
            SWEntity entity = null;
            if (comp.GetSuppression2() == (int)swComponentSuppressionState_e.swComponentSuppressed) return null;
            //bool isLight = false;
            //if (comp.GetSuppression2() == (int)swComponentSuppressionState_e.swComponentLightweight)
            //{
            //    isLight = true;
            //    comp.SetSuppression2((int)swComponentSuppressionState_e.swComponentResolved);
            //}
            if (comp.GetModelDoc2() == null) comp.SetSuppression2((int)swComponentSuppressionState_e.swComponentResolved);
            var typeofComp = ((ModelDoc2)comp.GetModelDoc2()).GetType();

            if (typeofComp != (int)swDocumentTypes_e.swDocASSEMBLY && typeofComp != (int)swDocumentTypes_e.swDocPART)
                throw new Exception("This compnent is not Part or Assembly.Type of comp is " + typeofComp);

            if (typeofComp == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                entity = new AssmblyEntity();
                var children = (object[])((AssemblyDoc)comp.GetModelDoc2()).GetComponents(true);

                foreach (var child in children)
                {
                    if (child == null) continue;
                    var childComp = (Component2)child;
                    var childName = childComp.Name2.Substring(0, childComp.Name2.LastIndexOf("-"));

                    SortedDictionary<string, SWEntity> children2;

                    if (childComp.GetSuppression2() == (int)swComponentSuppressionState_e.swComponentSuppressed) continue;
                    var typeofComp2 = ((ModelDoc2)childComp.GetModelDoc2()).GetType();

                    if (typeofComp2 == (int)swDocumentTypes_e.swDocASSEMBLY)
                        children2 = ((AssmblyEntity)entity).childrenAss;
                    else if (typeofComp2 == (int)swDocumentTypes_e.swDocPART)
                        children2 = ((AssmblyEntity)entity).childrenPart;
                    else throw new Exception("has a component which is not ass or part");


                    if (!children2.ContainsKey(childName))
                    {
                        children2.Add(childName, parseChild(childComp, (AssmblyEntity)entity));
                    }
                    else
                    {
                        children2[childName].amount++;
                    }
                }
            }
            else
            {
                entity = new PartEntity();
            }

            entity.parent = parentAss;

            var cmo = (ModelDoc2)comp.GetModelDoc2();
            insertAttr(cmo, entity);
            //if (isLight) comp.SetSuppression2((int)swComponentSuppressionState_e.swComponentLightweight);
            return entity;
        }

        //装量转换用 double to str
        private string dtoS(double num)
        {
            var r = Math.Round(num, 2);
            if (r == 0.00) r = Math.Round(num, 3);
            return r == 0.00 ? "0.00" : r.ToString();
        }
        public static readonly string[] ext = { ".SLDPRT", ".SLDASM", ".sldasm", ".sldprt"};
            
        private string fileNameDelExt(string fileName)
        {
            var tmp = fileName.Substring(fileName.Length - 7);
            foreach (string e in ext)
            {
                if ( tmp.Equals(e)) return fileName.Substring(0, fileName.Length - 7);
            }
            return fileName;
        }

        private void insertAttr(ModelDoc2 cmo, SWEntity entity)
        {
            string valout;
            var cpm = cmo.ConfigurationManager.ActiveConfiguration.CustomPropertyManager;


            entity.name = fileNameDelExt(cmo.GetTitle());

            entity.amount = 1;

            cpm.Get4("物料代码", false, out _, out valout);
            entity.wid = valout;

            cpm.Get4("原材料代码", false, out _, out valout);
            entity.materialId = valout;

            cpm.Get4("BOM组别", false, out _, out valout);
            entity.bomlv = valout;

            cpm.Get4("车间", false, out _, out valout);
            entity.workshop = valout;


            cpm.Get4("备注", false, out _, out valout);
            entity.ps = valout;

            cpm.Get4("委派方式", false, out _, out valout);
            entity.bailment = valout;

            var nStatus = 0;
            var vMassProp = (double[])cmo.Extension.GetMassProperties2(1, out nStatus,
                cmo.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY);
            if (vMassProp != null) entity.weight = dtoS(vMassProp[5]);

            cpm.Get4("外形尺寸", false, out _, out valout);
            entity.size = valout;

            cpm.Get4("规格型号", false, out _, out valout);
            entity.specification = valout;

            cpm.Get4("计量单位", false, out _, out valout);
            entity.unit = valout;

            cpm.Get4("仓库", false, out _, out valout);
            entity.storehouse = valout;

            cpm.Get4("名称", false, out _, out valout);
            entity.propName = valout;
        }

        private int countParentAmount(SWEntity parent)
        {
            if (parent == null) return 1;
            return parent.amount * countParentAmount(parent.parent);
        }

        //遍历导出
        private void traverseExport(AssmblyEntity assE, ISheet st, string pre, int count)
        {
            var row = st.CreateRow(from);
            string lv;
            if (assE.parent != null)
            {
                lv = pre + '.' + count;
                row.CreateCell(1).SetCellValue(assE.parent.wid);
                row.CreateCell(2).SetCellValue(countParentAmount(assE.parent));
            }
            else
            {
                row.CreateCell(1).SetCellValue(assE.parentId);
                row.CreateCell(2).SetCellValue(1);
                lv = count.ToString();
            }
            row.CreateCell(0).SetCellValue(lv);
            row.CreateCell(3).SetCellValue(assE.wid);
            row.CreateCell(4).SetCellValue(assE.materialId);
            row.CreateCell(5).SetCellValue(assE.amount);
            row.CreateCell(7).SetCellValue(assE.ps);
            row.CreateCell(8).SetCellValue(assE.size);
            row.CreateCell(9).SetCellValue(assE.specification);
            row.CreateCell(10).SetCellValue(assE.propName);
            row.CreateCell(11).SetCellValue(assE.name);
            row.CreateCell(12).SetCellValue(assE.weight);
            row.CreateCell(13).SetCellValue(assE.bailment);
            row.CreateCell(14).SetCellValue(assE.bomlv);
            row.CreateCell(15).SetCellValue(assE.unit);
            row.CreateCell(16).SetCellValue(assE.workshop);
            row.CreateCell(17).SetCellValue(assE.storehouse);
            from++;

            var enumerator = assE.childrenAss.GetEnumerator();
            var countChild = 0;
            while (enumerator.MoveNext())
            {
                countChild++;
                var kv = enumerator.Current;
                traverseExport((AssmblyEntity)kv.Value, st, lv, countChild);
            }

            enumerator = assE.childrenPart.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var kv = enumerator.Current;
                row = st.CreateRow(from);
                countChild++;
                row.CreateCell(0).SetCellValue(lv + '.' + countChild);
                row.CreateCell(1).SetCellValue(kv.Value.parent.wid);
                row.CreateCell(2).SetCellValue(countParentAmount(kv.Value.parent));
                row.CreateCell(3).SetCellValue(kv.Value.wid);
                row.CreateCell(4).SetCellValue(kv.Value.materialId);
                row.CreateCell(5).SetCellValue(kv.Value.amount);
                row.CreateCell(7).SetCellValue(kv.Value.ps);
                row.CreateCell(8).SetCellValue(kv.Value.size);
                row.CreateCell(9).SetCellValue(kv.Value.specification);
                row.CreateCell(10).SetCellValue(kv.Value.propName);
                row.CreateCell(11).SetCellValue(kv.Value.name);
                row.CreateCell(12).SetCellValue(kv.Value.weight);
                row.CreateCell(13).SetCellValue(kv.Value.bailment);
                row.CreateCell(14).SetCellValue(kv.Value.bomlv);
                row.CreateCell(15).SetCellValue(kv.Value.unit);
                row.CreateCell(16).SetCellValue(kv.Value.workshop);
                row.CreateCell(17).SetCellValue(kv.Value.storehouse);
                from++;            
            }
        }
        private bool preBom(SldWorks swApp)
        {
            if (swApp == null)
            {
                MessageBox.Show("未链接至sw，请先链接");
                return false;
            }
            swApp.CommandInProgress = true;
            var swModel = (ModelDoc2)swApp.ActiveDoc;
            if (swModel == null)
            {
                MessageBox.Show("未检测到sw打开文件，请检查：1.sw是否打开了文件 2.sw是否链接正确");
                return false;
            }
            if (swModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                MessageBox.Show("打开文件不是装配体");
                return false;
            }
            return true;
        }
        private string byteToStr(ModelDoc2 parentMod, Component2 com)
        {
            if (parentMod == null && com == null) return "F";
            Byte[] b = parentMod.Extension.GetPersistReference3(com) as Byte[];
            StringBuilder sb = new StringBuilder();
            var i = 0;
            foreach (var bb in b)
            {
                sb.Append(bb).Append(',');
                i++;
            }
            return sb.ToString(0, sb.Length-1);
        }

        private Byte[] strToByte(string str)
        {
            if (str == null) return null;
            var ss = str.Split(",");
            Byte[] res = new Byte[ss.Length];
            int i = 0;
            foreach (var s in ss)
            {
                res[i++] = Convert.ToByte(s);
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var swApp = SolidWorksSingleton.GetApplicationDirectly();
            if (!preBom(swApp)) return;

            string pathForExcel;
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xlsx files(*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "未命名";
            saveFileDialog.AddExtension = true;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                pathForExcel = saveFileDialog.FileName;
            }
            else
            {
                return;
            }

            //表头创建
            var wb = new XSSFWorkbook();
            var st = GenerateTitle(wb);


            swApp.CommandInProgress = true;
            var swModel = (ModelDoc2)swApp.ActiveDoc;
            var swAssy = (AssemblyDoc)swModel;

            this.label1.Visible = true;
            this.label1.Text = "正在解轻化零件..";
            this.progressBar1.Visible = true;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = 0;
            this.progressBar1.Step = 1;

            swAssy.ResolveAllLightweight();

            //首个装配体处理
            var assE = new AssmblyEntity();
            insertAttr(swModel, assE);
            assE.parentId = assE.wid;

            //每个部件处理

            var compList = (object[])swAssy.GetComponents(true);
            
            this.progressBar1.Maximum = compList.Length+1;


            foreach (var item in compList)
            {
                var swComponent = (Component2)item;
                if (swComponent.GetSuppression2() == (int)swComponentSuppressionState_e.swComponentSuppressed)
                    continue;
                //bool isLight = false;
                //if (swComponent.GetSuppression2() == (int)swComponentSuppressionState_e.swComponentLightweight)
                //{
                //    isLight = true;
                //    swComponent.SetSuppression2((int)swComponentSuppressionState_e.swComponentResolved);
                //}

                var filename = swComponent.Name2.Substring(0, swComponent.Name2.LastIndexOf('-'));

                SortedDictionary<string, SWEntity> children;
                if (swComponent.GetModelDoc2() == null) swComponent.SetSuppression2((int)swComponentSuppressionState_e.swComponentResolved);
                var typeofComp = ((ModelDoc2)swComponent.GetModelDoc2()).GetType();

                if (typeofComp == (int)swDocumentTypes_e.swDocASSEMBLY)
                    children = assE.childrenAss;
                else if (typeofComp == (int)swDocumentTypes_e.swDocPART)
                    children = assE.childrenPart;
                else throw new Exception("has a component which is not ass or part");

                if (!children.ContainsKey(filename))
                {
                    children.Add(filename, parseChild(swComponent, assE));
                }
                else
                {
                    children[filename].amount++;

                }
                progressBar1.PerformStep();
                label1.Text = (int)(((double)progressBar1.Value) / progressBar1.Maximum * 100) + "%/100%";
                //if (isLight) swComponent.SetSuppression2((int)swComponentSuppressionState_e.swComponentLightweight);
            }
            label1.Text = "正在加轻化零件..";
            swAssy.LightweightAllResolved();

            from = 1;
            label1.Text = "写入中.." + (int)(((double)progressBar1.Value) / progressBar1.Maximum * 100) + "%/100%";
            traverseExport(assE, st, "", 1);
            var fs = new FileStream(@pathForExcel, FileMode.Create, FileAccess.Write);
            wb.Write(fs);
            fs.Close();

            this.label1.Visible = false;
            this.progressBar1.Visible = false;
            MessageBox.Show("导出成功！");
        }

        private void btnLinkToAPI_Click(object sender, EventArgs e)
        {
            var swApp = SolidWorksSingleton.GetApplicationDirectly();
            if (swApp != null)
            {
                MessageBox.Show("sw已连接");
                return;
            }
            swApp = SolidWorksSingleton.GetApplication();
            if (swApp != null)
            {
                indicatorLightLabel.ForeColor = Color.Green;
                indicatorTextLabel.Text = "连接至pid:" + swApp.GetProcessID();
            }
        }

        private void btnManageProg_Click(object sender, EventArgs e)
        {
            var form2 = new Form2();
            form2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var swApp = SolidWorksSingleton.GetApplication();
            if (swApp != null)
            {
                indicatorLightLabel.ForeColor = Color.Green;
                indicatorTextLabel.Text = "连接至pid:" + swApp.GetProcessID();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var s = SolidWorksSingleton.GetApplicationDirectly();
            if (s != null)
                try
                {
                    s.GetProcessID();
                    return;
                }
                catch
                {
                    SolidWorksSingleton.Dipose();
                }

            indicatorLightLabel.ForeColor = Color.Red;
            indicatorTextLabel.Text = "未链接";
        }
        private string getCellStr(IRow row,int i)
        {
            var cell = row.GetCell(i);
            if (cell == null) return "";
            cell.SetCellType(CellType.String);
            return cell.StringCellValue;
        }

        private void childrenFandao(string pathForExcel,SldWorks swApp)
        {
            var fs = new FileStream(pathForExcel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var wb = new XSSFWorkbook(fs);
            var st = wb.GetSheet("bom");

            swApp.CommandInProgress = true;
            var swModel = (ModelDoc2)swApp.ActiveDoc;
            var swAssy = (AssemblyDoc)swModel;

            this.label1.Visible = true;
            this.label1.Text = "正在解轻化零件..";
            this.progressBar1.Visible = true;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = 0;
            this.progressBar1.Step = 1;

            swAssy.ResolveAllLightweight();
            var compList = (object[])swAssy.GetComponents(false);

            this.progressBar1.Maximum = (compList.Length + 1) * 2 + 1;

            Dictionary<string, List<ModelDoc2>> nameModelMap = new Dictionary<string, List<ModelDoc2>>();
            
            var s = swAssy.GetComponentCount(false);
            var fl = new List<ModelDoc2>();
            fl.Add(swModel);
            nameModelMap[fileNameDelExt(swModel.GetTitle())] = fl;

            progressBar1.PerformStep();
            label1.Text = (int)(((double)progressBar1.Value) / progressBar1.Maximum * 100) + " %/100%";

            foreach (Component2 comp in compList)
            {
                var i = comp.Name2.LastIndexOf('/');
                var name = comp.Name2.Substring(comp.Name2.LastIndexOf('/') + 1, comp.Name2.LastIndexOf('-') - i - 1);
                List<ModelDoc2> modelList;
                if (nameModelMap.ContainsKey(name)) modelList = nameModelMap[name];
                else
                {
                    modelList = new List<ModelDoc2>();
                }
                //如果是压缩的 就掠过
                if ((ModelDoc2)comp.GetModelDoc2() == null) continue;
                modelList.Add((ModelDoc2)comp.GetModelDoc2());
                nameModelMap[name] = modelList;
                
                progressBar1.PerformStep();
                label1.Text = (int)(((double)progressBar1.Value) / progressBar1.Maximum * 100) + "%/100%";
            }

            HashSet<string> set = new HashSet<string>();
            for (var i = 1; i <= st.LastRowNum; i++)
            {
                var row = st.GetRow(i);
                if (row == null|| row.GetCell(11)== null) continue;


                string wid = getCellStr(row, 3);
                string parentid = getCellStr(row, 1);
                string mid =  getCellStr(row, 4);
                string size = getCellStr(row, 9);
                string proname = getCellStr(row, 10);

                string name = row.GetCell(11).StringCellValue;

                if (set.Contains(name)) continue;
                set.Add(name);
                var modellist = nameModelMap[name];
                foreach (var model in modellist)
                {

                    var cpm = model.ConfigurationManager.ActiveConfiguration.CustomPropertyManager;
                    cpm.Set2("物料代码", wid);
                    cpm.Set2("装配物料代码", parentid);
                    cpm.Set2("原材料代码", mid);
                    cpm.Set2("规格型号", size);
                    cpm.Set2("名称", proname);

                    label1.Text =  (int)(((double)progressBar1.Value) / progressBar1.Maximum * 100) + "%/100%";
                    progressBar1.PerformStep();
                }
            }
            this.label1.Text = "正在加轻化零件..";
            swAssy.LightweightAllResolved();
            fs.Close();
            this.label1.Visible = false;
            this.progressBar1.Visible = false;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            var swApp = SolidWorksSingleton.GetApplicationDirectly();
            if (!preBom(swApp)) return;
            string pathForExcel;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xlsx files(*.xlsx)|*.xlsx";
            openFileDialog.AddExtension = true;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathForExcel = openFileDialog.FileName;
            }
            else
            {
                return;
            }

            childrenFandao(@pathForExcel, swApp);

            MessageBox.Show("反导完成！");
            
        }
    }
}