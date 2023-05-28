using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace my_balls
{
    public class Animator
    {
        private Circle c;
        public int a = 100;
        public int Dx
        {
            get;
            set;
        }
        private int Dy
        {
            get;
            set;
        }
        public Circle C
        {
            get { return c; }
        }
        private Thread? t = null;
        public bool IsAlive => t == null || t.IsAlive;
        public Size ContainerSize { get; set; }

        public Animator(Size containerSize, int x, int y, Color col)
        {
            int d = 50;
            Random rnd = new Random();
           /* int x = rnd.Next(0, containerSize.Width - d);
            int y = rnd.Next(0, containerSize.Height - d);*/
            c = new Circle(d, x, y, col);

            ContainerSize = containerSize;
        }

        public void Start()
        {
            Random rnd = new Random();
            do
            {
                Dx = rnd.Next(-10, 10);
                Dy = rnd.Next(-10, 10);
            } while (Dx == 0 && Dy == 0);
            int normal = Convert.ToInt32(Math.Sqrt(Dx * Dx + Dy * Dy));

            t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(30);
                    c.Move((Dx * 10) / normal, (Dy * 10) / normal);
                    is_wall(Dx, Dy);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void is_wall(int dx, int dy)
        {
            if (c.X + c.Diam >= ContainerSize.Width || c.X <= 0)
            {
                Dx = -dx;
            }
            if (c.Y + c.Diam >= ContainerSize.Height || c.Y <= 0)
            {
                Dy = -dy;
            }
        }

        public void PaintCircle(Graphics g)
        {
            c.Paint(g);
        }
    }
}