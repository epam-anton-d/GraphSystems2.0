using System;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class MyStar : Figure
    {
        private int tops;

        public MyStar(Color color, List<Point> points, int tops) : base(color, points)
        {
            this.tops = tops;
        }

        private List<Point> FindMyStarTops(List<Point> points, int tops)
        {
            Point center = points[0];
            Point p = points[1];

            double radius = Math.Sqrt(Math.Pow((center.X - p.X), 2) + Math.Pow((center.Y - p.Y), 2));
            tops *= 2;
            Point[] point = new Point[tops + 1];
            point[0].X = center.X;
            point[0].Y = (int)Math.Round(center.Y - radius);

            double alfa = 2 * Math.PI / tops;

            for (int i = 1; i <= ((point.Length - 1) / 2); i++)
            {
                if (i % 2 == 0)
                {
                    point[i].X = (int)Math.Round(center.X + radius * Math.Sin(i * alfa));
                    point[i].Y = (int)Math.Round(center.Y - radius * Math.Cos(i * alfa));
                }
                else
                {
                    point[i].X = (int)Math.Round(center.X + radius * Math.Sin(i * alfa) / 2);
                    point[i].Y = (int)Math.Round(center.Y - radius * Math.Cos(i * alfa) / 2);
                }
                if (point.Length - 1 - i == i)
                {
                    continue;
                }
                point[point.Length - 1 - i].X = 2 * center.X - point[i].X;
                point[point.Length - 1 - i].Y = point[i].Y;
            }

            point[point.Length - 1] = point[0];

            return new List<Point>(point);
        }

        public override List<Point> GetFigurePixels()
        {
            List<Point> tempPoints = FindMyStarTops(points, tops);
            tempPoints = UseTransfMatrix(tempPoints);
            return GetFillUpPixels(tempPoints);
        }
    }
}
