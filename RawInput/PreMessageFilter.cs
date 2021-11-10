using System.Windows.Forms;

namespace GESAP.Std.LowLayer.Windows.RawInput
{   
   
    public class PreMessageFilter : IMessageFilter
    {
        // is the current device is use to compare
        public static string hDevice = string.Empty;
        // This is filter
        public static string HIDDevice = string.Empty;

        // true  to filter the message and stop it from being dispatched 
        // false to allow the message to continue to the next filter or control.
        public bool PreFilterMessage(ref Message m)
        {
            if (HIDDevice == string.Empty || hDevice.Contains(HIDDevice))                
                return m.Msg == Win32.WM_KEYDOWN;
            else
                return false;
        }
    }
}
