
using System;
using System.Windows.Forms;

namespace WinFormsApp1
{    
    static class Program
    {
        public static string CpuId = string.Empty;
        public static NotifyIcon icon = new NotifyIcon();
        public static GESAP.Std.LowLayer.Windows.RawInput.RawInput _rawinput;
        public static Functions.Devices DeviceList = new Functions.Devices();

        public const string STBY_MSG= "Attesa lettura...";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
                       
            icon.Icon = new System.Drawing.Icon("./icon.ico");
            icon.Text = "GESAP 2021 - Remoting v.1.0.0, partenza sistema...";            
            icon.Visible = true;
            
            var CPUId = GESAP.Std.LowLayer.Windows.Functions.GetCPUId();
            if (CPUId.Item2 != null)
            {
                Application.Exit();                
            }
            else
            {
                CpuId = CPUId.Item1;
                var internetUp = Functions.CheckForInternetConnection().Result;
            }

            //This is list of HID Devices to be TRAPPED VID & PID
            DeviceList.Add("046D", "C31C", Functions.Devices.Types.RFID, true, 10); //RFID
            DeviceList.Add("AC90", "3002", Functions.Devices.Types.QR, false, 0); //QRCode scanner
         
            //Here you can put your test keyboard
            DeviceList.Add("046D", "C31C", Functions.Devices.Types.KEYBOARD, true, 0); //Bool parameter

            ContextMenuStrip contextMenu1 = new ContextMenuStrip();
            
            contextMenu1.Items.Add("Codice: "+CpuId);
            contextMenu1.Items.Add("Lettori configurati: " + DeviceList.Get().Count.ToString());
            contextMenu1.Items.Add(new ToolStripSeparator());
            contextMenu1.Items.Add("E&sci", null, new EventHandler(mnuExit_Click));
            icon.ContextMenuStrip = contextMenu1;
           
            icon.Text = "GESAP 2021 - Remoting v.1.0.0, in esecuzione..";
            
            Application.Run(new Stamp());
        }            

        private static void mnuExit_Click(object sender, EventArgs e)
        {            
            Application.Exit();
        }                   
    }
}
