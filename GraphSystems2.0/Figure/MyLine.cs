using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class MyLine : Figure
    {
        public MyLine(Color color, List<float[]> points) : base(color, points)
        {

        }

        public MyLine(Color color, List<PointF> points) : base(color, points)
        {

        }

        public override List<PointF> GetFigurePixels()
        {
            return base.GetLinePixels(Points);
        }
    }
}
