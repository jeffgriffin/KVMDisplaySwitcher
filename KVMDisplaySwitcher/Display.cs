using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KVMDisplaySwitcher.Properties;

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

        private static void PowerOffAll()
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

        private static void PowerOnAll()
        {
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
        }

        enum DISP_CHANGE : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

        [Flags()]
        public enum ChangeDisplaySettingsFlags : uint
        {
            CDS_NONE = 0,
            CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            CDS_FULLSCREEN = 0x00000004,
            CDS_GLOBAL = 0x00000008,
            CDS_SET_PRIMARY = 0x00000010,
            CDS_VIDEOPARAMETERS = 0x00000020,
            CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            CDS_RESET = 0x40000000,
            CDS_RESET_EX = 0x20000000,
            CDS_NORESET = 0x10000000
        }

        struct POINTL
        {
            public Int32 x;
            public Int32 y;
        }

        [Flags()]
        enum DM : int
        {
            Orientation = 0x1,
            PaperSize = 0x2,
            PaperLength = 0x4,
            PaperWidth = 0x8,
            Scale = 0x10,
            Position = 0x20,
            NUP = 0x40,
            DisplayOrientation = 0x80,
            Copies = 0x100,
            DefaultSource = 0x200,
            PrintQuality = 0x400,
            Color = 0x800,
            Duplex = 0x1000,
            YResolution = 0x2000,
            TTOption = 0x4000,
            Collate = 0x8000,
            FormName = 0x10000,
            LogPixels = 0x20000,
            BitsPerPixel = 0x40000,
            PelsWidth = 0x80000,
            PelsHeight = 0x100000,
            DisplayFlags = 0x200000,
            DisplayFrequency = 0x400000,
            ICMMethod = 0x800000,
            ICMIntent = 0x1000000,
            MediaType = 0x2000000,
            DitherType = 0x4000000,
            PanningWidth = 0x8000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        struct DEVMODE
        {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            [System.Runtime.InteropServices.FieldOffset(0)]
            public string dmDeviceName;
            [System.Runtime.InteropServices.FieldOffset(32)]
            public Int16 dmSpecVersion;
            [System.Runtime.InteropServices.FieldOffset(34)]
            public Int16 dmDriverVersion;
            [System.Runtime.InteropServices.FieldOffset(36)]
            public Int16 dmSize;
            [System.Runtime.InteropServices.FieldOffset(38)]
            public Int16 dmDriverExtra;
            [System.Runtime.InteropServices.FieldOffset(40)]
            public DM dmFields;

            [System.Runtime.InteropServices.FieldOffset(44)]
            Int16 dmOrientation;
            [System.Runtime.InteropServices.FieldOffset(46)]
            Int16 dmPaperSize;
            [System.Runtime.InteropServices.FieldOffset(48)]
            Int16 dmPaperLength;
            [System.Runtime.InteropServices.FieldOffset(50)]
            Int16 dmPaperWidth;
            [System.Runtime.InteropServices.FieldOffset(52)]
            Int16 dmScale;
            [System.Runtime.InteropServices.FieldOffset(54)]
            Int16 dmCopies;
            [System.Runtime.InteropServices.FieldOffset(56)]
            Int16 dmDefaultSource;
            [System.Runtime.InteropServices.FieldOffset(58)]
            Int16 dmPrintQuality;

            [System.Runtime.InteropServices.FieldOffset(44)]
            public POINTL dmPosition;
            [System.Runtime.InteropServices.FieldOffset(52)]
            public Int32 dmDisplayOrientation;
            [System.Runtime.InteropServices.FieldOffset(56)]
            public Int32 dmDisplayFixedOutput;

            [System.Runtime.InteropServices.FieldOffset(60)]
            public short dmColor; // See note below!
            [System.Runtime.InteropServices.FieldOffset(62)]
            public short dmDuplex; // See note below!
            [System.Runtime.InteropServices.FieldOffset(64)]
            public short dmYResolution;
            [System.Runtime.InteropServices.FieldOffset(66)]
            public short dmTTOption;
            [System.Runtime.InteropServices.FieldOffset(68)]
            public short dmCollate; // See note below!
            [System.Runtime.InteropServices.FieldOffset(72)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            [System.Runtime.InteropServices.FieldOffset(102)]
            public Int16 dmLogPixels;
            [System.Runtime.InteropServices.FieldOffset(104)]
            public Int32 dmBitsPerPel;
            [System.Runtime.InteropServices.FieldOffset(108)]
            public Int32 dmPelsWidth;
            [System.Runtime.InteropServices.FieldOffset(112)]
            public Int32 dmPelsHeight;
            [System.Runtime.InteropServices.FieldOffset(116)]
            public Int32 dmDisplayFlags;
            [System.Runtime.InteropServices.FieldOffset(116)]
            public Int32 dmNup;
            [System.Runtime.InteropServices.FieldOffset(120)]
            public Int32 dmDisplayFrequency;
        }

        [DllImport("user32.dll")]
        static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "ChangeDisplaySettingsEx")]
        static extern DISP_CHANGE ChangeDisplaySettingsExNoop(string lpszDeviceName, IntPtr noop, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);


        public static void PowerOff()
        {
            if (string.IsNullOrEmpty(Settings.Default.PowerOffDevices))
            {
                PowerOffAll();
                return;
            }
            foreach (var device in Settings.Default.PowerOffDevices.Split('|'))
                PowerOffDevice(device);
        }

        public static void PowerOn()
        {
            if (string.IsNullOrEmpty(Settings.Default.PowerOffDevices))
            {
                PowerOnAll();
                return;
            }
            var settings = Settings.Default.DeviceSettings.Split('|');
            foreach (var deviceSettingsString in settings)
                SetupDevice(deviceSettingsString);
            Commit();
        }

        private static void PowerOffDevice(string device)
        {
            SetupDevice(device, 0, 0);
            Commit();
        }

         private static void SetupDevice(string allSettings)
         {
             var settings = allSettings.Split(',');
             var intSettings = settings.Skip(1).Select(s=>int.Parse(s));
             SetupDevice(settings[0], intSettings.First(), intSettings.Skip(1).First(), intSettings.Skip(2).FirstOrDefault(), intSettings.Skip(3).FirstOrDefault());
         }

        private static void SetupDevice(string device, int width, int height, int x = 0, int y = 0)
        {
            ChangeDisplaySettingsFlags flags = ChangeDisplaySettingsFlags.CDS_NONE;
            if (x == 0 && y == 0)
                flags = ChangeDisplaySettingsFlags.CDS_SET_PRIMARY;

            DEVMODE screenMode = new DEVMODE();
            screenMode.dmSize = Convert.ToInt16(Marshal.SizeOf(screenMode));
            screenMode.dmDriverExtra = 0;
            screenMode.dmFields = DM.Position | DM.PelsHeight | DM.PelsWidth;
            screenMode.dmPelsWidth = width;
            screenMode.dmPelsHeight = height;

            POINTL deleteion;
            deleteion.x = x;
            deleteion.y = y;
            screenMode.dmPosition = deleteion;

            ChangeDisplaySettings(device, screenMode, flags);
        }

        private static void ChangeDisplaySettings(string device, DEVMODE mode, ChangeDisplaySettingsFlags flags = ChangeDisplaySettingsFlags.CDS_NONE)
        {
            ChangeDisplaySettingsEx(device,
                                    ref mode,
                                    IntPtr.Zero,
                                    flags | ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY | ChangeDisplaySettingsFlags.CDS_NORESET,
                                    IntPtr.Zero);
        }

        private static void Commit()
        {
            ChangeDisplaySettingsExNoop(null, IntPtr.Zero, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_NONE, IntPtr.Zero);
        }
    }
}
