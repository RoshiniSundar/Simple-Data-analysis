using System.Windows;

namespace population_data
{
    public partial class MainWindow : Window
    {
        VM vm = new VM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Analyse_Click(object sender, RoutedEventArgs e)
        {
            vm.Analyse();
        }

        private void Read_Click(object sender, RoutedEventArgs e)
        {
            vm.Read();
        }
    }
}
