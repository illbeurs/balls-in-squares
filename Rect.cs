using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_balls
{
    public class Rect
    {
        static List<String> colors = new List<String>(){ "Black", "Yellow", "Red", "Green", "Blue",
            "Pink", "Purple" };
        public int score {get; set;}
        private object locker = new();
        private Color col;
        private int x;
        private int y;
        public int Score { get; set;}
        public Color Col { get { return col; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public List<Animator> rect_animators = new List<Animator>();

        public Rect(int x, int y)
        {
            this.x = x;
            this.y = y;
            Random r = new Random();
            int i = r.Next(colors.Count);
            this.col = Color.FromName(colors[i]);
            colors.RemoveAt(i);
        }
        public void paint_rect(Graphics g)
        {
                Brush b = new SolidBrush(col);
                Pen p = new Pen(Color.Black, 3);
                g.FillRectangle(b, X - 40, Y - 40, 80, 80);
                g.DrawRectangle(p, X - 40, Y - 40, 80, 80);
                Font f = new Font("Calibri", 30);
                g.DrawString(Score.ToString(), f, new SolidBrush(Color.White), X - 20, Y - 20);
        }
        
       /* public void add_ball()
        {
            *//*if (!flag)
            {
                this.p.Start();
                flag = true;
            }*//*
            p.AddNew(X, Y);
        }*/
    }
}
