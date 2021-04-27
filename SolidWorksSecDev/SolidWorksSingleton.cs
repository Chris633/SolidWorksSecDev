using System;
using System.Diagnostics;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;

namespace SolidWorksSecDev
{
    internal class SolidWorksSingleton
    {
        private static SldWorks swApp;

        public static SldWorks GetApplication()
        {
            if (swApp == null)
            {
                var i = numOfRunningSW();
                if (i == 0)
                {
                    MessageBox.Show("未检测到sw进程");
                    return null;
                }

                if (i > 1)
                {
                    MessageBox.Show("检测到多个sw进程，请处理");
                    var form2 = new Form2();
                    form2.ShowDialog();
                }

                swApp = Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application")) as SldWorks;
            }
            return swApp;
        }

        public static int numOfRunningSW()
        {
            var processes = Process.GetProcesses();
            var i = 0;
            if (processes == null) throw new Exception("未获取到任何进程"); //加日志处理
            foreach (var process in processes)
                if (process.ProcessName.Equals("SLDWORKS"))
                    i++;
            return i;
        }

        public static SldWorks GetApplicationDirectly()
        {
            return swApp;
        }

        public static void Dipose()
        {
            if (swApp != null)
            {
                try
                {
                    swApp.ExitApp();
                }
                catch
                {
                }

                swApp = null;
            }
        }
    }
}