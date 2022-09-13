using BancoApp2.Datos;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
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
    public partial class FrmConsulta : Form
    {
        public FrmConsulta()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            List<Parametro> lst = new List<Parametro>();
            lst.Add(new Parametro("@apellido", txtApellido.Text));
            lst.Add(new Parametro("@nombre", txtNombre.Text));
            lst.Add(new Parametro("@dni", txtDni.Text));
            dgvConsulta.Rows.Clear();
            DataTable tabla = Conection.ObtenerInstancia().ConsultarBD("SP_CONSULTAR_CUENTAS", lst);

            foreach (DataRow fila in tabla.Rows)
            {
                dgvConsulta.Rows.Add(new object[]
                {
                    fila[0].ToString(),
                    fila[1].ToString(),
                    fila[2].ToString(),
                    fila[3].ToString() 
                });
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro que desea dar de baja la cuenta seleccionada?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dgvConsulta.CurrentRow != null)
                {
                    int cbu_nro = int.Parse(dgvConsulta.CurrentRow.Cells["ColCBU"].Value.ToString());
                    List<Parametro> lst = new List<Parametro>();
                    lst.Add(new Parametro("@cbu_nro", cbu_nro));

                    int afectadas = Conection.ObtenerInstancia().EjecutarBD("SP_ELIMINAR_CUENTA", lst);
                    if (afectadas == 1)
                    {
                        MessageBox.Show("La cuenta se dio de baja correctamente!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnConsultar_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("La cuenta no pudo ser dada de baja", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }
    }
}
