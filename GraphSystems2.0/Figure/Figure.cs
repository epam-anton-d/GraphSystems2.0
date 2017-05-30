using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal abstract class Figure
    {
        //protected List<Point> points;
        protected Color color;
        protected List<float[]> pointsMatrix;

        public Figure(Color color, List<float[]> pointsMatrix)
        {
            this.color = color;
            this.pointsMatrix = pointsMatrix;
        }

        public Figure(Color color, List<PointF> points)
        {
            this.color = color;
            this.Points = points;
        }

        public List<float[]> PointsMatrix
        {
            set
            {
                pointsMatrix = value;
            }
            get
            {
                return pointsMatrix;
            }
        }

        public List<PointF> Points
        {
            get
            {
                var tempPoints = new List<PointF>(pointsMatrix.Count);
                for (int i = 0; i < pointsMatrix.Count; i++)
                {
                    tempPoints.Add(new PointF(pointsMatrix[i][0], pointsMatrix[i][1]));
                }
                return tempPoints;
            }
            set
            {
                pointsMatrix = new List<float[]>(value.Count);
                for (int i = 0; i < value.Count; i++)
                {
                    if (pointsMatrix.Count == value.Count)
                    {
                        pointsMatrix[i] = new float[] { value[i].X, value[i].Y, 1 };
                    }
                    else
                    {
                        pointsMatrix.Add(new float[] { value[i].X, value[i].Y, 1 });
                    }
                }
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

        /// <summary>
        /// Получает все точки, принадлежащие отрезку линии.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<PointF> GetLinePixels(List<PointF> points)
        {
            var pixels = new List<PointF>();
            float dx, dy;
            int Sx = 0, Sy = 0;
            PointF pXY;
            float F = 0, Fx = 0, dFx = 0, Fy = 0, dFy = 0;
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

        public abstract List<PointF> GetFigurePixels();

        protected List<PointF> GetFillUpPixels(List<PointF> points)
        {
            int yMin = int.MaxValue,
                yMax = int.MinValue,
                x;

            var pixels = new List<PointF>();

            // Находим верхнюю-нижнюю границы фигуры.
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < yMin)
                {
                    yMin = (int)Math.Round(points[i].Y);
                }
                if (points[i].Y > yMax)
                {
                    yMax = (int)Math.Round(points[i].Y);
                }
            }

            if (yMax == yMin)
            {
                return points;
            }

            List<float>[] line = new List<float>[yMax - yMin - 1]; // overflow exception
            double[] b = new double[points.Count - 1];
            double[] k = new double[points.Count - 1];
            float dx;

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
                            line[y - yMin - 1] = new List<float>();
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
                            pixels.AddRange(GetLinePixels(new List<PointF> { new PointF(line[y - yMin - 1][i - 1], y), new PointF(line[y - yMin - 1][i], y) }));
                        }
                    }
                }

            }

            return pixels;
        }

        private List<float[]> MultiMatrix(List<float[]> a, List<float[]> b)
        {
            // m - строки.
            int m = a.Count;
            // n1 - столбцы.
            int n1 = a[0].Length;
            int n2 = b.Count;
            int k = b[0].Length;

            if (n1 == n2)
            {
                //var result = new double[m, k];
                var result = new List<float[]>(m);
                for (int i = 0; i < m; i++)
                {
                    result.Add(new float[k]);
                    for (int j = 0; j < k; j++)
                    {
                        for (int z = 0; z < n1; z++)
                        {
                            result[i][j] += a[i][z] * b[z][j];
                        }
                    }
                }

                return result;
            }

            return null;
        }

        private List<float[]> GetNewXOY(List<float[]> matrix, PointF p)
        {
            var m1 = new List<float[]>(3)
            {
                new float[] { 1, 0, 0 },
                new float[] { 0, 1, 0 },
                new float[] { -p.X, -p.Y, 1}
            };

            return MultiMatrix(matrix, m1);
        }

        private List<float[]> GetOldXOY(List<float[]> matrix, PointF p)
        {
            var m2 = new List<float[]>(3)
            {
                new float[] { 1, 0, 0 },
                new float[] { 0, 1, 0 },
                new float[] { p.X, p.Y, 1}
            };

            return MultiMatrix(matrix, m2);
        }

        public void Spin(double alfa, PointF p)
        {
            var spinMatrix = new List<float[]>(3)
            {
                new float[] { Convert.ToSingle(Math.Cos(alfa)), Convert.ToSingle(-Math.Sin(alfa)), 0 },
                new float[] { Convert.ToSingle(Math.Sin(alfa)), Convert.ToSingle(Math.Cos(alfa)), 0 },
                new float[] { 0, 0, 1 }
            };

            for (int i = 0; i < pointsMatrix.Count; i++)
            {
                pointsMatrix[i] = GetNewXOY(new List<float[]> { pointsMatrix[i] }, p)[0];
                pointsMatrix[i] = MultiMatrix(new List<float[]> { pointsMatrix[i] }, spinMatrix)[0];
                pointsMatrix[i] = GetOldXOY(new List<float[]> { pointsMatrix[i] }, p)[0];
            }
        }

        public void Scale(PointF centerOfFigure, float xScale)
        {
            var scaleMatrix = new List<float[]>(3)
            {
                new float[] { xScale, 0, 0 },
                new float[] { 0, 1, 0 },
                new float[] { 0, 0, 1 }
            };

            for (int i = 0; i < pointsMatrix.Count; i++)
            {
                pointsMatrix[i] = GetNewXOY(new List<float[]> { pointsMatrix[i] }, centerOfFigure)[0];
                pointsMatrix[i] = MultiMatrix(new List<float[]> { pointsMatrix[i] }, scaleMatrix)[0];
                pointsMatrix[i] = GetOldXOY(new List<float[]> { pointsMatrix[i] }, centerOfFigure)[0];
            }
        }

        public void Mirror(PointF mirrorPoint)
        {
            var mirrorMatrix = new List<float[]>(3)
            {
                new float[] { 1, 0, 0 },
                new float[] { 0, -1, 0 },
                new float[] { 0, 0, 1 }
            };

            for (int i = 0; i < pointsMatrix.Count; i++)
            {
                pointsMatrix[i] = GetNewXOY(new List<float[]> { pointsMatrix[i] }, mirrorPoint)[0];
                pointsMatrix[i] = MultiMatrix(new List<float[]> { pointsMatrix[i] }, mirrorMatrix)[0];
                pointsMatrix[i] = GetOldXOY(new List<float[]> { pointsMatrix[i] }, mirrorPoint)[0];
            }
        }
    }
}
