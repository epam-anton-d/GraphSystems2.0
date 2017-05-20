using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal abstract class Figure
    {
        protected List<Point> points;
        protected double[,] transfMatrix;
        protected Color color;

        public Figure(Color color, List<Point> points)
        {
            this.color = color;
            this.points = points;
            transfMatrix = new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            };
        }

        public List<Point> Points
        {
            get
            {
                return points;
            }
        }

        public double[,] TransfMatrix
        {
            get
            {
                return transfMatrix;
            }
            set
            {
                transfMatrix = value;
            }
        }

        public Color ColorF
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        protected List<Point> UseTransfMatrix()
        {
            List<Point> pointsTransf = new List<Point>();
            int x, y;
            
            for (int i = 0; i < points.Count; i++)
            {
                x = (int)(transfMatrix[0, 0] * points[i].X + transfMatrix[1, 0] * points[i].Y + transfMatrix[2, 0] * 1);
                y = (int)(transfMatrix[0, 1] * points[i].X + transfMatrix[1, 1] * points[i].Y + transfMatrix[2, 1] * 1);
                pointsTransf.Add(new Point(x, y));
            }

            return pointsTransf;
        }

        protected List<Point> UseTransfMatrix(List<Point> points)
        {
            List<Point> pointsTransf = new List<Point>();
            int x, y;

            for (int i = 0; i < points.Count; i++)
            {
                x = (int)(transfMatrix[0, 0] * points[i].X + transfMatrix[1, 0] * points[i].Y + transfMatrix[2, 0] * 1);
                y = (int)(transfMatrix[0, 1] * points[i].X + transfMatrix[1, 1] * points[i].Y + transfMatrix[2, 1] * 1);
                pointsTransf.Add(new Point(x, y));
            }

            return pointsTransf;
        }

        /// <summary>
        /// Получает все точки, принадлежащие отрезку линии.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<Point> GetLinePixels(List<Point> points)
        {
            List<Point> pixels = new List<Point>();
            int dx, dy, Sx = 0, Sy = 0;
            Point pXY;
            int F = 0, Fx = 0, dFx = 0, Fy = 0, dFy = 0;
            dx = points[1].X - points[0].X;
            dy = points[1].Y - points[0].Y;
            bool tr = true;
            Sx = Math.Sign(dx);
            Sy = Math.Sign(dy);
            if (Sx > 0)
                dFx = dy;
            else
                dFx = -dy;
            if (Sy > 0)
                dFy = dx;
            else
                dFy = -dx;
            //x = x1;
            //y = y1;
            pXY = points[0];
            F = 0;
            if (Math.Abs(dx) >= Math.Abs(dy))
            {
                // угол наклона <= 45 градусов
                do
                {
                    pixels.Add(pXY);
                    if (pXY.X == points[1].X)//(x == x2)
                        break;
                    Fx = F + dFx;
                    F = Fx - dFy;
                    pXY.X = pXY.X + Sx; //x = x + Sx;
                    if (Math.Abs(Fx) < Math.Abs(F))
                        F = Fx;
                    else
                        pXY.Y = pXY.Y + Sy;//y = y + Sy;
                } while (tr);
            }
            else // угол наклона > 45 градусов 
            {
                do
                { //Вывести пиксель с координатами х, у
                    pixels.Add(pXY);
                    if (pXY.Y == points[1].Y)//(y == y2)
                        break;
                    Fy = F + dFy;
                    F = Fy - dFx;
                    pXY.Y = pXY.Y + Sy;//y = y + Sy;
                    if (Math.Abs(Fy) < Math.Abs(F))
                        F = Fy;
                    else
                        pXY.X = pXY.X + Sx;//x = x + Sx;
                } while (tr);
            }

            return pixels;
        }

        public abstract List<Point> GetFigurePixels();

        protected List<Point> GetFillUpPixels(List<Point> points)
        {
            int yMin = int.MaxValue,
                yMax = int.MinValue,
                x;

            List<Point> pixels = new List<Point>();

            // Находим верхнюю-нижнюю границы фигуры.
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < yMin)
                {
                    yMin = points[i].Y;
                }
                if (points[i].Y > yMax)
                {
                    yMax = points[i].Y;
                }
            }

            List<int>[] line = new List<int>[yMax - yMin - 1]; // overflow exception
            double[] b = new double[points.Count - 1];
            double[] k = new double[points.Count - 1];
            int dx;

            // Находим коэффициенты линий, ограничивающих фигуру.
            for (int i = 1; i < points.Count; i++)
            {
                dx = points[i].X - points[i - 1].X;

                if (dx == 0)
                {
                    dx = 1;
                }

                b[i - 1] = ((points[i - 1].Y - points[i].Y * points[i - 1].X / points[i].X) * points[i].X) / dx; // divide by zero
                k[i - 1] = (points[i].Y - b[i - 1]) / points[i].X;
            }

            // Находим границы фигуры.
            for (int y = yMin + 1; y < yMax; y++)
            {
                for (int j = 0; j < points.Count - 1; j++)
                {
                    // Здесь косяк!!!!!       //if (((y > point[j].Y) && (y < point[j + 1].Y)) || ((y < point[j].Y) && (y > point[j + 1].Y)))
                    if (((y >= points[j].Y) && (y <= points[j + 1].Y)) || ((y <= points[j].Y) && (y >= points[j + 1].Y)))
                    {
                        x = (int)Math.Round((y - b[j]) / k[j]);

                        if (line[y - yMin - 1] == null)
                        {
                            line[y - yMin - 1] = new List<int>();
                            line[y - yMin - 1].Add(x);
                        }
                        else
                        {
                            line[y - yMin - 1].Add(x);
                        }

                        line[y - yMin - 1].Sort();
                    }
                }

                if (line[y - yMin - 1] != null)
                {
                    for (int i = 1; i < line[y - yMin - 1].Count; i += 2)
                    {
                        if (line[y - yMin - 1][i - 1] != line[y - yMin - 1][i])
                        {
                            pixels.AddRange(GetLinePixels(new List<Point> { new Point(line[y - yMin - 1][i - 1], y), new Point(line[y - yMin - 1][i], y) }));
                        }
                    }
                }

            }

            return pixels;
        }

        private double[,] MultiMatrix(double[,] a, double[,] b)
        {
            int m = a.GetLength(0);
            int n1 = a.GetLength(1);
            int n2 = b.GetLength(0);
            int k = b.GetLength(1);

            if (n1 == n2)
            {
                var result = new double[m, k];
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        for (int z = 0; z < n1; z++)
			            {
                            result[i, j] += a[i, z] * b[z, j];
			            }
                    }
                }

                return result;
            }

            return null;
        }

        private double[,] GetNewXOY(double[,] matrix, Point p)
        {
            var m1 = new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { -p.X, -p.Y, 1 }
            };

            return MultiMatrix(m1, matrix);
        }

        private double[,] GetOldXOY(double[,] matrix, Point p)
        {
            var m2 = new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { p.X, p.Y, 1 }
            };

            return MultiMatrix(matrix, m2);
        }

        public void Spin(double alfa, Point p)
        {
            var spinMatrix = new double[3, 3] 
            {
                { Math.Cos(alfa), -Math.Sin(alfa), 0 },
                { Math.Sin(alfa), Math.Cos(alfa), 0 },
                { 0, 0, 1 }
            };

            transfMatrix = GetNewXOY(transfMatrix, p);
            transfMatrix = MultiMatrix(transfMatrix, spinMatrix);
            transfMatrix = GetOldXOY(transfMatrix, p);
        }

        public void Scale(Point centerOfFigure, double xScale)
        {
            var scaleMatrix = new double[3, 3]
            {
                { xScale, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            };

            transfMatrix = GetNewXOY(transfMatrix, centerOfFigure);
            transfMatrix = MultiMatrix(transfMatrix, scaleMatrix);
            transfMatrix = GetOldXOY(transfMatrix, centerOfFigure);
        }

        public void Mirror(Point mirrorPoint)
        {
            var mirrorMatrix = new double[3, 3]
            {
                { 1, 0, 0 },
                { 0, -1, 0 },
                { 0, 0, 1 }
            };

            transfMatrix = GetNewXOY(transfMatrix, mirrorPoint);
            transfMatrix = MultiMatrix(transfMatrix, mirrorMatrix);
            transfMatrix = GetOldXOY(transfMatrix, mirrorPoint);
        }
    }
}
