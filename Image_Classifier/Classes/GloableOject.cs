using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Image_Classifier.Classes
{
    public class GloableOject
    {
        public static String curPath = @"K:\MainFolder\Wallpaper";
        public static String img_path = String.Empty;
        public static String preFileName = String.Empty;
        public static String img_filename = String.Empty;
        public static MainWindow mainWin = ((MainWindow)System.Windows.Application.Current.MainWindow);



        public static void logger(string data)
        {
            TextBlock log = new TextBlock();
            log.TextWrapping = System.Windows.TextWrapping.Wrap;
            log.Text = data;
            ((MainWindow)System.Windows.Application.Current.MainWindow).logViewer.Children.Add(log);
            ((MainWindow)System.Windows.Application.Current.MainWindow).log_scrollViewer.ScrollToEnd();
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

                //更改img_preview的圖片路徑
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(fpath);
                image.EndInit();
                ImageBehavior.SetAnimatedSource(((MainWindow)System.Windows.Application.Current.MainWindow).img_preview, image);
                mainWin.imgFileName.Text = GloableOject.img_filename;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                GloableOject.logger($"❌ [Error] NotFound Directory. Pls Choose The Main Directoy");
            }
            catch (System.IndexOutOfRangeException)
            {
                GloableOject.logger($"❌ [Error] No File in this Directory.");
            }
            catch (Exception error)
            {
                GloableOject.logger($"❌ [Error] {error}");
            }
        }
    }
}
