using System;
using Microsoft.Win32;
using System.Diagnostics;


namespace TaskbarTool
{
    // Needs refactoring
    public class MultiDisplays
    {
        public static void Set(byte[] bytes)
        {
            string path = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\MMStuckRects3";
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\MMStuckRects3");

            if (key != null)
            {
                string[] valueNames = key.GetValueNames();

                foreach (string name in valueNames)
                {
                    Registry.SetValue(path, name, bytes);
                }
            }
            else
            {
                throw new Exception("Registry key could not be found.");
            }
        }
    }
    


    public class RegKeyHandler
    { 
        public static void SetKeys(string pos, bool restartExplorer, bool setAll)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3");

            // Needs refactoring
            if (key != null)
            {
                byte[] data = (byte[])key.GetValue("Settings");
                string originalHex = BitConverter.ToString(data);
                string[] originalHexArray = originalHex.Split("-");

                string[] value = "30,00,00,00,fe,ff,ff,ff,7a,f4,00,00,03,00,00,00,30,00,00,00,30,00,00,00,00,00,00,00,00,00,00,00,00,0a,00,00,30,00,00,00,60,00,00,00,01,00,00,00".Split(",");
                byte[] newHex = new byte[value.Length];

                value[12] = pos;

                for (int i = 0; i < newHex.Length; i++)
                {
                    newHex[i] = Convert.ToByte(value[i], 16);
                }

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3", "Settings", newHex);

                var explorer = Process.GetProcessesByName("explorer")[0];

                if (restartExplorer == true)
                {
                    explorer.Kill();
                }

                if (setAll == true)
                {
                    MultiDisplays.Set(newHex);
                }
            }

            
        }

        public static void SetTaskbarSize(int size)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced");

            string newKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced";
            Registry.SetValue(newKey, "TaskbarSi", size);
        }

          
        public static void CreateBackup()
        {
            string single = "StuckRects3";
            string multi = "MMStuckRects3";
            string singleDisplayPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\" + single;
            string multiDisplayPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\" + multi;
            string destinationPath = "C:\\Users\\" + Environment.UserName + "\\Desktop";

            using (Process process = new Process())
            {
                try
                {
                    process.StartInfo.FileName = "reg.exe";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.Arguments = "export \"" + singleDisplayPath + "\" \"" + destinationPath + "\\StuckRects3.reg" + "\" /y";
                    process.Start();
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to make a copy of the registry keys.");
                }
            }

            using (Process process = new Process())
            {
                try
                {
                    process.StartInfo.FileName = "reg.exe";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.Arguments = "export \"" + multiDisplayPath + "\" \"" + destinationPath + "\\MMStuckRects3.reg" + "\" /y";
                    process.Start();
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to make a copy of the registry keys");
                }
            }

        }

    }
}
