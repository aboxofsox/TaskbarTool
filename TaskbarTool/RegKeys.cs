using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;

namespace TaskbarTool
{
    public class RegKeys
    {
    }

    public class RegKeyHandler
    { 
        public static void SetKeys(string pos, bool restartExplorer)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3");

            if (key != null)
            {
                byte[] data = (byte[])key.GetValue("Settings");
                string originalHex = BitConverter.ToString(data);
                string[] originalHexArray = originalHex.Split("-");

                string[] value = "30,00,00,00,fe,ff,ff,ff,7a,f4,00,00,03,00,00,00,30,00,00,00,30,00,00,00,00,00,00,00,00,00,00,00,00,0a,00,00,30,00,00,00,60,00,00,00,01,00,00,00".Split(",");
                string[] hex = new string[value.Length];
                byte[] newHex = new byte[value.Length];

                value[12] = pos;

                for (int i = 0; i < value.Length; i++)
                {
                    string h = "0x" + value[i];
                    hex[i] = h;
                }

                for (int i = 0; i < newHex.Length; i++)
                {
                    newHex[i] = Convert.ToByte(hex[i], 16);
                }

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3", "Settings", newHex);

                var explorer = Process.GetProcessesByName("explorer")[0];

                if (restartExplorer == true)
                {
                    explorer.Kill();
                }
            }
        }
    }


}
