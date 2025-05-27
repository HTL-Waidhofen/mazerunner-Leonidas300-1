using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;


namespace WpfExample
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Button3_Click(object sender, RoutedEventArgs e)
        {
            int i;
            for (i = 0; i <= 10; i++)
            {
                Thread.Sleep(500);
                Button3.Content = "Frag nicht was für Saft, einfach Orangensaft";


            }

        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
