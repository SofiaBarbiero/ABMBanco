using BancoApp2.Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace BancoApp2.Datos
{
    class Conection
    {
        private SqlConnection cnn;
        private static Conection instancia; //patron de diseño: singleton, declaro atributo
        public Conection()
        {
            cnn = new SqlConnection(Properties.Resources.cnnString);
        }
        public static Conection ObtenerInstancia() // singleton: metodo para instanciar por unica vez
        {
            if (instancia == null)
                instancia = new Conection();
            return instancia;
        }

        public int ObtenerProximo(string nombreSP, string nombrePOut) //Nro de proximo cliente
        {
            SqlCommand cmdProximo = new SqlCommand();
            cnn.Open();
            cmdProximo.Connection = cnn;
            cmdProximo.CommandType = CommandType.StoredProcedure;
            cmdProximo.CommandText = nombreSP;
            SqlParameter pOut = new SqlParameter();
            pOut.ParameterName = nombrePOut;
            pOut.DbType = DbType.Int32;
            pOut.Direction = ParameterDirection.Output;
            cmdProximo.Parameters.Add(pOut);
            cmdProximo.ExecuteNonQuery();
            cnn.Close();
            return (int)pOut.Value;
        }

        public DataTable ConsultarSQL(string nombreSP, List<Parametro> values) //para consulta sql con lst de parametros (sirve para: cargar combo, )
        {
            DataTable tabla = new DataTable();
            cnn.Open();
            SqlCommand cmd = new SqlCommand(nombreSP, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (values != null)
            {
                foreach (Parametro oParametro in values)
                {
                    cmd.Parameters.AddWithValue(oParametro.Clave, oParametro.Valor);
                }
            }

            tabla.Load(cmd.ExecuteReader());
            cnn.Close();

            return tabla;
        }

        public DataTable ConsultarSQL(string nombreSP)
        {
            SqlCommand cmdConsulta = new SqlCommand();
            DataTable table = new DataTable();
            cnn.Open();
            cmdConsulta.Connection = cnn;
            cmdConsulta.CommandType = CommandType.StoredProcedure;
            cmdConsulta.CommandText = nombreSP;
            table.Load(cmdConsulta.ExecuteReader());
            cnn.Close();
            return table;
        }

       

        
        public bool ConfirmarCliente(Cliente oCliente) //Ejecutar SQL sin parametros
        {
            bool ok = true;

            SqlTransaction trs = null;
            try
            {
                SqlCommand cmdMaestro = new SqlCommand();
                cnn.Open();
                trs = cnn.BeginTransaction();
                cmdMaestro.Connection = cnn;
                cmdMaestro.Transaction = trs;
                cmdMaestro.CommandText = "SP_INSERTAR_CLIENTE";
                cmdMaestro.CommandType = CommandType.StoredProcedure;
                cmdMaestro.Parameters.AddWithValue("@apellido", oCliente.Apellido);
                cmdMaestro.Parameters.AddWithValue("@nombre", oCliente.Nombre);
                cmdMaestro.Parameters.AddWithValue("@dni", oCliente.Dni);

                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@cliente_nro";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmdMaestro.Parameters.Add(pOut);
                cmdMaestro.ExecuteNonQuery();

                int clienteNro = (int)pOut.Value;
                SqlCommand cmdDetalle;
                foreach (Cuenta item in oCliente.Cuentas)
                {
                    cmdDetalle = new SqlCommand();
                    cmdDetalle.Connection = cnn;
                    cmdDetalle.Transaction = trs;
                    cmdDetalle.CommandText = "SP_INSERTAR_CUENTAS";
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@cbu", item.Cbu);
                    cmdDetalle.Parameters.AddWithValue("@saldo", item.Saldo);
                    cmdDetalle.Parameters.AddWithValue("@ultimo_mov", item.Ultimo.ToString("yyyy-MM-dd"));
                    cmdDetalle.Parameters.AddWithValue("@id_tipo_cuenta", item.Tipo.id_tipo_cuenta);
                    cmdDetalle.Parameters.AddWithValue("@id_cliente", clienteNro);
                    cmdDetalle.ExecuteNonQuery();

                }
                trs.Commit();
            }
            catch (Exception)
            {
                trs.Rollback();
                ok = false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return ok;
        }

        public int EjecutarBD(string strSql, List<Parametro> values) //Ejecutar SQL con parametros
        {
            int afectadas = 0;
            SqlTransaction trs = null;

            try
            {
                SqlCommand cmdEliminar = new SqlCommand();
                cnn.Open();
                trs = cnn.BeginTransaction();
                cmdEliminar.Connection = cnn;
                cmdEliminar.CommandType = CommandType.StoredProcedure;
                cmdEliminar.CommandText = strSql;
                cmdEliminar.Transaction = trs;

                if (values != null)
                {
                    foreach (Parametro param in values)
                    {
                        cmdEliminar.Parameters.AddWithValue(param.Clave, param.Valor);
                    }
                }

                afectadas = cmdEliminar.ExecuteNonQuery();
                trs.Commit();
            }
            catch (SqlException)
            {
                if (trs != null) { trs.Rollback(); }
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                    cnn.Close();

            }

            return afectadas;
        }

    }
}
