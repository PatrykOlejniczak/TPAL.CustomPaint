using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomPaint.Extension.Grayscale
{
    public class GrayscaleTransform : IExtension
    {
        public void Execute(Canvas canvas)
        {
            foreach (var canvasChild in canvas.Children)    
            {
                var propertyInfo = canvasChild.GetType().GetProperty("Stroke");
                if (propertyInfo != null)
                {
                    var stroke = propertyInfo.GetValue(canvasChild) as SolidColorBrush;

                    if (stroke != null)
                    {
                        propertyInfo.SetValue(canvasChild,
                            new SolidColorBrush(
                                Color.FromArgb(
                                    Convert.ToByte(stroke.Color.A),
                                    Convert.ToByte((stroke.Color.R + stroke.Color.G + stroke.Color.B) / 3),
                                    Convert.ToByte((stroke.Color.R + stroke.Color.G + stroke.Color.B) / 3),
                                    Convert.ToByte((stroke.Color.R + stroke.Color.G + stroke.Color.B) / 3)
                                )));
                    }
                }

                var propertyInfo2 = canvasChild.GetType().GetProperty("Fill");
                if (propertyInfo2 != null)
                {
                    var fill = propertyInfo2.GetValue(canvasChild) as SolidColorBrush;

                    if (fill != null)
                    {
                        propertyInfo2.SetValue(canvasChild,
                            new SolidColorBrush(
                                Color.FromArgb(
                                    Convert.ToByte(fill.Color.A),
                                    Convert.ToByte((fill.Color.R + fill.Color.G + fill.Color.B) / 3),
                                    Convert.ToByte((fill.Color.R + fill.Color.G + fill.Color.B) / 3),
                                    Convert.ToByte((fill.Color.R + fill.Color.G + fill.Color.B) / 3)
                                )));
                    }
                }
            }
        }
    }
}