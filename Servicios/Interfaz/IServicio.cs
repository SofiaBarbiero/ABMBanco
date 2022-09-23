using BancoApp2.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApp2.Servicios.Interfaz
{
    interface IServicio
    {
        int ObtenerProximo(); //Nro de proximo cliente
        
        List<TipoCuenta> ObtenerTodos(); // obtengo lista de objetos se usa para: cargar combos
    }
}
