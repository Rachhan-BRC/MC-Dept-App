using MachineDeptApp.Admin;
using MachineDeptApp.Inventory.Inprocess;
using MachineDeptApp.Inventory.KIT;
using MachineDeptApp.Inventory.MC_SD;
using MachineDeptApp.MCPlans;
using MachineDeptApp.MCSDControl;
using MachineDeptApp.MCSDControl.SDRec;
using MachineDeptApp.MCSDControl.WIR1__Wire_Stock_;
using MachineDeptApp.NG_Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using MachineDeptApp.Inventory;

namespace MachineDeptApp
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        private const int SW_RESTORE = 9;

        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "520c53bb-80df-427d-8866-dfa5bcc6f4d6"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    // Find the existing instance and bring it to the foreground
                    var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
                    {
                        if (process.Id != currentProcess.Id)
                        {
                            IntPtr handle = process.MainWindowHandle;

                            // Check if the window is minimized
                            if (IsIconic(handle))
                            {
                                ShowWindow(handle, SW_RESTORE);
                            }

                            // Check if the window is already in the foreground
                            if (GetForegroundWindow() != handle)
                            {
                                SetForegroundWindow(handle);
                            }
                            break;
                        }
                    }
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm());
                //For Design test
                //Application.Run(new NGInputForm());
                
            }
        }
    }
}
