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
    public partial class RunTo : Form
    {
        public RunTo()
        {
            InitializeComponent();
        }
        public decimal Run
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = 0; }
        }
    }
}
