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
using Image_Classifier.Classes;
using WpfAnimatedGif;
using Image_Classifier.User_Control;
using System.Threading;

namespace Image_Classifier
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
            if (GloableOject.img_filename == String.Empty)
            {
                GloableOject.logger($"❌🔎 [Error][Loading File] - No File Exsit.");
                return;
            }
            string path = targetFolder_path.Text.ToString();
            GloableOject.random_image(path);
            imgFileName.Text = GloableOject.img_filename;
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
            GloableOject.curPath = sPath;
            targetFolder_path.Text = sPath;
            targetFolder_path.ToolTip = sPath;

            GloableOject.random_image(sPath);
            GloableOject.logger($"✔⚙ [Set Main Directory] - Path: [ {sPath} ]");
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
                WindowStyle = WindowStyle.None,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            newWin.ShowDialog();
        }
               
        // 重新命名 Rename
        private void imgFileName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GloableOject.preFileName = imgFileName.Text;
        }
        private void imgFileName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (imgFileName.Text != GloableOject.preFileName)
            {
                try
                {
                System.IO.File.Move(GloableOject.img_path, GloableOject.curPath+'\\'+imgFileName.Text);
                GloableOject.logger($"[Renmae File] {GloableOject.img_filename} > {imgFileName.Text}");
                System.IO.File.Delete(GloableOject.img_filename);
                GloableOject.img_path = GloableOject.curPath + '\\' + imgFileName.Text;
                GloableOject.img_filename = imgFileName.Text;
                }
                catch (System.IO.IOException)
                {
                    GloableOject.logger($"❌⚠ [Error] [ReName File] - File Has Been Exsit.");
                }
                catch (System.ArgumentException)
                {
                    GloableOject.logger($"❌⚠ [Error] [ReName File] - Can Not Empty File Name.");
                }
                finally
                {
                    imgFileName.Text = GloableOject.img_filename;
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
                if (File.Exists(GloableOject.img_path))
                {
                    System.IO.File.Delete(GloableOject.img_path);
                    GloableOject.logger($"♻🗑 [Delete File] - FileName: [ {GloableOject.img_filename} ]");
                    GloableOject.random_image(GloableOject.curPath);
                }
                else
                {
                    GloableOject.logger($"❌🗑 [Error] [Delete File] - Can't Not Delete, File Dosen't Exsit.");
                }
            }
        }

        // 視窗最大化 Windows Maximized
        private void winMaximized(object sender, RoutedEventArgs e)
        {
            if (GloableOject.winState == "Normal")
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                GloableOject.winState = "Maximized";
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                GloableOject.winState = "Normal";
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

        // 設定按鈕 Settings Button
        private void settings_Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry, Not Yet Complete.");
        }

        // 開啟檔案位置按鈕 Open File Location
        private void openImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(GloableOject.img_path))
            {
                Process.Start("explorer.exe", "/select, " + GloableOject.img_path);
            }
            else
            {
                GloableOject.logger($"❌📤 [Error] [Open File Location] - Can't Not Open, File Dosen't Exsit.");
            }
        }

        private void favorit_Btn_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("Sorry, Function Not Yet Complete.");
            GloableOject.logger(img_row_1.Height.ToString());
            GloableOject.logger(img_row_2.Height.ToString());
            GloableOject.logger(img_row_3.Height.ToString());

        }

        // DarkMode 切換
        private void darkMode_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush dark = new SolidColorBrush(Color.FromRgb(37, 42, 51));
            if (GloableOject.darkmode == true)
            {
                mainGrid.Background = Brushes.White;
                controlPanel_Splitter.Background = Brushes.White;
                targetFolder_path.Foreground = dark;
                Opacity_label.Foreground = dark;
                version_label.Foreground = dark;
                logger_label.Foreground = dark;
                GloableOject.darkmode = false;
            }
            else
            {
                mainGrid.Background = dark;
                controlPanel_Splitter.Background = dark;
                targetFolder_path.Foreground = Brushes.White;
                Opacity_label.Foreground = Brushes.White;
                version_label.Foreground = Brushes.White;
                logger_label.Foreground = Brushes.White;
                GloableOject.darkmode = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Proladon/Image_Classifier_WPF");
        }

        private void topmostBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush dark = new SolidColorBrush(Color.FromRgb(37, 42, 51));
            if (GloableOject.topmost == false)
            {
                this.Topmost = true;
                GloableOject.topmost = true;
                topmose_Btn.Style = this.FindResource("ontop") as Style;
            }
            else
            {
                this.Topmost = false;
                GloableOject.topmost = false;
                topmose_Btn.Style = this.FindResource("DefaultBtn") as Style;
            }
        }
    }
}
