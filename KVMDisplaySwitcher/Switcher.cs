using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KVMDisplaySwitcher.Properties;

namespace KVMDisplaySwitcher
{
    public class Switcher
    {
        private static HidDevice _device;

        public static void Start()
        {
            _device = HidDevices.Enumerate(Settings.Default.WatchDeviceVID, Settings.Default.WatchDevicePID).FirstOrDefault();

            if (_device != null)
            {
                _device.OpenDevice();
                _device.Removed += () => Display.PowerOff();
                _device.Inserted += () => Display.PowerOn();
                _device.MonitorDeviceEvents = true;
            }
        }

        public static void Stop()
        {
            if (_device != null)
                _device.CloseDevice();
        }
    }
}
