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
            button1.BackColor = otherForm.BackColor;
            button2.BackColor = GPanel.DefaultForeColor;
            
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

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button2.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button2.BackColor = dlg.Color;
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = button4.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                button4.BackColor = dlg.Color;
            }
        }
        //public string FormTitle
        //{
        //    get
        //    {
        //        return numericUpDown1.Value;
        //    }
        //    set
        //    {
        //        numericUpDown1.Value = value;
        //    }
        //}

    }
}
