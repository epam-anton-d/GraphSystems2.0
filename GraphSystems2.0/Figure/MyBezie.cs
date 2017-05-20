using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class MyBezie : Figure
    {
        public MyBezie(Color color, List<PointF> points) : base(color, points)
        {

        }

        /// <summary>
        /// Получает все точки, принадлежащие кривой Безье.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public List<PointF> GetBeziePixels(List<PointF> points)
        {
            var pXY = new PointF[100];
            var fXY = new PointF[100];
            var pixels = new List<PointF>();
            double x = 0,
                   y = 0;
            double t = 0;
            int n = points.Count;

            double del = 0.02;
            
            for (int i = 0; i < 50; i++)
            {
                t = i * del;
                x = 0;
                y = 0;
                for (int j = 0; j < n; j++)
                {
                    x += Fact(n - 1) / (Fact(j) * Fact(n - 1 - j)) * Math.Pow(t, j) * Math.Pow((1 - t), (n - 1 - j)) * points[j].X;
                    y += Fact(n - 1) / (Fact(j) * Fact(n - 1 - j)) * Math.Pow(t, j) * Math.Pow((1 - t), (n - 1 - j)) * points[j].Y;
                }

                fXY[i].X = (int)Math.Round(Math.Pow((1 - t), 3) * points[0].X + 3 * Math.Pow((1 - t), 2) * t * points[1].X + 3 * (1 - t) * Math.Pow((t), 2) * points[2].X + t * t * t * points[3].X);
                fXY[i].Y = (int)Math.Round(Math.Pow((1 - t), 3) * points[0].Y + 3 * Math.Pow((1 - t), 2) * t * points[1].Y + 3 * (1 - t) * Math.Pow((t), 2) * points[2].Y + t * t * t * points[3].Y);

                pXY[i].X = (int)Math.Round(x);
                pXY[i].Y = (int)Math.Round(y);
                 
                if (i != 0)
                {
                    pixels.AddRange(GetLinePixels(new List<PointF> { pXY[i - 1], pXY[i] }));
                }
            }

            //TODO: Удалить задвойки точек.

            return pixels;
        }

        public override List<PointF> GetFigurePixels()
        {
            return GetBeziePixels(Points);
        }

        private int Fact(int x)
        {
            if (x == 0)
            {
                return 1;
            }
            if (x < 0)
            {
                throw new NotImplementedException();
            }

            while (x > 2)
            {
                return x * Fact(x - 1);
            }
            
            return x;
        }
    }
}
