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

        }

        private void SetTaskBar_Click(object sender, RoutedEventArgs e)
        {
            string Pos = "";

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

            string message = "Moved taskbar to " + Pos;

            RegKeyHandler.SetKeys(Pos, true);
        }
    }
}
