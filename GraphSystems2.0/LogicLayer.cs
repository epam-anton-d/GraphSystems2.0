using System;
using System.Collections.Generic;
using System.Drawing;

namespace Logic
{
    internal class LogicLayer
    {
        public List<PointF> clicks;
        public List<Figure> figureList;
        public List<PointF> selection;
        public Pen pen;
        public int firstSelectedFigure;
        public int secondSelectedFigure;
        public Font myFont;
        public PointF spinPoint;
        public PointF anglePoint;
        public PointF centerOfFigure;
        public float xScale;

        public LogicLayer()
        {
            clicks = new List<PointF>();
            figureList = new List<Figure>();
            selection = new List<PointF>();
            pen = new Pen(Color.Red);
            firstSelectedFigure = -1;
            secondSelectedFigure = -1;
            spinPoint = new PointF();
            anglePoint = new PointF();
            myFont = new Font("Arial", 14);
            centerOfFigure = new PointF();
            xScale = 0;
        }

        public void DrawMyFigure(Graphics drawArea, Pen pen, List<PointF> pixels)
        {
            foreach (var pixel in pixels)
            {
                DrawPoint(drawArea, pen, pixel);
            }
        }

        // Вывод точки (квадрата)
        public void DrawPoint(Graphics drawArea, Pen pen, PointF point)
        {
            drawArea.DrawRectangle(pen, point.X, point.Y, 1, 1);
        }

        public void Refresh(Graphics drawArea, List<Figure> figureList)
        {
            drawArea.Clear(SystemColors.Control);

            List<PointF> pixels;

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

        public List<PointF> GetSelection(Figure figure)
        {
            var pixels = figure.GetFigurePixels();

            int xMin = int.MaxValue,
                xMax = int.MinValue,
                yMin = int.MaxValue,
                yMax = int.MinValue;

            foreach (var pixel in pixels)
            {
                if (pixel.X < xMin)
                {
                    xMin = (int)Math.Round(pixel.X);
                }
                if (pixel.X > xMax)
                {
                    xMax = (int)Math.Round(pixel.X);
                }
                if (pixel.Y < yMin)
                {
                    yMin = (int)Math.Round(pixel.Y);
                }
                if (pixel.Y > yMax)
                {
                    yMax = (int)Math.Round(pixel.Y);
                }
            }

            xMin -= 5;
            xMax += 5;
            yMin -= 5;
            yMax += 5;

            pixels.Clear();

            pixels.AddRange(new MyLine(Color.Black, new List<float[]>() { new float[] { xMin, yMin, 1 }, new float[] { xMax, yMin, 1 } }).GetFigurePixels());
            pixels.AddRange(new MyLine(Color.Black, new List<float[]>() { new float[] { xMax, yMax, 1 }, new float[] { xMin, yMax, 1 } }).GetFigurePixels());
            pixels.AddRange(new MyLine(Color.Black, new List<float[]>() { new float[] { xMin, yMax, 1 }, new float[] { xMin, yMin, 1 } }).GetFigurePixels());
            pixels.AddRange(new MyLine(Color.Black, new List<float[]>() { new float[] { xMax, yMin, 1 }, new float[] { xMax, yMax, 1 } }).GetFigurePixels());

            return pixels;
        }
    }
}