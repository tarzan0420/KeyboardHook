using System;
using System.Windows.Forms;
using Utilities;

namespace WinFormsApp1
{
    public partial class Stamp : Form
    {
        System.Timers.Timer tmr = new System.Timers.Timer(4000);
        globalKeyboardHook gkh = new globalKeyboardHook();

        public Stamp()
        {
            InitializeComponent();

            tmr.Elapsed += Elapsed;
            tmr.AutoReset = false;
        }

        private void Elapsed(object sender, EventArgs e)
        {                                              
            this.Invoke((MethodInvoker)delegate
            {
                this.BackColor = System.Drawing.Color.LightGray;
                this.MainLabel.BackColor = System.Drawing.Color.LightGray;
                this.MainLabel.Text = "Attesa lettura...";   // runs on UI thread
                this.Refresh();
            });            

            Application.DoEvents();
        }
                       
        private void Stamp_Load(object sender, EventArgs e)
        {
            Program._rawinput = new GESAP.Std.LowLayer.Windows.RawInput.RawInput(Handle, false);

            Program._rawinput.AddMessageFilter();   // Adding a message filter will cause keypresses to be handled
            
            // Writes a file DeviceAudit.txt to the current directory
            GESAP.Std.LowLayer.Windows.RawInput.Win32.DeviceAudit();            

            Program._rawinput.KeyPressed += OnKeyPressed;

            //To be enabled when work is finish
            // Program.DeviceList.SampleEvent += KeybRead;

            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            for (int i = 0; i < 256; i++)
            {
                gkh.HookedKeys.Add((Keys)i);
            }
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void KeybRead(object sender, Functions.Devices.EventArgs e)
        {
            var color = System.Drawing.Color.LightGray;

            var route = GESAP.Std.LowLayer.DataXChange.QRCode.Route(e.Text, Program.CpuId,((int)GESAP.Std.LowLayer.BaseFunctions.Apps.NamesInfo.APP_InfoPoint).ToString());
            if (route.Item1.Endpoint != null)
            {                
                var send = GESAP.Std.LowLayer.APIConsumer.Commons.Simple.Get<bool>(route.Item1.Endpoint, route.Item1.Message);
                if(send.Item1)
                    color = System.Drawing.Color.Green;
                else
                    color = System.Drawing.Color.Red;                
            }
            else
                color = System.Drawing.Color.Red;

            this.Invoke((MethodInvoker)delegate
            {
                var persona = GESAP.Std.LowLayer.APIConsumer.Anagrafica.Personale.GetName(GESAPRemoting.Properties.Settings.Default.ApiEmployee, route.Item1.UserId);
                if (persona.Item2 == null)
                    this.MainLabel.Text = persona.Item1;

                this.BackColor = color;
                this.MainLabel.BackColor = color;
                this.Refresh();
            });
            
            tmr.Enabled = true;
        }
        private void OnKeyPressed(object sender, GESAP.Std.LowLayer.Windows.RawInput.RawInputEventArg e)
        {
            var isFromTrapDevice = Program.DeviceList.Process(e.KeyPressEvent);
            if (isFromTrapDevice)
                MainLabel.Text = e.KeyPressEvent.VKeyName.ToString();
            ////Console.WriteLine(isFromTrapDevice);
        }

        private void OnIsoTrueClick(object sender, EventArgs e)
        {
            for(int i=0; i< Program.DeviceList.Get().Count; i++ )
            {
                var item = Program.DeviceList.Get()[i];
                item.IsolateHook = true;
                Program.DeviceList.Get().RemoveAt(i);
                Program.DeviceList.Get().Insert(i, item);
            }        
        }

        private void OnIsoFalseClick(object sender, EventArgs e)
        {
            for (int i = 0; i < Program.DeviceList.Get().Count; i++)
            {
                var item = Program.DeviceList.Get()[i];
                item.IsolateHook = false;
                Program.DeviceList.Get().RemoveAt(i);
                Program.DeviceList.Get().Insert(i, item);
            }
        }
    }
}
