using Newtonsoft.Json;
using Picnel.io.Classes;
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
            if (Properties.Settings.Default.Current_Favorite == "None")
            {
                create_new_btn.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).DragMove();
        }

        // 儲存使用者Favorite設定 Save Current Favorite
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 當前無使用Favorite
            if (Properties.Settings.Default.Current_Favorite == "None")
            {
                Window.GetWindow(this).Close();
                Create_Favorite create_favorite = new Create_Favorite();
                Window newWin = new Window
                {
                    Height = 160,
                    Width = 400,
                    Content = create_favorite,
                    Topmost = true,
                    WindowStyle = WindowStyle.None,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                newWin.ShowDialog();

            }

            // 當前正使用Favorite
            if (Properties.Settings.Default.Current_Favorite != "None")
            {
                // 刪除原來路徑
                string target_path = string.Empty;
                foreach (string path in Properties.Settings.Default.Favorite_Path_List)
                {
                    string[] ary = path.Split(':', '"');
                    string key = ary[1].ToString();
                    if (key == Properties.Settings.Default.Current_Favorite) { target_path = path; }
                }
                Properties.Settings.Default.Favorite_Path_List.Remove(target_path);

                // 存取更新路徑
                Dictionary<string, string> favorite_path = new Dictionary<string, string>();
                favorite_path.Add(Properties.Settings.Default.Current_Favorite, GloableObject.curPath); //{"name":"path"}
                string path_jsonStr = JsonConvert.SerializeObject(favorite_path, Formatting.Indented);

                StringCollection favorite_path_list = new StringCollection();
                // 加回已存在的資料
                if (Properties.Settings.Default.Favorite_Path_List != null)
                {
                    foreach (string path in Properties.Settings.Default.Favorite_Path_List)
                    {
                        favorite_path_list.Add(path);
                    }
                }
                favorite_path_list.Add(path_jsonStr); // 加入新資料

                //TODO 更新控件
                // 刪除目前的
                StringCollection favorite_controls_list = new StringCollection();
                string target_controls = string.Empty;
                foreach (string name in Properties.Settings.Default.Favorite_Controls_List)
                {
                    string[] ary = name.Split(':', '"');
                    string key = ary[1].ToString();
                    if (key == Properties.Settings.Default.Current_Favorite) { target_path = name; }
                    favorite_controls_list.Add(name);
                }
                favorite_controls_list.Remove(target_path); // 刪除目標

                Dictionary<string, Dictionary<string, List<string>>> favorite_controls = new Dictionary<string, Dictionary<string, List<string>>>();
                Dictionary<string, List<string>> controls_list = new Dictionary<string, List<string>>();

                int controls_counter = 0;
                foreach (Folder_Control control in GloableObject.mainWin.control_panel.Children)
                {
                    string control_num = "control_" + controls_counter.ToString();
                    List<string> control_data = new List<string>();

                    //顏色
                    string color = control.colorTag.Background.ToString();
                    control_data.Add(color);
                    //aka
                    string aka = control.akaLabel.Text;
                    control_data.Add(aka);
                    //path
                    string path = control.folderPath.Text;
                    control_data.Add(path);

                    controls_list.Add(control_num, control_data); //{"control_x":"[color, aka, path]"}

                    controls_counter += 1;
                }
                favorite_controls.Add(Properties.Settings.Default.Current_Favorite, controls_list); //{"favor_name":{"control_x":"[color, aka, path]}}
                string controls_jsonStr = JsonConvert.SerializeObject(favorite_controls, Formatting.Indented);
                favorite_controls_list.Add(controls_jsonStr);

                Properties.Settings.Default.Favorite_Controls_List = favorite_controls_list;
                Properties.Settings.Default.Favorite_Path_List = favorite_path_list;
                Window.GetWindow(this).Close();
                GloableObject.logger($"✔🤍 [Save Favorite] - Success Save Favorite {Properties.Settings.Default.Current_Favorite}.", "HighLight");
                Properties.Settings.Default.Save();
            }
        }

        private void create_new_btn_Click(object sender, RoutedEventArgs e)
        {
            Create_Favorite create_favorite = new Create_Favorite();
            Window newWin = new Window
            {
                Height = 160,
                Width = 400,
                Content = create_favorite,
                Topmost = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            Window.GetWindow(this).Close();
            newWin.ShowDialog();
            
        }
    }
}
