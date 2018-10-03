using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulacion_TP6
{
    public class clSimuResultados
    {
        private clSimu simulacion;
        private double tMEA;
        private double tMEN;
        private double tMEB;
        private double tMAA;
        private double tMAN;
        private double tMAB;
        private double ptoS;
        private double ptoSS;
        private double ptoJ;
        public double TMEA { get => tMEA; }
        public double TMEN { get => tMEN; }
        public double TMEB { get => tMEB; }
        public double TMAA { get => tMAA; }
        public double TMAN { get => tMAN; }
        public double TMAB { get => tMAB; }
        public double PTOS { get => ptoS; }
        public double PTOSS { get => ptoSS; }
        public double PTOJ { get => ptoJ; }

        public clSimuResultados(clSimu simu)
        {
            this.simulacion = simu;
        }
        
        public void calcular()
        {
            double sumTOS = 0;
            double sumTOSS = 0;
            double sumTOJ = 0;
            //Alta
            tMEA = (simulacion.Ssa - simulacion.Slla - simulacion.StaTA) / simulacion.NTA;
            tMAA = simulacion.StaTA / simulacion.NTA;
            //Normal
            tMEN = (simulacion.Ssn - simulacion.Slln - simulacion.StaTN) / simulacion.NTN;
            tMAN = simulacion.StaTN / simulacion.NTN;
            //Baja
            tMEB = (simulacion.Ssb - simulacion.Sllb - simulacion.StaTB) / simulacion.NTB;
            tMAB = simulacion.StaTB / simulacion.NTB;

            for (int i = 0; i <= simulacion.CantS; i++) {
                sumTOS += simulacion.puestosS[i].Sto;
            }

            for (int i = 0; i <= simulacion.CantSS; i++)
            {
                sumTOSS += simulacion.puestosSS[i].Sto;
            }

            for (int i = 0; i <= simulacion.CantJ; i++)
            {
                sumTOJ += simulacion.puestosJ[i].Sto;
            }
            ptoS = sumTOS / (simulacion.Time.Hour*100+simulacion.Time.Minute);
            ptoSS = sumTOSS / (simulacion.Time.Hour * 100 + simulacion.Time.Minute);
            ptoJ = sumTOJ / (simulacion.Time.Hour * 100 + simulacion.Time.Minute);
        }
    }
}
