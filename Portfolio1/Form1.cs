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
            timer.Enabled = false;
            timer.Tick += Timer_Tick;
            toolStripStatusLabel1.Text = "Generations: 0";

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            generations++;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            gPanel1.Invalidate();
        }
        private void NextGen(object sender, PaintEventArgs e)
        {
            Brush cBrush = new SolidBrush(cColor);
            int cout = 0;

            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {

                    if (universe[x - 1, y - 1] == true)
                    {
                        cout++;
                    }
                    if (universe[x - 1, y] == true)
                    {
                        cout++;
                    }
                    if (universe[x - 1, y + 1] == true)
                    {
                        cout++;
                    }
                    if (universe[x, y - 1] == true)
                    {
                        cout++;
                    }
                    if (universe[x + 1, y - 1] == true)
                    {
                        cout++;
                    }
                    if (universe[x - 1, y - 1] == true)
                    {
                        cout++;
                    }
                    gPanel1.Invalidate();
                }
            }

        }

        private void gPanel1_Paint(object sender, PaintEventArgs e)
        {
            float wid = (float)gPanel1.ClientSize.Width / (float)universe.GetLength(0);
            float high = (float)gPanel1.ClientSize.Height / (float)universe.GetLength(1);

            Pen gPen = new Pen(gColor, 1);
            Brush cBrush = new SolidBrush(cColor);

            for (int y = 0; y < universe.GetLength(1); y++)
            {

                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    RectangleF rect = RectangleF.Empty;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;

        }

    

        private void PauseButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;

        }

        private void StepButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            timer.Tick += Timer_Tick;
            timer.Enabled = false;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = gPanel1.BackColor;

            if(DialogResult.OK == dlg.ShowDialog())
            {
               gPanel1.BackColor = dlg.Color;
            }
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogBox dlg = new DialogBox();
            
            if (DialogResult.OK == dlg.ShowDialog())
            {
                
            }

        }
    }
}
