using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AStar
{
    public partial class NewForm : Form
    {
        public int row;
        public int col;

        public NewForm()
        {
            InitializeComponent();
        }

        private void SetProperty(object sender, EventArgs e)
        {
            row = int.Parse(inputRowText.Text);
            col = int.Parse(inputColText.Text);
        }
    }
}
