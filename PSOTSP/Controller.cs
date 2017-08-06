using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSOTSP
{
    public partial class Controller : Form
    {

        private List<SolutionViewer> viewers;

        public Controller(List<SolutionViewer> viewers)
        {
            InitializeComponent();

            this.viewers = viewers;
            
        }

        private void Controller_Load(object sender, EventArgs e)
        {
            foreach (SolutionViewer viewer in viewers)
                viewer.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
