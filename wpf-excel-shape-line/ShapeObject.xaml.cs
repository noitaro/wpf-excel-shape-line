using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class ShapeObject : Thumb
    {
        private readonly ShapeObjectViewModel ViewModel;

        public ShapeObject()
        {
            InitializeComponent();

            ViewModel = new ShapeObjectViewModel();
            DataContext = ViewModel;

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Width = 100;
            Height = 100;

            Canvas.SetTop(this, 100);
            Canvas.SetLeft(this, 100);

            DragDelta += UserControl_DragDelta;

        }

        private void UserControl_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var point = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            point.X += e.HorizontalChange;
            point.Y += e.VerticalChange;

            Canvas.SetLeft(this, point.X);
            Canvas.SetTop(this, point.Y);

            if (ViewModel.TopLine != null)
            {
                var btn = Template.FindName("top", this) as Button;
                var btn_Vector = VisualTreeHelper.GetOffset(btn);
                var btnPoint = new Point(point.X, point.Y);
                btnPoint.X += btn_Vector.X;
                btnPoint.Y += btn_Vector.Y;
                ViewModel.TopLine.StartMove(btnPoint);
            }

            if (ViewModel.RightLine != null)
            {
                var btn = Template.FindName("right", this) as Button;
                var btn_Vector = VisualTreeHelper.GetOffset(btn);
                var btnPoint = new Point(point.X, point.Y);
                btnPoint.X += btn_Vector.X;
                btnPoint.Y += btn_Vector.Y;
                ViewModel.RightLine.StartMove(btnPoint);
            }

            if (ViewModel.BottomLine != null)
            {
                var btn = Template.FindName("bottom", this) as Button;
                var btn_Vector = VisualTreeHelper.GetOffset(btn);
                var btnPoint = new Point(point.X, point.Y);
                btnPoint.X += btn_Vector.X;
                btnPoint.Y += btn_Vector.Y;
                ViewModel.BottomLine.StartMove(btnPoint);
            }

            if (ViewModel.LeftLine != null)
            {
                var btn = Template.FindName("left", this) as Button;
                var btn_Vector = VisualTreeHelper.GetOffset(btn);
                var btnPoint = new Point(point.X, point.Y);
                btnPoint.X += btn_Vector.X;
                btnPoint.Y += btn_Vector.Y;
                ViewModel.LeftLine.StartMove(btnPoint);
            }

        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.IsMouseDown = true;

            var canvas = (Canvas)Parent;

            var btn = (Button)sender;
            var btn_Vector = VisualTreeHelper.GetOffset(btn);
            var point = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            point.X += btn_Vector.X;
            point.Y += btn_Vector.Y;

            var line = new LineObject(canvas);
            switch (btn.Name)
            {
                case "top":
                    ViewModel.TopLine = line;
                    ViewModel.TopLine.SetStartDirection(LineObjectViewModel.DirectionIndex.Top);
                    break;

                case "right":
                    ViewModel.RightLine = line;
                    ViewModel.RightLine.SetStartDirection(LineObjectViewModel.DirectionIndex.Right);
                    break;

                case "bottom":
                    ViewModel.BottomLine = line;
                    ViewModel.BottomLine.SetStartDirection(LineObjectViewModel.DirectionIndex.Bottom);
                    break;

                case "left":
                    ViewModel.LeftLine = line;
                    ViewModel.LeftLine.SetStartDirection(LineObjectViewModel.DirectionIndex.Left);
                    break;

                default:
                    return;
            }

            line.Addin(point, point);
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (!ViewModel.IsMouseDown) return;

            var btn = (Button)sender;
            var btn_Vector = VisualTreeHelper.GetOffset(btn);
            var point = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            point.X += btn_Vector.X;
            point.Y += btn_Vector.Y;

            var btnPoint = e.GetPosition((Button)sender);
            point.X += btnPoint.X;
            point.Y += btnPoint.Y;

            LineObject line;
            switch (btn.Name)
            {
                case "top":
                    line = ViewModel.TopLine;
                    break;

                case "right":
                    line = ViewModel.RightLine;
                    break;

                case "bottom":
                    line = ViewModel.BottomLine;
                    break;

                case "left":
                    line = ViewModel.LeftLine;
                    break;

                default:
                    return;
            }

            line.EndMove(point);
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.IsMouseDown = false;
        }
    }
}
