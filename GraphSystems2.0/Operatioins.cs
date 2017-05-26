using System;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal static class Operatioins
    {
        static public List<Figure> Intersection(Figure figure1, Figure figure2)
        {
            if ((figure1 is MyLine) || (figure1 is MyBezie) || (figure2 is MyLine) || (figure2 is MyBezie))
            {
                return null;
            }
            var points1 = figure1.Points;
            var points2 = figure2.Points;
            var figurePixels1 = figure1.GetFigurePixels();
            var figurePixels2 = figure2.GetFigurePixels();
            //var firstHalfIntersectionCurve = FindHalfIntersectionCurveAlter(points1, points2, figurePixels1, figurePixels2);
            //var SecondHalfIntersectionCurve = FindHalfIntersectionCurveAlter(points2, points1, figurePixels2, figurePixels1);
            var firstHalfIntersectionCurve = FindHalfIntersectionCircuit(points1, points2, figurePixels1, figurePixels2);
            var SecondHalfIntersectionCurve = FindHalfIntersectionCircuit(points2, points1, figurePixels2, figurePixels1);
            var resultFigurePoints = new List<List<PointF>>();
            var resultFigures = new List<Figure>();

            if (firstHalfIntersectionCurve.Count == 0 && SecondHalfIntersectionCurve.Count == 0)
            {
                return null;
            }
            else if (firstHalfIntersectionCurve.Count == 1 && SecondHalfIntersectionCurve.Count == 0)
            {
                resultFigurePoints = firstHalfIntersectionCurve;
            }
            else if (firstHalfIntersectionCurve.Count == 0 && SecondHalfIntersectionCurve.Count == 1)
            {
                resultFigurePoints = SecondHalfIntersectionCurve;
            }
            else 
            {
                if (firstHalfIntersectionCurve.Count >= SecondHalfIntersectionCurve.Count)
                {
                    resultFigurePoints = MakeCloseCircuit(firstHalfIntersectionCurve, SecondHalfIntersectionCurve);
                }
                else
                {
                    resultFigurePoints = MakeCloseCircuit(SecondHalfIntersectionCurve, firstHalfIntersectionCurve);
                }
            }

            for (int i = 0; i < resultFigurePoints.Count; i++)
            {
                if (resultFigurePoints[i].Count == 0)
                {
                    resultFigurePoints.RemoveAt(i);
                    break;
                }
                for (int j = resultFigurePoints[i].Count - 1; j > 0; j--)
                {
                    if (resultFigurePoints[i][j] == resultFigurePoints[i][j - 1])
                    {
                        resultFigurePoints[i].RemoveAt(j);
                    }
                }
                resultFigures.Add(new MyFigure(figure1.ColorF, resultFigurePoints[i]));
            }

            return resultFigures;
        }

        private static List<List<PointF>> MakeCloseCircuit(List<List<PointF>> circuit1, List<List<PointF>> circuit2)
        {
            bool isFirstTurn = true;
            var outputList = new List<List<PointF>>();
            outputList.Add(new List<PointF>());

            while (circuit1.Count != 0)
            {
                if (outputList[outputList.Count - 1].Count == 0 )
                {
                    isFirstTurn = false;
                    outputList[outputList.Count - 1].AddRange(circuit1[0]);
                    circuit1.RemoveAt(0);
                }
                if (isFirstTurn)
                {
                    for (int i = 0; i < circuit1.Count; i++)
                    {
                        if ((Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].X - circuit1[i][0].X) <= 1) && (Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].Y - circuit1[i][0].Y) <= 1))
                        {
                            outputList[outputList.Count - 1].AddRange(circuit1[i]);
                            circuit1.RemoveAt(i);
                            isFirstTurn = false;
                            break;
                        }
                        else if ((Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].X - circuit1[i][circuit1[i].Count - 1].X) <= 1) && (Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].Y - circuit1[i][circuit1[i].Count - 1].Y) <= 1))
                        {
                            circuit1[i].Reverse();
                            outputList[outputList.Count - 1].AddRange(circuit1[i]);
                            circuit1.RemoveAt(i);
                            isFirstTurn = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < circuit2.Count; i++)
                    {
                        if ((Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].X - circuit2[i][0].X) <= 1) && (Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].Y - circuit2[i][0].Y) <= 1))
                        {
                            outputList[outputList.Count - 1].AddRange(circuit2[i]);
                            circuit2.RemoveAt(i);
                            isFirstTurn = true;
                            break;
                        }
                        else if ((Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].X - circuit2[i][circuit2[i].Count - 1].X) <= 1) && (Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].Y - circuit2[i][circuit2[i].Count - 1].Y) <= 1))
                        {
                            circuit2[i].Reverse();
                            outputList[outputList.Count - 1].AddRange(circuit2[i]);
                            circuit2.RemoveAt(i);
                            isFirstTurn = true;
                            break;
                        }
                    }
                }
                if (Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].X - outputList[outputList.Count - 1][0].X) <= 1 && Math.Abs(outputList[outputList.Count - 1][outputList[outputList.Count - 1].Count - 1].Y - outputList[outputList.Count - 1][0].Y) <= 1)
                {
                    outputList.Add(new List<PointF>());
                }
            }

            return outputList;
        }


        private static List<List<PointF>> FindHalfIntersectionCircuit(List<PointF> points1, List<PointF> points2, List<PointF> figurePixels1, List<PointF> figurePixels2)
        {
            PointF pointInterSec;
            var newFigure = new List<List<PointF>>();
            newFigure.Add(new List<PointF>());
            bool intersecAdded = false;
            bool curveIsNotClosed = false;
            for (int i = 1; i < points1.Count; i++)
            {
                for (int j = 1; j < points2.Count; j++)
                {
                    pointInterSec = LineIntersection(points1[i - 1], points1[i], points2[j - 1], points2[j]);

                    if (pointInterSec.X != 0 && pointInterSec.Y != 0)
                    {
                        newFigure[newFigure.Count - 1].Add(pointInterSec);
                        intersecAdded = true;
                    }
                }

                if (IsPointAddictedToTheFigure(points1[i], figurePixels2))
                {
                    newFigure[newFigure.Count - 1].Add(points1[i]);
                    if (i == points1.Count - 1)
                    {
                        curveIsNotClosed = true;
                    }
                }
                else
                {
                    if (intersecAdded)
                    {
                        newFigure.Add(new List<PointF>());
                        intersecAdded = false;
                    }
                }
            }
            if (newFigure[newFigure.Count - 1].Count == 0)
            {
                newFigure.RemoveAt(newFigure.Count - 1);
            }
            if (curveIsNotClosed)
            {
                for (int i = newFigure[newFigure.Count - 1].Count - 1; i >= 0; i--)
                {
                    newFigure[0].Insert(0, newFigure[newFigure.Count - 1][i]);
                }
                newFigure.RemoveAt(newFigure.Count - 1);
            }
            return newFigure;
        }

        private static List<List<PointF>> FindHalfIntersectionCurveAlter(List<PointF> points1, List<PointF> points2, List<PointF> figurePixels1, List<PointF> figurePixels2)
        {
            PointF pointInterSec;
            var newFigure = new List<List<PointF>>();
            //newFigure.Add(new List<PointF>());
            bool insideFigure = false;
            //bool curveIsNotClosed = false;
            var newPoints1 = new List<PointF>();

            // сортировка точек первой фигуры для удобства нахождения пересечений.
            for (int i = 0; i < points1.Count - 1; i++)
            {
                if (!IsPointAddictedToTheFigure(points1[i], points2))
                {
                    for (int j = i; j < points1.Count - 1; j++)
                    {
                        newPoints1.Add(points1[j]);
                    }
                    for (int j = 0; j < i; j++)
                    {
                        newPoints1.Add(points1[j]);
                    }
                    break;
                }
            }

            // Добавляем пересечения.
            for (int i = 1; i < newPoints1.Count; i++)
            {
                for (int j = 1; j < points2.Count; j++)
                {
                    pointInterSec = LineIntersection(newPoints1[i - 1].X, newPoints1[i - 1].Y, newPoints1[i].X, newPoints1[i].Y, points2[j - 1].X, points2[j - 1].Y, points2[j].X, points2[j].Y);
                    if (pointInterSec.X != 0 && pointInterSec.Y != 0)
                    {
                        if (!insideFigure)
                        {
                            newFigure.Add(new List<PointF>());
                        }
                        newFigure[newFigure.Count - 1].Add(pointInterSec);
                        insideFigure = (insideFigure == false) ? true : false;
                    }
                }
                if (insideFigure)
                {
                    newFigure[newFigure.Count - 1].Add(newPoints1[i]);
                }
            }

            // Удаляем пустые вхождения.
            for (int i = newFigure.Count - 1; i >= 0; i--)
            {
                if (newFigure[i].Count == 0)
                {
                    newFigure.RemoveAt(i);
                }
            }

            return newFigure;
        }

        private static PointF LineIntersection(PointF point1, PointF point2, PointF point3, PointF point4)
        {
            return LineIntersection(point1.X, point1.Y, point2.X, point2.Y, point3.X, point3.Y, point4.X, point4.Y);
        }

        private static PointF LineIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            float k1, k2, b1, b2, x, y;

            b1 = FindB(x1, y1, x2, y2);
            k1 = FindK(x2, y2, b1);
            b2 = FindB(x3, y3, x4, y4);
            k2 = FindK(x4, y4, b2);
            x = (b2 - b1) / (k1 - k2);
            y = x * k1 + b1;

            if (b1 == float.NaN || b2 == float.NaN)
            {
                return new PointF(0, 0);
            }
            if ((Math.Sign(x1 - x) != Math.Sign(x2 - x)) && (Math.Sign(y2 - y) != Math.Sign(y1 - y)) && (Math.Sign(x3 - x) != Math.Sign(x4 - x)) && (Math.Sign(y3 - y) != Math.Sign(y4 - y)))
            {
                return new PointF(x, y);
            }

            return new PointF(0, 0);
        }

        private static float FindB(float x1, float y1, float x2, float y2)
        {
            return (y1 - (x1 * y2 / x2)) / (1 - (x1 / x2));
        }

        private static float FindK(float x, float y, float b)
        {
            return (y - b) / x;
        }

        private static bool IsPointAddictedToTheFigure(PointF point, List<PointF> figurePoints)
        {
            foreach (var item in figurePoints)
            {
                if (Math.Abs(item.X - point.X) < 2 && Math.Abs(item.Y - point.Y) < 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
