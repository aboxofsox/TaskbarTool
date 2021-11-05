using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskbarTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PositionBox.Items.Add("-- Select Position --");
            PositionBox.SelectedIndex = 0;


            PositionBox.Items.Add("Top");
            PositionBox.Items.Add("Left");
            PositionBox.Items.Add("Right");
            PositionBox.Items.Add("Bottom");

            TaskbarSizeBox.Items.Add("-- Taskbar Size --");
            TaskbarSizeBox.SelectedIndex = 0;

            TaskbarSizeBox.Items.Add("Small");
            TaskbarSizeBox.Items.Add("Default");
            TaskbarSizeBox.Items.Add("Large");

        }

        private void SetTaskBar_Click(object sender, RoutedEventArgs e)
        {
            string Pos = "";
            int Size = 1;
            bool unlock = false;

            bool setAll = false;

            if (SetAllDisplays.IsChecked == true)
            {
                setAll = true;
            }
            else
            {
                setAll = false;
            }

            switch (PositionBox.Text.ToLower())
            {
                case "top":
                    Pos = "01";
                    break;
                case "left":
                    Pos = "00";
                    break;
                case "right":
                    Pos = "02";
                    break;
                case "bottom":
                    Pos = "03";
                    break;
                default:
                    Pos = "03";
                    break;
            }


            switch (TaskbarSizeBox.Text.ToLower())
            {
                case "small":
                    Size = 0;
                    break;
                case "default":
                    Size = 1;
                    break;
                case "large":
                    Size = 2;
                    break;
                default:
                    Size = 1;
                    break;
            }

            if (UnlockTaskbar.IsChecked == true)
            {
                unlock = true;
            } 
            else
            {
                unlock = false;
            }
            Taskbar.SetPosition(Pos, true, setAll);
            Taskbar.SetSize(Size);
            Taskbar.Unlock(unlock);
        }

        private void BackupReg_Click(object sender, RoutedEventArgs e)
        {
            Taskbar.CreateBackup();
            MessageBox.Show("Registry keys were copied to your desktop.", "Backup Complete");
        }

        public void OpenControlSettings_Click(object sender, RoutedEventArgs e)
        {
            Taskbar.RunShell();
        }

        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
