using KeySender.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeySender.Core
{
    static class Messenger
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow(); // Get the active application.

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId); // Get the Process Id of the active application.

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam); // Send a key to the active application.

        public static void SendKey(KeyCommand key)
        {
            try
            {
                IntPtr hWnd = GetForegroundWindow();

                if (Config.IsLoggingEnabled)
                {
                    uint procId = 0;
                    GetWindowThreadProcessId(hWnd, out procId);
                    Process process = Process.GetProcessById((int)procId);
                    Log.Trace("Sending the key [{0}] to the active application [{1}].", key, process.ProcessName);
                }

                SendMessage(hWnd, 0x100, ((IntPtr)(ushort)key), (IntPtr)0); // 0x100 is the flag for a keydown event.
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
