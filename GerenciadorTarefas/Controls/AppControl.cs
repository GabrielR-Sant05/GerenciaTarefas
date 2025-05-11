using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Controls
{
    static class AppControl
    {
        public static void Encerrar()
        {
            Console.WriteLine("Encerrando aplicação...");
            Environment.Exit(0);
        }

        public static void Reiniciar()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            Console.WriteLine("Reiniciando aplicação...");
            Process.Start(new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = AppContext.BaseDirectory,
                UseShellExecute = true
            });

            Environment.Exit(0);
        }

        public static void MostrarCaminho()
        {
            Console.WriteLine("Executável: " + Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine("Diretório base: " + AppContext.BaseDirectory);
        }
    }
}
