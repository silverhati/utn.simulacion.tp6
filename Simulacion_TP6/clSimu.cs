using System;
using System.Collections.Generic;
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
        private int nsA, nsN, nsB,
                    nColaA, nColaN, nColaB;
        public int NsA { get => nsA; set => nsA = value; }
        public int NsN { get => nsN; set => nsN = value; }
        public int NsB { get => nsB; set => nsB = value; }
        public int NColaA { get => nColaA; set => nColaA = value; }
        public int NColaN { get => nColaN; set => nColaN = value; }
        public int NColaB { get => nColaB; set => nColaB = value; }

        //Otras
        DateTime T;
        public DateTime Time { get => T; set => T = value; }
        DateTime TF;
        DateTime TPLL;        
        DateTime HV = new DateTime(9999, 12, 31, 23, 59, 59);        
        List<clPuesto> puestos_S, puestos_SS, puestos_J;
        public List<clPuesto> puestosS { get => puestos_S; }
        public List<clPuesto> puestosSS { get => puestos_SS; }
        public List<clPuesto> puestosJ { get => puestos_J; }
        clSimuResultados resultados;
        bool procesar = true;
        const string cRecursoS = "S",
                     cRecursoSS = "SS",
                     cRecursoJ = "J",
                     cPrioridadAlta = "A",
                     cPrioridadNormal = "N",
                     cPrioridadBaja = "B";
        double nTA, nTN, nTB;
        double staTA, staTN, staTB, sSA, sSN, sSB,sLLA,sLLN,sLLB; //minutos
        public double NTA { get => nTA; set => nTA = value; }
        public double NTN { get => nTN; set => nTN = value; }
        public double NTB { get => nTB; set => nTB = value; }
        public double StaTA { get => staTA; set => staTA = value; }
        public double StaTN { get => staTN; set => staTN = value; }
        public double StaTB { get => staTB; set => staTB = value; }
        public double Ssa { get => sSA; set => sSA = value; }
        public double Ssn { get => sSN; set => sSN = value; }
        public double Ssb { get => sSB; set => sSB = value; }
        public double Slla { get => sLLA; set => sLLA = value; }
        public double Slln { get => sLLN; set => sLLN = value; }
        public double Sllb { get => sLLB; set => sLLB = value; }


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

            //Devolver resultados de la simulación
            resultados = this.CalcularResultados();
            Console.WriteLine("Datos ingresados. Cantidad Sr:" + cantS + " Cantidad Ssr:"+ cantSS + " Cantidad Jr: "+cantJ);
            Console.WriteLine("TMEA:" + resultados.TMEA + " TMEN:" + resultados.TMEN + " TMEB: " + resultados.TMEB);
            Console.WriteLine("TMAA:" + resultados.TMAA + " TMAN:" + resultados.TMAN + " TMAB: " + resultados.TMAB);
            Console.WriteLine("PTOS:" + resultados.PTOS + " PTOSS:" + resultados.PTOSS + " PTOJ: " + resultados.PTOJ);
            return resultados;
        }
        
        
        private void CondicionesIniciales()
        {
            //Tiempo Próxima Llegada
            TPLL = new DateTime(2019, 01, 01, 09, 00, 00);

            //Tiempo Final
            TF = new DateTime(2099, 12, 31, 18, 00, 00);

            //Inicializar puestos SENIOR
            puestos_S = new List<clPuesto>();
            for (var index = 0; index < cantS; index++)
            {
                puestos_S.Add(new clPuesto() { Tps = HV, Pta = null, Tipo = cRecursoS });
            }

            //Inicializar puestos SEMI SENIOR
            puestos_SS = new List<clPuesto>();
            for (var index = 0; index < cantSS; index++)
            {
                puestos_SS.Add(new clPuesto() { Tps = HV, Pta = null, Tipo = cRecursoSS });
            }

            //Inicializar puestos JUNIOR
            puestos_J = new List<clPuesto>();
            for (var index = 0; index < cantJ; index++)
            {
                puestos_J.Add(new clPuesto() { Tps = HV, Pta = null, Tipo = cRecursoJ });
            }
        }

        private void ProcesarLlegada()
        {
            double IA;
            string prioridadTicket;

            T = TPLL; //Avance del tiempo
            
            //Próxima llegada (EFNC)
            IA = clDatosVar.GenerarIA();
            TPLL = this.SumarTiempos(T, IA);

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
            double TA;
            string prioridadTicket = cPrioridadAlta;

            nsA += 1; //Actualizar el NS correspondiente
            sLLA += T.Hour * 100 + T.Minute;

            if (puestos_S[puestos_S.Count - 1].Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket
                puestos_S[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_S[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_S(); //Generar tiempo de atención
                puestos_S[puestos_S.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                staTA += (T.Hour * 100 + T.Minute);
                puestos_S[puestos_S.Count - 1].Pta = prioridadTicket;
            }
            else if (puestos_S[puestos_SS.Count - 1].Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket
                puestos_SS[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_SS[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_SS(); //Generar tiempo de atención
                puestos_SS[puestos_SS.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                staTA += (T.Hour * 100 + T.Minute);
                puestos_SS[puestos_SS.Count - 1].Pta = prioridadTicket;
            }
            else if (puestos_J[puestos_J.Count - 1].Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket
                puestos_J[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_J[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_J(); //Generar tiempo de atención
                puestos_J[puestos_J.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                staTA += (T.Hour * 100 + T.Minute);
                puestos_J[puestos_J.Count - 1].Pta = prioridadTicket;
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                nColaA += 1;
        }

        private void ProcesarLlegadaN()
        {
            double TA;
            string prioridadTicket = cPrioridadNormal;

            nsN += 1; //Actualizar el NS correspondiente
            sLLN += T.Hour * 100 + T.Minute;

            if (puestos_SS[puestos_SS.Count - 1].Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket
                puestos_SS[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_SS[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_SS(); //Generar tiempo de atención
                puestos_SS[puestos_SS.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                staTN += (T.Hour * 100 + T.Minute);
                puestos_SS[puestos_SS.Count - 1].Pta = prioridadTicket;
            }
            else if (puestos_S[puestos_S.Count - 1].Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket
                puestos_S[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_S[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_S(); //Generar tiempo de atención
                puestos_S[puestos_S.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                staTN += (T.Hour * 100 + T.Minute);
                puestos_S[puestos_S.Count - 1].Pta = prioridadTicket;                
            }
            else if (puestos_J[puestos_J.Count - 1].Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket
                puestos_J[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_J[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_J(); //Generar tiempo de atención
                puestos_J[puestos_J.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                staTN += (T.Hour * 100 + T.Minute);
                puestos_J[puestos_J.Count - 1].Pta = prioridadTicket;
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                nColaN += 1;
        }

        private void ProcesarLlegadaB()
        {
            double TA;
            string prioridadTicket = cPrioridadBaja;

            nsB += 1; //Actualizar el NS correspondiente
            sLLN += T.Hour * 100 + T.Minute;

            if (puestos_J[puestos_J.Count - 1].Tps == HV)
            {
                //Si hay un puesto JUNIOR libre, toma el ticket
                puestos_J[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_J[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_J(); //Generar tiempo de atención
                puestos_J[puestos_J.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                puestos_J[puestos_J.Count - 1].Pta = prioridadTicket;
                staTB += (T.Hour * 100 + T.Minute);
            }
            else if (puestos_SS[puestos_SS.Count - 1].Tps == HV)
            {
                //Si hay un puesto SEMI SENIOR libre, toma el ticket
                puestos_SS[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_SS[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_SS(); //Generar tiempo de atención
                puestos_SS[puestos_SS.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                puestos_SS[puestos_SS.Count - 1].Pta = prioridadTicket;
                staTB += (T.Hour * 100 + T.Minute);
            }
            else if (puestos_S[puestos_S.Count - 1].Tps == HV)
            {
                //Si hay un puesto SENIOR libre, toma el ticket
                puestos_S[puestos_S.Count - 1].Sto += ((T.Hour * 100 + T.Minute) - puestos_S[puestos_S.Count - 1].Ito); //Calculo el STO
                TA = clDatosVar.GenerarTA_S(); //Generar tiempo de atención
                puestos_S[puestos_S.Count - 1].Tps = this.SumarTiempos(T, TA); //Tps de este ticket
                puestos_S[puestos_S.Count - 1].Pta = prioridadTicket;
                staTB += (T.Hour * 100 + T.Minute);
            }
            else
                //Nadie lo pueda atender y el ticket queda encolado
                nColaB += 1;
        }


        private void ProcesarSalida(ref List<clPuesto> pPuestos, int pIndex)
        {
            double TA;
            T = pPuestos[pIndex].Tps; //Avance del tiempo

            //Actualizar el NS correspondiente a la salida
            if (String.Compare(pPuestos[pIndex].Pta, cPrioridadAlta) == 0)
                nsA -= 1; //Prioridad ALTA
            else if (String.Compare(pPuestos[pIndex].Pta, cPrioridadNormal) == 0)
                nsN -= 1; //Prioridad NORMAL
            else
                nsB -= 1; //Prioridad BAJA

            //Se continuará atendiendo mientras haya tickets encolados
            if (nColaA >= 1)
            {
                nColaA -= 1;
                pPuestos[pIndex].Pta = cPrioridadAlta;// Prioridad del nuevo ticket asignado
                TA = calcularTiempoAtencionTicket(pPuestos[pIndex]);
                pPuestos[pIndex].Tps = this.SumarTiempos(T, TA);
                staTA += TA;
                nTA++;
                sSA+=T.Hour*100+T.Minute;
            }
            else if (nColaN >= 1)
            {
                nColaN -= 1;
                pPuestos[pIndex].Pta = cPrioridadNormal;// Prioridad del nuevo ticket asignado
                TA = calcularTiempoAtencionTicket(pPuestos[pIndex]);
                pPuestos[pIndex].Tps = this.SumarTiempos(T, TA);
                staTN += TA;
                nTN++;
                sSN+= T.Hour * 100 + T.Minute;
            }
            else if (nColaB >= 1)
            {
                nColaB -= 1;
                pPuestos[pIndex].Pta = cPrioridadBaja;// Prioridad del nuevo ticket asignado
                TA = calcularTiempoAtencionTicket(pPuestos[pIndex]);
                pPuestos[pIndex].Tps = this.SumarTiempos(T, TA);
                staTB += TA;
                nTB++;
                sSB += T.Hour * 100 + T.Minute;
            }
            else
            {
                //El puesto queda vacío
                pPuestos[pIndex].Pta = null; 
                pPuestos[pIndex].Tps = HV;
                pPuestos[pIndex].Ito = T.Hour*100+T.Minute;
            }                            

        }

        private double calcularTiempoAtencionTicket(clPuesto puesto)
        {
            double TA = 0;
            //Generar el tiempo de atención
            if (String.Compare(puesto.Tipo, cRecursoS) == 0)
                TA = clDatosVar.GenerarTA_S();
            else if (String.Compare(puesto.Tipo, cRecursoSS) == 0)
                TA = clDatosVar.GenerarTA_SS();
            else
                TA = clDatosVar.GenerarTA_J();

            return TA;
        }

        private clSimuResultados CalcularResultados()
        {
            //TODO: calcularResultados()
            clSimuResultados resultados = new clSimuResultados(this);
            resultados.calcular();
            return resultados;
        }

        private DateTime SumarTiempos(DateTime pT, double pTA)
        {
            //Sumar cierta cant de minutos al datetime, teniendo en cuenta que el HORARIO LABORAL es de L a V de 9 a 18hs
            DateTime inicio = new DateTime(pT.Year, pT.Month, pT.Day,9,0,0);
            DateTime fin = new DateTime(pT.Year, pT.Month, pT.Day, 18, 0, 0);
            //Sumo minutos
            DateTime fechaActualizada = pT.AddMinutes(pTA);
            //Si esta en el medio retorna la fecha correspondiente
            if (fechaActualizada<fin && fechaActualizada>inicio)
            {
                return fechaActualizada;
            }
            else if (fechaActualizada > fin)
            {
                //La fecha es mayor y hay q modificarla
                //Primero consigo la diferencia entre la fecha fin y la recibida(actualizada), esa diferencia es la que se suma al dia siguiente
                TimeSpan diferencia = fechaActualizada - fin;
                //Calcular: si la fecha es viernes pasa para lunes
                if (pT.DayOfWeek.ToString().Equals("Friday"))
                {
                    return inicio.AddDays(3).Add(diferencia);
                }
                else {
                    return inicio.AddDays(1).Add(diferencia);
                }
            }
            else
            {
                return fechaActualizada;
            }
        }
    }


}
