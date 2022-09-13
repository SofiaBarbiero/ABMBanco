using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApp2.Dominio
{
    class Cliente
    {
        public int id_cliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public List<Cuenta> Cuentas { get; set; }

        public Cliente()
        {
            id_cliente = 0;
            Nombre = string.Empty;
            Apellido = string.Empty;
            Cuentas = new List<Cuenta>();
        }

        public void AgregarCuenta(Cuenta nueva)
        {
            Cuentas.Add(nueva);
        }
        public void EliminarCuenta(int cbu)
        {
            Cuentas.RemoveAt(cbu);
        }

    }
}

