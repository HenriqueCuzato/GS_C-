// Projeto: TECNOENERGIA
// Objetivo: Monitoramento de falhas de energia e ameaças cibernéticas

using System;
using System.Collections.Generic;

namespace TecnoenergiaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SistemaMonitoramento sistema = new SistemaMonitoramento();
            sistema.Iniciar();
        }
    }

    class Usuario
    {
        public string Nome { get; set; }
        public string Senha { get; set; }

        public bool Autenticar(string nome, string senha)
        {
            return nome == "admin" && senha == "1234";
        }
    }

    class FalhaEnergia
    {
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public string Gravidade { get; set; }
        public string Tipo { get; set; }

        public bool Validar()
        {
            return Data <= DateTime.Now && !string.IsNullOrEmpty(Local);
        }

        public string GerarAlerta()
        {
            if (Gravidade.ToLower() == "alta")
                return $"[ALERTA] Falha crítica detectada em {Local} às {Data}";
            else
                return $"[INFO] Falha registrada em {Local} às {Data}";
        }
    }

    class LogEvento
    {
        public DateTime DataHora { get; set; }
        public string Evento { get; set; }

        public LogEvento(string evento)
        {
            DataHora = DateTime.Now;
            Evento = evento;
        }

        public void Exibir()
        {
            Console.WriteLine($"[LOG] {DataHora}: {Evento}");
        }
    }

    class Relatorio
    {
        public List<FalhaEnergia> Falhas { get; set; }

        public void Gerar()
        {
            Console.WriteLine("\n===== RELATÓRIO DE FALHAS =====");
            foreach (var falha in Falhas)
            {
                Console.WriteLine($"- {falha.Local} | {falha.Data} | Gravidade: {falha.Gravidade} | Tipo: {falha.Tipo}");
            }
        }
    }

    class SistemaMonitoramento
    {
        private List<FalhaEnergia> listaFalhas = new List<FalhaEnergia>();
        private List<LogEvento> logs = new List<LogEvento>();

        public void Iniciar()
        {
            Console.WriteLine("=== SISTEMA TECNOENERGIA ===\n");
            Usuario usuario = new Usuario();

            Console.Write("Login: ");
            string login = Console.ReadLine();
            Console.Write("Senha: ");
            string senha = Console.ReadLine();

            if (!usuario.Autenticar(login, senha))
            {
                Console.WriteLine("Acesso negado!");
                return;
            }

            Console.WriteLine("Acesso permitido!\n");

            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("1 - Registrar Falha\n2 - Ver Relatório\n3 - Sair");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        RegistrarFalha();
                        break;
                    case "2":
                        GerarRelatorio();
                        break;
                    case "3":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        private void RegistrarFalha()
        {
            try
            {
                Console.Write("Local da falha: ");
                string local = Console.ReadLine();

                DateTime data;
                while (true)
                {
                    Console.Write("Data (yyyy-MM-dd HH:mm): ");
                    if (DateTime.TryParse(Console.ReadLine(), out data))
                        break;
                    Console.WriteLine("Formato de data inválido. Tente novamente.");
                }

                Console.Write("Gravidade (baixa/media/alta): ");
                string gravidade = Console.ReadLine();

                Console.Write("Tipo de falha (ex: pane elétrica, queda de energia, sobrecarga): ");
                string tipo = Console.ReadLine();

                FalhaEnergia falha = new FalhaEnergia
                {
                    Local = local,
                    Data = data,
                    Gravidade = gravidade,
                    Tipo = tipo
                };

                if (falha.Validar())
                {
                    listaFalhas.Add(falha);
                    Console.WriteLine(falha.GerarAlerta());
                    logs.Add(new LogEvento($"Falha registrada: {local} - {gravidade} - {tipo}"));
                }
                else
                {
                    Console.WriteLine("Dados inválidos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao registrar falha: " + ex.Message);
            }
        }

        private void GerarRelatorio()
        {
            Relatorio relatorio = new Relatorio { Falhas = listaFalhas };
            relatorio.Gerar();

            Console.WriteLine("\n===== LOGS DO SISTEMA =====");
            foreach (var log in logs)
            {
                log.Exibir();
            }
        }
    }
}
