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
    public partial class FrequenceDecrypted : Form
    {
        public FrequenceDecrypted()
        {
            InitializeComponent();
        }

        private void FrequenceDecrypted_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = ((Form1)Owner).frequenceTableDecrypted;
        }
    }
}
