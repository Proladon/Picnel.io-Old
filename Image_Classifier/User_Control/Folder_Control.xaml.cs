using System;
using System.Collections.Generic;
using System.IO;
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
using Image_Classifier.Classes;
using WpfAnimatedGif;

namespace Image_Classifier.User_Control
{
    /// <summary>
    /// Folder_Control.xaml 的互動邏輯
    /// </summary>
    public partial class Folder_Control : UserControl
    {
        public Folder_Control()
        {
            InitializeComponent();
        }
        private void delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);
        }

        private void moveTo_Btn_Click(object sender, RoutedEventArgs e)
        {
            String newPath = folderPath.Text.ToString();
            GloableOject.moveTo(GloableOject.img_path, newPath);
        }

        private void copyTo_Btn_Click(object sender, RoutedEventArgs e)
        {
            String newPath = folderPath.Text.ToString();
            GloableOject.copyTo(GloableOject.img_path, newPath );
        }
    }
}
