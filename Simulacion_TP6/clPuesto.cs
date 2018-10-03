using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulacion_TP6
{
    public class clPuesto : IComparable<clPuesto>
    {
        private DateTime tps; //Tiempo próxima salida
        private string pta;   //Prioridad ticket asignado
        private string tipo;  //Tipo recurso: S, SS, J
        private double ito; //Inicio tiempo ocioso
        private double sto; //Sumatoria tiempo ociosio

        public DateTime Tps { get => tps; set => tps = value; }
        public string Pta { get => pta; set => pta = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public double Ito  { get => ito; set => ito = value; }
        public double Sto { get => sto; set => sto = value; }

        public int CompareTo(clPuesto other)
        {
            // A call to this method makes a single comparison that is  
            // used for sorting.  

            // Determine the relative order of the objects being compared.  
            // Sort by *Tps* alphabetically

            int compare;
            if (this.Tps <= other.Tps)
                compare = -1;
            else
                compare = 1;

            return compare;
        }

    }
}
