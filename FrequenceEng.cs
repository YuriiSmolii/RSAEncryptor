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
    public partial class FrequenceEng : Form
    {
        public FrequenceEng()
        {
            InitializeComponent();
        }

        private void FrequenceEng_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = ((Form1)Owner).frequenceTableEng;
        }
    }
}
