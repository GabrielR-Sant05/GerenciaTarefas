using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vosk;

namespace GerenciadorTarefas.Controls
{
    class VoiceListener
    {
        //string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\.."));
        void TesteLib()
        {
            var model = new Model("Data/model"); // Substitua pelo caminho correto
            System.Console.WriteLine("Modelo carregado com sucesso!");
        }
    }
}
