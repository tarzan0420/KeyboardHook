using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WinFormsApp1
{   
    class Functions
    {
        public static async Task< bool >CheckForInternetConnection(int timeoutMs = 10000, string url = null)
        {
            try
            {
                url ??= CultureInfo.InstalledUICulture switch
                {
                    { Name: var n } when n.StartsWith("fa") => // Iran
                        "http://www.aparat.com",
                    { Name: var n } when n.StartsWith("zh") => // China
                        "http://www.baidu.com",
                    { Name: var n } when n.StartsWith("it") => // Italy
                        "http://www.google.com",
                    _ =>
                        "http://www.gstatic.com/generate_204",
                };

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                var response = await request.GetResponseAsync();

                if(response!=null)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        public class Devices
        {
            public enum Types
            {
                QR=0,
                RFID=1,
                KEYBOARD=2
            }
            private List<int> _blackList = new List<int>() { KEY_SHIFT, KEY_MENU }; //16, 18
            private Dictionary<int,string> _subst = new Dictionary<int, string> { { KEY_QUOTES, "#" }, { 190, "." }, { KEY_MINUS, "-" } };
            private Dictionary<int, string> _substShift = new Dictionary<int, string> { { 55, "/" }, { 190, ":" }, { KEY_MINUS, "_" } };
            
            private const int KEY_SHIFT= 16;
            private const int KEY_MINUS = 189;
            private const int KEY_MENU = 18;
            private const int KEY_QUOTES = 222;
            private const int KEY_ENTER = 13;
            
            public class EventArgs
            {
                public EventArgs(string text, int id) { Text = text; Id = id; }
                public string Text { get; } // readonly
                public int Id { get; } // readonly
            }

            // Declare the delegate (if using non-generic pattern).
            public delegate void EventHandler(object sender, EventArgs e);

            // Declare the event.
            public event EventHandler SampleEvent;


            private readonly List<Data> _list = new List<Data>();

            public struct Data
            {
                public string Vid;
                public string Pid;
                public bool IsolateHook; //This means that trapped signal must NOT been passed to other programs
                public List<int> MsgBuffer;               
                public Info info;
            }

            public struct Info
            {
                public int Id { get; internal set; }
                public Types Type;
                public int CodeLen;
            }

            public List<Data> Get()
            {
                return _list;
            }

            public void Clear(int Id)
            {
                var item = _list[Id];
                item.MsgBuffer.Clear();
                _list[Id] = item;
            }

            public bool Process(GESAP.Std.LowLayer.Windows.RawInput.KeyPressEvent evt)
            {           
                var type = MatchDevice(evt.DeviceName);
                if (type.Id > -1)
                {
                    Push(type.Id,type.Type, evt);
                    return true; //Returns true is key is pressed by a listed device to be trapped
                }

                return false;
            }

            public void Add(string Vid, string Pid, Types Type,bool IsolateHook, int? CodeLen = 0)
            {
                var item = new Data();
                item.info.Id = _list.Count;
                item.Vid = Vid;
                item.Pid = Pid;
                item.IsolateHook = IsolateHook;
                item.MsgBuffer = new List<int>();
                item.info.Type = Type;
                item.info.CodeLen = CodeLen.Value;

                _list.Add(item);
            }

            #region"Private"
            private void Push(int id, Types type, GESAP.Std.LowLayer.Windows.RawInput.KeyPressEvent evt)
            {
                if (evt.VKey != KEY_ENTER)
                {
                    var item = _list[id];
                    item.MsgBuffer.Add(evt.VKey);                    
                    _list[id] = item;
                }
                else
                {
                    // Raise the event in a thread-safe manner using the ?. operator.
                    if (_list[id].MsgBuffer.Any())
                    {
                        //Delete duplicates
                        var newMsg = Purify(_list[id].MsgBuffer);

                        var msg = "";
                        var previous = 0;
                        foreach (int letter in newMsg)
                        {
                            if (!_blackList.Contains(letter))
                            {
                                if (previous == KEY_SHIFT)
                                {
                                    if (_substShift.ContainsKey(letter))
                                        msg += _substShift.GetValueOrDefault(letter);
                                    else
                                        msg += ((char)letter).ToString();
                                }
                                else
                                {
                                    if (_subst.ContainsKey(letter))
                                        msg += _subst.GetValueOrDefault(letter);
                                    else
                                        msg += ((char)letter).ToString().ToLower();
                                }
                            }

                            previous = letter;
                        }

                        SampleEvent?.Invoke(this, new EventArgs(msg, id));
                        Clear(id);
                    }
                }
            }

            private List<int> Purify(List<int> list)
            {
                Dictionary<int, int> _jumps = new Dictionary<int, int> { { KEY_SHIFT, 2 }, { KEY_MENU, 2 } };

                var jumpCnt = -1;
                var takeCnt = 0;
                var newMsg = new List<int>();

                for (var i = 0; i < list.Count(); i++)
                {
                    if (takeCnt >= 0)
                    {
                        newMsg.Add(list[i]);
                        takeCnt++;

                        _jumps.TryGetValue(list[i], out int take);
                        if (takeCnt >= take)
                        {
                            takeCnt = -1;
                            jumpCnt = 0;
                        }
                    }
                    else
                    {
                        jumpCnt++;

                        _jumps.TryGetValue(list[i], out int jump);
                        if (jumpCnt >= jump)
                        {
                            takeCnt = 0;
                            jumpCnt = -1;
                        }
                    }
                }

                return newMsg;
            }

            public bool IsExistDeviceList(string name)
            {
                const string VID = "#VID_";
                const string PID = "&PID_";

                if (name != "")
                {
                    var vid = name.Substring(name.IndexOf(VID) + VID.Length, 4);
                    var pid = name.Substring(name.IndexOf(PID) + PID.Length, 4);

                    foreach (var item in Get())
                    {
                        if (item.Vid == vid && item.Pid == pid)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public bool GetIsoHookedState(string name)
            {
                const string VID = "#VID_";
                const string PID = "&PID_";

                if (name != "")
                {
                    var vid = name.Substring(name.IndexOf(VID) + VID.Length, 4);
                    var pid = name.Substring(name.IndexOf(PID) + PID.Length, 4);

                    foreach (var item in Get())
                    {
                        if (item.Vid == vid && item.Pid == pid)
                        {
                            return item.IsolateHook;
                        }
                    }
                }                
                return false;
            }

            private Info MatchDevice(string name)
            {
                const string VID = "#VID_";
                const string PID = "&PID_";

                if (name != "")
                {
                    var vid = name.Substring(name.IndexOf(VID) + VID.Length, 4);
                    var pid = name.Substring(name.IndexOf(PID) + PID.Length, 4);

                    foreach (var item in Get())
                    {
                        if (item.Vid == vid && item.Pid == pid)
                        {
                            return item.info;
                        }
                    }
                }

                var tmp = new Info();
                tmp.Id = -1;
                return tmp;
            }

            public bool IsbIsolateHook()
            {
                foreach (var item in Get())
                {
                    if (item.IsolateHook == true)
                    {
                        return true;
                    }
                }
                return false;
            }
            #endregion
        }
    }
}
