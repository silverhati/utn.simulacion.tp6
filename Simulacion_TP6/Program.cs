using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Simulacion_TP6
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ESTO LO PUSE PARA PROBAR LA GENERACION DE LOS NUMEROS ALEATORIOS
            //BORRAR 
            System.IO.StreamWriter IA;
            IA = File.CreateText("C:\\IA.txt");

            System.IO.StreamWriter TAJ;
            TAJ = File.CreateText("C:\\TAJ.txt");

            System.IO.StreamWriter TASS;
            TASS = File.CreateText("C:\\TASS.txt");

            System.IO.StreamWriter TAS;
            TAS = File.CreateText("C:\\TAS.txt");

            for (int i = 0; i < 5000; i++)
            {
                IA.WriteLine(clDatosVar.GenerarIA().ToString());
            }

            for (int i = 0; i < 5000; i++)
            {
                TAJ.WriteLine(clDatosVar.GenerarTA_J().ToString());   
            }

            for (int i = 0; i < 5000; i++)
            {
                TASS.WriteLine(clDatosVar.GenerarTA_SS().ToString());
            }

            for (int i = 0; i < 5000; i++)
            {
                TAS.WriteLine(clDatosVar.GenerarTA_S().ToString());
            }

            IA.Close();
            TAJ.Close();
            TASS.Close();
            TAS.Close();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //clSimu simu = new clSimu(0,0,0);
            //simu.SumarTiempos(new DateTime(2018,10,5,15,26,22),180);
        }
    }
}
