﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntangleLines
{
    public class myPoint
    {
        public int X, Y, ID;
        public myPoint(int x, int y, int id)
        {
            X = x;
            Y = y;
            ID = id;
        }

        public myPoint(myPoint a)
        {
            X = a.X;
            Y = a.Y;
            ID = a.ID;
        }

        public void print()
        {
            Console.WriteLine("ID, X, Y: " + ID + ", " + X + ", " + Y);
        }

        public System.Drawing.Point getPoint()
        {
            return new System.Drawing.Point(X, Y);
        }
    }
}
