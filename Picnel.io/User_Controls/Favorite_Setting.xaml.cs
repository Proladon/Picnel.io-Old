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

namespace Picnel.io.User_Controls
{
    /// <summary>
    /// Favorite_Setting.xaml 的互動邏輯
    /// </summary>
    public partial class Favorite_Setting : UserControl
    {
        public Favorite_Setting()
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Save Current Favorite
            Create_Favorite create_favorite = new Create_Favorite();
            Window newWin = new Window
            {
                Height = 200,
                Width = 400,
                Content = create_favorite,
                Topmost = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            newWin.ShowDialog();
            Favorite favorite = new Favorite();
            favorite.Height = 60;
            favorite_panel.Children.Add(favorite);
        }
    }
}
