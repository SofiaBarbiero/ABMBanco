using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApp2.Dominio
{
    class TipoCuenta
    {
        public int id_tipo_cuenta { get; set; }
        public string tipoCuenta { get; set; }
        public TipoCuenta(int id, string tipo)
        {
            this.id_tipo_cuenta = id;
            this.tipoCuenta = tipo;
        }
        public TipoCuenta()
        {
            id_tipo_cuenta = 0;
            tipoCuenta = string.Empty;
        }

    }
}
