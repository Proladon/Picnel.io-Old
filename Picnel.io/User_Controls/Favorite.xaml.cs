using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Newtonsoft.Json;
using Picnel.io.Classes;

namespace Picnel.io.User_Controls
{
    /// <summary>
    /// Favorite.xaml 的互動邏輯
    /// </summary>
    public partial class Favorite : UserControl
    {
        public Favorite()
        {
            InitializeComponent();
        }

        // 刪除Favorite
        private void favorite_delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete File Forever?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                ((Panel)this.Parent).Children.Remove(this);
                // 名稱
                Properties.Settings.Default.Favorite_List.Remove(this.favorite_aka.Text);

                //路徑
                string target_path = string.Empty;
                foreach (string path in Properties.Settings.Default.Favorite_Path_List)
                {
                    string[] ary = path.Split(':', '"');
                    string key = ary[1].ToString();

                    if (key == this.favorite_aka.Text)
                    {
                        target_path = path;
                    }
                }
                Properties.Settings.Default.Favorite_Path_List.Remove(target_path);
                Properties.Settings.Default.Save();
            }
        }

        // 選擇Favorite
        private void Favorite_Click(object sender, RoutedEventArgs e)
        {

            GloableObject.mainWin.targetFolder_path.Text = this.favorite_path.Text;
            GloableObject.mainWin.targetFolder_path.ToolTip = this.favorite_path.Text;
            GloableObject.curPath = this.favorite_path.Text;
            GloableObject.random_image(GloableObject.curPath);
            Properties.Settings.Default.Current_Favorite = this.favorite_aka.Text;

            // 控件

            GloableObject.mainWin.control_panel.Children.Clear();

            foreach (string name in Properties.Settings.Default.Favorite_Controls_List)
            {
                string[] ary = name.Split(':', '"');
                string key = ary[1].ToString();
                // 解第一層dict
                if (key == this.favorite_aka.Text)
                {
                    // {"favorite_name":{"control_x":["color","aka","paht"], "control_x":["color","aka","paht"]}}
                    Dictionary<string, Dictionary<string, List<string>>> favorite_controls_list = (Dictionary<string, Dictionary<string, List<string>>>)JsonConvert.DeserializeObject(name, typeof(Dictionary<string, Dictionary<string, List<string>>>));
                    // "{control_x":["color","aka","paht"], "control_x":["color","aka","paht"]}
                    Dictionary<string, List<string>> controls = favorite_controls_list[key];
                    foreach (string control in controls.Keys)
                    {
                        List<string> data = controls[control];
                        Folder_Control folder_control = new Folder_Control();
                        // 顏色
                        BrushConverter tagColor = new BrushConverter(); // 轉換顏色
                        folder_control.colorTag.Background = (Brush)tagColor.ConvertFrom(data[0]);
                        // aka
                        folder_control.akaLabel.Text = data[1];
                        // path
                        folder_control.folderPath.Text = data[2];
                        folder_control.akaLabel.ToolTip = data[2];
                        folder_control.Height = 25;
                        GloableObject.mainWin.control_panel.Children.Add(folder_control);

                    }
                }    
            }
            GloableObject.logger($"✔🔄 [Switch Favorite] - Switch to Favorite {this.favorite_aka.Text}", "HighLight");
            Window.GetWindow(this.Parent).Close();
        }
    }
}
