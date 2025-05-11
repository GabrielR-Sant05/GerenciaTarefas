// Feito por Gabriel Santos e Enzzo Radanovis
//Turma Eng. Computação

using GerenciadorTarefas.Controls;
using GerenciadorTarefas.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

using var context = new TarefaContext();
context.Database.EnsureCreated();



SQLitePCL.Batteries_V2.Init();

Console.WriteLine("Bem vindo, digite um comando do gerenciador");
Console.WriteLine("Comandos: iniciar, status, caminho, ajuda, sair");
while (true)
{    
    Console.Write("comando> ");
    string comando = Console.ReadLine().ToLower();
        switch (comando)
    {
        case "iniciar":
            Console.WriteLine("Antes deseja iniciar com dados pré gravados para teste.(S/N)");
            var res = Console.ReadLine().ToLower();
            if (res == "s")
            {
                AdicionarDadosTeste(res);
                Console.WriteLine("iniciando Gerenciador de tarefas."); 
            }
            else if (res == "n")
            {
                GerenciaRTarefa(res);
                Console.WriteLine("iniciando Gerenciador de tarefas.");
            }
                break;

        case "status":
            Console.WriteLine("Ativo");
            break;

        case "caminho":
            AppControl.MostrarCaminho();
            break;

        case "ajuda":
            Console.WriteLine("Comandos: iniciar, status, caminho, ajuda, sair");
            break;

        case "sair":
            AppControl.Encerrar();
            break;        

        default:            
            Console.WriteLine($"Comando inválido: {comando}.");
            break;
    }    
}

static string StatusToTexto(int status)
{
    return status switch
    {
        0 => "Pendente",
        1 => "Concluída"
    };
}

void AdicionarDadosTeste(string choice)
{
    context.tarefa.RemoveRange(context.tarefa);
    context.SaveChanges();
    context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'tarefa';");

    context.tarefa.Add(new Tarefa { Nome = "Limpar Vidraças", Status = 0 });
    context.tarefa.Add(new Tarefa { Nome = "Atualizar Banco de Dados", Status = 0 });
    context.tarefa.Add(new Tarefa { Nome = "Atualizar Planilha", Status = 0 });
    context.tarefa.Add(new Tarefa { Nome = "Detetizar campo de futebol", Status = 1 });
    context.tarefa.Add(new Tarefa { Nome = "Ir ao mercado", Status = 1 });
    context.tarefa.Add(new Tarefa { Nome = "Comprar presente", Status = 1 });
    context.SaveChanges();
    GerenciaRTarefa(choice);
}
void AtualizaID()
{
    var works = context.tarefa.OrderBy(t => t.Nome).ToList();
    context.tarefa.RemoveRange(context.tarefa);
    context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'tarefa';");
    context.SaveChanges();
    foreach (var t in works)
    {
        t.Id = 0; // Garante que o EF vai gerar um novo ID
        context.tarefa.Add(t);
    }

    context.SaveChanges();
}

void GerenciaRTarefa(string choice)
{
    if(choice == "n")
    {
        context.tarefa.RemoveRange(context.tarefa);
        context.SaveChanges();
        context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'tarefa';");
        GerenciaTarefa();
    }
    else
    {
        GerenciaTarefa();
    }
}
void GerenciaTarefa()
{
    while (true)
    {
        Console.WriteLine("\n=== MENU ===");
        Console.WriteLine("1. Adicionar tarefa");
        Console.WriteLine("2. Listar tarefas");
        Console.WriteLine("3. Atualizar tarefa");
        Console.WriteLine("4. Remover tarefa");
        Console.WriteLine("5. Concluir tarefa");
        Console.WriteLine("6. Sair");
        Console.Write("Escolha: ");
        var escolha = Console.ReadLine();

        switch (escolha)
        {
            case "1":
                Console.Write("Título: ");
                var titulo = Console.ReadLine();

                Console.Write("Status (0 = Pendente, 1 = Concluída): ");
                int status = int.TryParse(Console.ReadLine(), out var st) ? st : 0;

                context.tarefa.Add(new Tarefa { Nome = titulo, Status = status });
                context.SaveChanges();
                Console.WriteLine("Tarefa adicionada!");

                break;

            case "2":
                var tarefas = context.tarefa.ToList();
                foreach (var t in tarefas)
                    Console.WriteLine($"{t.Id}: {t.Nome} - {StatusToTexto(t.Status)}");
                break;

            case "3":
                Console.Write("ID da tarefa: ");
                if (int.TryParse(Console.ReadLine(), out int idEdit))
                {
                    var tarefa = context.tarefa.Find(idEdit);
                    if (tarefa != null)
                    {
                        Console.Write("Deseja Editar o nome da tarefa.\n(S/N)");
                        var res = Console.ReadLine().ToLower();
                        if (res == "s")
                        {
                            Console.Write("Novo título: ");
                            tarefa.Nome = Console.ReadLine();
                        }
                        else if(res == "n")
                        {
                            Console.WriteLine("Nome inalterado.");
                        }
                        else
                        {
                            Console.WriteLine("Resposta não reconhecida.");
                        }

                            Console.Write("Novo status (0 = Pendente, 1 = Concluída): ");
                        tarefa.Status = int.TryParse(Console.ReadLine(), out var novoStatus) ? novoStatus : tarefa.Status;

                        context.SaveChanges();
                        Console.WriteLine("Tarefa atualizada.");
                    }
                }
                break;

            case "4":
                Console.Write("ID da tarefa: ");
                if (int.TryParse(Console.ReadLine(), out int idRemove))
                {
                    var tarefa = context.tarefa.Find(idRemove);
                    if (tarefa != null)
                    {
                        Console.WriteLine("Deseja mesmo remover a tarefa.\n(S/N)");
                        var res = Console.ReadLine().ToLower();
                        if (res == "s")
                        {
                            context.tarefa.Remove(tarefa);
                            context.SaveChanges();
                            Console.WriteLine("Tarefa removida.");

                            AtualizaID();

                            context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Operação cancelada.");
                        }
                    }
                }
                break;

            case "5":
                var tarefasl = context.tarefa.Where(t => t.Status == 0).OrderBy(t => t.Nome).ToList();
                foreach (var t in tarefasl)
                {
                    Console.WriteLine($"ID: {t.Id} | Título: {t.Nome} | Status: {t.Status}");
                }
                Console.WriteLine("Digite o ID da tarefa a ser concluida");
                if(int.TryParse(Console.ReadLine(), out int id))
                {
                    var ltarefas = context.tarefa.Find(id);
                    if(ltarefas != null)
                    {
                        int newstatus = 1;
                        ltarefas.Status = newstatus;
                        context.SaveChanges();
                        Console.WriteLine("Tarefa Concluída.");
                    }
                }
                break;

            case "6":
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
}