using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulacion_TP6
{
    public class clTicket
    {
        private string prioridad;   //prioridad: A, N, B
        private DateTime tll;       //tiempo de llegada
        private DateTime ts;        //tiempo de salida
        private double ta;          //tiempo de atención
        private double tesperacola; //tiempo de espera en cola

        public string Prioridad { get => prioridad; set => prioridad = value; }
        public DateTime Tll { get => tll; set => tll = value; }
        public DateTime Ts { get => ts; set => ts = value; }
        public double Ta { get => ta; set => ta = value; }
        public double Tesperacola { get => tesperacola; set => tesperacola = value; }

        //Constructor
        public clTicket(string iPrioridad)
        {
            prioridad = iPrioridad;
        }
    }
}
