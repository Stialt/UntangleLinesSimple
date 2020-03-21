using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UntangleLines
{
    public partial class Form1 : Form
    {

        static int NOT_STARTED = 0;
        static int PLAYING = 1;
        int playState = NOT_STARTED;

        //Flags
        public bool isDebug = false;
        public bool isShowInit = false; //Set to true to see initial graph

        //Global values
        public int N = 0;
        int radius = 5;
        int movedPointIndex = -1;

        //Global Random Generator
        Random random = new Random();
        
        //Global structures
        myPoint[] points;
        int[,] Edges;
        myPoint[] randomPoints;



        //--------------------------------FORM AND CONTROLS---------------------------
        public Form1()
        {
            InitializeComponent();
            this.Width = 1080;
            this.Height = 720;
            this.DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Black);
            Brush myBrush = new SolidBrush(Color.Black);
            

            if (isShowInit) drawGraph(g, points);
            drawGraph(g, randomPoints);
        }
        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse is Down" + e.X + ", " + e.Y);

            if (playState == PLAYING)
            {
                //Pick the closest point
                Point p = new Point(e.X, e.Y);

                int min = distSqr(p, randomPoints[0].getPoint());
                int index = 0;

                for (int i = 1; i < N; i++)
                {
                    int temp = distSqr(p, randomPoints[i].getPoint());
                    if (temp < min)
                    {
                        min = temp;
                        index = i;
                    }
                }
                if (min < 400)
                {
                    movedPointIndex = index;
                    if (isDebug)
                        Console.WriteLine("Closest point is " + index);
                }
            }

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse is moved: " + e.X + ", " + e.Y);
            //Update coordinates of picked point
            if (movedPointIndex != -1 && playState == PLAYING)
            {
                randomPoints[movedPointIndex].X = e.X;
                randomPoints[movedPointIndex].Y = e.Y;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (playState == PLAYING)
            {
                movedPointIndex = -1;
                //Check for intersections
                if (!globalIntersectRandom(Edges))
                {
                    MessageBox.Show("You won!");
                    playState = NOT_STARTED;
                    checkBox1.Enabled = true;
                    buttonStart.Enabled = true;
                    buttonGiveUp.Enabled = false;
                    numericUpDownN.Enabled = true;

                }
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (playState == NOT_STARTED)
            {
                N = Convert.ToInt32(numericUpDownN.Value);
                Console.WriteLine("N is " + N);
                createPuzzle();
                Invalidate();
                playState = PLAYING;
                checkBox1.Enabled = false;
                buttonStart.Enabled = false;
                buttonGiveUp.Enabled = true;
                numericUpDownN.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                isShowInit = true;
                labelInitState.Visible = true;
            }
            else
            {
                isShowInit = false;
                labelInitState.Visible = false;
            }
        }

        private void buttonGiveUp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You lost.");
            playState = NOT_STARTED;
            checkBox1.Enabled = true;
            buttonStart.Enabled = true;
            buttonGiveUp.Enabled = false;
            numericUpDownN.Enabled = true;

        }






        //-------------------------------PUZZLE GENERATION------------------------------

        public void createPuzzle()
        {
            points = initPoints(400, N, new Point(250, 400));

            if (isDebug)
            {
                Console.WriteLine("Points are: ");
                for (int i = 0; i < N; i++)
                {
                    Console.WriteLine(points[i].ID + ": " + points[i].X + ", " + points[i].Y);
                }
            }

            Edges = randomEdges(N, points);

            randomPoints = randomizePoints(points);

            while (!globalIntersectRandom(Edges))
                randomPoints = randomizePoints(points);
        }

        //Swap Points
        public myPoint[] randomizePoints(myPoint[] points)
        {
            myPoint[] randomPoints = new myPoint[N];
            for (int i = 0; i < N; i++)
            {
                randomPoints[i] = new myPoint(points[i]);
            }

            List<int> inds = new List<int>();
            for (int i = 0; i < N; i++)
                inds.Add(i);

            List<int> randInds = new List<int>();
            while(inds.Count > 0)
            {
                int ind = random.Next(inds.Count);
                randInds.Add(inds[ind]);
                inds.RemoveAt(ind);
            }

            if (isDebug)
            {
                Console.WriteLine("Points random indeces:");
                for (int i = 0; i < randInds.Count; i++) {
                    Console.Write(" " + randInds[i]);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < N; i++)
            {
                //randomPoints[i] = points[randInds[i]];
                int shift = 500;
                if (!isShowInit)
                    shift = 0;
                randomPoints[i].X = points[randInds[i]].X + shift;
                randomPoints[i].Y = points[randInds[i]].Y;
            }

            return randomPoints;
        }

        public int[,] randomEdges(int N, myPoint[] points)
        {

            Console.WriteLine("Random Edges initialization");

            int[,] Edges = new int[N, N];
            List<int> edges = new List<int>();
            for (int i = 0; i < N * N; i++)
                edges.Add(i);

            List<int> randOrder = new List<int>();

            if (isDebug)
                Console.WriteLine("Edges count is " + edges.Count);

            for (int i = 0; i < N * N; i++)
            {   
                //1. Take one random index
                int ind = random.Next(edges.Count);

                if (isDebug)
                {
                    Console.Write(" " + edges[ind]);
                }

                //2. Add to randOrder
                randOrder.Add(edges[ind]);

                //3. Remove from edges
                edges.RemoveAt(ind);
                
            }

            if (isDebug)
                Console.WriteLine("\nEdges count is " + edges.Count);

            for (int i = 0; i < N; i++)
            {
                //Edges[i, (i + 1) % N] = Edges[(i + 1) % N, i] = 1;
            }

            if (isDebug)
                for (int i = 0; i < N; i++)
                {
                    for (int j= 0; j < N; j++)
                    {
                        Console.Write(" " + Edges[i, j]);
                    }
                    Console.WriteLine();
                }

            //For each edge in randOrder:
            for (int i = 0; i < randOrder.Count; i++)
            {
                //1. Convert to edge indices in Matrix
                int x = randOrder[i] / N;
                int y = randOrder[i] % N;

                if (x == y) continue;

                if (isDebug)
                {
                    //Console.WriteLine("Edge #" + randOrder[i] + ": (i,j)=(" + x + "," + y + ")");
                }


                //if (Edges[x, y] == 1)
                //    continue;

                //2. Check if will create intersection
                Edges[x, y] = Edges[y,x] = 1;

                //3. If yes: skip
                if (globalIntersect(Edges))
                {
                    Edges[x, y] = Edges[y,x] = 0;
                    //continue;

                    //if (isDebug) Console.WriteLine("BAD Creates Intersection\n");
                }
                else
                {
                    //if (isDebug) Console.WriteLine("GOOD No Intersection\n");
                }


                //4. If no: randomize between 0 and 1
                /*else
                {
                    Edges[x, y] = random.Next(2);
                }*/

            }

            if (isDebug)
            {
                Console.WriteLine("\n\nEdges are: ");
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        Console.Write(" " + Edges[i, j]);
                    }
                    Console.WriteLine();
                }
            }

            return Edges;
        }

        //Initialize points
        private myPoint[] initPoints(int size, int N, Point center)
        {
            myPoint[] points = new myPoint[N];
            points[0] = new myPoint(center.X, center.Y - size/2, 0);

            for (int i = 0; i < N - 1; i++)
            {
                points[i + 1] = rotate_point(center.X, center.Y, 2 * Math.PI / N, points[i]);
                points[i + 1].ID = i + 1;
            }

            return points;
        }

        //Rotate Point aroudn center
        myPoint rotate_point(int cx, int cy, double angle, myPoint point)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);


            myPoint p = new myPoint(point);

            // translate point back to origin:
            p.X -= cx;
            p.Y -= cy;

            // rotate point
            int xnew = (int) Math.Round(p.X * c - p.Y * s);
            int ynew = (int) Math.Round(p.X * s + p.Y * c);

            // translate point back:
            p.X = xnew + cx;
            p.Y = ynew + cy;
            return p;
        }






        //-------------------------DRAWING---------------------------
        private void drawPoint(Graphics g, Point x)
        {
            Brush myBrush = new SolidBrush(Color.Black);
            g.FillEllipse(myBrush, x.X - radius, x.Y - radius, radius * 2, radius * 2);
            
        }

        private void drawPoint(Graphics g, myPoint x)
        {
            Brush myBrush = new SolidBrush(Color.Black);
            g.FillEllipse(myBrush, x.X - radius, x.Y - radius, radius * 2, radius * 2);
        }

        public void drawLine(Graphics g, int x, int y)
        {
            Pen myPen = new Pen(Color.Black);
            /*Console.WriteLine("\nDrawing line between:");
            points[x].print();
            points[y].print();
            Console.WriteLine(points[x].getPoint().X + " " + points[x].getPoint().Y + 
                " "+ points[y].getPoint().X + " " + points[y].getPoint().Y);
                */
            g.DrawLine(myPen, points[x].getPoint(), points[y].getPoint());
        }

        public void drawLine(Graphics g, myPoint x, myPoint y)
        {
            Pen myPen = new Pen(Color.Black);
            /*Console.WriteLine("\nDrawing line between:");
            x.print();
            y.print();
            Console.WriteLine(x.getPoint().X + " " + x.getPoint().Y + " " + y.getPoint().X + " " + y.getPoint().Y);
            */
            g.DrawLine(myPen, x.getPoint(), y.getPoint());
        }

        public void drawGraph(Graphics g, myPoint[] points)
        {
            for (int i = 0; i < N; i++)
            {
                drawPoint(g, points[i]);
            }


            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (Edges[i, j] == 1)
                    {
                        if (isDebug)
                        {
                            //Console.WriteLine("Edge " + i + "," + j + " exists.");
                        }
                        drawLine(g, points[i], points[j]);
                    }
                }
            }
        }







        //----------------------------INTERSECT CHECKING------------------------------
       
        static Boolean onSegment(myPoint p, myPoint q, myPoint r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        static int orientation(myPoint p, myPoint q, myPoint r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/ 
            // for details of below formula. 
            int val = (q.Y - p.Y) * (r.X - q.X) -
                    (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0; // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }
        
        static Boolean doIntersect(myPoint p1, myPoint q1, myPoint p2, myPoint q2)
        {
            // Find the four orientations needed for general and 
            // special cases 
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case 
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases 
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1 
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases 
        }

        bool globalIntersect(int[,] Edges)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Edges[i, j] == 1)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            for (int l = 0; l < N; l++)
                            {
                                //If not same edge
                                if (k == i && l == j || k == j && l == i) continue;

                                //If share point
                                if (i == k || i == l || j == k || j == l) continue;

                                //If edge exists
                                if (Edges[k, l] == 1)
                                {
                                    //Check intersection
                                    if (doIntersect(points[i], points[j], points[k], points[l]))
                                    {
                                        //Console.WriteLine("Intersection between: [" + i + "," + j + "] and [" + k + "," + l + "]");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        bool globalIntersectRandom(int[,] Edges)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Edges[i, j] == 1)
                    {
                        for (int k = 0; k < N; k++)
                        {
                            for (int l = 0; l < N; l++)
                            {
                                //If not same edge
                                if (k == i && l == j || k == j && l == i) continue;

                                //If share point
                                if (i == k || i == l || j == k || j == l) continue;

                                //If edge exists
                                if (Edges[k, l] == 1)
                                {
                                    //Check intersection
                                    if (doIntersect(randomPoints[i], randomPoints[j], randomPoints[k], randomPoints[l]))
                                    {
                                        //Console.WriteLine("Intersection between: [" + i + "," + j + "] and [" + k + "," + l + "]");
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        int distSqr(Point x, Point y)
        {
            return (int)(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));
        }

    }
}
