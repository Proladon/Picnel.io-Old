using Newtonsoft.Json;
using Picnel.io.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinForms = System.Windows.Forms;
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
    /// Create_Favorite.xaml 的互動邏輯
    /// </summary>
    public partial class Create_Favorite : UserControl
    {
        public Create_Favorite()
        {
            InitializeComponent();
        }

        private void Favorite_Create_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        // 創建新Favorite
        private void Favorite_Create_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Current_Favorite = Favorite_Name.Text;
            if(Favorite_Name.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Favorite Name");
                return;
            }
            else
            {
                if (Properties.Settings.Default.Favorite_List == null)
                {
                    // 存名稱
                    StringCollection favorite_list = new StringCollection();
                    favorite_list.Add(Favorite_Name.Text); // 加入新資料

                    // 存路徑
                    Dictionary<string, string> favorite_path = new Dictionary<string, string>();
                    favorite_path.Add(Favorite_Name.Text, GloableObject.curPath);
                    string jsonStr = JsonConvert.SerializeObject(favorite_path, Formatting.Indented);
                    StringCollection favorite_path_list = new StringCollection();
                    favorite_path_list.Add(jsonStr); // 加入新資料

                    // 存控件
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
                    favorite_controls.Add(Favorite_Name.Text, controls_list); //{"favor_name":{"control_x":"[color, aka, path]}}
                    string controls_jsonStr = JsonConvert.SerializeObject(favorite_controls, Formatting.Indented);
                    StringCollection favorite_controls_list = new StringCollection();
                    favorite_controls_list.Add(controls_jsonStr);

                    // 存檔
                    Properties.Settings.Default.Favorite_List = favorite_list;
                    Properties.Settings.Default.Favorite_Path_List = favorite_path_list;
                    Properties.Settings.Default.Favorite_Controls_List = favorite_controls_list;
                    Properties.Settings.Default.Save();
                }
                else 
                {
                    // 存名稱
                    StringCollection favorite_list = new StringCollection();
                    // 加回已存在的資料
                    foreach (string favorite in Properties.Settings.Default.Favorite_List)
                    {
                        favorite_list.Add(favorite);
                    }
                    favorite_list.Add(Favorite_Name.Text); // 加入新資料
                    GloableObject.logger(favorite_list.ToString());

                    // 存路徑
                    Dictionary<string, string> favorite_path = new Dictionary<string, string>();
                    favorite_path.Add(Favorite_Name.Text, GloableObject.curPath); //{"name":"path"}
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

                    // 存控件
                    Dictionary<string, Dictionary<string, List<string>>> favorite_controls = new Dictionary<string, Dictionary<string, List<string>>>();
                    Dictionary<string, List<string>> controls_list = new Dictionary<string, List<string>>();
                    // 加回已存在的資料
                    StringCollection favorite_controls_list = new StringCollection();
                    foreach (string name in Properties.Settings.Default.Favorite_Controls_List)
                    {
                        favorite_controls_list.Add(name);
                    }

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
                    favorite_controls.Add(Favorite_Name.Text, controls_list); //{"favor_name":{"control_x":"[color, aka, path]}}
                    string controls_jsonStr = JsonConvert.SerializeObject(favorite_controls, Formatting.Indented);
                    favorite_controls_list.Add(controls_jsonStr);

                    Properties.Settings.Default.Favorite_List = favorite_list;
                    Properties.Settings.Default.Favorite_Path_List = favorite_path_list;
                    Properties.Settings.Default.Favorite_Controls_List = favorite_controls_list;
                    Properties.Settings.Default.Save();
                }
                Window.GetWindow(this).Close();
                GloableObject.logger($"✔🤍 [Create Favorite] - Create Favorite {Favorite_Name.Text}.", "HighLight");

            }
        }
    }
}
