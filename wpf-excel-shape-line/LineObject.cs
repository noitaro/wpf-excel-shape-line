using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class LineObject
    {
        private readonly LineObjectViewModel ViewModel;

        public LineObject(Canvas parent)
        {
            ViewModel = new LineObjectViewModel()
            {
                Canvas = parent
            };
        }

        public void SetPoint(PathFigure pathFigure, Point startPoint, Point endPoint)
        {
            var width = Math.Abs(endPoint.X - startPoint.X);
            var height = Math.Abs(endPoint.Y - startPoint.Y);

            var point1 = new Point(startPoint.X - (width / 2), startPoint.Y + (height / 2));
            var point2 = new Point(endPoint.X + (width / 2), endPoint.Y - (height / 2));

            switch (ViewModel.startDirection)
            {
                case LineObjectViewModel.DirectionIndex.Top:
                    point1.X = startPoint.X;
                    point1.Y = startPoint.Y - (height / 2);
                    break;

                case LineObjectViewModel.DirectionIndex.Right:
                    point1.X = startPoint.X + (width / 2);
                    point1.Y = startPoint.Y;
                    break;

                case LineObjectViewModel.DirectionIndex.Bottom:
                    point1.X = startPoint.X;
                    point1.Y = startPoint.Y + (height / 2);
                    break;

                case LineObjectViewModel.DirectionIndex.Left:
                    point1.X = startPoint.X - (width / 2);
                    point1.Y = startPoint.Y;
                    break;

                default:
                    point1.X = startPoint.X;
                    point1.Y = startPoint.Y;
                    break;
            }

            switch (ViewModel.endDirection)
            {
                case LineObjectViewModel.DirectionIndex.Top:
                    point2.X = endPoint.X;
                    point2.Y = endPoint.Y - (height / 2);
                    break;

                case LineObjectViewModel.DirectionIndex.Right:
                    point2.X = endPoint.X + (width / 2);
                    point2.Y = endPoint.Y;
                    break;

                case LineObjectViewModel.DirectionIndex.Bottom:
                    point2.X = endPoint.X;
                    point2.Y = endPoint.Y + (height / 2);
                    break;

                case LineObjectViewModel.DirectionIndex.Left:
                    point2.X = endPoint.X - (width / 2);
                    point2.Y = endPoint.Y;
                    break;

                default:
                    point2.X = endPoint.X;
                    point2.Y = endPoint.Y;
                    break;
            }

            var bezierSegment = (BezierSegment)pathFigure.Segments.First();

            pathFigure.StartPoint = startPoint;
            bezierSegment.Point1 = point1;
            bezierSegment.Point2 = point2;
            bezierSegment.Point3 = endPoint;
        }

        public void SetStartDirection(LineObjectViewModel.DirectionIndex directionIndex)
        {
            ViewModel.startDirection = directionIndex;
        }

        public void Addin(Point startPoint, Point endPoint)
        {
            // 線
            ViewModel.linePath = new Path()
            {
                Stroke = Brushes.Crimson,
                StrokeThickness = 3
            };
            PathGeometry lineGeometry = new PathGeometry();
            PathFigure lineFigure = new PathFigure() { StartPoint = new Point(0, 0) };
            lineFigure.Segments.Add(new BezierSegment(new Point(0, 0), new Point(0, 0), new Point(0, 0), true));
            lineGeometry.Figures.Add(lineFigure);
            ViewModel.linePath.Data = lineGeometry;

            Canvas.SetLeft(ViewModel.linePath, 0);
            Canvas.SetTop(ViewModel.linePath, 0);
            ViewModel.Canvas.Children.Add(ViewModel.linePath);

            // 開始位置
            ViewModel.startPath = new Path()
            {
                Fill = Brushes.LightPink,
                Stretch = Stretch.Fill,
                Width = 10,
                Height = 10,
                Data = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, 10, 10),
                    RadiusX = 3,
                    RadiusY = 3
                }
            };
            var startPositionBehaviors = Interaction.GetBehaviors(ViewModel.startPath);
            var startPositionBehavior = new MouseDragElementBehavior()
            {
                ConstrainToParentBounds = true,
                X = startPoint.X,
                Y = startPoint.Y
            };
            startPositionBehaviors.Add(startPositionBehavior);
            startPositionBehavior.Dragging += StartPositionBehavior_Dragging;

            Canvas.SetLeft(ViewModel.startPath, startPoint.X);
            Canvas.SetTop(ViewModel.startPath, startPoint.Y);
            ViewModel.Canvas.Children.Add(ViewModel.startPath);

            // 終了位置
            ViewModel.endPath = new Path()
            {
                Fill = Brushes.LightBlue,
                Stretch = Stretch.Fill,
                Width = 10,
                Height = 10,
                Data = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, 10, 10),
                    RadiusX = 3,
                    RadiusY = 3
                }
            };
            var endPositionBehaviors = Interaction.GetBehaviors(ViewModel.endPath);
            var endPositionBehavior = new MouseDragElementBehavior()
            {
                ConstrainToParentBounds = true,
                X = endPoint.X,
                Y = endPoint.Y
            };
            endPositionBehaviors.Add(endPositionBehavior);
            endPositionBehavior.Dragging += EndPositionBehavior_Dragging;

            Canvas.SetLeft(ViewModel.endPath, endPoint.X);
            Canvas.SetTop(ViewModel.endPath, endPoint.Y);
            ViewModel.Canvas.Children.Add(ViewModel.endPath);

            // 線の描画
            SetPoint(
                lineFigure,
                new Point(startPositionBehavior.X + (ViewModel.startPath.Width / 2), startPositionBehavior.Y + (ViewModel.startPath.Height / 2)),
                new Point(endPositionBehavior.X + (ViewModel.endPath.Width / 2), endPositionBehavior.Y + (ViewModel.endPath.Height / 2)));
        }

        public void StartPositionBehavior_Dragging(object sender, MouseEventArgs e)
        {
            var mouseDragElementBehavior = (MouseDragElementBehavior)sender;
            var pathFigure = ((PathGeometry)ViewModel.linePath.Data).Figures.First();
            var bezierSegment = (BezierSegment)pathFigure.Segments.First();

            SetPoint(
                pathFigure,
                new Point(
                    mouseDragElementBehavior.X + (ViewModel.startPath.Width / 2),
                    mouseDragElementBehavior.Y + (ViewModel.startPath.Height / 2)),
                bezierSegment.Point3
                );
        }

        public void EndPositionBehavior_Dragging(object sender, MouseEventArgs e)
        {
            var mouseDragElementBehavior = (MouseDragElementBehavior)sender;
            var pathFigure = ((PathGeometry)ViewModel.linePath.Data).Figures.First();

            SetPoint(
                pathFigure,
                pathFigure.StartPoint,
                new Point(
                    mouseDragElementBehavior.X + (ViewModel.endPath.Width / 2),
                    mouseDragElementBehavior.Y + (ViewModel.endPath.Height / 2))
                );
        }

        public void StartMove(Point point)
        {
            var startPositionBehaviors = Interaction.GetBehaviors(ViewModel.startPath);
            var mouseDragElementBehavior = (MouseDragElementBehavior)startPositionBehaviors.First();
            mouseDragElementBehavior.SetValue(MouseDragElementBehavior.XProperty, point.X);
            mouseDragElementBehavior.SetValue(MouseDragElementBehavior.YProperty, point.Y);
            StartPositionBehavior_Dragging(mouseDragElementBehavior, null);
        }

        public void EndMove(Point point)
        {
            var endPositionBehaviors = Interaction.GetBehaviors(ViewModel.endPath);
            var mouseDragElementBehavior = (MouseDragElementBehavior)endPositionBehaviors.First();
            mouseDragElementBehavior.SetValue(MouseDragElementBehavior.XProperty, point.X);
            mouseDragElementBehavior.SetValue(MouseDragElementBehavior.YProperty, point.Y);
            EndPositionBehavior_Dragging(mouseDragElementBehavior, null);
        }

    }
}
