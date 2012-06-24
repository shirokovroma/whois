using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace whois
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void canButton_Click(object sender, EventArgs e)
        {
            Config.ActiveForm.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Config.ActiveForm.Close();
        }
    }
}
