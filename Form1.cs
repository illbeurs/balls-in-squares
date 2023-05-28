namespace my_balls
{
    public partial class Form1 : Form
    {
        DBHelper dbh;
        private bool flag = false;
        bool IsSleep = false;
        private Graphics g;
        private Painter p;
        Thread t;
        private object locker = new();
        public Form1()
        {
            InitializeComponent();
            dbh = new DBHelper();
            g = panel1.CreateGraphics();
            p = new Painter(g, dbh);
            p.Start();
            t = new Thread(() =>
            {
                while (true)
                {
                    if (p.rects.Count > 0)
                    {
                        
                            foreach (var rect in p.rects)
                            {
                                p.AddNew(rect, rect.X, rect.Y);
                                IsSleep = true;
                                Thread.Sleep(2000);
                            }
                            IsSleep = false;
                            Thread.Sleep(100);  // может поспим?
                            
                    
                    }
                    //Thread.Sleep(30);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flag = true;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                while (IsSleep)
                {
                    continue;
                }
                Rect r = new Rect(e.X, e.Y);
                p.rects.Add(r);
                dbh.add_elem(r.Col.Name);
                //p.AddNew(r, r.X, r.Y);
    
            }
            flag = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
          
            p.Stop();
            g = panel1.CreateGraphics();
            p = new Painter(g, dbh);
            p.Start();
        }
    }
}