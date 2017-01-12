using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Portfolio1
{
    public partial class Form1 : Form
    {
        int generations = 0;
        int runtogen = -1;
        int timeInt = 20;
        int mX = 30;
        int mY = 30;

        bool[,] universe;
        bool[,] scratchPad;
        Color gColor = Color.Black;
        Color cColor = Color.Orange;
        Color dColor = Color.Beige;
        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();

            timer.Interval = timeInt;
            timer.Enabled = false;
            timer.Tick += Timer_Tick;
            toolStripStatusLabel1.Text = "Generations: 0";

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];
            NextGen();
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            gPanel1.Invalidate();
        }
        private void NextGen()
        {
            generations++;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x,y] == true && NeighborCount(x,y) < 2 || NeighborCount(x, y) > 3)
                    {
                        scratchPad[x, y] = false;
                    }
                    else if (universe[x, y] == true && NeighborCount(x, y) == 2 ||  NeighborCount(x, y) == 3)
                    {
                        scratchPad[x, y] = true;
                    }
                    else if (universe[x, y] == false && NeighborCount(x, y) == 3)
                    {
                        scratchPad[x, y] = true;
                    }
                    else
                    {
                        scratchPad[x, y] = false;
                    }
                }
            }
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;
        }
        private int NeighborCount(int x, int y)
        {
            int cout = 0;

            if(x < (mX - 1)){
                if (universe[x + 1, y] == true)
                    cout++;
            }
            if (x < (mX - 1) && y < (mY - 1)){
                if (universe[x + 1, y + 1] == true)
                    cout++;
            }
            if (y < 29){
                if (universe[x, y + 1] == true)
                    cout++;
            }
            if (y < 29 && x != 0){
                if (universe[x - 1, y + 1] == true)
                    cout++;
            }
            if (x != 0){
                if (universe[x - 1, y] == true)
                    cout++;
            }
            if (x != 0 && y != 0){
                if (universe[x - 1, y - 1] == true)
                    cout++;
            }
            if (y != 0){
                if (universe[x, y - 1] == true)
                    cout++;
            }

            if (x < 29 && y != 0){
                if (universe[x + 1, y - 1] == true)
                    cout++;
            }

            return cout;

        }
        private void gPanel1_Paint(object sender, PaintEventArgs e)
        {
            if (generations != runtogen){ }
            else { timer.Enabled = false; runtogen = -1; }

            float wid = (float)gPanel1.ClientSize.Width / (float)universe.GetLength(0);
            float high = (float)gPanel1.ClientSize.Height / (float)universe.GetLength(1);

            Pen gPen = new Pen(gColor, 1);
            Pen dPen = new Pen(dColor, 2);
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
                    if (x % 10 == 0) { e.Graphics.DrawLine(dPen, rect.X, x, rect.X, rect.Y* Width); }
                    if (y % 10 == 0) { e.Graphics.DrawLine(dPen, y, rect.Y, rect.X * Height, rect.Y); }

                    Font font = new Font("Arial", high / 2);
                    
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    
                    int neighbors = NeighborCount(x,y);
                    if (neighbors == 0) { }
                    else if(neighbors == 3 || neighbors == 2)
                    {
                        e.Graphics.DrawString(neighbors.ToString(), font, Brushes.White, rect, stringFormat);
                    }
                    else
                        e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rect, stringFormat);
                }
            }
            gPen.Dispose();
            cBrush.Dispose();
        }

        private void gPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float wid = gPanel1.ClientSize.Width / (float)universe.GetLength(0);
                float high = gPanel1.ClientSize.Height / (float)universe.GetLength(1);

                float x = e.X / wid;
                float y = e.Y / high;

                universe[(int)x, (int)y] = !universe[(int)x, (int)y];
                
                gPanel1.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            gPanel1.Invalidate();
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
            NextGen();
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            gPanel1.Invalidate();
        }
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            runtogen = -1;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            gPanel1.Invalidate();
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

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogBox dlg = new DialogBox();
            RunTo run = new RunTo();
            run.BackColor = this.toolStrip1.BackColor;

            dlg.BackColor = this.toolStrip1.BackColor;
            dlg.Backgr = gPanel1.BackColor;
            dlg.Forgr = cColor;
            dlg.Grid = gColor;
            dlg.Gridx10 = dColor;
            dlg.TimerInterval = timeInt;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                gColor = dlg.Grid;
                dColor = dlg.Gridx10;
                cColor = dlg.Forgr;
                gPanel1.BackColor = dlg.Backgr;
                timeInt = (int)dlg.TimerInterval;


                gPanel1.Invalidate();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            runtogen = -1;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            gPanel1.Invalidate();
        }

        private void nextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NextGen();
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            gPanel1.Invalidate();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGen();
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString();
            gPanel1.Invalidate();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void pauseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void runToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTo dlg = new RunTo();

            dlg.BackColor = this.toolStrip1.BackColor;
            dlg.Run = runtogen;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                runtogen = (int)dlg.Run;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.

                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.

                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                    }
                }

                // Close the file.
                reader.Close();
            }
        }
    }
}
