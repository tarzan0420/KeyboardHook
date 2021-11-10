# KeyboardHook
We have made a Keyboard trap program, it trap keys for listed HID devices using VID & PID info. The project is completed but one issue need to solve: for each device to be trapped, a flag named IsolateHook coud be set; if it's set to true, key press must be isolated and not transferred to other Apps.

To test it, setup VID & PID for your keyboard, setting IsolateHook to true and run program. Open notepad.exe and try to press "A" key. A key must be trapped by the App and not by notepad.exe.
