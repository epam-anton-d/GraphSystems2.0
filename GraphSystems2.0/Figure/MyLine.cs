using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class MyLine : Figure
    {
        public MyLine(Color color, List<Point> points) : base(color, points)
        {

        }

        public override List<Point> GetFigurePixels()
        {
            List<Point> tempPoints = UseTransfMatrix();
            return base.GetLinePixels(tempPoints);
        }
    }
}
