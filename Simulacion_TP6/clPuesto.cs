using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulacion_TP6
{
    public class clPuesto : IComparable<clPuesto>
    {
        private static Int64 idSeed = 0;
        private string tipo;  //Tipo recurso: S, SS, J
        private clTicket ticket; //ticket asignado
        private DateTime tps; //Tiempo próxima salida        
        private DateTime ito; //Inicio tiempo ocioso
        private double sto; //Sumatoria tiempo ociosio
        private Int64 id = idSeed++;

        public string Tipo { get => tipo; set => tipo = value; }
        public clTicket Ticket { get => ticket; set => ticket = value; }
        public DateTime Tps { get => tps; set => tps = value; }        
        public DateTime Ito  { get => ito; set => ito = value; }
        public double Sto { get => sto; set => sto = value; }

        //Valores posibles de tipo de recurso
        static public string RecursoS() { return "S"; }
        static public string RecursoSS() { return "SS"; }
        static public string RecursoJ() { return "J"; }


        public string Pta()
        {
            if (Ticket != null)
                return Ticket.Prioridad;
            else
                return null;
        }

        public double AtenderTicket(clTicket iTicket, DateTime iT) //IT: instante de atención
        { 
            //Asignar ticket
            this.Ticket = iTicket;

            //Calcular tiempo de espera en cola del ticket
            this.Ticket.Tesperacola = clDatosVar.RestarFechas(iT, ticket.Tll);

            //Generar tiempo de atención para ticket
            this.Ticket.Ta = this.GenerarTA();

            //Calcular tiempo de salida
            ticket.Ts = clDatosVar.SumarTiempos(iT, this.Ticket.Ta); //Tps de este ticket
            this.Tps = ticket.Ts;

            return this.Ticket.Ta;
        }

        private double GenerarTA()
        {
            double TA = 0;

            //Generar el tiempo de atención
            if (String.Compare(this.Tipo, RecursoS()) == 0)
                TA = clDatosVar.GenerarTA_S();
            else if (String.Compare(this.Tipo, RecursoSS()) == 0)
                TA = clDatosVar.GenerarTA_SS();
            else
                TA = clDatosVar.GenerarTA_J();

            return TA;
        }

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
