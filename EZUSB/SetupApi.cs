/* ----------------------------------------------------------
文件名称：SetupApi.cs

作者：秦建辉

MSN：splashcn@msn.com
QQ：36748897

博客：http://blog.csdn.net/jhqin

开发环境：
    Visual Studio V2010
    .NET Framework 4 Client Profile

版本历史：
    V1.0	2011年09月05日
			实现对setupapi.dll接口的PInvoke

参考资料：
    http://www.pinvoke.net/
------------------------------------------------------------ */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Splash.IO.PORTS
{    
    /// <summary>
    /// SetupApi.dll的PInvoke
    /// </summary>
    public partial class USB
    {
        #region ENUM
        [Flags]
        private enum DIGCF
        {
            DIGCF_DEFAULT = 0x00000001,
            DIGCF_PRESENT = 0x00000002,
            DIGCF_ALLCLASSES = 0x00000004,          // 设备安装类
            DIGCF_PROFILE = 0x00000008,
            DIGCF_DEVICEINTERFACE = 0x00000010,     // 设备接口类
        }
        #endregion

        #region STRUCT
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public static readonly SP_DEVINFO_DATA Empty = new SP_DEVINFO_DATA(Marshal.SizeOf(typeof(SP_DEVINFO_DATA)));
            public UInt32 cbSize;
            public Guid ClassGuid;
            public UInt32 DevInst;
            public IntPtr Reserved;

            private SP_DEVINFO_DATA(int size)
            {
                cbSize = (UInt32)size;
                ClassGuid = Guid.Empty;
                DevInst = 0;
                Reserved = IntPtr.Zero;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public static readonly SP_DEVICE_INTERFACE_DATA Empty = new SP_DEVICE_INTERFACE_DATA(Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA)));
            public UInt32 cbSize;
            public Guid InterfaceClassGuid;
            public UInt32 Flags;
            public UIntPtr Reserved;

            private SP_DEVICE_INTERFACE_DATA(int size)
            {
                cbSize = (uint)size;
                InterfaceClassGuid = Guid.Empty;
                Flags = 0;
                Reserved = UIntPtr.Zero;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String DevicePath;
        }
        #endregion

        #region API
        #region SetupDiGetClassDevs
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPTStr)] String Enumerator,
            IntPtr hwndParent,
            DIGCF Flags
            );        

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevs(
            IntPtr ClassGuid,       // null 
            String Enumerator,
            IntPtr hwndParent,
            DIGCF Flags
            );
        #endregion

        #region SetupDiGetClassDevsEx
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevsEx(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPTStr)] String Enumerator,
            IntPtr hwndParent,
            DIGCF Flags,
            IntPtr DeviceInfoSet,
            [MarshalAs(UnmanagedType.LPTStr)] String MachineName,
            IntPtr Reserved
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevsEx(
            IntPtr ClassGuid,
            [MarshalAs(UnmanagedType.LPTStr)] String Enumerator,
            IntPtr hwndParent,
            DIGCF Flags,
            IntPtr DeviceInfoSet,
            [MarshalAs(UnmanagedType.LPTStr)] String MachineName,
            IntPtr Reserved
            );           
        #endregion

        #region SetupDiEnumDeviceInterfaces
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            ref SP_DEVINFO_DATA devInfo,
            ref Guid interfaceClassGuid,
            UInt32 memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            [MarshalAs(UnmanagedType.AsAny)] Object devInfo,
            ref Guid interfaceClassGuid,
            UInt32 memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
            );
        #endregion

        #region SetupDiGetDeviceInterfaceDetail
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            Int32 deviceInterfaceDetailDataSize,
            out Int32 requiredSize,
            ref SP_DEVINFO_DATA deviceInfoData
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            Int32 deviceInterfaceDetailDataSize,
            out Int32 requiredSize,
            [MarshalAs(UnmanagedType.AsAny)] object deviceInfoData      // null
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            Int32 deviceInterfaceDetailDataSize,
            IntPtr requiredSize,                                        // null
            [MarshalAs(UnmanagedType.AsAny)] object deviceInfoData      // null
            );
        #endregion

        #region SetupDiCreateDeviceInfoList
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiCreateDeviceInfoList(ref Guid ClassGuid, IntPtr hwndParent);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiCreateDeviceInfoList(IntPtr ClassGuid, IntPtr hwndParent);
        #endregion

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, UInt32 MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

        #region SetupDiGetDeviceInstanceId
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInstanceId(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            StringBuilder DeviceInstanceId,
            Int32 DeviceInstanceIdSize,
            out Int32 RequiredSize
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInstanceId(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            StringBuilder DeviceInstanceId,
            Int32 DeviceInstanceIdSize,
            IntPtr RequiredSize
            );
        #endregion

        #region SetupDiDeviceOperator
        //        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, UInt32 Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, UInt32 iEnumerator, IntPtr hParent, UInt32 nFlags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr lpInfoSet, UInt32 dwIndex, SP_DEVINFO_DATA devInfoData);
//
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr lpInfoSet, SP_DEVINFO_DATA DeviceInfoData, UInt32 Property, UInt32 PropertyRegDataType, StringBuilder PropertyBuffer, UInt32 PropertyBufferSize, IntPtr RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiSetClassInstallParams(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, IntPtr ClassInstallParams, int ClassInstallParamsSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiCallClassInstaller(UInt32 InstallFunction, IntPtr DeviceInfoSet, IntPtr DeviceInfoData);
        #endregion

        //PARMS
        public const int DIGCF_ALLCLASSES = (0x00000004);
        public const int DIGCF_PRESENT = (0x00000002);
        public const int INVALID_HANDLE_VALUE = -1;
        public const int SPDRP_DEVICEDESC = (0x00000000);
        public const int MAX_DEV_LEN = 1000;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = (0x00000001);
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = (0x00000004);
        public const int DBT_DEVTYP_DEVICEINTERFACE = (0x00000005);
        public const int DBT_DEVNODES_CHANGED = (0x0007);
        public const int WM_DEVICECHANGE = (0x0219);
        public const int DIF_PROPERTYCHANGE = (0x00000012);
        public const int DICS_FLAG_GLOBAL = (0x00000001);
        public const int DICS_FLAG_CONFIGSPECIFIC = (0x00000002);
        public const int DICS_ENABLE = (0x00000001);
        public const int DICS_DISABLE = (0x00000002);

        [StructLayout(LayoutKind.Sequential)]
        public class SP_CLASSINSTALL_HEADER
        {
            public int cbSize;
            public int InstallFunction;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
            public int StateChange;
            public int Scope;
            public int HwProfile;
        };
        //Name:     SetDeviceState
        //Inputs:   string[],bool
        //Outputs:  bool
        //Errors:   This method may throw the following exceptions.
        //          Failed to enumerate device tree!
        //Remarks:  This is nearly identical to the method above except it
        //          tries to match the hardware description against the criteria
        //          passed in.  If a match is found, that device will the be
        //          enabled or disabled based on bEnable.
        public bool SetDeviceState(string[] match, bool bEnable)
        {
            try
            {
                Guid myGUID = System.Guid.Empty;
                IntPtr hDevInfo = SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, DIGCF_ALLCLASSES | DIGCF_PRESENT);
                if (hDevInfo.ToInt32() == INVALID_HANDLE_VALUE)
                {
                    return false;
                }
                SP_DEVINFO_DATA DeviceInfoData;
                DeviceInfoData = new SP_DEVINFO_DATA();
                DeviceInfoData.cbSize = 28;
                //is devices exist for class
                DeviceInfoData.DevInst = 0;
                DeviceInfoData.ClassGuid = System.Guid.Empty;
                DeviceInfoData.Reserved = IntPtr.Zero;
                UInt32 i;
                StringBuilder DeviceName = new StringBuilder("");
                DeviceName.Capacity = MAX_DEV_LEN;
                for (i = 0; SetupDiEnumDeviceInfo(hDevInfo, i,ref DeviceInfoData); i++)
                {
                    //Declare vars
                    while (!SetupDiGetDeviceRegistryProperty(hDevInfo,
                        DeviceInfoData,
                        SPDRP_DEVICEDESC,
                        0,
                        DeviceName,
                        MAX_DEV_LEN,
                        IntPtr.Zero))
                    {
                        //Skip
                    }
                    bool bMatch = true;
                    foreach (string search in match)
                    {
                        if (!DeviceName.ToString().ToLower().Contains(search.ToLower()))
                        {
                            bMatch = false;
                            break;
                        }
                    }
                    if (bMatch)
                    {
                        ChangeIt(hDevInfo, DeviceInfoData, bEnable);
                    }
                }
                SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to enumerate device tree!", ex);
            }
            return true;
        }
        //Name:     ChangeIt
        //Inputs:   pointer to hdev, SP_DEV_INFO, bool
        //Outputs:  bool
        //Errors:   This method may throw the following exceptions.
        //          Unable to change device state!
        //Remarks:  Attempts to enable or disable a device driver.  
        //          IMPORTANT NOTE!!!   This code currently does not check the reboot flag.
        //          =================   Some devices require you reboot the OS for the change
        //                              to take affect.  If this describes your device, you 
        //                              will need to look at the SDK call:
        //                              SetupDiGetDeviceInstallParams.  You can call it 
        //                              directly after ChangeIt to see whether or not you need 
        //                              to reboot the OS for you change to go into effect.
        private bool ChangeIt(IntPtr hDevInfo, SP_DEVINFO_DATA devInfoData, bool bEnable)
        {
            try
            {
                //Marshalling vars
                int szOfPcp;
                IntPtr ptrToPcp;
                int szDevInfoData;
                IntPtr ptrToDevInfoData;

                SP_PROPCHANGE_PARAMS pcp = new SP_PROPCHANGE_PARAMS();
                if (bEnable)
                {
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
                    pcp.StateChange = DICS_ENABLE;
                    pcp.Scope = DICS_FLAG_GLOBAL;
                    pcp.HwProfile = 0;

                    //Marshal the params
                    szOfPcp = Marshal.SizeOf(pcp);
                    ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                    Marshal.StructureToPtr(pcp, ptrToPcp, true);
                    szDevInfoData = Marshal.SizeOf(devInfoData);
                    ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);

                    if (SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(SP_PROPCHANGE_PARAMS))))
                    {
                        SetupDiCallClassInstaller(DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                    }
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
                    pcp.StateChange = DICS_ENABLE;
                    pcp.Scope = DICS_FLAG_CONFIGSPECIFIC;
                    pcp.HwProfile = 0;
                }
                else
                {
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
                    pcp.StateChange = DICS_DISABLE;
                    pcp.Scope = DICS_FLAG_CONFIGSPECIFIC;
                    pcp.HwProfile = 0;
                }
                //Marshal the params
                szOfPcp = Marshal.SizeOf(pcp);
                ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                Marshal.StructureToPtr(pcp, ptrToPcp, true);
                szDevInfoData = Marshal.SizeOf(devInfoData);
                ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);
                Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, true);

                bool rslt1 = SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(SP_PROPCHANGE_PARAMS)));
                bool rstl2 = SetupDiCallClassInstaller(DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                if ((!rslt1) || (!rstl2))
                {
                    throw new Exception("Unable to change device state!");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
