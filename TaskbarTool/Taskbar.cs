using System;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;


namespace TaskbarTool
{
    // Needs refactoring

    public class KeyData
    {
        public static string Path = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3";
        public static string ShortPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3";
        public static string MMPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\MMStuckRects3";
        public static string MMShortPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\MMStuckRects3";
        public static string AdvancedPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced";
        public static string Destination = "C:\\Users\\" + Environment.UserName + "\\Desktop";
    }

    public class TaskbarMultiDisplay
    {
        public static void Set(byte[] bytes)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyData.MMShortPath);

            if (key != null)
            {
                string[] valueNames = key.GetValueNames();

                foreach (string name in valueNames)
                {
                    Registry.SetValue(KeyData.MMPath, name, bytes);
                }
            }
            else
            {
                throw new Exception("Registry key could not be found.");
            }
        }
    }
    
    public class Taskbar
    { 
        public static void SetPosition(string pos, bool restartExplorer, bool setAll)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyData.ShortPath);

            if (key != null)
            {
                byte[] keyValue = (byte[])key.GetValue("Settings");
                string[] value = BitConverter.ToString(keyValue).Split("-");

                byte[] newHex = new byte[value.Length];

                value[12] = pos;

                for (int i = 0; i < newHex.Length; i++)
                {
                    newHex[i] = Convert.ToByte(value[i], 16);
                }

                Registry.SetValue(KeyData.Path, "Settings", newHex);

                var explorer = Process.GetProcessesByName("explorer")[0];

                if (restartExplorer == true)
                {
                    explorer.Kill();
                }

                if (setAll == true)
                {
                    TaskbarMultiDisplay.Set(newHex);
                }
            }

        }

        public static void SetSize(int size)
        {
            Registry.SetValue(KeyData.AdvancedPath, "TaskbarSi", size);
        }

          
        public static void CreateBackup()
        {

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
                    process.StartInfo.Arguments = "export \"" + KeyData.Path + "\" \"" + KeyData.Destination + "\\StuckRects3.reg" + "\" /y";
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
                    process.StartInfo.Arguments = "export \"" + KeyData.MMPath + "\" \"" + KeyData.Destination + "\\MMStuckRects3.reg" + "\" /y";
                    process.Start();
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    Application.Current.Shutdown();
                }
            }

        }

    }
}
