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
    public partial class DialogBox : Form
    {
        private Form1 otherForm = new Form1();

        public DialogBox()
        {
            InitializeComponent();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button1.BackColor = dlg.Color;
            }
        }
        public Color Grid
        {
            get { return button1.BackColor; }
            set { button1.BackColor = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button2.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button2.BackColor = dlg.Color;
            }
        }
        public Color Gridx10
        {
            get { return button2.BackColor; }
            set { button2.BackColor = value; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button3.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button3.BackColor = dlg.Color;
            }
        }
        public Color Backgr
        {
            get { return button3.BackColor; }
            set { button3.BackColor = value; }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button4.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button4.BackColor = dlg.Color;
            }
        }
        public Color Forgr
        {
            get { return button4.BackColor; }
            set { button4.BackColor = value; }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button6.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button6.BackColor = dlg.Color;
            }
        }
        public Color LiveCell
        {
            get { return button6.BackColor; }
            set { button6.BackColor = value; }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button7.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button7.BackColor = dlg.Color;
            }
        }
        public Color DeadCell
        {
            get { return button6.BackColor; }
            set { button6.BackColor = value; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Random colorRand = new Random();
            Color randGridColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randGridx10Color = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randBackColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randCellColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randLiveCellColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));
            Color randDeadCellColor = Color.FromArgb(colorRand.Next(256), colorRand.Next(256), colorRand.Next(256));

            button1.BackColor = randGridColor;
            button2.BackColor = randGridx10Color;
            button3.BackColor = randBackColor;
            button4.BackColor = randCellColor;
            button6.BackColor = randLiveCellColor;
            button7.BackColor = randDeadCellColor;

        }

        public decimal TimerInterval
        {
            get{ return numericUpDown1.Value; }
            set{ numericUpDown1.Value = value; }
        }
        public decimal UniWidth
        {
            get{ return numericUpDown3.Value; }
            set{ numericUpDown3.Value = value; }
        }
        public int UniHeight
        {
            get{ return (int)numericUpDown2.Value; }
            set{ numericUpDown2.Value = value; }
        }
        public int GridType
        {
            get
            {
                if (radioButton1.Checked) return 1;

                if (radioButton2.Checked) return 2;

                return 3;
            }

            set
            {
                if (value == 1)
                {
                    radioButton1.Checked = true;
                }
                else if (value == 2)
                {
                    radioButton2.Checked = true;
                }
                else
                {
                    radioButton3.Checked = true;
                }
            }
        }

    }
}
