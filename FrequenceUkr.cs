using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaesarEncryptor
{
    public partial class FrequenceUkr : Form
    {
        public FrequenceUkr()
        {
            InitializeComponent();
        }

        private void FrequenceUkr_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = ((Form1)Owner).frequenceTableUkr;
        }
    }
}
