using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulacion_TP6
{
    static public class clDatosVar
    {
        //La creo acá para que no se cree cada vez que se llama a los métodos, mejora la aleatoreidad
        static Random random = new Random();

        static public double GenerarIA()
        {
            Double a = 1; //dominio funcion: a < x < infinito
            Double potencia1 = 2.551150568906;
            Double potencia2 = 0.39198;

            return Math.Pow((Math.Pow(a, potencia2))/(1 - random.NextDouble() * Math.Pow(a, potencia2)), potencia1);
        }
        static public double GenerarTA_S()
        {
            Double x1;
            Double y1;

            Double b = 25935; //cota superior de datos obtenidos
            Double a = 14.756; //este valor es igual a gamma, dominio funcion: gamma < x < infinito
            Double M = 0.00731433; //maximo valor que alcanza la funcion TASS

            do
            {

                x1 = random.NextDouble() * (b - a) + a;
                y1 = random.NextDouble() * M;

            } while (funcionTAS(x1) < y1);

            return x1;
        }

        private static double funcionTAS(double x)
        {
            Double alpha = 1.3702;
            Double beta = 84.858;
            Double gamma = 14.756;

            return (alpha / beta) * Math.Pow((x - gamma) / beta, alpha - 1) * Math.Pow(1 + Math.Pow((x - gamma)/ beta, alpha), -2);
        }

        static public double GenerarTA_SS()
        {
            Double x1;
            Double y1;

            Double b = 37590; //cota superior de datos obtenidos
            Double a = 10.294; //este valor es igual a gamma, dominio funcion: gamma < x < infinito
            Double M = 0.00676714; //maximo valor que alcanza la funcion TASS

            do
            {

                x1 = random.NextDouble() * (b - a) + a;
                y1 = random.NextDouble() * M;

            } while (funcionTASS(x1) < y1);

            return  x1;
        }

        private static double funcionTASS(double x)
        {
            Double constante = 3.29824;
            Double gamma = 10.294;
            Double otraConstante = -34.1755;

            Double pasoIntermedio1 = (Math.Pow(Math.E, (otraConstante / (x - gamma))));
            Double pasoIntermedio2 = (Math.Pow(x - gamma, 1.5));

            return constante * (pasoIntermedio1 / pasoIntermedio2);
        }

        static public Double GenerarTA_J()
        {
            Double x1;
            Double y1;

            Double b = 25770; //cota superior de datos obtenidos
            Double a = 10.156; //este valor es igual a gamma, dominio funcion: gamma < x < infinito
            Double M = 0.00608767; //maximo valor que alcanza la funcion TAJ

            do {

                x1 = random.NextDouble() * (b - a) + a;
                y1 = random.NextDouble() * M;

            } while (funcionTAJ(x1) < y1);

            return x1;
        }

        static public Double funcionTAJ(Double x)
        {
            Double constante = 3.47744;
            Double gamma = 10.156;
            Double otraConstante = -37.99;
            
            Double pasoIntermedio1 = (Math.Pow(Math.E, (otraConstante / (x - gamma))));
            Double pasoIntermedio2 = (Math.Pow(x - gamma, 1.5));

            return constante * (pasoIntermedio1 / pasoIntermedio2);
        }

        static public string GenerarPrioridadTicket()
        {
            /* Distribución
             * ALTA: 14,42%
             * NORMAL: 68,96%
             * BAJA: 16,62 %
            */
            Random random = new Random();
            int number = random.Next(0, 10000);

            if (number <= 1442)
                return "A"; //Prioridad ALTA
            else if (number <= 3104)
                return "B"; //Prioridad BAJA
            else return "N"; //Prioridad NORMAL
        }

        //Método auxiliares
        static public DateTime SumarTiempos(DateTime pT, double pTA)
        {
            //Sumar cierta cant de minutos al datetime, teniendo en cuenta que el HORARIO LABORAL es de L a V de 9 a 18hs
            DateTime inicio = new DateTime(pT.Year, pT.Month, pT.Day, 9, 0, 0);
            DateTime fin = new DateTime(pT.Year, pT.Month, pT.Day, 18, 0, 0);
            //Sumo minutos
            DateTime fechaActualizada = pT.AddMinutes(pTA);
            //Si esta en el medio retorna la fecha correspondiente
            if (fechaActualizada < fin && fechaActualizada > inicio)
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
                else
                {
                    return inicio.AddDays(1).Add(diferencia);
                }
            }
            else
            {
                return fechaActualizada;
            }
        }

        static public double RestarFechas(DateTime iFechaFin, DateTime iFechaIni)
        {
            //TODO: RestarFechas() - Resta dos fechas, devuelve resultado en minutos
            // Contemplar días y horarios laborales
            TimeSpan span;

            //if (iFechaFin.Date == iFechaIni.Date)
            //{
                span = iFechaFin.Subtract(iFechaIni);
                return Convert.ToDouble(span.TotalMinutes);
            //}            
        }
    }
}
