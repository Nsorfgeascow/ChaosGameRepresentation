using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Diagnostics;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private System.Drawing.Point[] referencePointsSierpinski;
        private System.Drawing.Point[] referencePointsDNA;
        private string genome;
        Bitmap bitmap;
        private int resolutionCounter;
        List<String> actg = new List<String>();
        List<int> numberOfSubsequences = new List<int>();
        int minActgList = 0;
        int maxActgList = 0;
        int n = 20;

        private void NewGame()
        {
            referencePointsSierpinski = new System.Drawing.Point[]
            {
            new System.Drawing.Point(0, 600),
            new System.Drawing.Point(600, 600),
            new System.Drawing.Point(300, 81)
            };

            referencePointsDNA = new System.Drawing.Point[]
            {
            new System.Drawing.Point(0, 600),
            new System.Drawing.Point(600, 600),
            new System.Drawing.Point(600, 0),
            new System.Drawing.Point(0, 0)
            };


            actg.Add("C");
            actg.Add("G");
            actg.Add("A");
            actg.Add("T");

            resolutionCounter = 0;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (CBox.SelectedItem == ts)
            {
                DrawSierpinski();
            }
            if (CBox.SelectedItem == dna)
            {
                DrawCGR();
            }
        }

        void DrawSierpinski()
        {
            bitmap = new Bitmap(600, 600);

            var rand = new System.Random();
            var point = new System.Drawing.Point(rand.Next(600), rand.Next(600));
            int iteration = int.Parse(TextBox.Text); ;

            for (int count = 0; count < iteration; count++)
            {
                bitmap.SetPixel(point.X, point.Y, System.Drawing.Color.Black);
                int i = rand.Next(3);
                point.X = (point.X + referencePointsSierpinski[i].X) / 2;
                point.Y = (point.Y + referencePointsSierpinski[i].Y) / 2;
            }

            var hBitmap = bitmap.GetHbitmap();
            var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DrawingArea.Source = result;
        }

        void DrawCGR()
        {
            genome = TextBox.Text;

            bitmap = new Bitmap(600, 600);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, System.Drawing.Color.White);
                }
            }

            var point = new System.Drawing.Point(300, 300);
            int j;

            for (int i = 0; i < TextBox.Text.Length; i++)
            {
                switch (genome[i])
                {
                    case 'A':
                        j = 0;
                        point.X = (point.X + referencePointsDNA[j].X) / 2;
                        point.Y = (point.Y + referencePointsDNA[j].Y) / 2;
                        bitmap.SetPixel(point.X, point.Y, System.Drawing.Color.Gray);
                        break;
                    case 'C':
                        j = 3;
                        point.X = (point.X + referencePointsDNA[j].X) / 2;
                        point.Y = (point.Y + referencePointsDNA[j].Y) / 2;
                        bitmap.SetPixel(point.X, point.Y, System.Drawing.Color.Gray);
                        break;
                    case 'T':
                        j = 1;
                        point.X = (point.X + referencePointsDNA[j].X) / 2;
                        point.Y = (point.Y + referencePointsDNA[j].Y) / 2;
                        bitmap.SetPixel(point.X, point.Y, System.Drawing.Color.Gray);
                        break;
                    case 'G':
                        j = 2;
                        point.X = (point.X + referencePointsDNA[j].X) / 2;
                        point.Y = (point.Y + referencePointsDNA[j].Y) / 2;
                        bitmap.SetPixel(point.X, point.Y, System.Drawing.Color.Gray);
                        break;
                }
            }

            var hBitmap = bitmap.GetHbitmap();
            var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            DrawingArea.Source = result;
        }

        int linesDividingCGR = 0;
        float resolutionFactor = 600;

        private int CountSubsequences(int i, int j)
        {
            int counter = 0;

            for (int m = (int)(resolutionFactor * i); m < (int)(resolutionFactor * (i + 1)); m++)
            {
                for (int n = (int)(resolutionFactor * j); n < (int)(resolutionFactor * (j + 1)); n++)
                {
                    if (bitmap.GetPixel(m, n).ToArgb() == System.Drawing.Color.Gray.ToArgb())
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }

        private void Resolution(object sender, RoutedEventArgs e)
        {
            DrawCGR();
            resolutionCounter += 1;
            var blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            var p1 = new System.Drawing.Point(300, 0);
            var p2 = new System.Drawing.Point(300, 600);
            var r1 = new System.Drawing.Point(0, 300);
            var r2 = new System.Drawing.Point(600, 300);

            var l1 = new System.Drawing.Point(0, 0);
            var l2 = new System.Drawing.Point(0, 0);


            linesDividingCGR += linesDividingCGR + 1;

            resolutionFactor = resolutionFactor / 2;
            numberOfSubsequences.Clear();

            for (int i = 0; i < (int)Math.Pow(2, resolutionCounter); i++)
            {
                for (int j = 0; j < (int)Math.Pow(2, resolutionCounter); j++)
                {
                    numberOfSubsequences.Add(CountSubsequences(i, j));
                }
            }

            for (int i = 0; i < (int)Math.Pow(2, resolutionCounter); i++)
            {
                for (int j = 0; j < (int)Math.Pow(2, resolutionCounter); j++)
                {
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.DrawString(numberOfSubsequences[j + (i * (int)Math.Pow(2, resolutionCounter))].ToString(), new Font("Times New Roman", n), System.Drawing.Brushes.DarkBlue, (int)(resolutionFactor * i), (int)(resolutionFactor * j));
                    }
                }
            }

            for (int i = 1; i < linesDividingCGR + 1; i++)
            {
                p1.X = (int)(resolutionFactor * i);
                p2.X = (int)(resolutionFactor * i);
                r1.Y = (int)(resolutionFactor * i);
                r2.Y = (int)(resolutionFactor * i);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawLine(blackPen, p1, p2);
                    graphics.DrawLine(blackPen, r1, r2);
                }
            }

            minActgList += (int)Math.Pow(4, resolutionCounter - 1);
            maxActgList += (int)Math.Pow(4, resolutionCounter);

            for (int k = minActgList - 1; k < maxActgList; k++)
            {
                actg.Add("C" + actg[k]);
                actg.Add("G" + actg[k]);
                actg.Add("A" + actg[k]);
                actg.Add("T" + actg[k]);
            }

            int p = minActgList - 1;

            int w;

            for (int l = 0; l < (int)Math.Pow(4, resolutionCounter - 1); l++)
            {
                if (l == 0)
                    w = 0;
                else
                    w = (int)(Math.Pow(2, resolutionCounter - 1) * (l / ((int)Math.Pow(2, resolutionCounter - 1))));


                for (int i = 0; i < (int)Math.Pow(2, resolutionCounter - 1) * 2; i += 2)
                {
                    l1.X = (int)((resolutionFactor * (i + 1)) - (resolutionFactor / 2)) - (int)(n * 1.5);
                    l2.X = (int)((resolutionFactor * (i + 1)) + (resolutionFactor / 2)) - (int)(n * 1.5);

                    for (int j = w; j < ((int)Math.Pow(2, resolutionCounter - 1) * ((l + 1) / ((int)Math.Pow(2, resolutionCounter - 1)))); j += 2)
                    {
                        l1.Y = (int)((resolutionFactor * (j + 1)) - (resolutionFactor / 2)) - 10;
                        l2.Y = (int)((resolutionFactor * (j + 1)) + (resolutionFactor / 2)) - 10;

                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.DrawString(actg[p], new Font("Times New Roman", n), System.Drawing.Brushes.Black, l1.X, l1.Y);
                            graphics.DrawString(actg[p + 1], new Font("Times New Roman", n), System.Drawing.Brushes.Black, l2.X, l1.Y);
                            graphics.DrawString(actg[p + 2], new Font("Times New Roman", n), System.Drawing.Brushes.Black, l1.X, l2.Y);
                            graphics.DrawString(actg[p + 3], new Font("Times New Roman", n), System.Drawing.Brushes.Black, l2.X, l2.Y);

                            p += 4;
                        }
                    }
                }
            }

            n = (int)(n * 0.75);

            var hBitmap = bitmap.GetHbitmap();
            var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            DrawingArea.Source = result;
        }

        private void Zapisz(object sender, RoutedEventArgs e)
        {
            const string filename = "Chaos Game.png";
            bitmap.Save(filename);
            Process.Start(filename);
        }
    }
}