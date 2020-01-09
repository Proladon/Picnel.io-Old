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
                    Properties.Settings.Default.Favorite_List = favorite_list;
                    Properties.Settings.Default.Favorite_Path_List = favorite_path_list;
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
                    
                    Properties.Settings.Default.Favorite_List = favorite_list;
                    GloableObject.logger(Properties.Settings.Default.Favorite_List.ToString());
                    Properties.Settings.Default.Favorite_Path_List = favorite_path_list;
                    Properties.Settings.Default.Save();
                }
                Window.GetWindow(this).Close();
                GloableObject.logger($"✔🤍 [Create Favorite] - Create Favorite {Favorite_Name.Text}.", "HighLight");

            }
        }
    }
}
