﻿using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Objektorientierung
{
    class Spieler
    {
        public int x;
        public int y;
        public Image image;
        public MainWindow.Direction direction=MainWindow.Direction.None;
        public List<Rechteck> rechtecke;

        public Spieler(List<Rechteck> rechtecke)
        {
            x = 1;
            y = 1;
            this.rechtecke = rechtecke;

        }
        public void SetDirection(MainWindow.Direction direction)
        {
            this.direction = direction;

        }
        public void Move()
        {
            int currentX = x;
            int currentY = y;

            if(direction==MainWindow.Direction.Left)
            {
                x--;
            }
            if (direction == MainWindow.Direction.Right)
            {
                x++;
            }
            if (direction == MainWindow.Direction.Up)
            {
                y--;
            }
            if (direction == MainWindow.Direction.Down)
            {
                y++;
            }

            bool collision = false;
            foreach(Rechteck r in rechtecke)
            {
                if (r.position_x == x *MainWindow.grid_size && r.position_y==y*MainWindow.grid_size)
                {
                    collision = true;
                }
            }
            if (collision)
            {
                x = currentX;
                y = currentY;
            }

            Canvas.SetLeft(image, x * MainWindow.grid_size);
            Canvas.SetTop(image, y * MainWindow.grid_size);
        }

    }
    class Rechteck
    {
        public double laenge = 1;
        public double breite = 2;

        public double position_x = 0;
        public double position_y = 0;

        public double FlaecheBerechnen()
        {
            return laenge * breite;
        }
        public Rechteck(double laenge, double breite, double position_x, double position_y)
        {
            this.laenge = laenge;
            this.breite = breite;
            this.position_x = position_x;
            this.position_y = position_y;
        }
        public override string ToString()
        {
            return $"rechteck: {laenge}x{breite} [{position_x}x{position_y}]";
        }
    }

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Direction
        {
            Up,Down,Left,Right,None
        }

        DispatcherTimer timer = null;
        List<Rechteck> rechtecke = new List<Rechteck>();
        Spieler spieler;

        public static int grid_size = 10;

        public MainWindow()
        {
            InitializeComponent();

            spieler = new Spieler(rechtecke);

            Button button = new Button();
            button.Width = 100;
            button.Content = "Click mich";

            StreamReader reader = new StreamReader("wallsList.txt");
            string wallsList = reader.ReadToEnd();
            string[] walls = wallsList.Split('\n');

            for(int i = 0; i < walls.Length; i++)
            {
                int x = int.Parse(walls[i].Split(',')[0])*grid_size;
                int y = int.Parse(walls[i].Split(',')[1])*grid_size;

                Rechteck r = new Rechteck(1,1,x,y);
                rechtecke.Add(r);
                lstRechtecke.Items.Add(r);
            }
        }

        private void Update(object sender, EventArgs e)
        {
            spieler.Move();
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string laengeStr = this.tbxLaenge.Text;
                double laenge = double.Parse(laengeStr);
                string breiteStr = this.tbxBreite.Text;
                double breite = double.Parse(breiteStr);
                string position_xStr = this.tbxPosx.Text;
                double position_x = double.Parse(position_xStr);
                string position_yStr = this.tbxPosy.Text;
                double position_y = double.Parse(position_yStr);
                if (lstRechtecke.SelectedItem != null)
                {
                    Rechteck r = (Rechteck)this.lstRechtecke.SelectedItem;
                    r.laenge = laenge;
                    r.breite = breite;
                    //nächstes mal weiter
                    lstRechtecke.Items.Refresh();

                }
                else
                {
                    Rechteck r = new Rechteck(laenge, breite, position_x, position_y);
                    lstRechtecke.Items.Add(r);
                    rechtecke.Add(r);

                }
                tbxLaenge.Clear();
                tbxBreite.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ungültige Eingabe!");
            }
        }

        private void lstRechtecke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rechteck r = (Rechteck)this.lstRechtecke.SelectedItem;
            try
            {
                tbxLaenge.Text = r.laenge.ToString();
                tbxBreite.Text = r.breite.ToString();
            }
            catch (NullReferenceException) { }
        }

        private void Zeichnen_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < rechtecke.Count; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Width = rechtecke[i].laenge * grid_size;
                rectangle.Height = rechtecke[i].breite*grid_size;
                rectangle.StrokeThickness = 2;
                rectangle.Stroke = Brushes.Black;
                Canvas.SetTop(rectangle, rechtecke[i].position_x);
                Canvas.SetLeft(rectangle, rechtecke[i].position_y); //hier nächstes mal weiter programmieren (Positionierung funktioniert nicht)
                myCanvas.Children.Add(rectangle);
            }

            spieler.image = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource= new Uri("Josef-Stalin.jpg",UriKind.Relative);
            bitmap.EndInit();
            spieler.image.Source = bitmap;
            spieler.image.Width = grid_size*1.5;
            spieler.image.Height = grid_size*1.5;
            Canvas.SetLeft(spieler.image, spieler.x* grid_size);
            Canvas.SetTop(spieler.image, spieler.y* grid_size);
            myCanvas.Children.Add(spieler.image);
        }

        private void Loeschen_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                spieler.SetDirection(Direction.Left);
            }
            else if (e.Key == Key.Right)
            {
                spieler.SetDirection(Direction.Right);
            }
            else if (e.Key == Key.Up)
            {
                spieler.SetDirection(Direction.Up);
            }
            else if (e.Key == Key.Down)
            {
                spieler.SetDirection(Direction.Down);
            }

        }

        private void Starten_Click(object sender, RoutedEventArgs e)
        {
            if( stp_sidebar.Visibility == Visibility.Collapsed)
            {
                stp_sidebar.Visibility = Visibility.Visible;
            }
            else
            {
                stp_sidebar.Visibility= Visibility.Collapsed;

                timer = new DispatcherTimer(DispatcherPriority.Render);
                timer.Interval = TimeSpan.FromMilliseconds(300);
                timer.Tick += Update;
                timer.Start();
            }
        }
    }
}