﻿using System;
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
using System.IO;
using WinForms = System.Windows.Forms;
using Image_Classifier.Classes;


namespace Image_Classifier.User_Control
{
    /// <summary>
    /// CreateControl_Diaolog.xaml 的互動邏輯
    /// </summary>
    public partial class CreateControl_Dialog : UserControl
    {
        public CreateControl_Dialog()
        {
            InitializeComponent();
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            folderDialog.ShowDialog();
            String sPath = folderDialog.SelectedPath;
            choseFolder_path.Text = sPath;
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            string color = Convert.ToString(choossColorBtn.SelectedColor); // 取得選取顏色
            if (color == string.Empty)
            {
                color = "#FFFFFF";
            }
            Folder_Control folder_control = new Folder_Control(); // 創建 Folder_Control 實例
            BrushConverter tagColor = new BrushConverter(); // 轉換顏色
            folder_control.colorTag.Background = (Brush)tagColor.ConvertFrom(color); // 設定Lable控件 colorTag 的顏色
            folder_control.akaLabel.Text = folderAKA_label.Text;
            folder_control.folderPath.Text = choseFolder_path.Text;
            folder_control.ToolTip = choseFolder_path.Text;
            folder_control.akaLabel.FontFamily = new FontFamily("Consolas Bold");
            //將Folder_Control 實例 添加到 MainWindow 的 control_panel 中
            ((MainWindow)System.Windows.Application.Current.MainWindow).control_panel.Children.Add(folder_control); 
            GloableOject.logger($"✔🕹[Create Target Folder Control] AKA :[{folderAKA_label.Text}] ; Path: [{choseFolder_path.Text}]");
            // 清空此Dialog元件的所有訊息
            folderAKA_label.Text = string.Empty;
            choseFolder_path.Text = string.Empty;
            choossColorBtn.SelectedColor = Color.FromRgb(255,255,255);
        }

        private void folderAKA_label_GotFocus(object sender, RoutedEventArgs e)
        {
            folderAKA_label.Text = string.Empty;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainGrid.Focus();
            Window.GetWindow(this).DragMove();
        }
    }
}