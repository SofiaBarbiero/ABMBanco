using BancoApp2.Datos.Interfaz;
using BancoApp2.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApp2.Datos.Implementacion
{
    internal class ClienteDao : IDaoCliente
    {
        public int ObtenerProximo() //Nro de proximo cliente 
        {
            string sp = "SP_PROXIMO_ID";
            string nombrePOut = "@next";
            return Conection.ObtenerInstancia().ObtenerProximo(sp, nombrePOut);
        }

        public List<TipoCuenta> ObtenerTodos() // para cargar combos
        {
            List<TipoCuenta> lst = new List<TipoCuenta>();
            string sp = "SP_LISTAR_TIPOS_CUENTAS";
            DataTable table = Conection.ObtenerInstancia().ConsultarSQL(sp, null);
            foreach(DataRow dr in table.Rows)
            { 
                //Mapear un registro de la table a un objeto de dominio
                int id = int.Parse(dr["id_tipo_cuenta"].ToString()); //nombre de la col de sql
                string tipo = dr["nombre_cuenta"].ToString(); //nombre de la col de sql
                TipoCuenta aux = new TipoCuenta(id, tipo);
                lst.Add(aux);
            }
            return lst;
        }
    }
}
