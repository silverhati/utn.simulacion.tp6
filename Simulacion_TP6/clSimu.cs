using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                //Ordenar puestos_S, puestos_SS y puestos_J por el tiempo de salida,
                //de menor a mayor
                puestos_S.Sort();
                puestos_SS.Sort();
                puestos_J.Sort();

                //Obtener el menor TPS para SENIOR, SEMI SENIOR y JUNIOR
                if (puestos_S.Count != 0)
                    TpsS = puestos_S[0].Tps;
                else //En el caso de que no haya puestos SENIOR (variable de control cantS == 0)
                    TpsS = HV;

                if (puestos_SS.Count != 0)
                    TpsSS = puestos_SS[0].Tps;
                else //En el caso de que no haya puestos SEMI SENIOR (variable de control cantSS == 0)
                    TpsSS = HV;

                if (puestos_J.Count != 0)
                    TpsJ = puestos_J[0].Tps;
                else //En el caso de que no haya puestos JUNIOR (variable de control cantJ == 0)
                    TpsJ = HV;

                //Determinar si se procesará una LLEGADA o una SALIDA
                if (TPLL <= TpsS && TPLL <= TpsSS && TPLL <= TpsJ)
                { this.ProcesarLlegada(); } //LLEGADA
                else if (TpsS <= TpsSS && TpsS <= TpsJ)
                { this.ProcesarSalida(ref puestos_S, 0); } //SALIDA SENIOR
                else if (TpsSS <= TpsS && TpsSS <= TpsJ)
                { this.ProcesarSalida(ref puestos_SS, 0); } //SALIDA SEMI SENIOR
                else
                { this.ProcesarSalida(ref puestos_J, 0); } //SALIDA JUNIOR

                if (T >= TF)
                    if (nsA + nsN + nsB == 0)
                        procesar = false; //Cortar el ciclo while
                    else
                        TPLL = HV; //Vaciamiento
            }

            //Calcular resultados de la simulación
            resultados = new clSimuResultados(this);
            resultados.Calcular();

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

            return resultados;
        }
        
        
        private void CondicionesIniciales()
        {
            //Tiempo Próxima Llegada (Tiempo Inicial)
            Tinicial = new DateTime(2019, 01, 01, 00, 00, 00);
            TPLL = Tinicial;

            //Tiempo Final
            TF = new DateTime(3020, 01, 01, 18, 00, 00);

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

        private void ProcesarLlegada()
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
                this.ProcesarLlegadaA(); //Llegada ticket Prioridad ALTA
            else if (String.Compare(prioridadTicket,cPrioridadNormal) == 0)
                this.ProcesarLlegadaN(); //Llegada ticket Prioridad NORMAL
            else
                this.ProcesarLlegadaB(); //Llegada ticket Prioridad BAJA
        }

        private void ProcesarLlegadaA()
        {
            clTicket ticket;
            string prioridadTicket = cPrioridadAlta;

            //Llegada de un nuevo ticket
            nsA += 1; //Actualizar el NS correspondiente
            nTA += 1; //Actualizar el NT correspondiente
            ticket = new clTicket(prioridadTicket);
            ticket.Tll = T; //tiempo de llegada del ticket

            if (puestos_S[puestos_S.Count - 1].Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket      
                staTA += puestos_S[puestos_S.Count - 1].AtenderTicket(ticket, T);                     
                
                //El puesto deja de estar ocioso
                puestos_S[puestos_S.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_S[puestos_S.Count - 1].Ito);
                puestos_S[puestos_S.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestos_SS[puestos_SS.Count - 1].Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket           
                staTA += puestos_SS[puestos_SS.Count - 1].AtenderTicket(ticket, T);                

                //El puesto deja de estar ocioso
                puestos_SS[puestos_SS.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_SS[puestos_SS.Count - 1].Ito);
                puestos_SS[puestos_SS.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestos_J[puestos_J.Count - 1].Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket       
                staTA += puestos_J[puestos_J.Count - 1].AtenderTicket(ticket, T);                

                //El puesto deja de estar ocioso
                puestos_J[puestos_J.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_J[puestos_J.Count - 1].Ito);
                puestos_J[puestos_J.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                cola_A.Add(ticket);
        }

        private void ProcesarLlegadaN()
        {
            clTicket ticket;
            string prioridadTicket = cPrioridadNormal;

            //Llegada de un nuevo ticket
            nsN += 1; //Actualizar el NS correspondiente
            nTN += 1; //Actualizar el NT correspondiente
            ticket = new clTicket(prioridadTicket);
            ticket.Tll = T; //tiempo de llegada del ticket
          
            if (puestos_SS[puestos_SS.Count - 1].Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket   
                staTN += puestos_SS[puestos_SS.Count - 1].AtenderTicket(ticket, T);

                //El puesto deja de estar ocioso
                puestos_SS[puestos_SS.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_SS[puestos_SS.Count - 1].Ito);
                puestos_SS[puestos_SS.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestos_S[puestos_S.Count - 1].Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket           
                staTN += puestos_S[puestos_S.Count - 1].AtenderTicket(ticket, T);                

                //El puesto deja de estar ocioso
                puestos_S[puestos_S.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_S[puestos_S.Count - 1].Ito);
                puestos_S[puestos_S.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestos_J[puestos_J.Count - 1].Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket           
                staTN += puestos_J[puestos_J.Count - 1].AtenderTicket(ticket, T);                

                //El puesto deja de estar ocioso
                puestos_J[puestos_J.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_J[puestos_J.Count - 1].Ito);
                puestos_J[puestos_J.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                cola_N.Add(ticket);            
        }

        private void ProcesarLlegadaB()
        {
            clTicket ticket;
            string prioridadTicket = cPrioridadBaja;

            //Llegada de un nuevo ticket
            nsB += 1; //Actualizar el NS correspondiente
            nTB += 1; //Actualizar el NT correspondiente
            ticket = new clTicket(prioridadTicket);
            ticket.Tll = T; //tiempo de llegada del ticket

            if (puestos_J[puestos_J.Count - 1].Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket           
                staTB += puestos_J[puestos_J.Count - 1].AtenderTicket(ticket, T);
                
                //El puesto deja de estar ocioso
                puestos_J[puestos_J.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_J[puestos_J.Count - 1].Ito);
                puestos_J[puestos_J.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestos_SS[puestos_SS.Count - 1].Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket           
                staTB += puestos_SS[puestos_SS.Count - 1].AtenderTicket(ticket, T);
                
                //El puesto deja de estar ocioso
                puestos_SS[puestos_SS.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_SS[puestos_SS.Count - 1].Ito);
                puestos_SS[puestos_SS.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else if (puestos_S[puestos_S.Count - 1].Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket           
                staTB += puestos_S[puestos_S.Count - 1].AtenderTicket(ticket, T);;

                //El puesto deja de estar ocioso
                puestos_S[puestos_S.Count - 1].Sto += clDatosVar.RestarFechas(T, puestos_S[puestos_S.Count - 1].Ito);
                puestos_S[puestos_S.Count - 1].Ito = DateTime.MinValue; //1/1/0001 00:00:00 (no puedo ponerle null)
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                cola_B.Add(ticket);            
        }


        private void ProcesarSalida(ref List<clPuesto> pPuestos, int pIndex)
        {            
            //Avance del tiempo
            T = pPuestos[pIndex].Tps;

            //Sale el ticket            
            //Actualizar el NS correspondiente a la salida 
            if (String.Compare(pPuestos[pIndex].Pta(), cPrioridadAlta) == 0)
            { nsA -= 1; }  //Prioridad ALTA   
            else if (String.Compare(pPuestos[pIndex].Pta(), cPrioridadNormal) == 0)
            { nsN -= 1; }  //Prioridad NORMAL 
            else
            { nsB -= 1; }  //Prioridad BAJA            

            //El puesto continuará atendiendo mientras haya tickets encolados
            if (Cola_A.Count >= 1)
            {
                //Atender ticket de prioridad ALTA
                staTA += pPuestos[pIndex].AtenderTicket(Cola_A[0], T); //sumarizamos tiempos de atención tickets de prioridad ALTA
                Cola_A.RemoveAt(0); //disminuye la cola tickets prioridad ALTA                
                secA += pPuestos[pIndex].Ticket.Tesperacola; //Sumatoria tiempos de espera en Cola ALTA
            }
            else if (Cola_N.Count >= 1)
            {
                //Atender ticket de prioridad NORMAL
                staTN += pPuestos[pIndex].AtenderTicket(Cola_N[0], T);  //sumarizamos tiempos de atención tickets de prioridad NORMAL
                Cola_N.RemoveAt(0); //disminuye la cola tickets prioridad NORMAL                
                secN += pPuestos[pIndex].Ticket.Tesperacola; //Sumatoria tiempos de espera en Cola NORMAL
            }
            else if (Cola_B.Count >= 1)
            {
                //Atender ticket de prioridad BAJA
                staTB += pPuestos[pIndex].AtenderTicket(Cola_B[0], T); //sumarizamos tiempos de atención tickets de prioridad BAJA
                Cola_B.RemoveAt(0); //disminuye la cola tickets prioridad BAJA                                    
                secB += pPuestos[pIndex].Ticket.Tesperacola; //Sumatoria tiempos de espera en Cola BAJA
            }
            else
            {
                //El puesto queda vacío
                pPuestos[pIndex].Ticket = null; 
                pPuestos[pIndex].Tps = HV;
                pPuestos[pIndex].Ito = T;
            }                            

        }

    }


}
