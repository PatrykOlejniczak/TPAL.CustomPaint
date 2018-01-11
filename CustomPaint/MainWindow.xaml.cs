using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CustomPaint.Extension;

namespace CustomPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolidColorBrush actualColor = Brushes.Black;
        private string activatedShape = "Line";

        Point currentPoint = new Point();
        private UndoRedo undoRedo;
        public MainWindow()
        {
            InitializeComponent();
            ClrPckerBackground.SelectedColor = Color.FromRgb(0, 0, 0);
            undoRedo = new UndoRedo(PaintSurface);
            undoRedo.SetStateForUndoRedo();
            LineButton.Background = Brushes.CornflowerBlue;
            LoadAssemblies();
        }

        private void LoadAssemblies()
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"../../dlls");

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
                allAssemblies.Add(Assembly.LoadFile(dll));

            foreach (var assembly in allAssemblies)
            {
                assembly.GetTypes()
                    .Where(t => t != typeof(IExtension) && typeof(IExtension).IsAssignableFrom(t))
                    .ToList()
                    .ForEach(x =>
                    {
                        var instance = (IExtension) Activator.CreateInstance(x);

                        var button = new Button() { Content = x.Name };
                        button.Margin = new Thickness(0, 0, 0, 10);
                        button.Click += (sender, args) =>
                        {
                            try
                            {
                                instance.Execute(PaintSurface);
                                undoRedo.SetStateForUndoRedo();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        };

                        ExtensionStackPanel.Children.Add(button);
                    });
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (activatedShape)
                {
                    case "Line":
                        DrawLine(e);
                        break;
                }
            }
        }

        private void UndoButton_OnClick(object sender, RoutedEventArgs e)
        {
            undoRedo.Undo(1);
        }

        private void RedoButton_OnClick(object sender, RoutedEventArgs e)
        {
            undoRedo.Redo(1);
        }

        private void PaintSurface_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (activatedShape)
            {
                case "Rectangle":
                    DrawRectangle(e);
                    break;
                case "Box":
                    DrawBox(e);
                    break;
                case "Circle":
                    DrawCircle(sender, e);
                    break;
            }

            undoRedo.SetStateForUndoRedo();
        }

        private void DrawLine(MouseEventArgs e)
        {
            Line line = new Line();

            line.Stroke = actualColor;
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            line.X2 = e.GetPosition(this).X;
            line.Y2 = e.GetPosition(this).Y;

            currentPoint = e.GetPosition(this);

            PaintSurface.Children.Add(line);
        }

        private void DrawRectangle(MouseEventArgs e)
        {
            Rectangle rect = new Rectangle
            {
                Stroke = actualColor,
                Fill = _fillColor,
                StrokeThickness = 1
            };

            var pos = e.GetPosition(PaintSurface);

            var x = Math.Min(pos.X, currentPoint.X);
            var y = Math.Min(pos.Y, currentPoint.Y);

            var w = Math.Max(pos.X, currentPoint.X) - x;
            var h = Math.Max(pos.Y, currentPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            PaintSurface.Children.Add(rect);
            Canvas.SetTop(rect, Math.Min(pos.Y, currentPoint.Y));
            Canvas.SetLeft(rect, Math.Min(pos.X, currentPoint.X));
        }

        private void DrawBox(MouseEventArgs e)
        {
            Rectangle rect = new Rectangle
            {
                Stroke = actualColor,
                Fill = _fillColor,
                StrokeThickness = 1
            };

            var pos = e.GetPosition(PaintSurface);

            var x = Math.Min(pos.X, currentPoint.X);
            var y = Math.Min(pos.Y, currentPoint.Y);

            var w = Math.Max(pos.X, currentPoint.X) - x;
            var h = Math.Max(pos.Y, currentPoint.Y) - y;

            rect.Width = Math.Max(w,h);
            rect.Height = Math.Max(w, h);

            PaintSurface.Children.Add(rect);
            Canvas.SetTop(rect, Math.Min(pos.Y, currentPoint.Y));
            Canvas.SetLeft(rect, Math.Min(pos.X, currentPoint.X));
        }

        private void DrawCircle(object sender, MouseEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            Rectangle rect = new Rectangle
            {
                Stroke = actualColor,
                Fill = _fillColor,
                StrokeThickness = 1
            };

            var pos = e.GetPosition(PaintSurface);

            var x = Math.Min(pos.X, currentPoint.X);
            var y = Math.Min(pos.Y, currentPoint.Y);

            var w = Math.Max(pos.X, currentPoint.X) - x;
            var h = Math.Max(pos.Y, currentPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            rect.RadiusX = 180;
            rect.RadiusY = 188;

            PaintSurface.Children.Add(rect);
            Canvas.SetTop(rect, Math.Min(pos.Y, currentPoint.Y));
            Canvas.SetLeft(rect, Math.Min(pos.X, currentPoint.X));
        }

        private void LineButton_OnClick(object sender, RoutedEventArgs e)
        {
            activatedShape = "Line";
            LineButton.Background = Brushes.CornflowerBlue;
            RectangleButton.Background = Brushes.LightGray;
            ElipseButton.Background = Brushes.LightGray;
            BoxButton.Background = Brushes.LightGray;
        }

        private void RectangleButton_OnClick(object sender, RoutedEventArgs e)
        {
            activatedShape = "Rectangle";
            LineButton.Background = Brushes.LightGray;
            RectangleButton.Background = Brushes.CornflowerBlue;
            ElipseButton.Background = Brushes.LightGray;
            BoxButton.Background = Brushes.LightGray;
        }

        private void ElipseButton_OnClick(object sender, RoutedEventArgs e)
        {
            activatedShape = "Circle";
            LineButton.Background = Brushes.LightGray;
            RectangleButton.Background = Brushes.LightGray;
            ElipseButton.Background = Brushes.CornflowerBlue;
            BoxButton.Background = Brushes.LightGray;
        }

        private void BoxButton_OnClick(object sender, RoutedEventArgs e)
        {
            activatedShape = "Box";
            LineButton.Background = Brushes.LightGray;
            RectangleButton.Background = Brushes.LightGray;
            ElipseButton.Background = Brushes.LightGray;
            BoxButton.Background = Brushes.CornflowerBlue;
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                actualColor = new SolidColorBrush(e.NewValue.Value);
            }
        }

        private SolidColorBrush _fillColor = Brushes.Transparent;
        private void ClrPcker_Fill_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                _fillColor = new SolidColorBrush(e.NewValue.Value);
            }
        }
    }
}