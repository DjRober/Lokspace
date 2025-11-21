using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lokspace
{
    public partial class MainAlumnoForm : Form
    {
        private Usuario usuario;
        public MainAlumnoForm(Usuario usuario)
        {
            this.usuario = usuario;
            InitializeComponent();
        }

        private void MainAlumnoForm_Load(object sender, EventArgs e)
        {

        }
    }
}
