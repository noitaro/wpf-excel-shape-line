using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddShape_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Add(new ShapeObject());
        }
    }
}
