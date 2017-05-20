using System;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class LogicLayer
    {
        public List<Point> clicks;
        public List<Figure> figureList;
        public List<Point> selection;
        public Pen pen;
        public int firstSelectedFigure;
        public int secondSelectedFigure;
        public Point spinPoint;
        public Font myFont;
        public Point anglePoint;
        public Point centerOfFigure;
        public double xScale;

        public LogicLayer()
        {
            clicks = new List<Point>();
            figureList = new List<Figure>();
            selection = new List<Point>();
            pen = new Pen(Color.Red);
            firstSelectedFigure = -1;
            secondSelectedFigure = -1;
            spinPoint = new Point();
            anglePoint = new Point();
            myFont = new Font("Arial", 14);
            centerOfFigure = new Point();
            xScale = 0;
        }

        public void DrawMyFigure(Graphics drawArea, Pen pen, List<Point> pixels)
        {
            foreach (var pixel in pixels)
            {
                DrawPoint(drawArea, pen, pixel);
            }
        }

        // Вывод точки (квадрата)
        public void DrawPoint(Graphics drawArea, Pen pen, Point point)
        {
            drawArea.DrawRectangle(pen, point.X, point.Y, 1, 1);
        }

        public void Refresh(Graphics drawArea, List<Figure> figureList)
        {
            drawArea.Clear(SystemColors.Control);

            List<Point> pixels;

            foreach (var figure in figureList)
            {
                pixels = figure.GetFigurePixels();
                DrawMyFigure(drawArea, new Pen(figure.ColorF), pixels);
            }
        }

        public void CreateNewFigure(Color color, Figure figure, Pen pen, Graphics drawArea)
        {
            figureList.Add(figure);
            this.pen = pen;
            DrawMyFigure(drawArea, pen, figureList[figureList.Count - 1].GetFigurePixels());
            clicks.Clear();
        }

        public List<Point> GetSelection(Figure figure)
        {
            List<Point> pixels = figure.GetFigurePixels();

            int xMin = int.MaxValue,
                xMax = int.MinValue,
                yMin = int.MaxValue,
                yMax = int.MinValue;

            foreach (var pixel in pixels)
            {
                if (pixel.X < xMin)
                {
                    xMin = pixel.X;
                }
                if (pixel.X > xMax)
                {
                    xMax = pixel.X;
                }
                if (pixel.Y < yMin)
                {
                    yMin = pixel.Y;
                }
                if (pixel.Y > yMax)
                {
                    yMax = pixel.Y;
                }
            }

            xMin -= 5;
            xMax += 5;
            yMin -= 5;
            yMax += 5;

            pixels.Clear();

            pixels.AddRange(new MyLine(Color.Black, new List<Point>() { new Point(xMin, yMin), new Point(xMax, yMin) }).GetFigurePixels());
            pixels.AddRange(new MyLine(Color.Black, new List<Point>() { new Point(xMax, yMin), new Point(xMax, yMax) }).GetFigurePixels());
            pixels.AddRange(new MyLine(Color.Black, new List<Point>() { new Point(xMax, yMax), new Point(xMin, yMax) }).GetFigurePixels());
            pixels.AddRange(new MyLine(Color.Black, new List<Point>() { new Point(xMin, yMax), new Point(xMin, yMin) }).GetFigurePixels());

            return pixels;
        }
    }
}