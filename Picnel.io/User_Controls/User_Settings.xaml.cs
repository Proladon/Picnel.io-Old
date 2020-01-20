using Picnel.io.Properties;
using Picnel.io.Settings_Pages;
using Picnel.io.User_Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Picnel.io.User_Controls
{
    /// <summary>
    /// User_Settings.xaml 的互動邏輯
    /// </summary>
    public partial class User_Settings : UserControl
    {
        public User_Settings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).DragMove();
        }


        internal void Change_Setting(Button name)
        {
            settings_frame.Children.Clear();
            List<Button> setting_list = new List<Button> { ui, logger, gif_viewer, image_viewer };
            foreach (Button setting_name in setting_list)
            {
                if (setting_name != name)
                {
                    setting_name.Background = Brushes.Gray;
                    setting_name.Foreground = Brushes.White;
                }
                else
                {
                    setting_name.Background = new SolidColorBrush(Color.FromRgb(95,253,224));
                    setting_name.Foreground = Brushes.Gray;
                }
            }

            if (name == logger)
            {
                Logger_Settings uc_name = new Logger_Settings();
                settings_frame.Children.Add(uc_name);
            }
            else if (name == gif_viewer)
            {
                GIF_Viewer_Settings uc_name = new GIF_Viewer_Settings();
                settings_frame.Children.Add(uc_name);
            }
            
        }
        
        // UI_settings
            // TODO Folder control size
            // TODO Background Color

        // gif_viewer_setting
        private void gif_viewer_Click(object sender, RoutedEventArgs e)
        {
            Change_Setting(gif_viewer);
        }

        //Logger
        private void logger_Click(object sender, RoutedEventArgs e)
        {
            Change_Setting(logger);
        }
        
        // Image_Viewer
        private void image_viewer_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Apply_Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
