using CalcWPFVeryCool.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace CalcWPFVeryCool.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

    }
}
