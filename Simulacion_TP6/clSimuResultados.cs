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
        
        public void Calcular()
        {
            double totalMinOcio,
                   totalMinSimu;

            //Tiempos medios de espera en cola
            this.tMEA = simulacion.SecA / simulacion.NTA;
            this.tMEN = simulacion.SecN / simulacion.NTN;
            this.tMEB = simulacion.SecB / simulacion.NTB;

            //Tiempos medios de atención
            this.tMAA = simulacion.StaTA / simulacion.NTA; 
            this.tMAN = simulacion.StaTN / simulacion.NTN;
            this.tMAB = simulacion.StaTB/ simulacion.NTB;

            //Porcentajes de tiempos ociosos
            totalMinSimu = clDatosVar.RestarFechas(simulacion.TiempoActual, simulacion.TiempoInicial);

            totalMinOcio = 0;
            for (int i = 0; i < simulacion.CantS; i++) {
                //Sumar total de tiempo ocioso de todos los puestos SENIOR
                totalMinOcio += simulacion.Puestos_S[i].Sto; 
                if (simulacion.Puestos_S[i].Ito != DateTime.MinValue &&
                    simulacion.Puestos_S[i].Ito <= simulacion.TiempoActual)
                {
                    totalMinOcio += clDatosVar.RestarFechas(simulacion.TiempoActual, simulacion.Puestos_S[i].Ito);
                }
            }
            ptoS = totalMinOcio / simulacion.CantS / totalMinSimu;

            totalMinOcio = 0;
            for (int i = 0; i < simulacion.CantSS; i++)
            {
                //Sumar total de tiempo ocioso de todos los puestos SEMI SENIOR
                totalMinOcio += simulacion.Puestos_SS[i].Sto;
                if (simulacion.Puestos_SS[i].Ito != DateTime.MinValue &&
                    simulacion.Puestos_SS[i].Ito <= simulacion.TiempoActual) //1/1/0001 00:00:00 
                {
                    totalMinOcio += clDatosVar.RestarFechas(simulacion.TiempoActual, simulacion.Puestos_SS[i].Ito);
                }
            }
            ptoSS = totalMinOcio / simulacion.CantSS / totalMinSimu;

            totalMinOcio = 0;
            for (int i = 0; i < simulacion.CantJ; i++)
            {
                //Sumar total de tiempo ocioso de todos los puestos JUNIOR
                totalMinOcio += simulacion.Puestos_J[i].Sto; 
                if (simulacion.Puestos_J[i].Ito != DateTime.MinValue &&
                    simulacion.Puestos_J[i].Ito <= simulacion.TiempoActual) //1/1/0001 00:00:00 
                {
                    totalMinOcio += clDatosVar.RestarFechas(simulacion.TiempoActual, simulacion.Puestos_J[i].Ito);
                }
            }
            ptoJ = totalMinOcio / simulacion.CantJ / totalMinSimu;
        }
    }
}
