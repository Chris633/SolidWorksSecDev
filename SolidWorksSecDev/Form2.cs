using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SolidWorksSecDev
{
    public partial class Form2 : Form
    {
        int swPid;
        public Form2()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("pid", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("进程名", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("窗口名", 350, HorizontalAlignment.Left);
            listView1.Columns.Add("绑定", 100, HorizontalAlignment.Left);
                        
            var swApp = SolidWorksSingleton.GetApplicationDirectly();
            
            if (swApp == null) swPid = 0;
            else
                swPid = swApp.GetProcessID();

            listView1.BeginUpdate();
            listView1.Items.Clear();
            var processes = Process.GetProcesses();

            foreach (var process in processes)
                if (process.ProcessName.Equals("SLDWORKS"))
                {
                    var lvi = new ListViewItem();
                    lvi.Text = process.Id.ToString();
                    lvi.SubItems.Add(process.ProcessName);
                    lvi.SubItems.Add(process.MainWindowTitle);
                    if (process.Id == swPid) lvi.SubItems.Add("√");
                    listView1.Items.Add(lvi);
                }

            listView1.EndUpdate();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            Form1.getForm1().Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
                try
                {
                    Process.GetProcessById(int.Parse(item.Text)).Kill();
                    item.Remove();
                }
                catch (ArgumentException ex)
                {
                    item.Remove();
                }
                catch (Exception ex)
                {
                }
        }
    }
}