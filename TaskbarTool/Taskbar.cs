using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Web;


namespace TaskbarTool
{
    public class KeyData
    {
        public static readonly string Path = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3";
        public static readonly string ShortPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3";
        public static readonly string MMPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\MMStuckRects3";
        public static readonly string MMShortPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\MMStuckRects3";
        public static readonly string AdvancedPath = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced";
        public static readonly string ShellPackagePath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Shell\\Update\\Packages";
        public static readonly string ShellPackageShortPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Shell\\Update\\Packages";
        public static readonly string Destination = "C:\\Users\\" + Environment.UserName + "\\Desktop";
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

        public static void Unlock(bool option)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(KeyData.ShellPackageShortPath);
            Registry.SetValue(KeyData.ShellPackagePath, "UndockingDisabled", 0);
            
            switch (option)
            {
                case true:
                    Registry.SetValue(KeyData.ShellPackagePath, "UndockingDisabled", 1);
                    Taskbar.RunShell();
                    break;
                case false:
                    Registry.SetValue(KeyData.ShellPackagePath, "UndockingDisabled", 0);
                    break;
            }
            
        }

        public static void RunShell()
        {
            using (Process cmd = new())
            {
                try
                {
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.RedirectStandardError = true;
                    cmd.StartInfo.Arguments = "/C start shell:::{05d7b0f4-2121-4eff-bf6b-ed3f69b894d9}";
                    cmd.Start();

                    string stdout = cmd.StandardOutput.ReadToEnd();
                    string stderr = cmd.StandardError.ReadToEnd();

                    cmd.WaitForExit();


                }
                catch (Exception e)
                {
                    throw new Exception("Unable to start shell.", e);
                }
                finally
                {
                    Trace.WriteLine("Shell has started");
                }
            }
        }

        public static void CreateBackup()
        {

            using (Process process = new())
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
                    throw new Exception("Unable to make a copy of the registry keys.", e);
                }
            }

            using (Process process = new())
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
                    throw new Exception("Unable to make a copy of the registry keys.", e);
                }
            }

        }

    }
}
