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

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = targetFolder_path.Content.ToString();
            GloableOject.random_image(path);
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

            GloableOject.random_image(sPath);
            //imgFileName.Text = GloableOject.img_filename;
        }

        private void CreateControl_Click(object sender, RoutedEventArgs e)
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
            if (MessageBox.Show("Close Application?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                System.IO.File.Delete(GloableOject.img_path);
                GloableOject.logger($"♻🗑 [Delete File] - FileName: [ {GloableOject.img_filename} ]");
                GloableOject.random_image(GloableOject.curPath);
            }
        }

        private void imgFileName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GloableOject.preFileName = imgFileName.Text;
        }
        private void imgFileName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (imgFileName.Text != GloableOject.preFileName)
            {
                // 重新命名
                System.IO.File.Move(GloableOject.img_path, GloableOject.curPath+'\\'+imgFileName.Text);
                GloableOject.logger($"[Renmae File] {GloableOject.img_filename} > {imgFileName.Text}");
                System.IO.File.Delete(GloableOject.img_filename);
                GloableOject.img_path = GloableOject.curPath + '\\' + imgFileName.Text;
                GloableOject.img_filename = imgFileName.Text;
            }
        }

    }
}
