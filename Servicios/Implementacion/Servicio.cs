using BancoApp2.Datos.Implementacion;
using BancoApp2.Datos.Interfaz;
using BancoApp2.Dominio;
using BancoApp2.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApp2.Servicios.Implementacion
{
    class Servicio : IServicio
    {
        private IDaoCliente dao;

        public Servicio()
        {
            dao = new ClienteDao();
        }

        public int ObtenerProximo()
        {
            return dao.ObtenerProximo();
        }

        public List<TipoCuenta> ObtenerTodos() // obtengo lista de objetos se usa para: cargar combos
        {
            return dao.ObtenerTodos();
        }
    }
}
