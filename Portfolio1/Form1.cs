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
        int cells = 0;
        int seed = 4221;
        int timeInt = 20;
        int mX = 30;
        int mY = 30;
        int gridT = 0;
        int random = 0;

        bool[,] Playtemp;
        bool[,] universe;
        bool[,] scratchPad;
        Color gColor = Color.Blue;
        Color cColor = Color.Orange;
        Color dColor = Color.Beige;

        Color gColorTemp = Color.Black;
        Color dColorTemp = Color.Beige;

        Color LiveCellColor = Color.White;
        Color DeadCellColor = Color.Magenta;


        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();

            gColor = Properties.Settings.Default.GridColor;
            dColor = Properties.Settings.Default.GridColorx10;
            gPanel1.BackColor = Properties.Settings.Default.PanelColor;
            cColor = Properties.Settings.Default.CellColor;
            LiveCellColor = Properties.Settings.Default.LiveCellColor;
            DeadCellColor = Properties.Settings.Default.DeadCellColor;
            timeInt = Properties.Settings.Default.TimerInterval;
            mX = Properties.Settings.Default.mXWidth;
            mY = Properties.Settings.Default.mYHeight;
            gridT = Properties.Settings.Default.BoundryT;

            gColorTemp = Properties.Settings.Default.TempGC;
            dColorTemp = Properties.Settings.Default.TempGCx10;


            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];
            Playtemp = new bool[mX, mY];
            timer.Enabled = false;
            timer.Tick += Timer_Tick;

            if (gridT == 0)
            {
                gridT = 2;
            }
            else { };

        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGen();
        
            if (gridT == 3)
            {

            }
            
            gPanel1.Invalidate();
        }
        //Checks to see if in the next generation if it is alive or dead
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
        //Gets the Neighbor Count for the current live cells
        private int NeighborCount(int x, int y)
        {
            int cout = 0;

            #region For
            //for (int nx = x - 1; nx <= x + 1; nx++)
            //{
            //    if (nx < 0 || nx >= gPanel1.ClientSize.Width)
            //    {
            //        continue;
            //    }

            //    for (int ny = y - 1; ny <= y +1 ; ny++)
            //    {
            //        if (ny < 0 || ny >= gPanel1.ClientSize.Height)
            //        {
            //            continue;
            //        }
            //        if (nx ==x || y == ny)
            //        {
            //            continue;
            //        }
            //        if(universe[nx,ny])
            //        cout++;
            //    }
            //}
            #endregion
            //Starts Toroidal
            if (gridT == 1)
            {
                for (int col = y - 1; col <= y + 1; ++col)
                {
                    int tempcol = col;
                    if (col < 0)
                    {
                        tempcol = (universe.GetLength(1) - 1);

                    }
                    else if (col > (universe.GetLength(1) - 1))
                    {
                        tempcol = 0;

                    }
                    for (int row = x - 1; row <= x + 1; row++)
                    {
                        int temprow = row;

                        if (row < 0)
                        {
                            temprow = (universe.GetLength(0) - 1);
                        }
                        else if (row > (universe.GetLength(0) - 1))
                        {
                            temprow = 0;
                        }

                        if (tempcol == y && temprow == x) { }
                        else if (col < 0 || col >= mY || row < 0 || row >= mX)
                        {
                                if (universe[temprow, tempcol] == true)
                                    cout++;
                        }
                    }
                }
            }

            #region If
            if (x < (mX - 1))
            {
                if (universe[x + 1, y] == true)
                    cout++;
            }
            if (x < (mX - 1) && y < (mY - 1))
            {
                if (universe[x + 1, y + 1] == true)
                    cout++;
            }
            if (y < (mY - 1))
            {
                if (universe[x, y + 1] == true)
                    cout++;
            }
            if (y < (mY - 1) && x != 0)
            {
                if (universe[x - 1, y + 1] == true)
                    cout++;
            }
            if (x != 0)
            {
                if (universe[x - 1, y] == true)
                    cout++;
            }
            if (x != 0 && y != 0)
            {
                if (universe[x - 1, y - 1] == true)
                    cout++;
            }
            if (y != 0)
            {
                if (universe[x, y - 1] == true)
                    cout++;
            }

            if (x < (mX - 1) && y != 0)
            {
                if (universe[x + 1, y - 1] == true)
                    cout++;
            }
            #endregion

            return cout;

        }
        //Pains Everything onto The Panel
        private void gPanel1_Paint(object sender, PaintEventArgs e)
        {
            timer.Interval = timeInt;
            cells = 0;

            if (generations != runtogen){ }
            else { timer.Enabled = false; runtogen = -1; }

            if (gColor != Color.Transparent) { gridVisableToolStripMenuItem.Checked = true; }

            float wid = (float)gPanel1.ClientSize.Width / (float)universe.GetLength(0);
            float high = (float)gPanel1.ClientSize.Height / (float)universe.GetLength(1);

            Pen gPen = new Pen(gColor, 1);
            Pen dPen = new Pen(dColor, 2);
            Brush cBrush = new SolidBrush(cColor);
            Brush LiveCellBrush = new SolidBrush(LiveCellColor);
            Brush DeadCellBrush = new SolidBrush(DeadCellColor);
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
                        cells++;
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
                            e.Graphics.DrawString(neighbors.ToString(), font, LiveCellBrush, rect, stringFormat);
                        }
                        else
                        {
                            e.Graphics.DrawString(neighbors.ToString(), font, DeadCellBrush, rect, stringFormat);
                        }
                    }
                }
            }


            if (headsUpVisableToolStripMenuItem.Checked == true || headsUpVisableToolStripMenuItem.Checked == true)
            {
                e.Graphics.DrawString("Generations: " + generations.ToString(), hubF, Brushes.White, 0, gPanel1.Height - 80);
                e.Graphics.DrawString("Cells: " + cells.ToString(), hubF, Brushes.White, 0, gPanel1.Height - 60);

                if (gridT == 1) { e.Graphics.DrawString("Boundry Type: " + "Toroidal", hubF, Brushes.White, 0, gPanel1.Height - 40); }
                else if (gridT == 2) { e.Graphics.DrawString("Boundry Type: " + "Finite", hubF, Brushes.White, 0, gPanel1.Height - 40); }
                else if (gridT == 3) { e.Graphics.DrawString("Boundry Type: " + "Infinite", hubF, Brushes.White, 0, gPanel1.Height - 40); }

                e.Graphics.DrawString("Universe Size: { Width = " + mX.ToString() + " Height = " + mY.ToString() + "}", hubF, Brushes.White, 0, gPanel1.Height - 20);
            }
            else { }

            if (gridT == 1) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Toroidal"; }
            else if (gridT == 2) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed  + " Boundry Type: " + "Finite"; }
            else if (gridT == 3) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Infinite"; }

            dPen.Dispose();
            gPen.Dispose();
            cBrush.Dispose();
        }
        //Initialize painting the live cell when clicking on said loctation
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

            if (gridT == 1) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Toroidal"; }
            else if (gridT == 2) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Finite"; }
            else if (gridT == 3) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Infinite"; }

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
            if (generations == 0)
            {
                Playtemp = new bool[mX, mY];

                for (int y = 0; y < mY; y++)
                {
                    for (int x = 0; x < mX; x++)
                    {
                        Playtemp[x, y] = universe[x, y];
                    }
                }
            }

            timer.Enabled = true;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void StepButton1_Click(object sender, EventArgs e)
        {
            NextGen();

            if (gridT == 3)
            {

            }

            gPanel1.Invalidate();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            runtogen = -1;
            cells = 0;

            if (gridT == 1) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Toroidal"; }
            else if (gridT == 2) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Finite"; }
            else if (gridT == 3) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Infinite"; }

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
            dlg.GridType = gridT;
            dlg.LiveCell = LiveCellColor;
            dlg.DeadCell = DeadCellColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                int oldUniverseY = universe.GetLength(1);
                int oldUniverseX = universe.GetLength(0);

                gColor = dlg.Grid;
                dColor = dlg.Gridx10;
                cColor = dlg.Forgr;
                gPanel1.BackColor = dlg.Backgr;
                LiveCellColor = dlg.LiveCell;
                DeadCellColor = dlg.DeadCell;
                timeInt = (int)dlg.TimerInterval;
                gridT = dlg.GridType;

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

            if (gridT == 1) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Toroidal"; }
            else if (gridT == 2) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed  + " Boundry Type: " + "Finite"; }
            else if (gridT == 3) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Infinite"; }

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            gPanel1.Invalidate();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////
        private void nextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NextGen();

            if (gridT == 3)
            {

            }

            gPanel1.Invalidate();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGen();

            if (gridT == 3)
            {

            }

            gPanel1.Invalidate();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////
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
                timer.Enabled = true;
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
                writer.WriteLine("!Name: " + "Name");

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
                        if(universe[x,y] == true)
                        {
                            writer.Write('O');
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else if(universe[x,y] == false)
                        {
                            writer.Write('.');
                        }
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine();
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        //Finished Open!! :D
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;
            random = 0;

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
                    if(row[0] == '!') { }
                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if(row[0] != '!') { maxHeight++; }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    if(row.Length != maxWidth)
                        maxWidth = row.Length;
                }
                mX = maxWidth;
                mY = maxHeight;
                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                scratchPad = new bool[maxWidth, maxHeight];
                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row[0] == '!') { continue; }
                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        if(row[xPos] == 'O')
                        {
                            universe[xPos, random] = true;
                        }
                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == '.')
                        {
                            universe[xPos, random] = false;
                        }
                    }
                    random++;

                }

                // Close the file.
                reader.Close();
            }
            gPanel1.Invalidate();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;
            random = 0;
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
                    if (row[0] == '!') { }
                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if (row[0] != '!') { maxHeight++; }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    if (row.Length != maxWidth)
                        maxWidth = row.Length;
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
                    if (row[0] == '!') { continue; }
                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.

                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        if (row[xPos] == 'O')
                        {
                            try
                            {
                                universe[xPos, random] = true;
                            }
                            catch (Exception)
                            {
                                ;
                            }
                        }
                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == '.')
                        {

                        }
                    }
                    random++;

                }

                // Close the file.
                reader.Close();
            }
            gPanel1.Invalidate();
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
            Properties.Settings.Default.LiveCellColor = LiveCellColor;
            Properties.Settings.Default.DeadCellColor = DeadCellColor;
            Properties.Settings.Default.TimerInterval = timeInt;
            Properties.Settings.Default.mXWidth = mX;
            Properties.Settings.Default.mYHeight = mY;
            Properties.Settings.Default.BoundryT = gridT;
            gColorTemp = Properties.Settings.Default.TempGC;
            dColorTemp = Properties.Settings.Default.TempGCx10;


            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            gPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gColor = Properties.Settings.Default.GridColor;
            dColor = Properties.Settings.Default.GridColorx10;
            cColor = Properties.Settings.Default.CellColor;
            LiveCellColor = Properties.Settings.Default.LiveCellColor;
            DeadCellColor = Properties.Settings.Default.DeadCellColor;
            Properties.Settings.Default.TempGC = gColorTemp;
            Properties.Settings.Default.TempGCx10 = dColorTemp;


            gPanel1.Invalidate();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            gPanel1.BackColor = Properties.Settings.Default.PanelColor;
            gColor = Properties.Settings.Default.GridColor;
            dColor = Properties.Settings.Default.GridColorx10;
            cColor = Properties.Settings.Default.CellColor;
            LiveCellColor = Properties.Settings.Default.LiveCellColor;
            DeadCellColor = Properties.Settings.Default.DeadCellColor;
            Properties.Settings.Default.TempGC = gColorTemp;
            Properties.Settings.Default.TempGCx10 = dColorTemp;

            gPanel1.Invalidate();
        }

        private void headsUpVisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (headsUpVisableToolStripMenuItem.Checked == true || headsUpVisableToolStripMenuItem1.Checked == true)
            {
                headsUpVisableToolStripMenuItem.Checked = false;
                headsUpVisableToolStripMenuItem1.Checked = false;
                gPanel1.Invalidate();
            }
            else
            {
                headsUpVisableToolStripMenuItem.Checked = true;
                headsUpVisableToolStripMenuItem1.Checked = true;
                gPanel1.Invalidate();
            }
        }

        private void randomizeByTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Random rand = new Random();
            float wid = (float)universe.GetLength(0);
            float high = (float)universe.GetLength(1);

            for (int i = 0; i < rand.Next(); i++)
            {
                float x = rand.Next((int)wid);
                float y = rand.Next((int)high);

                universe[(int)x, (int)y] = !universe[(int)x, (int)y];
            }

            gPanel1.Invalidate();
        }

        private void randomizefromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            float wid = (float)universe.GetLength(0);
            float high = (float)universe.GetLength(1);

            for (int i = 0; i < seed; i++)
            {
                float x = rand.Next((int)wid);
                float y = rand.Next((int)high);

                universe[(int)x, (int)y] = !universe[(int)x, (int)y];
            }

            gPanel1.Invalidate();
        }

        private void randomizefromNewSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Seed dlg = new Seed();

            dlg.BackColor = this.toolStrip1.BackColor;
            dlg.NewSeed = seed;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = (int)dlg.NewSeed;

                Random rand = new Random();
                float wid = (float)universe.GetLength(0);
                float high = (float)universe.GetLength(1);

                for (int i = 0; i < seed; i++)
                {
                    float x = rand.Next((int)wid);
                    float y = rand.Next((int)high);

                    universe[(int)x, (int)y] = !universe[(int)x, (int)y];
                }

                gPanel1.Invalidate();

            }

        }

        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Random colorRand = new Random();
            Color randGridColor     = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randGridx10Color  = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randBackColor     = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randCellColor     = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randLiveCellColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randDeadCellColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));

            gColor = randGridColor;
            dColor = randGridx10Color;
            cColor = randBackColor;
            gPanel1.BackColor = randCellColor;
            LiveCellColor = randLiveCellColor;
            DeadCellColor = randDeadCellColor;

            gPanel1.Invalidate();
        }

        private void resetUniverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            timeInt = Properties.Settings.Default.TimerInterval;

            int oldUniverseY = universe.GetLength(1);
            int oldUniverseX = universe.GetLength(0);

            mX = Properties.Settings.Default.mXWidth;
            mY = Properties.Settings.Default.mYHeight;

            bool[,] temp = new bool[mX, mY];


            if (mY < oldUniverseY)
                oldUniverseY = mY;
            if (mX < oldUniverseX)
                oldUniverseX = mX;

            for (int y = 0; y < oldUniverseY; y++)
            {
                for (int x = 0; x < oldUniverseX; x++)
                {
                    temp[x, y] = universe[x, y];
                }
            }

            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];

            for (int y = 0; y < mY; y++)
            {
                for (int x = 0; x < mX; x++)
                {
                    universe[x, y] = temp[x, y];
                    scratchPad[x, y] = temp[x, y];
                }
            }

            gPanel1.Invalidate();
        }

        private void reloadUniverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            timeInt = Properties.Settings.Default.TimerInterval;


            int oldUniverseY = universe.GetLength(1);
            int oldUniverseX = universe.GetLength(0);

            mX = Properties.Settings.Default.mXWidth;
            mY = Properties.Settings.Default.mYHeight;

            bool[,] temp = new bool[mX, mY];


            if (mY < oldUniverseY)
                oldUniverseY = mY;
            if (mX < oldUniverseX)
                oldUniverseX = mX;

            for (int y = 0; y < oldUniverseY; y++)
            {
                for (int x = 0; x < oldUniverseX; x++)
                {
                    temp[x, y] = universe[x, y];
                }
            }


            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];

            for (int y = 0; y < mY; y++)
            {
                for (int x = 0; x < mX; x++)
                {
                    universe[x, y] = temp[x, y];
                    scratchPad[x, y] = temp[x, y];
                }
            }

            gPanel1.Invalidate();
        }

        private void randomizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            timeInt = rand.Next(100);

            int oldUniverseY = universe.GetLength(1);
            int oldUniverseX = universe.GetLength(0);



            mX = rand.Next(5, 100);
            mY = rand.Next(5, 100);

            bool[,] temp = new bool[mX, mY];


            if (mY < oldUniverseY)
                oldUniverseY = mY;
            if (mX < oldUniverseX)
                oldUniverseX = mX;

            for (int y = 0; y < oldUniverseY; y++)
            {
                for (int x = 0; x < oldUniverseX; x++)
                {
                    temp[x, y] = universe[x, y];
                }
            }

            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];

            for (int y = 0; y < mY; y++)
            {
                for (int x = 0; x < mX; x++)
                {
                    universe[x, y] = temp[x, y];
                    scratchPad[x, y] = temp[x, y];
                }
            }

            gPanel1.Invalidate();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            timer.Stop();
            generations = 0;
            runtogen = -1;
            cells = 0;

            if (gridT == 1) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Toroidal"; }
            else if (gridT == 2) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Finite"; }
            else if (gridT == 3) { toolStripStatusLabel1.Text = "Generations: " + generations.ToString() + " Cells: " + cells.ToString() + " Seed: " + seed + " Boundry Type: " + "Infinite"; }

            universe = new bool[mX, mY];
            scratchPad = new bool[mX, mY];

            for (int y = 0; y < mY; y++)
            {
                for (int x = 0; x < mX; x++)
                {
                    universe[x, y] = Playtemp[x, y];
                    scratchPad[x, y] = Playtemp[x, y];
                }
            }
            gPanel1.Invalidate();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();
            dlg.BackColor = this.toolStrip1.BackColor;
            dlg.Discription = this.toolStrip1.BackColor;
            dlg.DiscriptionText = Color.White;
            dlg.ShowDialog();
        }

    }
}
