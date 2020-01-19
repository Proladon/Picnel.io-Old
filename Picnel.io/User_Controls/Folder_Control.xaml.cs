using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Picnel.io.Classes;
using WpfAnimatedGif;

namespace Picnel.io.User_Controls
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
        // 刪除控件
        private void delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((Panel)this.Parent).Children.Remove(this);
        }

        // 移動(剪下/貼上) 操作
        private void moveTo_Btn_Click(object sender, RoutedEventArgs e)
        {
            String newPath = folderPath.Text.ToString();
            GloableObject.moveTo(GloableObject.img_path, newPath);
        }

        // 複製操作
        private void copyTo_Btn_Click(object sender, RoutedEventArgs e)
        {
            String newPath = folderPath.Text.ToString();
            GloableObject.copyTo(GloableObject.img_path, newPath);
        }

        // 開啟資料夾
        private void colorTag_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(this.folderPath.Text))
            {
                Process.Start("explorer.exe",this.folderPath.Text + '\\');
            }
            else
            {
                GloableObject.logger($"❌📤 [Error] [Open File Location] - Can't Not Open, File Dosen't Exsit.");
            }
        }

        // 重新編輯 Control
        private void edit_btn_Click(object sender, RoutedEventArgs e)
        {
            GloableObject.temp_control = this;
            Folder_Control_Edit control_edit = new Folder_Control_Edit();
            // color
            ColorConverter tagColor = new ColorConverter();
            control_edit.control_color.SelectedColor = (Color)tagColor.ConvertFrom(GloableObject.temp_control.colorTag.Background.ToString());
            control_edit.control_color.Background = GloableObject.temp_control.colorTag.Background;
            // Name
            control_edit.control_name.Text = GloableObject.temp_control.akaLabel.Text;
            // Path
            control_edit.control_path.Content = GloableObject.temp_control.folderPath.Text;

            Window control_edit_win = new Window()
            {
                Height = 150,
                Width = 400,
                Content = control_edit,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            control_edit_win.ShowDialog();
        }
    }
}
