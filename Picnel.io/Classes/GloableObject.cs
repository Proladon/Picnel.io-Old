using Newtonsoft.Json;
using Picnel.io;
using Picnel.io.User_Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Picnel.io.Classes
{
    public class GloableObject
    {
        //public static Boolean topmost = false;

        public static int cur_img_position = 0;
        public static List<string> file_list = null;
        public static String curPath = string.Empty;
        public static String lastPath = string.Empty; //紀錄上次選取的資料夾位置 (FolderDiolog)
        public static String img_path = string.Empty;
        public static String preFileName = string.Empty;
        public static String img_filename = string.Empty;
        public static String file_ex = string.Empty;
        public static String winState = "Normal";
        public static String[] normal_img = { ".jpg", ".jpeg", ".png", ".bmp", ".tif", ".ico", ".apng" };
        public static String[] gif_img = { ".gif", ".webp" };
        public static String[] video = { ".mp4", ".avi" };
        public static BitmapSource img = null;
        public static Folder_Control temp_control = null;

        public static MainWindow mainWin = ((MainWindow)System.Windows.Application.Current.MainWindow);

        // 事件紀錄器 Logger
        public static void logger(string data, string type="Normal")
        {
            TextBlock log = new TextBlock();
            log.TextWrapping = System.Windows.TextWrapping.Wrap;
            log.Margin = new System.Windows.Thickness(3, 3, 3, 3);
            log.FontFamily = new FontFamily("Consolas Bold");
            log.FontSize = Properties.Settings.Default.Logger_FontSize;
            if (type == "Normal")
            {
                log.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else if (type == "HighLight")
            {
                SolidColorBrush blue_green = (SolidColorBrush)Application.Current.FindResource("blue_green");
                log.Foreground = blue_green;
            }
            log.Text = data;
            mainWin.logViewer.Children.Add(log);
            mainWin.log_scrollViewer.ScrollToEnd();
        }

        // 資料夾資訊
        public static void folderInfo()
        {
            int filesCount = Directory.GetFiles(curPath, "*", SearchOption.TopDirectoryOnly).Length;
            int foldersCount = Directory.GetDirectories(curPath, "*", SearchOption.TopDirectoryOnly).Length;
            mainWin.folder_info.Text = $"{filesCount} Files / {foldersCount} Folders";
            mainWin.targetFolder_path.Text = System.IO.Path.GetFileName(curPath);
            mainWin.targetFolder_path.ToolTip = curPath;
        }


        // 更換圖片路徑
        public static void change_img(string path)
        {
            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.DecodePixelWidth = 400;
                image.UriSource = new Uri(path);
                image.EndInit();
                GloableObject.img_path = path;
                GloableObject.img_filename = System.IO.Path.GetFileName(path);
                GloableObject.file_ex = Path.GetExtension(img_filename).ToLower();

                if (normal_img.Contains(file_ex))
                {
                    logger("img");
                    mainWin.gif_previewer.Visibility = System.Windows.Visibility.Collapsed;
                    mainWin.normal_img_previewer.Visibility = System.Windows.Visibility.Visible;
                    mainWin.normal_img_previewer.Source = image;
                }
                else if (gif_img.Contains(file_ex))
                {
                    logger("gif");
                    WpfAnimatedGif.ImageBehavior.SetAutoStart(mainWin.gif_previewer, true);
                    mainWin.normal_img_previewer.Visibility = System.Windows.Visibility.Collapsed;
                    mainWin.gif_previewer.Visibility = System.Windows.Visibility.Visible;
                    ImageBehavior.SetAnimatedSource(mainWin.gif_previewer, image);
                }
                else if (video.Contains(file_ex))
                {
                    mainWin.normal_img_previewer.Source = new BitmapImage(new Uri(@"\src\file_not_Support.png", UriKind.Relative));
                    logger("⚠ [Warning] - Not Support Video Files Yet.");
                    return;
                }
                else
                {
                    logger("else");
                    mainWin.normal_img_previewer.Source = new BitmapImage(new Uri(@"\src\file_not_Support.png", UriKind.Relative));
                    logger($"⚠ [Warning] - Not Support {file_ex} Files.");
                }
                mainWin.imgFileName.Text = GloableObject.img_filename;

                mainWin.imgFileName.Text = Path.GetFileName(path);
                GC.Collect();
            }
            catch (System.NotSupportedException)
            {

            }
            catch (System.IO.DirectoryNotFoundException)
            {
                GloableObject.logger($"❌ [Error] - NotFound Directory. Pls Choose The Main Directoy");
            }
            catch (System.IndexOutOfRangeException)
            {
                GloableObject.logger($"❌ [Error] - No File in this Directory.");
            }
            catch (Exception error)
            {
                GloableObject.logger($"❌ [Error] - {error}");
            }
        }


        public static BitmapImage change_src(string path)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.DecodePixelWidth = 400;
            image.UriSource = new Uri(path);
            image.EndInit();
            mainWin.normal_img_previewer.Source = image;
            mainWin.imgFileName.Text = Path.GetFileName(path);
            GC.Collect();
            return image;
        }

        // 隨機圖片
        public static void random_image(string path)
        {
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
                GloableObject.img_path = fpath;
                GloableObject.img_filename = System.IO.Path.GetFileName(fpath);
                GloableObject.file_ex = Path.GetExtension(img_filename).ToLower();

                // 判斷檔案類型 使用哪種圖片顯示器
                if (normal_img.Contains(file_ex))
                {
                    // 使用 原生Image  更改img_preview的圖片路徑
                    mainWin.gif_previewer.Visibility = System.Windows.Visibility.Collapsed;
                    mainWin.normal_img_previewer.Visibility = System.Windows.Visibility.Visible;
                    BitmapImage image = change_src(fpath);
                    img = image;
                    mainWin.normal_img_previewer.Source = image;
                    GC.Collect();
                }
                else if (gif_img.Contains(file_ex))
                {
                    WpfAnimatedGif.ImageBehavior.SetAutoStart(mainWin.gif_previewer, true);
                    mainWin.normal_img_previewer.Visibility = System.Windows.Visibility.Collapsed;
                    mainWin.gif_previewer.Visibility = System.Windows.Visibility.Visible;
                    // 使用 wpfGIF 更改img_preview的圖片路徑
                    BitmapImage image = change_src(fpath);
                    img = image;
                    ImageBehavior.SetAnimatedSource(mainWin.gif_previewer, image);
                    GC.Collect();
                }
                else if (video.Contains(file_ex))
                {
                    mainWin.normal_img_previewer.Source = new BitmapImage(new Uri(@"\src\file_not_Support.png", UriKind.Relative));
                    logger("⚠ [Warning] - Not Support Video Files Yet.");
                    return;
                }
                else
                {
                    mainWin.normal_img_previewer.Source = new BitmapImage(new Uri(@"\src\file_not_Support.png", UriKind.Relative));
                    logger($"⚠ [Warning] - Not Support {file_ex} Files.");
                }
                mainWin.imgFileName.Text = GloableObject.img_filename;

            }
            catch (System.IO.DirectoryNotFoundException)
            {
                GloableObject.logger($"❌ [Error] - NotFound Directory. Pls Choose The Main Directoy");
            }
            catch (System.IndexOutOfRangeException)
            {
                GloableObject.logger($"❌ [Error] - No File in this Directory.");
            }
            catch (Exception error)
            {
                GloableObject.logger($"❌ [Error] - {error}");
            }
        }

        // 移動檔案操作 
        public static void moveTo(string file, string path)
        {
            try
            {
                System.IO.File.Move(file, path + '\\' + GloableObject.img_filename);
                logger($"✔📤 [Move File] - [ {img_filename} ] move to [ {path + '\\'} ]");
                random_image(curPath);

            }
            catch (System.IO.IOException)
            {
                logger($"❌📤 [Error] [Move File] - File Has Been Exsit In {path}.");
            }
            catch (System.ArgumentException)
            {
                logger($"❌📥 [Error] [Move File] - File Doesn't Exsit.");
            }
        }
        
        // 複製檔案操作
        public static void copyTo(string file, string path)
        {
            try
            {
                System.IO.File.Copy(file, path + '\\' + GloableObject.img_filename);
                logger($"✔📥 [Copy File] - [ {img_filename} ] copy to [ {path + '\\'} ]");
            }
            catch (System.IO.IOException)
            {
                logger($"❌📥 [Error] [Copy File] - File Has Been Exsit In {path}.");
            }
            catch (System.ArgumentException)
            {
                logger($"❌📥 [Error] [Copy File] - File Doesn't Exsit.");
            }
        }
    }
}
