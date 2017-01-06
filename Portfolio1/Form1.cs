using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portfolio1
{
    public partial class Form1 : Form
    {
        bool[,] universe = new bool[12, 6];
        Color gColor = Color.Black;
        Color cColor = Color.Orange;
        Timer timer = new Timer();
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            timer.Interval = 20;
            timer.Enabled = true;
            timer.Tick += Timer_Tick;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            generations++;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            gPanel1.Invalidate();
        }

        private void gPanel1_Paint(object sender, PaintEventArgs e)
        {
            int wid = gPanel1.ClientSize.Width / universe.GetLength(0);
            int high = gPanel1.ClientSize.Height / universe.GetLength(1);

            Pen gPen = new Pen(gColor, 1);
            Brush cBrush = new SolidBrush(cColor);

            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    Rectangle rect = Rectangle.Empty;
                    rect.X = x * wid;
                    rect.Y = y * high;
                    rect.Width = wid;
                    rect.Height = high;

                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cBrush, rect);
                    }

                    e.Graphics.DrawRectangle(gPen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
            gPen.Dispose();
            cBrush.Dispose();
        }

        private void gPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int wid = gPanel1.ClientSize.Width / universe.GetLength(0);
                int high = gPanel1.ClientSize.Height / universe.GetLength(1);

                int x = e.X / wid;
                int y = e.Y / high;

                universe[x, y] = !universe[x, y];

                gPanel1.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
