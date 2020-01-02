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

namespace Image_Classifier
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

        private void random_image(string path)
        {

            //System.IO.DirectoryNotFoundException
            if (path == string.Empty)
            {
                return;
            }
            try
            {
                // 從資料夾中隨機選取一張圖片
                string[] filePaths = Directory.GetFiles(path);
                var ran = new Random();
                int ran_index = ran.Next(filePaths.Length);
                string fpath = filePaths[ran_index];
                GloableOject.img_path = fpath;
                GloableOject.img_filename = System.IO.Path.GetFileName(fpath);

                //更改img_preview的圖片路徑
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(fpath);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(img_preview, image);

            }
            catch(System.IO.DirectoryNotFoundException)
            {
                return;
            }
            catch(Exception error)
            {
                GloableOject.logger($"❌[Error] {error}");
            }
        }

        private void switch_img()
        {

        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = targetFolder_path.Content.ToString();
            random_image(path);
            imgFileName.Text = GloableOject.img_filename;
        }

        private void chooseTargetFolder_Btn_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            folderDialog.ShowDialog();
            String sPath = folderDialog.SelectedPath;
            GloableOject.curPath = sPath;
            targetFolder_path.Content = sPath;
            targetFolder_path.ToolTip = sPath;

            random_image(sPath);
            imgFileName.Text = GloableOject.img_filename;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateControl_Dialog test_create = new CreateControl_Dialog();
            Window newWin = new Window
            {
                Title = "Create test",
                Height = 200,
                Width = 500,
                Content = test_create,

            };
            newWin.ShowDialog();
        }

        private void deleteImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            // TODO Delete Image File
        }

        private void imgFileName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GloableOject.preFileName = imgFileName.Text;
        }
        private void imgFileName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (imgFileName.Text != GloableOject.preFileName)
            {
                //切換空圖片
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"G:\Gif reply\GIF\move.gif");
                image.EndInit();
                ImageBehavior.SetAnimatedSource(img_preview, image);
                // 重新命名
                System.IO.File.Move(GloableOject.img_path, GloableOject.curPath+'\\'+imgFileName.Text);
                GloableOject.logger($"[Renmae File] {GloableOject.img_filename} > {imgFileName.Text}");
                System.IO.File.Delete(GloableOject.img_filename);
                GloableOject.img_filename = imgFileName.Text;
                //切回原圖
                var b = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"G:\Gif reply\GIF\move.gif");
                image.EndInit();
                ImageBehavior.SetAnimatedSource(img_preview, b);
            }
        }

    }
}
