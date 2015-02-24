using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KVMDisplaySwitcher
{
    public static class Display
    {
        [Flags]
        enum MessageBroadcastFlags : uint
        {
            BSF_QUERY = 0x00000001,
            BSF_IGNORECURRENTTASK = 0x00000002,
            BSF_FLUSHDISK = 0x00000004,
            BSF_NOHANG = 0x00000008,
            BSF_POSTMESSAGE = 0x00000010,
            BSF_FORCEIFHUNG = 0x00000020,
            BSF_NOTIMEOUTIFNOTHUNG = 0x00000040,
            BSF_ALLOWSFW = 0x00000080,
            BSF_SENDNOTIFYMESSAGE = 0x00000100,
            BSF_RETURNHDESK = 0x00000200,
            BSF_LUID = 0x00000400
        }

        [Flags]
        enum MessageBroadcastRecipients : uint
        {
            BSM_ALLCOMPONENTS = 0x00000000,
            BSM_VXDS = 0x00000001,
            BSM_NETDRIVER = 0x00000002,
            BSM_INSTALLABLEDRIVERS = 0x00000004,
            BSM_APPLICATIONS = 0x00000008,
            BSM_ALLDESKTOPS = 0x00000010
        }

        [DllImport("user32", SetLastError = true)]
        private static extern int BroadcastSystemMessage(MessageBroadcastFlags flags, ref MessageBroadcastRecipients lpInfo, uint Msg, IntPtr wParam, IntPtr lParam);

        public static void PowerOff()
        {
            MessageBroadcastRecipients recipients = MessageBroadcastRecipients.BSM_ALLDESKTOPS;
            BroadcastSystemMessage(
                MessageBroadcastFlags.BSF_FORCEIFHUNG,
                ref recipients,
                0x0112,         // WM_SYSCOMMAND
                (IntPtr)0xf170, // SC_MONITORPOWER
                (IntPtr)0x0002  // POWER_OFF
                );
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001;

        public static void PowerOn()
        {
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
        }
    }
}
