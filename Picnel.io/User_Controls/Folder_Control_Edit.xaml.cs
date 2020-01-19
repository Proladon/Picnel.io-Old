using Picnel.io.Classes;
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
using Xceed.Wpf.Toolkit;
using WinForms = System.Windows.Forms;

namespace Picnel.io.User_Controls
{
    /// <summary>
    /// Folder_Control_Edit.xaml 的互動邏輯
    /// </summary>
    public partial class Folder_Control_Edit : UserControl
    {
        public Folder_Control_Edit()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).DragMove();
        }

        // 更改資料夾路徑 Edit Path
        private void edit_path_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.ShowNewFolderButton = true;
            folderDialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderDialog.SelectedPath = GloableObject.lastPath;
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result == WinForms.DialogResult.Cancel)
            {
                return;
            }
            String sPath = folderDialog.SelectedPath;
            control_path.Content = sPath;
            GloableObject.lastPath = sPath;
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            // Update Color
            GloableObject.temp_control.colorTag.Background = this.control_color.Background;
            // Update Name
            GloableObject.temp_control.akaLabel.Text = this.control_name.Text;
            // Update Path
            GloableObject.temp_control.folderPath.Text = control_path.Content.ToString();
            GloableObject.temp_control.ToolTip = control_path.Content;

            // 清除 temp_control 並垃圾回收
            GloableObject.temp_control = null;
            GC.Collect();
            Window.GetWindow(this).Close();

        }

        // 取消變更 並垃圾回收
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            GloableObject.temp_control = null;
            GC.Collect();
            Window.GetWindow(this).Close();
        }

        // ColorPicker Background 同步
        private void control_color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            BrushConverter converter = new BrushConverter();
            this.control_color.Background = (Brush)converter.ConvertFrom(this.control_color.SelectedColor.ToString());
        }


    }
}
