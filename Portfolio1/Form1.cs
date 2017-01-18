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
        int cells = 0;

        bool[,] universe;
        bool[,] scratchPad;
        Color gColor = Color.Blue;
        Color cColor = Color.Orange;
        Color dColor = Color.Beige;

        Color gColorTemp = Color.Black;
        Color cColorTemp = Color.Orange;
        Color dColorTemp = Color.Beige;




        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();

            gColor = Properties.Settings.Default.GridColor;
            dColor = Properties.Settings.Default.GridColorx10;
            gPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cColor = Properties.Settings.Default.CellColor;

            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];
            timer.Enabled = false;
            timer.Tick += Timer_Tick;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGen();
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
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
            if (y < (mY - 1)){
                if (universe[x, y + 1] == true)
                    cout++;
            }
            if (y < (mY - 1) && x != 0){
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

            if (x < (mX - 1) && y != 0){
                if (universe[x + 1, y - 1] == true)
                    cout++;
            }

            return cout;

        }
        private void gPanel1_Paint(object sender, PaintEventArgs e)
        {
            timer.Interval = timeInt;
            CellCount();

            if (generations != runtogen){ }
            else { timer.Enabled = false; runtogen = -1; }

            if (gColor != Color.Transparent) { gridVisableToolStripMenuItem.Checked = true; }

            float wid = (float)gPanel1.ClientSize.Width / (float)universe.GetLength(0);
            float high = (float)gPanel1.ClientSize.Height / (float)universe.GetLength(1);

            Pen gPen = new Pen(gColor, 1);
            Pen dPen = new Pen(dColor, 2);
            Brush cBrush = new SolidBrush(cColor);
            Font hubF = new Font("Arial", 12);

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
                    if (x % 10 == 0) { e.Graphics.DrawLine(dPen, rect.X, x, rect.X, rect.Y * Width); }
                    if (y % 10 == 0) { e.Graphics.DrawLine(dPen, y, rect.Y, rect.X * Height, rect.Y); }

                    if (neighborCountVisableToolStripMenuItem.Checked == true)
                    {
                        Font font = new Font("Arial", high / 2);

                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        int neighbors = NeighborCount(x, y);
                        if (neighbors == 0) { }
                        else if (neighbors == 3 || neighbors == 2)
                        {
                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.White, rect, stringFormat);
                        }
                        else
                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Magenta, rect, stringFormat);
                    }
                }
            }

            e.Graphics.DrawString("Generations: " + generations.ToString(), hubF, Brushes.White, 0, gPanel1.Height - 80);
            e.Graphics.DrawString("Cells: " + cells.ToString(), hubF, Brushes.White, 0, gPanel1.Height - 60);
            e.Graphics.DrawString("Boundry Type: " + "Finite", hubF, Brushes.White, 0, gPanel1.Height - 40);
            e.Graphics.DrawString("Universe Size: { Width = " + mX.ToString() + " Height = " + mY.ToString() +"}", hubF, Brushes.White, 0, gPanel1.Height - 20);

            dPen.Dispose();
            gPen.Dispose();
            cBrush.Dispose();
        }
        private void CellCount()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x,y] == true)
                        cells++;
                }
            }
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
            runtogen = -1;
            cells = 0;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
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
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
            gPanel1.Invalidate();
        }
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            runtogen = -1;
            cells = 0;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            gPanel1.Invalidate();
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
            dlg.UniHeight = mY;  
            dlg.UniWidth = mX;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                int oldUniverseY = universe.GetLength(1);
                int oldUniverseX = universe.GetLength(0);

                gColor = dlg.Grid;
                dColor = dlg.Gridx10;
                cColor = dlg.Forgr;
                gPanel1.BackColor = dlg.Backgr;
                timeInt = (int)dlg.TimerInterval;

                bool[,] temp = new bool[(int)dlg.UniWidth, (int)dlg.UniHeight];
                

                if (dlg.UniHeight < oldUniverseY)
                    oldUniverseY = (int)dlg.UniHeight;
                if (dlg.UniWidth < oldUniverseX)
                    oldUniverseX = (int)dlg.UniWidth;

                for (int y = 0; y < oldUniverseY; y++)
                {
                    for (int x = 0; x < oldUniverseX; x++)
                    {
                        temp[x, y] = universe[x,y];
                    }
                }

                mX = (int)dlg.UniWidth;
                mY = (int)dlg.UniHeight;

                universe = new bool[(int)dlg.UniWidth, (int)dlg.UniHeight];
                scratchPad = new bool[(int)dlg.UniWidth, (int)dlg.UniHeight];

                for (int y = 0; y < (int)dlg.UniHeight; y++)
                {
                    for (int x = 0; x < (int)dlg.UniWidth; x++)
                    {
                        universe[x, y] = temp[x, y];
                        scratchPad[x, y] = temp[x, y];
                    }
                }


                gPanel1.Invalidate();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            runtogen = -1;
            cells = 0;
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
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
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
            gPanel1.Invalidate();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGen();
            toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString();
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

        private void gridVisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridVisableToolStripMenuItem.Checked == true || gridVisableToolStripMenuItem1.Checked == true)
            {
                gridVisableToolStripMenuItem.Checked = false;
                gridVisableToolStripMenuItem1.Checked = false;
                gColorTemp = gColor;
                gColor = Color.Transparent;
                dColorTemp = dColor;
                dColor = Color.Transparent;
                gPanel1.Invalidate();
            }
            else {
                gridVisableToolStripMenuItem.Checked = true;
                gridVisableToolStripMenuItem1.Checked = true;
                gColor = gColorTemp;
                dColor = dColorTemp;
                gPanel1.Invalidate();
            }
        }

        private void neighborCountVisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (neighborCountVisableToolStripMenuItem.Checked == true || neighborCountVisableToolStripMenuItem1.Checked == true)
            {
                neighborCountVisableToolStripMenuItem.Checked = false;
                neighborCountVisableToolStripMenuItem1.Checked = false;
                gPanel1.Invalidate();
            }
            else
            {
                neighborCountVisableToolStripMenuItem.Checked = true;
                neighborCountVisableToolStripMenuItem1.Checked = true;
                gPanel1.Invalidate();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.PanelColor = gPanel1.BackColor;
            Properties.Settings.Default.GridColor = gColor;
            Properties.Settings.Default.GridColorx10 = dColor;
            Properties.Settings.Default.CellColor = cColor;
            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            gPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gColor = Properties.Settings.Default.GridColor;
            dColor = Properties.Settings.Default.GridColorx10;
            cColor = Properties.Settings.Default.CellColor;

        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            gPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gColor = Properties.Settings.Default.GridColor;
            dColor = Properties.Settings.Default.GridColorx10;
            cColor = Properties.Settings.Default.CellColor;

        }
    }
}
