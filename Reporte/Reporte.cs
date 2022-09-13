using Microsoft.ReportingServices.Diagnostics.Internal;
using BancoApp2.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BancoApp2.Reporte
{
    public partial class FrmReporte : Form
    {
        public FrmReporte()
        {
            InitializeComponent();
        }

        private void Reporte_Load(object sender, EventArgs e)
        {
            DataTable table = Conection.ObtenerInstancia().ConsultarSQL("SP_REPORTE_CLIENTES");
            rvReporte.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSetBanco", table));
            rvReporte.RefreshReport();
        }
    }
}
