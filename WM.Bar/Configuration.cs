using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WM.Bar
{
    static class Configuration
    {
        public static List<Label> WorkSpaces { get; set; }
        public static List<Label> StatusItems { get; set; }
        public static Brush BackgroundColor { get; } = (Brush)new BrushConverter().ConvertFrom("#23232D");
        public static Brush BackgroundColorLighter { get; } = (Brush)new BrushConverter().ConvertFrom("#2c2c36");
        public static Brush AccentColor { get; } = (Brush)new BrushConverter().ConvertFrom("#7289da");
        public static Brush ForegroundColor { get; } = (Brush)new BrushConverter().ConvertFrom("#eae5e5");
        public static string[] JNumbers = {"一", "二", "三", "四", "五", "六", "七", "八", "九"};
    }
}
