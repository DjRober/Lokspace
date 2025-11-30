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
    public partial class ReservasPersonalesDocente : Form
    {

        public ReservasPersonalesDocente()
        {
            InitializeComponent();
        }


        private void ReservasPersonalesDocente_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnNuevaReserva_Click(object sender, EventArgs e)
        {
            new NuevaReservaDocente().ShowDialog();
            this.Hide();
        }

        private void listaReservasDocente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
