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
        public bool hardMode = true;

        //Global values
        public int N = 0;
        int radius = 5;
        int movedPointIndex = -1;

        
        //Global structures
        //myPoint[] points;
        //int[,] Edges;
        //myPoint[] randomPoints;

        Puzzle puzzle = new Puzzle();



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

            if (isShowInit) drawGraph(g, puzzle.points, puzzle.Edges);
            drawGraph(g, puzzle.randomPoints, puzzle.Edges);
        }
        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse is Down" + e.X + ", " + e.Y);

            if (playState == PLAYING)
            {
                //Pick the closest point
                Point p = new Point(e.X, e.Y);

                int min = IntersectUtil.distSqr(p, puzzle.randomPoints[0].getPoint());
                int index = 0;

                for (int i = 1; i < N; i++)
                {
                    int temp = IntersectUtil.distSqr(p, puzzle.randomPoints[i].getPoint());
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
                puzzle.randomPoints[movedPointIndex].X = e.X;
                puzzle.randomPoints[movedPointIndex].Y = e.Y;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (playState == PLAYING)
            {
                movedPointIndex = -1;
                //Check for intersections
                if (!(IntersectUtil.globalIntersect(puzzle.Edges, puzzle.randomPoints, N)))
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
            if (hardMode)
            {
                if (playState == NOT_STARTED)
                {
                    N = Convert.ToInt32(numericUpDownN.Value);
                    puzzle.N = N;

                    Console.WriteLine("N is " + N);
                    //puzzle = new Puzzle();
                    puzzle.createPuzzle(N);
                    
                    Invalidate();
                    playState = PLAYING;
                    checkBox1.Enabled = false;
                    buttonStart.Enabled = false;
                    buttonGiveUp.Enabled = true;
                    numericUpDownN.Enabled = false;
                }
            }
            else
            {
                N = Convert.ToInt32(numericUpDownN.Value);
                puzzle.N = N;

                Console.WriteLine("N is " + N);
                puzzle.createPuzzle(N);

                Invalidate();
                playState = PLAYING;
                buttonGiveUp.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                isShowInit = true;
                puzzle.isShowInit = true;
                labelInitState.Visible = true;
            }
            else
            {
                isShowInit = false;
                puzzle.isShowInit = false;
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
            g.DrawLine(myPen, puzzle.points[x].getPoint(), puzzle.points[y].getPoint());
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

        public void drawGraph(Graphics g, myPoint[] points, int [,] Edges)
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



        private void labelInitState_Click(object sender, EventArgs e){}

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;
            puzzle.createPuzzle(N);

            Invalidate();
        }
    }
}
