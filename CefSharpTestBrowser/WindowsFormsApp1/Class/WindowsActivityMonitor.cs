using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Gma.System.MouseKeyHook;
using WindowsFormsApp1;

namespace SkydevCSTool.Class
{
    class WindowsActivityMonitor
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);


        public static int GetInactiveTime()
        {
            LASTINPUTINFO info = new LASTINPUTINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            if (GetLastInputInfo(ref info))
                return (int)TimeSpan.FromMilliseconds(Environment.TickCount - info.dwTime).TotalSeconds;
            else
                return 0;
        }

    }
}
