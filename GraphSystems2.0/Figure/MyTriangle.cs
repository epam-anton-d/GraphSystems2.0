using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class MyTriangle : Figure
    {
        public MyTriangle(Color color, List<Point> points) : base(color, points)
        {

        }

        private List<Point> FindMyIsotriangleTops(List<Point> points)
        {
            int yFant;
            int xFant;

            points.Add(points[0]);
            xFant = (int)(Math.Round((2 * points[0].Y * points[2].Y - Math.Pow(points[0].X, 2) - Math.Pow(points[0].Y, 2) + Math.Pow(points[1].X, 2) + Math.Pow(points[1].Y, 2) - 2 * points[1].Y * points[2].Y) / (2 * (points[1].X - points[0].X))));
            yFant = (int)(Math.Round((2 * points[1].X * points[2].X - 2 * points[0].X * points[2].X + Math.Pow(points[0].X, 2) + Math.Pow(points[0].Y, 2) - Math.Pow(points[1].X, 2) - Math.Pow(points[1].Y, 2)) / (2 * (points[0].Y - points[1].Y))));

            if (Math.Abs(xFant - points[2].X) < Math.Abs(yFant - points[2].Y))
            {
                points[2] = new Point(xFant, points[2].Y);
                //points.Add(new Point(xFant, points[2].Y));
            }
            else
            {
                points[2] = new Point(points[2].X, yFant);
                //points.Add(new Point(points[2].X, yFant));
            }
            //points.RemoveAt(2);

            return points;
        }

        public override List<Point> GetFigurePixels()
        {
            List<Point> tempPoints = FindMyIsotriangleTops(points);
            tempPoints = UseTransfMatrix(tempPoints);
            return GetFillUpPixels(tempPoints);
        }
    }
}
