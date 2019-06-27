using System.Windows.Media;

namespace Starcraft_BO_helper
{
    public static class BrushExtension
    {
        public static Color GetColor(this Brush brush)
        {
            return ((SolidColorBrush)brush).Color;
        }
    }
}
