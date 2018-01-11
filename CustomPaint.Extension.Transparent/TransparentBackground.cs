using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomPaint.Extension.Transparent
{
    public class TransparentBackground : IExtension
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
                                    Convert.ToByte(stroke.Color.R / 3),
                                    Convert.ToByte(stroke.Color.G / 3),
                                    Convert.ToByte(stroke.Color.B / 3)
                                )));
                    }
                }

                propertyInfo = canvasChild.GetType().GetProperty("Fill");
                if (propertyInfo != null)
                {
                    var fill = propertyInfo.GetValue(canvasChild) as SolidColorBrush;

                    if (fill != null)
                    {
                        propertyInfo.SetValue(canvasChild,
                            new SolidColorBrush(
                                Color.FromArgb(
                                    Convert.ToByte(0),
                                    Convert.ToByte(fill.Color.R / 3),
                                    Convert.ToByte(fill.Color.G / 3),
                                    Convert.ToByte(fill.Color.B / 3)
                                )));
                    }
                }
            }
        }
    }
}
