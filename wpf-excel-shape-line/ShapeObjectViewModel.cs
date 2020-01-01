namespace WpfApp1
{
    public class ShapeObjectViewModel
    {
        public bool IsMouseDown { get; set; } = false;

        public LineObject TopLine { get; set; }
        public LineObject RightLine { get; set; }
        public LineObject BottomLine { get; set; }
        public LineObject LeftLine { get; set; }
    }
}
