namespace Lokspace
{
    partial class MainAlumnoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnCerrarSesion = new System.Windows.Forms.Button();
            this.btnMisReservas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(201, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 458);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // BtnCerrarSesion
            // 
            this.BtnCerrarSesion.Location = new System.Drawing.Point(25, 152);
            this.BtnCerrarSesion.Name = "BtnCerrarSesion";
            this.BtnCerrarSesion.Size = new System.Drawing.Size(97, 23);
            this.BtnCerrarSesion.TabIndex = 1;
            this.BtnCerrarSesion.Text = "Cerrar Sesion";
            this.BtnCerrarSesion.UseVisualStyleBackColor = true;
            this.BtnCerrarSesion.Click += new System.EventHandler(this.BtnCerrarSesion_Click);
            // 
            // btnMisReservas
            // 
            this.btnMisReservas.Location = new System.Drawing.Point(25, 213);
            this.btnMisReservas.Name = "btnMisReservas";
            this.btnMisReservas.Size = new System.Drawing.Size(97, 23);
            this.btnMisReservas.TabIndex = 2;
            this.btnMisReservas.Text = "Mis Reservas";
            this.btnMisReservas.UseVisualStyleBackColor = true;
            this.btnMisReservas.Click += new System.EventHandler(this.btnMisReservas_1);
            // 
            // MainAlumnoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 483);
            this.Controls.Add(this.btnMisReservas);
            this.Controls.Add(this.BtnCerrarSesion);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainAlumnoForm";
            this.Text = "MainAlumnoForm";
            this.Load += new System.EventHandler(this.MainAlumnoForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnCerrarSesion;
        private System.Windows.Forms.Button btnMisReservas;
    }
}