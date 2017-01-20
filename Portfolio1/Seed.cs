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
    public partial class Seed : Form
    {
        public Seed()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            numericUpDown1.Value = rand.Next(100000);
        }

        public decimal NewSeed
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

    }
}
