using MySql.Data.MySqlClient;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Atributos:
        private AuthService authService = new AuthService(); // Autentificador

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var usuario = authService.Login(txtEmail.Text, txtPassword.Text);

            if(usuario != null)
            {
                this.Hide();
                MostrarInterfazSegunRol(usuario);
            }
            else
            {
                MessageBox.Show("Credenciales invalidas");
            }
        }

        private void MostrarInterfazSegunRol(Usuario usuario)
        {
            Form form;
            switch (usuario.id_rol)
            {
                case 1:
                    form = new MainAdminForm(usuario);
                    break;

                case 2:
                    form = new MainDocenteForm(usuario);
                    break;

                case 3:
                    form = new MainAlumnoForm(usuario);
                    break;

                default:
                    throw new Exception("Rol no valido");
            }

            form.Show();

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }

    // Servicio de autentificacion
    public class AuthService
    {
        private UsuarioService usuarioService = new UsuarioService();

        public Usuario Login(string email, string password)
        {
            // Validacion basica
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, ingresa email y contraseña");
                return null;
            }

            // Buscar usuario en la base de datos
            return this.usuarioService.Login(email.Trim(), password);
        }
    }
}
