using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Image_Classifier.Classes
{
    public class GloableOject
    {
        public static String curPath = @"K:\MainFolder\Wallpaper";
        public static String img_path = String.Empty;
        public static String preFileName = String.Empty;
        public static String img_filename = String.Empty;
        

        public static void logger(string data)
        {
            TextBlock log = new TextBlock();
            log.TextWrapping = System.Windows.TextWrapping.Wrap;
            log.Text = data;
            ((MainWindow)System.Windows.Application.Current.MainWindow).logViewer.Children.Add(log);
            ((MainWindow)System.Windows.Application.Current.MainWindow).log_scrollViewer.ScrollToEnd();
        }
    }
}
