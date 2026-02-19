namespace KEYSTOCK_Desktop.Formularios
{
    partial class frmVinculoProv
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVinculoProv));
            this.cmbProductos = new System.Windows.Forms.ComboBox();
            this.cmbProveedores = new System.Windows.Forms.ComboBox();
            this.txtSKUProveedor = new System.Windows.Forms.TextBox();
            this.txtCosto = new System.Windows.Forms.TextBox();
            this.dgvVinculos = new System.Windows.Forms.DataGridView();
            this.btnVincular = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SKUProveedor = new System.Windows.Forms.Label();
            this.Costo = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVinculos)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbProductos
            // 
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new System.Drawing.Point(5, 58);
            this.cmbProductos.Name = "cmbProductos";
            this.cmbProductos.Size = new System.Drawing.Size(121, 21);
            this.cmbProductos.TabIndex = 0;
            this.cmbProductos.SelectedIndexChanged += new System.EventHandler(this.cmbProductos_SelectedIndexChanged);
            // 
            // cmbProveedores
            // 
            this.cmbProveedores.FormattingEnabled = true;
            this.cmbProveedores.Location = new System.Drawing.Point(5, 109);
            this.cmbProveedores.Name = "cmbProveedores";
            this.cmbProveedores.Size = new System.Drawing.Size(121, 21);
            this.cmbProveedores.TabIndex = 1;
            this.cmbProveedores.SelectedIndexChanged += new System.EventHandler(this.cmbProveedores_SelectedIndexChanged);
            // 
            // txtSKUProveedor
            // 
            this.txtSKUProveedor.Location = new System.Drawing.Point(161, 110);
            this.txtSKUProveedor.Name = "txtSKUProveedor";
            this.txtSKUProveedor.Size = new System.Drawing.Size(100, 20);
            this.txtSKUProveedor.TabIndex = 2;
            // 
            // txtCosto
            // 
            this.txtCosto.Location = new System.Drawing.Point(161, 59);
            this.txtCosto.Name = "txtCosto";
            this.txtCosto.Size = new System.Drawing.Size(100, 20);
            this.txtCosto.TabIndex = 3;
            this.txtCosto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCosto_KeyPress);
            // 
            // dgvVinculos
            // 
            this.dgvVinculos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVinculos.Location = new System.Drawing.Point(12, 183);
            this.dgvVinculos.Name = "dgvVinculos";
            this.dgvVinculos.Size = new System.Drawing.Size(766, 255);
            this.dgvVinculos.TabIndex = 4;
            // 
            // btnVincular
            // 
            this.btnVincular.Location = new System.Drawing.Point(312, 82);
            this.btnVincular.Name = "btnVincular";
            this.btnVincular.Size = new System.Drawing.Size(75, 23);
            this.btnVincular.TabIndex = 5;
            this.btnVincular.Text = "Vincular";
            this.btnVincular.UseVisualStyleBackColor = true;
            this.btnVincular.Click += new System.EventHandler(this.btnVincular_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Productos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Proveedores";
            // 
            // SKUProveedor
            // 
            this.SKUProveedor.AutoSize = true;
            this.SKUProveedor.Location = new System.Drawing.Point(176, 87);
            this.SKUProveedor.Name = "SKUProveedor";
            this.SKUProveedor.Size = new System.Drawing.Size(78, 13);
            this.SKUProveedor.TabIndex = 9;
            this.SKUProveedor.Text = "SKUProveedor";
            // 
            // Costo
            // 
            this.Costo.AutoSize = true;
            this.Costo.Location = new System.Drawing.Point(176, 33);
            this.Costo.Name = "Costo";
            this.Costo.Size = new System.Drawing.Size(34, 13);
            this.Costo.TabIndex = 8;
            this.Costo.Text = "Costo";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCosto);
            this.groupBox1.Controls.Add(this.SKUProveedor);
            this.groupBox1.Controls.Add(this.cmbProductos);
            this.groupBox1.Controls.Add(this.Costo);
            this.groupBox1.Controls.Add(this.cmbProveedores);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSKUProveedor);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnVincular);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 144);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Productos-Proveedores";
            // 
            // frmVinculoProv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvVinculos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmVinculoProv";
            this.Text = "Productos-Proveedores";
            this.Load += new System.EventHandler(this.frmVinculoProv_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVinculos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbProductos;
        private System.Windows.Forms.ComboBox cmbProveedores;
        private System.Windows.Forms.TextBox txtSKUProveedor;
        private System.Windows.Forms.TextBox txtCosto;
        private System.Windows.Forms.DataGridView dgvVinculos;
        private System.Windows.Forms.Button btnVincular;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SKUProveedor;
        private System.Windows.Forms.Label Costo;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}