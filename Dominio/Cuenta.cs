using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApp2.Dominio
{
    class Cuenta
    {
        public int Cbu { get; set; }
        public double Saldo { get; set; }
        public TipoCuenta Tipo { get; set; }
        public DateTime Ultimo { get; set; }

        public Cuenta()
        {
            Cbu = 0;
            Saldo = 0;
            Tipo = null;
            Ultimo = DateTime.Now;
        }

        public Cuenta(int cbu, double saldo, TipoCuenta tipo, DateTime ultimo)
        {
            this.Cbu = cbu;
            this.Saldo = saldo;
            this.Tipo = tipo;
            this.Ultimo = ultimo;
        }
    }
}
