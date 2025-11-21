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

        private void Login_Load(object sender, EventArgs e)
        {

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
            switch (usuario.Tipo)
            {
                case "admin":
                    form = new MainAdminForm(usuario);
                    break;

                case "docente":
                    form = new MainDocenteForm(usuario);
                    break;

                case "alumno":
                    form = new MainAlumnoForm(usuario);
                    break;

                default:
                    throw new Exception("Rol no valido");
            }

            form.Show();

        }

    }

    // Modelo de Usuario base - temporal
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Tipo { get; set; } // "admin", "docente", "alumno"
        public bool Activo { get; set; }
    }

    // Servicio de autentificacion
    public class AuthService
    {
        /* Usuario hardcodeados -
         * A la mera hora sera a traves de consultas a la base de datos
         */
        private List<Usuario> usuariosHardcodeados = new List<Usuario>
        {
            new Usuario { Id = 1, Email = "admin@utcj.com", Password = "123456", Tipo = "admin", Nombre = "Admin", Activo = true},
            new Usuario { Id = 2, Email = "docente@utcj.com", Password = "123456", Tipo = "docente", Nombre = "Docente", Activo = true},
            new Usuario { Id = 3, Email = "alumno@utcj.com", Password = "123456", Tipo = "alumno", Nombre = "Alumno", Activo = true}
        };

        public Usuario Login(string email, string password)
        {
            return usuariosHardcodeados.FirstOrDefault(u => u.Email == email && u.Password == password && u.Activo);
        }
    }
}
