using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrafikaAlap
{
    public partial class Form1 : Form
    {
        private Graphics g;

        private readonly List<RectangleInfo> rectangles = new List<RectangleInfo>();
        
        private readonly Random random = new Random();

        private int verticalChance = 2;

        private Color[] colors =
        {
            Color.White,
            Color.White,
            Color.White,
            Color.White,
            Color.White,
            Color.Red,
            Color.Yellow,
            Color.Blue
        };

        public Form1()
        {
            InitializeComponent();

            // Keret létrehozása
            rectangles.Add(new RectangleInfo()
            {
                X = 0,
                Y = 0,
                Width = canvas.Width - 1,
                Height = canvas.Height - 1,
                Color = Color.White
            });

            // Vonal irány alapértelmezett értéke
            hScrollBar1.Value = this.verticalChance;
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            this.g = e.Graphics;

            foreach (RectangleInfo rectangle in rectangles)
            {
                g.FillRectangle(new SolidBrush(rectangle.Color), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                g.DrawRectangle(Pens.Black, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
            canvas.Invalidate();
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int clickedIndex = rectangles.FindIndex(rectangle =>
                e.X > rectangle.X && e.X < rectangle.X + rectangle.Width &&
                e.Y > rectangle.Y && e.Y < rectangle.Y + rectangle.Height
            );

            if (clickedIndex == -1)
            {
                return;
            }

            RectangleInfo clickedRectangle = rectangles[clickedIndex];

            rectangles.RemoveAt(clickedIndex);

            SplitRectangleAt(clickedRectangle, new Point(e.X - clickedRectangle.X, e.Y - clickedRectangle.Y));
        }

        private void SplitRectangleAt(RectangleInfo rectangle, Point position)
        {
            if (SelectOrientation() > 0)
            {
                rectangles.Add(new RectangleInfo()
                {
                    X = rectangle.X,
                    Y = rectangle.Y,
                    Width = position.X,
                    Height = rectangle.Height,
                    Color = GenerateRandomColor()
                });

                rectangles.Add(new RectangleInfo()
                {
                    X = rectangle.X + position.X,
                    Y = rectangle.Y,
                    Width = rectangle.Width - position.X,
                    Height = rectangle.Height,
                    Color = GenerateRandomColor()
                });
            }
            else
            {
                rectangles.Add(new RectangleInfo()
                {
                    X = rectangle.X,
                    Y = rectangle.Y,
                    Width = rectangle.Width,
                    Height = position.Y,
                    Color = GenerateRandomColor()
                });

                rectangles.Add(new RectangleInfo()
                {
                    X = rectangle.X,
                    Y = rectangle.Y + position.Y,
                    Width = rectangle.Width,
                    Height = rectangle.Height - position.Y,
                    Color = GenerateRandomColor()
                });
            }
            canvas.Invalidate();
        }

        private int SelectOrientation()
        {
            if(this.verticalChance < -1)
                return -1;
            return random.Next(-1, this.verticalChance);
        }

        private Color GenerateRandomColor()
        {
            return colors[random.Next(0, colors.Length)];
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            this.verticalChance = hScrollBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hScrollBar1.Value = 2;
        }
    }
}
