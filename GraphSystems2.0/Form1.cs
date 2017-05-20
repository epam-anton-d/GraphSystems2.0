using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Logic;

namespace GraphSystems2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        InterfaceMakeUp makeUp;
        Graphics drawArea;
        LogicLayer logic;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            makeUp = new InterfaceMakeUp();
            drawArea = CreateGraphics();
            logic = new LogicLayer();

            this.Text = "Графические системы. Вариант №1";

            for (int topCounter = 3; topCounter < 21; topCounter++)
            {
                starTopsBox.Items.Add(topCounter);
            }

            var colors = new Color[] { Color.Blue, Color.Red, Color.Green };
            foreach (var item in colors)
            {
                chooseColorBox.Items.Add(item);
            }
            chooseColorBox.Sorted = true;
            starTopsBox.SelectedIndex = 2;
            chooseColorBox.SelectedIndex = 0;
        }

        private void lineButton_Click(object sender, EventArgs e)
        {
            makeUp.isLineButtonClicked = (makeUp.isLineButtonClicked == false) ? true : false;
            makeUp.isBezieButtonClicked = false;
            makeUp.isTriangleButtonClicked = false;
            makeUp.isStarButtonClicked = false;
            makeUp.isEditButtonClicked = false;

            ChoosePushButtonsColor();
            StopEvents();
            MouseClick += DrawLineClick;
        }

        void DrawLineClick(object sender, MouseEventArgs e)
        {
            logic.clicks.Add(e.Location);
            if (logic.clicks.Count == 2)
            {
                Color color = (Color)chooseColorBox.SelectedItem;
                logic.pen = new Pen(color);
                CreateNewFigure(color, new MyLine(color, new List<Point>(logic.clicks)), logic.pen);
            }
        }

        private void bezieButton_Click(object sender, EventArgs e)
        {
            makeUp.isBezieButtonClicked = (makeUp.isBezieButtonClicked == false) ? true : false;
            makeUp.isLineButtonClicked = false;
            makeUp.isTriangleButtonClicked = false;
            makeUp.isStarButtonClicked = false;
            makeUp.isEditButtonClicked = false;

            ChoosePushButtonsColor();
            StopEvents();
            MouseClick += DrawBezieClick;
            MouseClick += DrawBezieRightClick;
        }

        void DrawBezieRightClick(object sender, MouseEventArgs e)
        {
            if (logic.clicks.Count >= 4 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //logic.figureList.Add(new MyBezie((Color)chooseColorBox.SelectedItem, logic.clicks));
                //logic.pen = new Pen((Color)chooseColorBox.SelectedItem);
                //logic.DrawMyFigure(drawArea, logic.pen, logic.figureList[logic.figureList.Count - 1].GetFigurePixels());
                //logic.clicks.Clear();
                Color color = (Color)chooseColorBox.SelectedItem;
                logic.pen = new Pen(color);
                CreateNewFigure(color, new MyBezie(color, new List<Point>(logic.clicks)), logic.pen);
            }
        }

        void DrawBezieClick(object sender, MouseEventArgs e)
        {
            logic.clicks.Add(e.Location);
        }

        private void triangleButton_Click(object sender, EventArgs e)
        {
            makeUp.isTriangleButtonClicked = (makeUp.isTriangleButtonClicked == false) ? true : false;
            makeUp.isLineButtonClicked = false;
            makeUp.isBezieButtonClicked = false;
            makeUp.isStarButtonClicked = false;
            makeUp.isEditButtonClicked = false;

            ChoosePushButtonsColor();
            StopEvents();
            MouseClick += DrawTriangleClick;
        }

        void DrawTriangleClick(object sender, MouseEventArgs e)
        {
            logic.clicks.Add(e.Location);
            if (logic.clicks.Count == 3)
            {
                Color color = (Color)chooseColorBox.SelectedItem;
                logic.pen = new Pen(color);
                CreateNewFigure(color, new MyTriangle(color, new List<Point>(logic.clicks)), logic.pen);
            }
        }

        private void starButton_Click(object sender, EventArgs e)
        {
            makeUp.isStarButtonClicked = (makeUp.isStarButtonClicked == false) ? true : false;
            makeUp.isLineButtonClicked = false;
            makeUp.isBezieButtonClicked = false;
            makeUp.isTriangleButtonClicked = false;
            makeUp.isEditButtonClicked = false;

            StopEvents();
            ChoosePushButtonsColor();
            MouseClick += DrawStarClick;
        }

        void DrawStarClick(object sender, MouseEventArgs e)
        {
            logic.clicks.Add(e.Location);
            if (logic.clicks.Count == 2)
            {
                Color color = (Color)chooseColorBox.SelectedItem;
                logic.pen = new Pen(color);
                CreateNewFigure(color, new MyStar(color, new List<Point>(logic.clicks), Convert.ToInt32(starTopsBox.SelectedItem)), logic.pen);
            }
        }

        private void intersecButton_Click(object sender, EventArgs e)
        {

        }

        private void symDiffButton_Click(object sender, EventArgs e)
        {

        }

        private void spinButton_Click(object sender, EventArgs e)
        {
            MakeButtonsFalse();
            StopEvents();
            if (logic.firstSelectedFigure != -1)
            {
                MouseClick += MouseOClick;
            }
        }

        void MouseOClick(object sender, MouseEventArgs e)
        {
            MouseClick -= MouseOClick;
            Pen pen = new Pen(Color.Red);

            logic.spinPoint = new Point(e.X, e.Y);
            for (int i = e.X - 1; i <= e.X + 1; i++)
            {
                for (int j = e.Y - 1; j <= e.Y + 1; j++)
                {
                    logic.DrawPoint(drawArea, pen, new Point(i, j));
                }
            }
            
            pen = new Pen(Color.Black);
            drawArea.DrawLine(pen, e.X - 100, e.Y, e.X + 100, e.Y);
            drawArea.DrawLine(pen, e.X, e.Y - 100, e.X, e.Y + 100);
            drawArea.DrawString("0°", logic.myFont, Brushes.Black, new PointF(e.X + 90, e.Y - 20));
            drawArea.DrawString("90°", logic.myFont, Brushes.Black, new PointF(e.X + 10, e.Y - 90));
            drawArea.DrawString("180°", logic.myFont, Brushes.Black, new PointF(e.X - 90, e.Y + 10));
            drawArea.DrawString("270°", logic.myFont, Brushes.Black, new PointF(e.X + 10, e.Y + 90));
            drawArea.DrawString("360°", logic.myFont, Brushes.Black, new PointF(e.X + 70, e.Y + 10));

            MouseClick += MouseAngleClick;
        }

        void MouseAngleClick(object sender, MouseEventArgs e)
        {
            MouseClick -= MouseAngleClick;
            logic.anglePoint = new Point(e.X, e.Y);
            double angle = - Math.Atan2(logic.spinPoint.Y - logic.anglePoint.Y, logic.spinPoint.X - logic.anglePoint.X) + Math.PI;// / Math.PI * 180;

            logic.figureList[logic.firstSelectedFigure].Spin(angle, logic.spinPoint);
            logic.DrawMyFigure(drawArea, new Pen(Color.Black), logic.figureList[logic.firstSelectedFigure].GetFigurePixels());
            Thread.Sleep(5000);

            Refresh();
        }

        private void scaleByOXButton_Click(object sender, EventArgs e)
        {
            MakeButtonsFalse();
            StopEvents();
            if (logic.firstSelectedFigure != -1)
            {
                int yMin = int.MaxValue,
                    xMin = int.MaxValue,
                    yMax = int.MinValue,
                    xMax = int.MinValue;

                foreach (var item in logic.selection)
                {
                    if (item.Y < yMin)
                    {
                        yMin = item.Y;
                    }
                    if (item.Y > yMax)
                    {
                        yMax = item.Y;
                    }
                    if (item.X < xMin)
                    {
                        xMin = item.X;
                    }
                    if (item.X > xMax)
                    {
                        xMax = item.X;
                    }
                }

                logic.centerOfFigure = new Point((xMax + xMin) / 2, (yMax + yMin) / 2);
                logic.xScale = (xMax - logic.centerOfFigure.X);
                drawArea.FillEllipse(Brushes.Black, logic.centerOfFigure.X - 5, logic.centerOfFigure.Y - 5, 10, 10); // DrawEllipse(new Pen(Color.Black), centerOfFigure.X, centerOfFigure.Y, 100, 100);
                MouseClick += MouseScaleClick;
            }
        }

        void MouseScaleClick(object sender, MouseEventArgs e)
        {
            MouseClick -= MouseScaleClick;
            logic.xScale = Math.Abs((e.X - logic.centerOfFigure.X) / logic.xScale);
            logic.figureList[logic.firstSelectedFigure].Scale(logic.centerOfFigure, logic.xScale);
            Refresh();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            makeUp.isEditButtonClicked = (makeUp.isEditButtonClicked == false) ? true : false;
            makeUp.isLineButtonClicked = false;
            makeUp.isBezieButtonClicked = false;
            makeUp.isTriangleButtonClicked = false;
            makeUp.isStarButtonClicked = false;

            ChoosePushButtonsColor();
            StopEvents();
            MouseClick += EditButttonClick;
        }

        void EditButttonClick(object sender, MouseEventArgs e)
        {
            if (logic.firstSelectedFigure != -1 && Form1.ModifierKeys != Keys.Shift)
            {
                logic.firstSelectedFigure = -1;
                Refresh();
            }
            else if (logic.secondSelectedFigure != -1 && Form1.ModifierKeys == Keys.Shift)
            {
                logic.secondSelectedFigure = -1;
                Refresh();
            }
            if (Form1.ModifierKeys == Keys.Shift)
            {
                for (int i = 0; i < logic.figureList.Count; i++)
                {
                    foreach (var pix in logic.figureList[i].GetFigurePixels())
                    {
                        if (Math.Abs(pix.X - e.X) < 5 && Math.Abs(pix.Y - e.Y) < 5)
                        {
                            logic.secondSelectedFigure = i;
                            logic.pen = new Pen(Color.Black);
                            logic.selection = logic.GetSelection(logic.figureList[i]);
                            logic.DrawMyFigure(drawArea, logic.pen, logic.selection);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < logic.figureList.Count; i++)
                {
                    foreach (var pix in logic.figureList[i].GetFigurePixels())
                    {
                        if (Math.Abs(pix.X - e.X) < 5 && Math.Abs(pix.Y - e.Y) < 5)
                        {
                            logic.firstSelectedFigure = i;
                            logic.pen = new Pen(Color.Black);
                            logic.selection = logic.GetSelection(logic.figureList[i]);
                            logic.DrawMyFigure(drawArea, logic.pen, logic.selection);
                            break;
                        }
                    }
                }
            }
        }

        private void mirroButton_Click(object sender, EventArgs e)
        {
            MakeButtonsFalse();
            StopEvents();
            if (logic.firstSelectedFigure != -1)
            {
                MouseClick += MouseMirrorClick;
            }
        }

        void MouseMirrorClick(object sender, MouseEventArgs e)
        {
            MouseClick -= MouseMirrorClick;
            drawArea.DrawLine(new Pen(Color.Black), 0, e.Y, Screen.PrimaryScreen.Bounds.Size.Width, e.Y);
            drawArea.DrawString("Отражение", logic.myFont, Brushes.Black, new PointF(200.0f, e.Y - 20.0f));//new PointF(Screen.PrimaryScreen.Bounds.Size.Width - 200.0f, e.Y - 20.0f));
            logic.figureList[logic.firstSelectedFigure].Mirror(new Point(0, e.Y));

            logic.DrawMyFigure(drawArea, new Pen(Color.Black), logic.figureList[logic.firstSelectedFigure].GetFigurePixels());
            Thread.Sleep(5000);

            Refresh();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            MakeButtonsFalse();
            ChoosePushButtonsColor();
            StopEvents();
            if (logic.firstSelectedFigure != -1)
            {
                logic.figureList.RemoveAt(logic.firstSelectedFigure);
                Refresh();
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            drawArea.Clear(SystemColors.Control);
            StopEvents();
            logic.figureList.Clear();
            MakeButtonsFalse();
        }

        /// <summary>
        /// Задаем цвет нажимных клавиш.
        /// </summary>
        private void ChoosePushButtonsColor()
        {
            lineButton.BackColor = makeUp.ChooseButtonColor(makeUp.isLineButtonClicked);
            bezieButton.BackColor = makeUp.ChooseButtonColor(makeUp.isBezieButtonClicked);
            triangleButton.BackColor = makeUp.ChooseButtonColor(makeUp.isTriangleButtonClicked);
            starButton.BackColor = makeUp.ChooseButtonColor(makeUp.isStarButtonClicked);
            editButton.BackColor = makeUp.ChooseButtonColor(makeUp.isEditButtonClicked);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void MakeButtonsFalse()
        {
            makeUp.MakeButtonsFalse();
            ChoosePushButtonsColor();
        }

        private void StopEvents()
        {
            MouseClick -= DrawLineClick;
            MouseClick -= DrawBezieClick;
            MouseClick -= DrawBezieRightClick;
            MouseClick -= DrawTriangleClick;
            MouseClick -= DrawStarClick;
            MouseClick -= EditButttonClick;
            MouseClick -= MouseOClick;
            MouseClick -= MouseAngleClick;
            MouseClick -= scaleByOXButton_Click;
            logic.clicks.Clear();
        }

        private new void Refresh()
        {
            logic.Refresh(drawArea, logic.figureList);
        }

        private void CreateNewFigure(Color color, Figure figure, Pen pen)
        {
            logic.CreateNewFigure(color, figure, pen, drawArea);
        }
    }
}
