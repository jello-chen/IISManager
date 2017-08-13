using IISManager.Core.Domain;
using Microsoft.Win32;
using System;

namespace IISManager.Core.Helper
{
    public class IISHelper
    {
        public static bool IsIIS7AndHigher()
        {
            #region OS Version
            /*****************************************************************************
                Operating System             Version     PlatformID
                Windows 8                    6.2         VER_PLATFORM_WIN32_NT (=2)
                Windows 7                    6.1         VER_PLATFORM_WIN32_NT
                Windows Server 2008 R2       6.1         VER_PLATFORM_WIN32_NT
                Windows Server 2008          6.0         VER_PLATFORM_WIN32_NT
                Windows Vista                6.0         VER_PLATFORM_WIN32_NT
                Windows Server 2003 R2       5.2         VER_PLATFORM_WIN32_NT
                Windows Server 2003          5.2         VER_PLATFORM_WIN32_NT
                Windows XP 64-Bit Edition    5.2         VER_PLATFORM_WIN32_NT
                Windows XP                   5.1         VER_PLATFORM_WIN32_NT
                Windows 2000                 5.0         VER_PLATFORM_WIN32_NT
                Windows NT 4.0               4.0         VER_PLATFORM_WIN32_NT
                Windows NT 3.51              3.51 ?      VER_PLATFORM_WIN32_NT
                Windows Millennium Edition   4.90        VER_PLATFORM_WIN32_WINDOWS (=1)
                Windows 98                   4.10        VER_PLATFORM_WIN32_WINDOWS
                Windows 95                   4.0         VER_PLATFORM_WIN32_WINDOWS
                Windows 3.1                  3.1 ?       VER_PLATFORM_WIN32s (=0)
                *****************************************************************************/
            #endregion

            return Environment.OSVersion.Version.Major > 5;
        }

        public static IISVersion GetIISVersion()
        {
            int majorVersion = 0;
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\INetStp");
                if (key != null)
                    majorVersion = Convert.ToInt32(key.GetValue("MajorVersion"));
            }
            catch { }

            return majorVersion == 6 ? IISVersion.Six : (majorVersion > 6 ? IISVersion.SevenOrUpper : IISVersion.Unknown);
        }
    }
}
