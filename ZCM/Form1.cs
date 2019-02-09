using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;



namespace ZCM
{
    public partial class Form1 : Form
    {
        private int WIDTH;
        private int HEIGHT;
        private int Ox;
        private int Oy;

        //private double la = 1.404071;
        //private double la2 = 1.108738;
        //private double s = 80;
        //private double wr = 0.500000;
        //private int gsx = 100;
        //private int gsy = 100;


        
        private double w;
        private double theta;
        private double r;
        private double t;
        private double gamma;
        private double I;
        private double k;
        private double len1;
        private double len2;
        private double fend;

        private double x;
        private double y;

        private double scale;

        private bool dragging = false;

        public Bitmap cusp;
        public Bitmap area;
        private Graphics g;

        private Pen blackp = new Pen(Color.Black, 1.0f);

        private int controlr = 4;

        public Form1()
        {      
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WIDTH = this.Width;
            HEIGHT = this.Height;

            Ox = WIDTH / 2 - WIDTH / 8;
            Oy = HEIGHT / 2;
            scale = 60;

            t = 0.0;
            I = 50.0;
            fend = 20.0;
            len1 = len2 = 10.0;
            r = 5.0;
            k = 2500;
            gamma = 2000.0;
            w = 0;
            theta = Math.PI - 0.5;
            x = 30;
            y = 10;

            cusp = (Bitmap)Bitmap.FromFile("1.bmp");

            pbox.Width = WIDTH;
            pbox.Height = HEIGHT;
            

            area = new Bitmap(pbox.Width, pbox.Height);
            g = Graphics.FromImage(area);

            //DrawCusp();
        }


        public void DrawAxis()
        {
            g.Clear(Color.White);
            g.DrawImage(cusp, 0, 0);
            //g.DrawLine(blackp, Ox, 0, Ox, HEIGHT);
            //g.DrawLine(blackp, 0, Oy, WIDTH, Oy);
        }



        private double F1(double _w, double t)
        {
            double F1x, F1y, F2x, F2y;

            double cost = Math.Cos(theta);
            double sint = Math.Sin(theta);

            double xc = r * cost;
            double yc = r * sint;

            double curlen1 = Math.Sqrt((xc + fend) * (xc + fend) + yc * yc);
            double curlen2 = Math.Sqrt((xc - x) * (xc - x) + (yc - y) * (yc - y));

            double q = k * (curlen1 - len1) / curlen1;
            F1x = q * (-r * cost - fend);
            F1y = q * (-r * sint);

            q = k * (curlen2 - len2) / curlen2;
            F2x = q * (x - r * cost);
            F2y = q * (y - r * sint);

            F1x += F2x;
            F1y += F2y;

            double F = -F1x * sint + F1y * cost;

            double c = (r * F - gamma * _w) / I;

            return c;
        }


        public void Solve(double dt)
        {
            double k1 = dt * F1(w, t);
            double k2 = dt * F1(w + k1 / 2.0, t + dt / 2.0);
            double k3 = dt * F1(w + k2 / 2.0, t + dt / 2.0);
            double k4 = dt * F1(w + k3, t + dt);
            double k5 = 1.0/6.0 * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
            w += k5;

            // Runge-Kutta








            // Euler
            //w += F1(w, t) * dt;
            theta += w * dt;


        }


        private int Sx(double _x)
        {
            int res = (int)(Ox + _x * (double)WIDTH / scale);
            return res;
        }

        private int Sy(double _y)
        {
            int res = (int)(Oy - _y * (double)WIDTH / scale);
            return res;
        }


        public void Draw()
        {
            int rs = (int)(r * (double)WIDTH / scale);
            g.DrawEllipse(blackp, Sx(0.0) - rs, Sy(0.0) - rs, 2 * rs, 2 * rs);


            double cost = Math.Cos(theta);
            double sint = Math.Sin(theta);

            double xc = r * cost;
            double yc = r * sint;



            g.DrawLine(blackp, Sx(-fend), Sy(0), Sx(xc), Sy(yc));
            g.DrawLine(blackp, Sx(x), Sy(y), Sx(xc), Sy(yc));
            g.DrawLine(blackp, Sx(0), Sy(0), Sx(xc), Sy(yc));

            g.FillEllipse(Brushes.Green, Sx(x) - controlr, Sy(y) - controlr, 2 *controlr, 2 * controlr);

            g.DrawString(String.Format("w = {0:0.00}", w), new Font(FontFamily.GenericSansSerif, 12.0f), Brushes.Black, new PointF(20.0f, 20.0f));
            g.DrawString(String.Format("theta = {0:0.00}", theta), new Font(FontFamily.GenericSansSerif, 12.0f), Brushes.Black, new PointF(20.0f, 40.0f));
            g.DrawString(String.Format("x = {0:0.00}", x), new Font(FontFamily.GenericSansSerif, 12.0f), Brushes.Black, new PointF(20.0f, 60.0f));
            g.DrawString(String.Format("y = {0:0.00}", y), new Font(FontFamily.GenericSansSerif, 12.0f), Brushes.Black, new PointF(20.0f, 80.0f));

            pbox.Image = area;
        }


        private void DrawCusp()
        {

            List<Point> points = new List<Point>();


            double du = 0.01;
            double dv = -0.01;
            double dt = 0.01;

            double u, v;
            u = 13.8;
            v = 20.0;
            while (u < 25.0)
            {
                while (Math.Abs(v) <= 20.0)
                {
                    x = u;
                    y = v;

                    

                    double oldyc = r * Math.Sin(theta);

                    //Solve(dt);
                    //if (Math.Abs(w) > 0.2)
                    //{
                    //    points.Add(new Point(Sx(x), Sy(y)));
                    //}

                    

                    do
                    {
                        Solve(dt);
                    } while (Math.Abs(w) > 0.01);


                    double newyc = r * Math.Sin(theta);

                    if (oldyc * newyc < 0)
                    {

                        if (v > 0)
                        {
                            //g.DrawRectangle(blackp, Sx(u), Sy(v), 1, 1);
                            points.Add(new Point(Sx(u), Sy(v)));
                        }
                    }

                    v += dv;

                }



                dv *= -1.0;
                u += du;

                if (v > 0)
                {
                    v -= 0.01;
                }
                else
                {
                    v += 0.01;
                }

            }

            //for (double v = 0.0; v < 20.0; v += dv)      
            //{
            //    for (double u = 5.0; u < 40.0; u += du)
            //    {
                    

            //    }

            //}

            int count = points.Count;
            for (int i = 0; i < count; i++)
            {
                double temp = -((double)points[count - i - 1].Y - Oy) / ((double)WIDTH / scale);
                points.Add(new Point(points[count - i - 1].X, Sy(-temp)));
            }
            g.DrawLines(blackp, points.ToArray());

            cusp = new Bitmap(WIDTH, HEIGHT);
            Graphics gb = Graphics.FromImage(cusp);
            gb.DrawLines(blackp, points.ToArray());
     
            cusp.Save("2.bmp");

        }

        private void pbox_MouseDown(object sender, MouseEventArgs e)
        {
            int xs = Sx(x);
            int ys = Sy(y);

            if (e.X >= xs - controlr && e.X <= xs + controlr && e.Y >= ys - controlr && e.Y <= ys + controlr)
            {
                dragging = true;
                
            }
        }

        private void pbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                x = ((double)e.X - Ox) / ((double)WIDTH / scale);
                y = -((double)e.Y - Oy) / ((double)WIDTH / scale);
            }
        }

        private void pbox_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            WIDTH = this.Width;
            HEIGHT = this.Height;
            Ox = WIDTH / 2 - WIDTH / 8;
            Oy = HEIGHT / 2;
            scale = 60;
            pbox.Width = WIDTH;
            pbox.Height = HEIGHT;


            area = new Bitmap(pbox.Width, pbox.Height);
            g = Graphics.FromImage(area);

        }


    }

}
