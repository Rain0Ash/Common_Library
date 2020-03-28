// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Common_Library.Workstation
{
    public enum ProcessorArchitecture
    {
        Unknown,
        Bit32,
        Bit64,
        Itanium64,
        Arm,
        Arm64
    }

    public static partial class Hardware
    {
        /// <summary>
        /// Retrieving Processor Id.
        /// </summary>
        /// <returns></returns>
        /// 
        public static String GetProcessorID()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String id = String.Empty;
            foreach (ManagementBaseObject o in moc)
            {
                ManagementObject mo = (ManagementObject) o;

                id = mo.Properties["processorID"].Value.ToString();
                break;
            }

            return id;
        }

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        public static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        public static ProcessorArchitecture GetProcessorBits()
        {
            ProcessorArchitecture pbits = ProcessorArchitecture.Unknown;

            try
            {
                SYSTEM_INFO lSystemInfo = new SYSTEM_INFO();
                GetNativeSystemInfo(ref lSystemInfo);

                pbits = lSystemInfo.uProcessorInfo.wProcessorArchitecture switch
                {
                    12 => ProcessorArchitecture.Arm64, // ARM64
                    9 => ProcessorArchitecture.Bit64, // AMD64
                    6 => ProcessorArchitecture.Itanium64, // IA64
                    5 => ProcessorArchitecture.Arm, // ARM
                    0 => ProcessorArchitecture.Bit32, // Intel
                    _ => ProcessorArchitecture.Unknown
                };
            }
            catch
            {
                // Ignore        
            }

            return pbits;
        }

        /// <summary>
        /// Retrieving HDD Serial Number.
        /// </summary>
        /// <returns></returns>
        public static String GetHDDSerialNumber()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            String result = String.Empty;
            foreach (ManagementBaseObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }

            return result;
        }

        /// <summary>
        /// Retrieving System MAC Address.
        /// </summary>
        /// <returns></returns>
        public static String GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            String macAddress = String.Empty;
            foreach (ManagementBaseObject o in moc)
            {
                ManagementObject mo = (ManagementObject) o;
                if (macAddress == String.Empty)
                {
                    if ((Boolean) mo["IPEnabled"])
                    {
                        macAddress = mo["MacAddress"].ToString();
                    }
                }

                mo.Dispose();
            }

            macAddress = macAddress.Replace(":", String.Empty);
            return macAddress;
        }

        /// <summary>
        /// Retrieving Motherboard Manufacturer.
        /// </summary>
        /// <returns></returns>
        public static String GetBoardMaker()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementBaseObject o in searcher.Get())
            {
                ManagementObject wmi = (ManagementObject) o;
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "Board Maker: Unknown";
        }

        /// <summary>
        /// Retrieving Motherboard Product Id.
        /// </summary>
        /// <returns></returns>
        public static String GetBoardProductID()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Product").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "Product: Unknown";
        }

        /// <summary>
        /// Retrieving CD-DVD Drive Path.
        /// </summary>
        /// <returns></returns>
        public static String GetCDRomDrive()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Drive").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "CD ROM Drive Letter: Unknown";
        }

        /// <summary>
        /// Retrieving BIOS Maker.
        /// </summary>
        /// <returns></returns>
        public static String GetBIOSMaker()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "BIOS Maker: Unknown";
        }

        /// <summary>
        /// Retrieving BIOS Serial Number.
        /// </summary>
        /// <returns></returns>
        public static String GetBIOSSerialNumber()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "BIOS Serial Number: Unknown";
        }

        /// <summary>
        /// Retrieving BIOS Caption.
        /// </summary>
        /// <returns></returns>
        public static String GetBIOSCaption()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Caption").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "BIOS Caption: Unknown";
        }

        /// <summary>
        /// Retrieving System Account Name.
        /// </summary>
        /// <returns></returns>
        public static String GetAccountName()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Name").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "User Account Name: Unknown";
        }

        /// <summary>
        /// Retrieving Physical RAM Memory in bytes.
        /// </summary>
        /// <returns></returns>
        public static Int64 GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            Int64 memSize = 0;

            // In case more than one Memory sticks are installed
            foreach (ManagementBaseObject obj in oCollection)
            {
                Int64 mCap = Convert.ToInt64(obj["Capacity"]);
                memSize += mCap;
            }

            return memSize;
        }

        /// <summary>
        /// Retrieving Number of RAM Slot on Motherboard.
        /// </summary>
        /// <returns></returns>
        public static Int32 GetRAMSlots()
        {
            Int32 memSlots = 0;
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
            ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
            ManagementObjectCollection oCollection2 = oSearcher2.Get();
            foreach (ManagementBaseObject obj in oCollection2)
            {
                memSlots = Convert.ToInt32(obj["MemoryDevices"]);
            }

            return memSlots;
        }

        //Get CPU Temprature.
        /// <summary>
        /// method for retrieving the CPU Manufacturer
        /// using the WMI class
        /// </summary>
        /// <returns>CPU Manufacturer</returns>
        public static String GetCPUManufacturer()
        {
            String cpuMan = String.Empty;
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementBaseObject obj in objCol)
            {
                if (cpuMan == String.Empty)
                {
                    // only return manufacturer from first CPU
                    cpuMan = obj.Properties["Manufacturer"].Value.ToString();
                }
            }

            return cpuMan;
        }

        /// <summary>
        /// method to retrieve the CPU's current
        /// clock speed using the WMI class
        /// </summary>
        /// <returns>Clock speed</returns>
        public static Int32 GetCPUCurrentClockSpeed()
        {
            Int32 cpuClockSpeed = 0;
            //create an instance of the Managemnet class with the
            //Win32_Processor class
            ManagementClass mgmt = new ManagementClass("Win32_Processor");
            //create a ManagementObjectCollection to loop through
            ManagementObjectCollection objCol = mgmt.GetInstances();
            //start our loop for all processors found
            foreach (ManagementBaseObject obj in objCol)
            {
                if (cpuClockSpeed == 0)
                {
                    // only return cpuStatus from first CPU
                    cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
                }
            }

            //return the status
            return cpuClockSpeed;
        }

        /// <summary>
        /// method to retrieve the network adapters
        /// default IP gateway using WMI
        /// </summary>
        /// <returns>adapters default IP gateway</returns>
        public static String GetDefaultIPGateway()
        {
            //create out management class object using the
            //Win32_NetworkAdapterConfiguration class to get the attributes
            //of the network adapter
            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //create our ManagementObjectCollection to get the attributes with
            ManagementObjectCollection objCol = mgmt.GetInstances();
            String gateway = String.Empty;
            //loop through all the objects we find
            foreach (ManagementBaseObject obj in objCol)
            {
                if (gateway == String.Empty) // only return MAC Address from first card
                {
                    //grab the value from the first network adapter we find
                    //you can change the string to an array and get all
                    //network adapters found as well
                    //check to see if the adapter's IPEnabled
                    //equals true
                    if ((Boolean) obj["IPEnabled"])
                    {
                        gateway = obj["DefaultIPGateway"].ToString();
                    }
                }

                //dispose of our object
                obj.Dispose();
            }

            //replace the ":" with an empty space, this could also
            //be removed if you wish
            gateway = gateway.Replace(":", String.Empty);
            //return the mac address
            return gateway;
        }

        /// <summary>
        /// Retrieve CPU Speed.
        /// </summary>
        /// <returns></returns>
        public static Double? GetCPUSpeedInGHz()
        {
            Double? gHz = null;
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                foreach (ManagementBaseObject mo in mc.GetInstances())
                {
                    gHz = 0.001 * (UInt32) mo.Properties["CurrentClockSpeed"].Value;
                    break;
                }
            }

            return gHz;
        }

        /// <summary>
        /// Retrieving Current Language
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentLanguage()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("CurrentLanguage").ToString();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "BIOS Maker: Unknown";
        }

        /// <summary>
        /// Retrieving Current Language.
        /// </summary>
        /// <returns></returns>
        public static String GetOSInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementBaseObject wmi in searcher.Get())
            {
                try
                {
                    return ((String) wmi["Caption"]).Trim() + ", " + (String) wmi["Version"] + ", " +
                           (String) wmi["OSArchitecture"];
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return "BIOS Maker: Unknown";
        }

        [DllImport("user32.dll")]
        private static extern Boolean EnumDisplaySettings(String lpszDeviceName, Int32 iModeNum, ref DEVMODE lpDevMode);

        private const Int32 EnumCurrentSettings = -1;

        public static Monitor GetMonitor(Int32 id)
        {
            return GetMonitor(Screen.AllScreens[id], id);
        }

        public static Monitor GetMonitor(Screen screen)
        {
            return GetMonitor(screen, Array.IndexOf(Screen.AllScreens, screen));
        }

        public static Monitor GetMonitor(Screen screen, Int32 customID)
        {
            DEVMODE dm = new DEVMODE {dmSize = (Int16) Marshal.SizeOf(typeof(DEVMODE))};
            EnumDisplaySettings(screen.DeviceName, EnumCurrentSettings, ref dm);
            return new Monitor(customID, screen.DeviceName, screen.Bounds, screen.WorkingArea, screen.Bounds, dm);
        }

        public static Monitor GetPrimaryMonitor()
        {
            return GetMonitor(Screen.PrimaryScreen, 0);
        }

        public static Monitor[] GetMonitors()
        {
            return Screen.AllScreens.Select(screen => GetMonitor(screen)).ToArray();
        }

        /// <summary>
        /// Retrieving Processor Information.
        /// </summary>
        /// <returns></returns>
        public static String GetProcessorInformation()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String info = String.Empty;
            foreach (ManagementBaseObject o in moc)
            {
                ManagementObject mo = (ManagementObject) o;
                String name = (String) mo["Name"];
                name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®")
                    .Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                info = name + ", " + (String) mo["Caption"] + ", " + (String) mo["SocketDesignation"];
                //mo.Properties["Name"].Value.ToString();
                //break;
            }

            return info;
        }

        /// <summary>
        /// Retrieving Computer Name.
        /// </summary>
        /// <returns></returns>
        public static String GetComputerName()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            String info = String.Empty;
            foreach (ManagementBaseObject o in moc)
            {
                ManagementObject mo = (ManagementObject) o;
                info = (String) mo["Name"];
                //mo.Properties["Name"].Value.ToString();
                //break;
            }

            return info;
        }
    }
}