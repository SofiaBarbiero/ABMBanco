using BancoApp2.Datos;
using BancoApp2.Dominio;
using BancoApp2.Servicios.Implementacion;
using BancoApp2.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BancoApp2.Formularios
{
    public partial class FrmBanco : Form
    {
        private IServicio gestor;
        private Cliente nuevo;
        public FrmBanco()
        {
            InitializeComponent();
            gestor = new Servicio();
            nuevo = new Cliente();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarCombo();
            ObtenerProximo(); //Nro de proximo cliente
            txtApellido.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDNI.Text = string.Empty;
            txtCBU.Text = string.Empty;
            txtSaldo.Text = string.Empty;
            dtpUltimo.Value = DateTime.Now;
            this.ActiveControl = txtApellido;
        }

        private void ObtenerProximo() //Nro de proximo cliente
        {
            int next = gestor.ObtenerProximo();
            if (next > 0)
                lblCliente.Text = "Cliente Nº: " + next.ToString();
            else
                MessageBox.Show("Error de datos. No se puede obtener Nº de presupuesto!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CargarCombo() //para cargar combo
        {
                cboTipo.DataSource = gestor.ObtenerTodos();
                cboTipo.ValueMember = "id_tipo_cuenta"; //nombre del atributo de la clase
                cboTipo.DisplayMember = "tipoCuenta"; //nombre del atributo de la clase
                cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboTipo.Text.Equals(String.Empty))
            {
                MessageBox.Show("Debe seleccionar un tipo de cuenta!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtCBU.Text == "" || !int.TryParse(txtCBU.Text, out _))
            {
                MessageBox.Show("Debe ingresar un CBU válido!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataRowView item = (DataRowView)cboTipo.SelectedItem;
            int id = Convert.ToInt32(item.Row.ItemArray[0]);
            string tipo = item.Row.ItemArray[1].ToString();
            TipoCuenta t = new TipoCuenta(id, tipo);

            int cbu = Convert.ToInt32(txtCBU.Text);
            double saldo = Convert.ToDouble(txtSaldo.Text);
            DateTime ultimo = dtpUltimo.Value;

            Cuenta c = new Cuenta(cbu, saldo, t, ultimo);
            nuevo.AgregarCuenta(c);
            dgvCuentas.Rows.Add(c.Cbu, c.Saldo, c.Tipo.id_tipo_cuenta, c.Tipo.tipoCuenta, c.Ultimo);
        }

        public void GuardarCliente()
        {
            nuevo.Apellido = txtApellido.Text;
            nuevo.Nombre = txtNombre.Text;
            nuevo.Dni = Convert.ToInt32(txtDNI.Text);

            if (Conection.ObtenerInstancia().ConfirmarCliente(nuevo))
            {
                MessageBox.Show("Cliente registrado", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("ERROR. No se pudo registrar el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {

            if (txtApellido.Text == "")
            {
                MessageBox.Show("Debe ingresar un apellido!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dgvCuentas.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos una cuenta!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GuardarCliente();
        }
    }
}
