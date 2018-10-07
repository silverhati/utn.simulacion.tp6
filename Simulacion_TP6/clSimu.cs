using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Simulacion_TP6
{
    public class clSimu
    {
        
        //Variables de Control
        private int cantS, cantSS, cantJ;
        public int CantS { get => cantS; set => cantS = value; }
        public int CantSS { get => cantSS; set => cantSS = value; }
        public int CantJ { get => cantJ; set => cantJ = value; }

        //Variables de Estado
        private int nsA, nsN, nsB;
        public int NsA { get => nsA; set => nsA = value; }
        public int NsN { get => nsN; set => nsN = value; }
        public int NsB { get => nsB; set => nsB = value; }

        private List<clTicket> cola_A, cola_N, cola_B;
        public List<clTicket> Cola_A { get => cola_A; set => cola_A = value; }
        public List<clTicket> Cola_N { get => cola_N; set => cola_N = value; }
        public List<clTicket> Cola_B { get => cola_B; set => cola_B = value; }

        //Otras        
        DateTime T,
                 Tinicial,
                 TF,
                 TPLL,
                 HV = new DateTime(9999, 12, 31, 23, 59, 59);
                 //DUMMYTIME = new DateTime(0001, 01, 01, 00, 00, 00);
        public DateTime TiempoActual { get => T; }
        public DateTime TiempoInicial { get => Tinicial; }
        public DateTime TiempoFinal { get => TF; }

        double nTA, nTN, nTB; //Cantidad de tickets que entraron al sistema
        double secA, secN, secB; //Sumatorias de tiempos de Espera en cola, en minutos
        double staTA, staTN, staTB; //Sumatorias de tiempos de atención, en minutos 
        public double NTA { get => nTA; set => nTA = value; }
        public double NTN { get => nTN; set => nTN = value; }
        public double NTB { get => nTB; set => nTB = value; }
        public double SecA { get => secA; set => secA = value; }
        public double SecN { get => secN; set => secN = value; }
        public double SecB { get => secB; set => secB = value; }
        public double StaTA { get => staTA; set => staTA = value; }
        public double StaTN { get => staTN; set => staTN = value; }
        public double StaTB { get => staTB; set => staTB = value; }

        List<clPuesto> puestos_S, puestos_SS, puestos_J;
        public List<clPuesto> Puestos_S { get => puestos_S; }
        public List<clPuesto> Puestos_SS { get => puestos_SS; }
        public List<clPuesto> Puestos_J { get => puestos_J; }


        bool procesar = true;
        const string cPrioridadAlta = "A",
                     cPrioridadNormal = "N",
                     cPrioridadBaja = "B";   

        //Constructor
        public clSimu(int pCantS, int pCantSS, int pCantJ)
        {
            cantS = pCantS;
            cantSS = pCantSS;
            cantJ = pCantJ;
        }

        //Métodos
        public clSimuResultados Iniciar()
        {            
            DateTime TpsS, TpsSS, TpsJ;
            clSimuResultados resultados;

            //Inicializar variables
            this.CondicionesIniciales();

            while (procesar == true)
            {
                //Busco los puestos con menor Tiempo de Proxima Salida.
                clPuesto puestoSeniorMenorTps = puestos_S.Aggregate((puesto1, puesto2) => puesto1.Tps <= puesto2.Tps ? puesto1 : puesto2);
                clPuesto puestoSemiSeniorMenorTps = puestos_SS.Aggregate((puesto1, puesto2) => puesto1.Tps <= puesto2.Tps ? puesto1 : puesto2);
                clPuesto puestoJuniorMenorTps = puestos_J.Aggregate((puesto1, puesto2) => puesto1.Tps <= puesto2.Tps ? puesto1 : puesto2);

                //Busco los puestos con mayor Tiempo de Proxima Salida.
                clPuesto puestoSeniorMayorTps = puestos_S.Aggregate((puesto1, puesto2) => puesto1.Tps > puesto2.Tps ? puesto1 : puesto2);
                clPuesto puestoSemiSeniorMayorTps = puestos_SS.Aggregate((puesto1, puesto2) => puesto1.Tps > puesto2.Tps ? puesto1 : puesto2);
                clPuesto puestoJuniorMayorTps = puestos_J.Aggregate((puesto1, puesto2) => puesto1.Tps > puesto2.Tps ? puesto1 : puesto2);

                //puestoLibreS = (puestoLibreS.Tps == HV) ? puestoLibreS : new clPuesto() { Tps = DUMMYTIME };;
                //clPuesto puestoLibreSS = puestos_SS.FirstOrDefault(puesto => puesto.Tps == HV) ?? new clPuesto() { Tps = DUMMYTIME };
                //clPuesto puestoLibreJ = puestos_J.FirstOrDefault(puesto => puesto.Tps == HV) ?? new clPuesto() { Tps = DUMMYTIME };

                //Obtener el menor TPS para SENIOR, SEMI SENIOR y JUNIOR
                if (puestos_S.Count != 0)
                    TpsS = puestoSeniorMenorTps.Tps;
                else //En el caso de que no haya puestos SENIOR (variable de control cantS == 0)
                    TpsS = HV;

                if (puestos_SS.Count != 0)
                    TpsSS = puestoSemiSeniorMenorTps.Tps;
                else //En el caso de que no haya puestos SEMI SENIOR (variable de control cantSS == 0)
                    TpsSS = HV;

                if (puestos_J.Count != 0)
                    TpsJ = puestoJuniorMenorTps.Tps;
                else //En el caso de que no haya puestos JUNIOR (variable de control cantJ == 0)
                    TpsJ = HV;

                //Determinar si se procesará una LLEGADA o una SALIDA
                if (TPLL <= TpsS && TPLL <= TpsSS && TPLL <= TpsJ)
                { this.ProcesarLlegada(ref puestoSeniorMayorTps, ref puestoSemiSeniorMayorTps, ref puestoJuniorMayorTps); } //LLEGADA
                else if (TpsS <= TpsSS && TpsS <= TpsJ)
                { this.ProcesarSalida(ref puestoSeniorMenorTps); } //SALIDA SENIOR
                else if (TpsSS <= TpsS && TpsSS <= TpsJ)
                { this.ProcesarSalida(ref puestoSemiSeniorMenorTps); } //SALIDA SEMI SENIOR
                else
                { this.ProcesarSalida(ref puestoJuniorMenorTps); } //SALIDA JUNIOR

                if (T >= TF)
                    if (nsA + nsN + nsB == 0)
                        procesar = false; //Cortar el ciclo while
                    else
                        TPLL = HV; //Vaciamiento
            }

            //Calcular resultados de la simulación
            resultados = new clSimuResultados(this);
            resultados.Calcular();

            //Grabo los resultados en el archivo solo si cumplo con los parametros de performance requeridos.
            if (this.cumploParametrosPerformance(resultados))
            {
                vuelcoAArchivo(resultados);
            }

            return resultados;
        }

        private bool cumploParametrosPerformance(clSimuResultados resultados)
        {
            return (
                resultados.TMEA <= 60 &&
                resultados.TMEN <= 240 &&
                resultados.TMEB <= 1440 &&
                resultados.PtoGral <= 0.25
                );
        }

        public void vuelcoAArchivo(clSimuResultados resultados)
        {
            String path = "C:\\ResultadosCSV.txt";

            if (!File.Exists(path))
            {
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(

                        "Cantidad Senior" + ";" + "Cantidad SemiSenior" + ";" + "Cantidad Junior" + ";" +

                        "TMEA" + ";" + "TMEN" + ";" + "TMEB" + ";" +

                        "TMAA" + ";" + "TMAN" + ";" + "TMAB" + ";" +

                        "PTOS" + ";" + "PTOSS" + ";" + "PTOJ" + Environment.NewLine +

                        this.CantS + ";" + this.CantSS + ";" + this.CantJ + ";" +

                        resultados.TMEA + ";" + resultados.TMEN + ";" + resultados.TMEB + ";" +

                        resultados.TMAA + ";" + resultados.TMAN + ";" + resultados.TMAB + ";" +

                        resultados.PTOS + ";" + resultados.PTOSS + ";" + resultados.PTOJ

                        );
                }
            }
            else
            {
                File.AppendAllText(path,
                        this.CantS + ";" + this.CantSS + ";" + this.CantJ + ";" +

                        resultados.TMEA + ";" + resultados.TMEN + ";" + resultados.TMEB + ";" +

                        resultados.TMAA + ";" + resultados.TMAN + ";" + resultados.TMAB + ";" +

                        resultados.PTOS + ";" + resultados.PTOSS + ";" + resultados.PTOJ + Environment.NewLine);
            }
        }

        private void CondicionesIniciales()
        {
            //Tiempo Próxima Llegada (Tiempo Inicial)
            Tinicial = new DateTime(2019, 01, 01, 00, 00, 00);
            TPLL = Tinicial;

            //Tiempo Final
            TF = new DateTime(2020, 12, 31, 23, 59, 59);

            //Inicializar puestos SENIOR
            puestos_S = new List<clPuesto>();
            for (var index = 0; index < cantS; index++)
            {
                puestos_S.Add(new clPuesto() { Tipo = clPuesto.RecursoS(), Ticket = null, Tps = HV, Ito = Tinicial, Sto = 0});
            }

            //Inicializar puestos SEMI SENIOR
            puestos_SS = new List<clPuesto>();
            for (var index = 0; index < cantSS; index++)
            {
                puestos_SS.Add(new clPuesto() { Tipo = clPuesto.RecursoSS(), Ticket = null, Tps = HV, Ito = Tinicial, Sto = 0 });
            }

            //Inicializar puestos JUNIOR
            puestos_J = new List<clPuesto>();
            for (var index = 0; index < cantJ; index++)
            {
                puestos_J.Add(new clPuesto() { Tipo = clPuesto.RecursoJ(), Ticket = null, Tps = HV, Ito = Tinicial, Sto = 0 });
            }

            //Inicializar cola de tickets prioridad ALTA
            Cola_A = new List<clTicket>();

            //Inicializar cola de tickets prioridad NORMAL
            Cola_N = new List<clTicket>();

            //Inicializar cola de tickets prioridad BAJA
            Cola_B = new List<clTicket>();
        }

        private void ProcesarLlegada(ref clPuesto puestoSeniorMayorTps, ref clPuesto puestoSemiSeniorMayorTps, ref clPuesto puestoJuniorMayorTps)
        {
            double IA;
            string prioridadTicket;

            T = TPLL; //Avance del tiempo
            
            //Próxima llegada (EFNC)
            IA = clDatosVar.GenerarIA();
            TPLL = clDatosVar.SumarTiempos(T, IA);

            //Determinar prioridad del nuevo ticket
            prioridadTicket = clDatosVar.GenerarPrioridadTicket();

            if (String.Compare(prioridadTicket, cPrioridadAlta) == 0)
                this.ProcesarLlegadaA(ref puestoSeniorMayorTps, ref puestoSemiSeniorMayorTps, ref puestoJuniorMayorTps); //Llegada ticket Prioridad ALTA
            else if (String.Compare(prioridadTicket,cPrioridadNormal) == 0)
                this.ProcesarLlegadaN(ref puestoSeniorMayorTps, ref puestoSemiSeniorMayorTps, ref puestoJuniorMayorTps); //Llegada ticket Prioridad NORMAL
            else
                this.ProcesarLlegadaB(ref puestoSeniorMayorTps, ref puestoSemiSeniorMayorTps, ref puestoJuniorMayorTps); //Llegada ticket Prioridad BAJA
        }

        private void ProcesarLlegadaA(ref clPuesto puestoSeniorMayorTps, ref clPuesto puestoSemiSeniorMayorTps, ref clPuesto puestoJuniorMayorTps)
        {
            clTicket ticket;
            string prioridadTicket = cPrioridadAlta;

            //Llegada de un nuevo ticket
            nsA += 1; //Actualizar el NS correspondiente
            nTA += 1; //Actualizar el NT correspondiente
            ticket = new clTicket(prioridadTicket);
            ticket.Tll = T; //tiempo de llegada del ticket

            if (puestoSeniorMayorTps.Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket      
                staTA += puestoSeniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoSeniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoSeniorMayorTps.Ito);
                puestoSeniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestoSemiSeniorMayorTps.Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket           
                staTA += puestoSemiSeniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoSemiSeniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoSemiSeniorMayorTps.Ito);
                puestoSemiSeniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestoJuniorMayorTps.Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket       
                staTA += puestoJuniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoJuniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoJuniorMayorTps.Ito);
                puestoJuniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                cola_A.Add(ticket);
        }

        private void ProcesarLlegadaN(ref clPuesto puestoSeniorMayorTps, ref clPuesto puestoSemiSeniorMayorTps, ref clPuesto puestoJuniorMayorTps)
        {
            clTicket ticket;
            string prioridadTicket = cPrioridadNormal;

            //Llegada de un nuevo ticket
            nsN += 1; //Actualizar el NS correspondiente
            nTN += 1; //Actualizar el NT correspondiente
            ticket = new clTicket(prioridadTicket);
            ticket.Tll = T; //tiempo de llegada del ticket
          
            if (puestoSemiSeniorMayorTps.Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket   
                staTN += puestoSemiSeniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoSemiSeniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoSemiSeniorMayorTps.Ito);
                puestoSemiSeniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestoSeniorMayorTps.Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket           
                staTN += puestoSeniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoSeniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoSeniorMayorTps.Ito);
                puestoSeniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestoJuniorMayorTps.Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket           
                staTN += puestoJuniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoJuniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoJuniorMayorTps.Ito);
                puestoJuniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                cola_N.Add(ticket);            
        }

        private void ProcesarLlegadaB(ref clPuesto puestoSeniorMayorTps, ref clPuesto puestoSemiSeniorMayorTps, ref clPuesto puestoJuniorMayorTps)
        {
            clTicket ticket;
            string prioridadTicket = cPrioridadBaja;

            //Llegada de un nuevo ticket
            nsB += 1; //Actualizar el NS correspondiente
            nTB += 1; //Actualizar el NT correspondiente
            ticket = new clTicket(prioridadTicket);
            ticket.Tll = T; //tiempo de llegada del ticket

            if (puestoJuniorMayorTps.Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket           
                staTB += puestoJuniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoJuniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoJuniorMayorTps.Ito);
                puestoJuniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestoSemiSeniorMayorTps.Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket           
                staTB += puestoSemiSeniorMayorTps.AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestoSemiSeniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoSemiSeniorMayorTps.Ito);
                puestoSemiSeniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestoSeniorMayorTps.Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket           
                staTB += puestoSeniorMayorTps.AtenderTicket(ticket, T);;

                //El puesto deja de estar ocioso
                puestoSeniorMayorTps.Sto += clDatosVar.RestarFechas(T, puestoSeniorMayorTps.Ito);
                puestoSeniorMayorTps.Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                cola_B.Add(ticket);            
        }


        private void ProcesarSalida(ref clPuesto pPuesto)
        {
            //Avance del tiempo
            T = pPuesto.Tps;

            //Sale el ticket            
            //Actualizar el NS correspondiente a la salida 
            if (String.Compare(pPuesto.Pta(), cPrioridadAlta) == 0)
            { nsA -= 1; }  //Prioridad ALTA   
            else if (String.Compare(pPuesto.Pta(), cPrioridadNormal) == 0)
            { nsN -= 1; }  //Prioridad NORMAL 
            else
            { nsB -= 1; }  //Prioridad BAJA            

            //El puesto continuará atendiendo mientras haya tickets encolados
            if (Cola_A.Count >= 1)
            {
                //Atender ticket de prioridad ALTA
                staTA += pPuesto.AtenderTicket(Cola_A[0], T); //sumarizamos tiempos de atención tickets de prioridad ALTA
                Cola_A.RemoveAt(0); //disminuye la cola tickets prioridad ALTA                
                secA += pPuesto.Ticket.Tesperacola; //Sumatoria tiempos de espera en Cola ALTA
            }
            else if (Cola_N.Count >= 1)
            {
                //Atender ticket de prioridad NORMAL
                staTN += pPuesto.AtenderTicket(Cola_N[0], T);  //sumarizamos tiempos de atención tickets de prioridad NORMAL
                Cola_N.RemoveAt(0); //disminuye la cola tickets prioridad NORMAL                
                secN += pPuesto.Ticket.Tesperacola; //Sumatoria tiempos de espera en Cola NORMAL
            }
            else if (Cola_B.Count >= 1)
            {
                //Atender ticket de prioridad BAJA
                staTB += pPuesto.AtenderTicket(Cola_B[0], T); //sumarizamos tiempos de atención tickets de prioridad BAJA
                Cola_B.RemoveAt(0); //disminuye la cola tickets prioridad BAJA                                    
                secB += pPuesto.Ticket.Tesperacola; //Sumatoria tiempos de espera en Cola BAJA
            }
            else
            {
                //El puesto queda vacío
                pPuesto.Ticket = null; 
                pPuesto.Tps = HV;
                pPuesto.Ito = T;
            }                            

        }

    }


}
