namespace my_balls
{
    public class Painter
    {
        Rect r;
        DBHelper dbh;
        private object locker = new();
        private List<Animator> animators = new();
        public List<Rect> rects = new List<Rect>();
        private Size containerSize;
        private Thread t;
        private Graphics mainGraphics;
        private BufferedGraphics bg;
        private bool isAlive, modify;

        private double Dist(Circle a, Circle b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private bool Is_touch(Circle a, Circle b)
        {
            return Dist(a, b) <= a.Diam;
        }

        private volatile int objectsPainted = 0;
        public Thread PainterThread => t;
        public Graphics MainGraphics
        {
            get => mainGraphics;
            set
            {
                lock (locker)
                {
                    mainGraphics = value;
                    ContainerSize = mainGraphics.VisibleClipBounds.Size.ToSize();
                    bg = BufferedGraphicsManager.Current.Allocate(
                        mainGraphics, new Rectangle(new Point(0, 0), ContainerSize)
                    );
                    objectsPainted = 0;
                }
            }
        }

        public Size ContainerSize
        {
            get => containerSize;
            set
            {
                containerSize = value;
                foreach (var animator in animators)
                {
                    animator.ContainerSize = ContainerSize;
                }
            }
        }

        public Painter(Graphics mainGraphics, DBHelper dbhelper)
        {
            dbh = dbhelper;
            MainGraphics = mainGraphics;
        }
        public void AddNew(Rect r, int x, int y)
        {
                var a = new Animator(ContainerSize, x, y, r.Col);
                r.rect_animators.Add(a);
                animators.Add(a);
                a.Start();
            
        }

        public void Start()
        {
            isAlive = true;
            bool delball = false;
            t = new Thread(() =>
            {
                try
                {
                    while (isAlive)
                    {
                        animators.RemoveAll(it => !it.IsAlive);
                        lock (locker)
                        {
                            if (PaintOnBuffer())
                            {
                                bg.Render(MainGraphics);
                                foreach (var ball1 in animators)
                                {
                                    foreach (var ball2 in animators)
                                    {
                                        if (Is_touch(ball1.C, ball2.C)  && ball1.C.Color != ball2.C.Color)
                                        {
                                            animators.Remove(ball1);
                                            foreach(var rect in rects)
                                            {
                                                if (rect.Col == ball2.C.Color)
                                                {
                                                    rect.Score += 1;
                                                    dbh.change_score(rect.Col.Name, rect.Score);
                                                    break;
                                                }
                                            }
                                            delball = true;
                                            break;
                                        }
                                    }
                                    if (delball)
                                    {
                                        break;
                                    }
                                }
                                delball = false;
                            }
                        }
                        //if (isAlive) Thread.Sleep(30);
                    }
                }
                catch (ArgumentException e) { }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            isAlive = false;
            t.Interrupt();
        }

        private bool PaintOnBuffer()
        {
            objectsPainted = 0;
            var objectsCount = animators.Count;
            bg.Graphics.Clear(Color.White);
            foreach (var rect in rects)
            {
                //rect.Score = dbh.select_color_score(rect.Col.ToString(), locker);
                rect.paint_rect(bg.Graphics);
            }
            foreach (var animator in animators)
            {
                animator.PaintCircle(bg.Graphics);
                objectsPainted++;
            }

            return objectsPainted == objectsCount;
        }
    }
}