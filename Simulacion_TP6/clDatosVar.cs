using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulacion_TP6
{
    static public class clDatosVar
    {
        static public double GenerarIA()
        {
            //TODO: lógica que devuelva un IA
            Random random = new Random();
            return Convert.ToDouble(random.Next(0, 100));

        }
        static public double GenerarTA_S()
        {
            //TODO: lógica que devuelva un TA_S (tiempo atención SENIOR)
            Random random = new Random();
            return Convert.ToDouble(random.Next(15, 120));
        }
        static public double GenerarTA_SS()
        {
            //TODO: lógica que devuelva un TA_SS (tiempo atención SEMI SENIOR)
            Random random = new Random();
            return Convert.ToDouble(random.Next(15, 120));
        }
        static public double GenerarTA_J()
        {
            //TODO: lógica que devuelva un TA_J (tiempo atención JUNIOR)
            Random random = new Random();
            return Convert.ToDouble(random.Next(15, 120));
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
    }
}
