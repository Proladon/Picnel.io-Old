using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Image_Classifier.Classes
{
    public class GloableOject
    {
        public static Boolean darkmode = true;
        public static Boolean topmost = false;
        public static String curPath = @"K:\MainFolder\Wallpaper";
        public static String img_path = String.Empty;
        public static String preFileName = String.Empty;
        public static String img_filename = String.Empty;
        public static String file_ex = String.Empty;
        public static String winState = "Normal";
        public static String[] normal_img = { ".jpg", ".jpeg", ".png", ".bmp" };
        public static String[] gif_img = { ".gif" };
        public static String[] video = { ".mp4", ".avi" };
        public static BitmapSource img = null;

        public static MainWindow mainWin = ((MainWindow)System.Windows.Application.Current.MainWindow);
        public static void logger(string data)
        {
            TextBlock log = new TextBlock();
            log.TextWrapping = System.Windows.TextWrapping.Wrap;
            log.Margin = new System.Windows.Thickness(3, 3, 3, 3);
            log.FontFamily = new FontFamily("Consolas Bold");
            log.FontSize = 15;
            log.Foreground = new SolidColorBrush(Colors.Gray);
            log.Text = data;
            mainWin.logViewer.Children.Add(log);
            mainWin.log_scrollViewer.ScrollToEnd();
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
            GC.Collect();
            return image;
        }

        public static void random_image(string path)
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
                GloableOject.file_ex = Path.GetExtension(img_filename).ToLower();

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
            mainWin.imgFileName.Text = GloableOject.img_filename;

            }
            catch (System.IO.DirectoryNotFoundException)
            {
                GloableOject.logger($"❌ [Error] - NotFound Directory. Pls Choose The Main Directoy");
            }
            catch (System.IndexOutOfRangeException)
            {
                GloableOject.logger($"❌ [Error] - No File in this Directory.");
            }
            catch (Exception error)
            {
                GloableOject.logger($"❌ [Error] - {error}");
            }
        }

        public static void moveTo(string file, string path)
        {
            try
            {
                System.IO.File.Move(file, path + '\\' + GloableOject.img_filename);
                random_image(curPath);
                logger($"✔📤 [Move File] - [ {img_filename} ] move to [ {path + '\\'} ]");

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
        public static void copyTo(string file, string path)
        {
            try
            {
                System.IO.File.Copy(file, path + '\\' + GloableOject.img_filename);
                logger($"✔📥 [Copy File] - [ {img_filename} ] copy to [ {path + '\\'} ]");
            }
            catch(System.IO.IOException)
            {
                logger($"❌📥 [Error] [Copy File] - File Has Been Exsit In {path}.");
            }
            catch(System.ArgumentException)
            {
                logger($"❌📥 [Error] [Copy File] - File Doesn't Exsit.");
            }
        }


    }
}
