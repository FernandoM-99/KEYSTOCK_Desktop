namespace KEYSTOCK_Desktop.Formularios
{
    partial class frmRoles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRoles));
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.Roles = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.Roles.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNombreRol
            // 
            this.txtNombreRol.Location = new System.Drawing.Point(19, 62);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.Size = new System.Drawing.Size(100, 20);
            this.txtNombreRol.TabIndex = 0;
            this.txtNombreRol.TextChanged += new System.EventHandler(this.txtNombreRol_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rol";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dgvRoles
            // 
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Location = new System.Drawing.Point(12, 163);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.Size = new System.Drawing.Size(776, 229);
            this.dgvRoles.TabIndex = 2;
            this.dgvRoles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoles_CellClick);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(143, 62);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(156, 24);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(224, 33);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 24);
            this.btnLimpiar.TabIndex = 4;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(143, 33);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 24);
            this.btnEditar.TabIndex = 5;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // Roles
            // 
            this.Roles.Controls.Add(this.txtNombreRol);
            this.Roles.Controls.Add(this.btnEditar);
            this.Roles.Controls.Add(this.label1);
            this.Roles.Controls.Add(this.btnLimpiar);
            this.Roles.Controls.Add(this.btnGuardar);
            this.Roles.Location = new System.Drawing.Point(27, 31);
            this.Roles.Name = "Roles";
            this.Roles.Size = new System.Drawing.Size(331, 113);
            this.Roles.TabIndex = 6;
            this.Roles.TabStop = false;
            this.Roles.Text = "Roles";
            // 
            // frmRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 410);
            this.Controls.Add(this.Roles);
            this.Controls.Add(this.dgvRoles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRoles";
            this.Text = "Roles";
            this.Load += new System.EventHandler(this.frmRoles_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.Roles.ResumeLayout(false);
            this.Roles.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtNombreRol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.GroupBox Roles;
    }
}