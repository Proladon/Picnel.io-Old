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
using Microsoft.Win32; //FileDialog
using WinForms = System.Windows.Forms; //FolderDialog
using System.IO; //Folder, Directory
using System.Diagnostics; //Debug.WriteLine
using Picnel.io.Classes;
using WpfAnimatedGif;
using Picnel.io.User_Controls;
using System.Threading;

namespace Picnel.io
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            WpfAnimatedGif.ImageBehavior.SetAutoStart(gif_previewer, false);
        }

        private void Window_DragMove(object sender, MouseButtonEventArgs e)
        {
            try
            {
                mainGrid.Focus();
                this.DragMove();
            }
            catch (System.InvalidOperationException)
            {
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Next按鈕 Next Button
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GloableObject.img_filename == String.Empty)
            {
                GloableObject.logger($"❌🔎 [Error][Loading File] - No File Exsit.");
                return;
            }
            string path = targetFolder_path.Text.ToString();
            GloableObject.random_image(path);
            imgFileName.Text = GloableObject.img_filename;
        }

        private void chooseTargetFolder_Btn_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = folderDialog.ShowDialog();
            if (result == WinForms.DialogResult.Cancel)
            {
                return;
            }
            String sPath = folderDialog.SelectedPath;
            GloableObject.curPath = sPath;
            targetFolder_path.Text = sPath;
            targetFolder_path.ToolTip = sPath;

            GloableObject.random_image(sPath);
            GloableObject.logger($"✔⚙ [Set Main Directory] - Path: [ {sPath} ]");
        }

        // 新建目標資料夾 Create Target Control
        private void CreateControl_Click(object sender, RoutedEventArgs e)
        {
            CreateControl_Dialog test_create = new CreateControl_Dialog();
            Window newWin = new Window
            {
                Title = "Create test",
                Height = 300,
                Width = 600,
                Content = test_create,
                Topmost = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            newWin.ShowDialog();
        }

        // 重新命名 Rename
        private void imgFileName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GloableObject.preFileName = imgFileName.Text;
        }
        private void imgFileName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (imgFileName.Text != GloableObject.preFileName)
            {
                try
                {
                    System.IO.File.Move(GloableObject.img_path, GloableObject.curPath + '\\' + imgFileName.Text);
                    GloableObject.logger($"[Renmae File] {GloableObject.img_filename} > {imgFileName.Text}");
                    System.IO.File.Delete(GloableObject.img_filename);
                    GloableObject.img_path = GloableObject.curPath + '\\' + imgFileName.Text;
                    GloableObject.img_filename = imgFileName.Text;
                }
                catch (System.IO.IOException)
                {
                    GloableObject.logger($"❌⚠ [Error] [ReName File] - File Has Been Exsit.");
                }
                catch (System.ArgumentException)
                {
                    GloableObject.logger($"❌⚠ [Error] [ReName File] - Can Not Empty File Name.");
                }
                finally
                {
                    imgFileName.Text = GloableObject.img_filename;
                }
            }
        }

        // 刪除檔案 Delte File
        private void deleteImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete File Forever?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                if (File.Exists(GloableObject.img_path))
                {
                    System.IO.File.Delete(GloableObject.img_path);
                    GloableObject.logger($"♻🗑 [Delete File] - FileName: [ {GloableObject.img_filename} ]");
                    GloableObject.random_image(GloableObject.curPath);
                }
                else
                {
                    GloableObject.logger($"❌🗑 [Error] [Delete File] - Can't Not Delete, File Dosen't Exsit.");
                }
            }
        }

        // 視窗最大化 Windows Maximized
        private void winMaximized(object sender, RoutedEventArgs e)
        {
            if (GloableObject.winState == "Normal")
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                GloableObject.winState = "Maximized";
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                GloableObject.winState = "Normal";
            }
        }
        // 視窗最小化 Windows Minimized
        private void winMinimized(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        // 視窗透明度 Windows Opacity
        private void Opacity_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            String value = Convert.ToString(Convert.ToInt32(Opacity_slider.Value * 100)) + '%';
            Opacity_label.Text = value;
        }

        // 使用者設定按鈕 Settings Button
        private void settings_Btn_Click(object sender, RoutedEventArgs e)
        {
            User_Settings user_settings = new User_Settings();
            Window newWin = new Window
            {
                Title = "Create test",
                Height = 500,
                Width = 600,
                Content = user_settings,
                Topmost = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            newWin.ShowDialog();
        }

        // 開啟檔案位置按鈕 Open File Location
        private void openImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(GloableObject.img_path))
            {
                Process.Start("explorer.exe", "/select, " + GloableObject.img_path);
            }
            else
            {
                GloableObject.logger($"❌📤 [Error] [Open File Location] - Can't Not Open, File Dosen't Exsit.");
            }
        }

        // 最愛路徑 Favorite Path
        private void favorit_Btn_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("Sorry, Function Not Yet Complete.");
            Favorite_Setting user_settings = new Favorite_Setting();
            Window newWin = new Window
            {
                Height = 500,
                Width = 400,
                Content = user_settings,
                Topmost = true,
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            newWin.ShowDialog();
        }

        // DarkMode 切換
        private void darkMode_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush dark = new SolidColorBrush(Color.FromRgb(37, 42, 51));
            if (GloableObject.darkmode == true)
            {
                mainGrid.Background = Brushes.White;
                controlPanel_Splitter.Background = Brushes.White;
                targetFolder_path.Foreground = dark;
                Opacity_label.Foreground = dark;
                version_label.Foreground = dark;
                logger_label.Foreground = dark;
                GloableObject.darkmode = false;
            }
            else
            {
                mainGrid.Background = dark;
                controlPanel_Splitter.Background = dark;
                targetFolder_path.Foreground = Brushes.White;
                Opacity_label.Foreground = Brushes.White;
                version_label.Foreground = Brushes.White;
                logger_label.Foreground = Brushes.White;
                GloableObject.darkmode = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("https://github.com/Proladon/Image_Classifier_WPF");
        }



        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Opacity = 0.8;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void copyBtn_LostFocus(object sender, RoutedEventArgs e)
        {
            copyBtn.IsChecked = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (GloableObject.img_path == String.Empty)
            {
                GloableObject.logger($"❌📑 [Error] [Copy File Path] - No File Exsit.");
            }
            else
            {
                Clipboard.SetText(GloableObject.img_path);
                GloableObject.logger($"✔📑 [Error] [Copy File Path] - Copy [ {GloableObject.img_path} ] To ClipBoard.");
                copyBtn.IsChecked = false;
            }
        }
        
        private void copy_img_Click(object sender, RoutedEventArgs e)
        {
            if (GloableObject.img == null)
            {
                copyBtn.IsChecked = false;
                GloableObject.logger("❌📑 [Error] [Copy Image] - No Image Exsit.");
                return;
            }
            else
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(GloableObject.img_path);
                image.EndInit();
                Clipboard.SetImage(image);
                GC.Collect();
                GloableObject.logger($"✔📑 [Copy Image] - Copy [ {GloableObject.img_filename} ] To ClipBoard.");
                copyBtn.IsChecked = false;
            }
        }


        // 至頂視窗 Most On Top
        private void topmose_Btn_Checked(object sender, RoutedEventArgs e)
        {
            appWindow.Topmost = true;
        }
        private void topmose_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            appWindow.Topmost = false;
        }

        /* Button 版本
        private void topmostBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush dark = new SolidColorBrush(Color.FromRgb(37, 42, 51));
            if (GloableObject.topmost == false)
            {
                appWindow.Topmost = true;
                GloableObject.topmost = true;
                topmose_Btn.Style = this.FindResource("ontop") as Style;
            }
            else
            {
                appWindow.Topmost = false;
                GloableObject.topmost = false;
                topmose_Btn.Style = this.FindResource("DefaultBtn") as Style;
            }
        }
        */

    }
}
