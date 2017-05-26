using System;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class MyFigure : Figure
    {
        public MyFigure(Color color, List<PointF> points)
            : base(color, points)
        {

        }

        public override List<PointF> GetFigurePixels()
        {
            return GetFillUpPixels(Points);
        }
    }
}
