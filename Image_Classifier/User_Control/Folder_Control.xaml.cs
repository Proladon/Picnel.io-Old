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
    }
}
