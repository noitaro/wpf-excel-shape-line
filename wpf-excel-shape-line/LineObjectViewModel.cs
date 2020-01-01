using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class LineObjectViewModel
    {
        public Path startPath { get; set; }
        public DirectionIndex startDirection { get; set; } = DirectionIndex.None;
        public Path endPath { get; set; }
        public DirectionIndex endDirection { get; set; } = DirectionIndex.None;

        public Path linePath { get; set; }
        public Canvas Canvas { get; set; }

        public enum DirectionIndex
        {
            None,
            Top,
            Right,
            Bottom,
            Left
        }
    }
}
