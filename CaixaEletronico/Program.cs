using System;
using System.Collections.Generic;
using System.IO;

class CaixaEletronico
{
    static List<string> transacoes = new List<string>();
    static string nomeCliente, cpfCliente;

    static void Main()
    {
        Console.Write("Informe seu nome: ");
        nomeCliente = Console.ReadLine();

        Console.Write("Informe seu CPF (somente números): ");
        cpfCliente = Console.ReadLine();

        if (cpfCliente.Length != 11 || !ulong.TryParse(cpfCliente, out _))
        {
            Console.WriteLine("CPF incorreto. O CPF deve ter 11 dígitos.");
            return;
        }

        decimal saldo = 1000.00m;
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== Caixa Eletrônico ===");
            Console.WriteLine("1. Depósito");
            Console.WriteLine("2. Saque");
            Console.WriteLine("3. Extrato");
            Console.WriteLine("4. Transferência");
            Console.WriteLine("5. Histórico de Transações");
            Console.WriteLine("6. Sair");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out int opcao))
            {
                switch (opcao)
                {
                    case 1: RealizarTransacao(ref saldo, "depósito"); break;
                    case 2: RealizarTransacao(ref saldo, "saque"); break;
                    case 3: Console.WriteLine($"Saldo atual: R$ {saldo}"); break;
                    case 4: RealizarTransacao(ref saldo, "transferência"); break;
                    case 5: MostrarHistorico(); break;
                    case 6: continuar = false; break;
                    default: Console.WriteLine("Opção inválida."); break;
                }
            }

            if (continuar)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static void RealizarTransacao(ref decimal saldo, string tipo)
    {
        Console.Write($"Informe o valor do {tipo}: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
        {
            if (tipo == "saque" && valor > saldo)
            {
                Console.WriteLine("Saldo insuficiente.");
                return;
            }
            saldo += tipo == "depósito" ? valor : -valor;
            string comprovante = GerarComprovante($"{tipo}: R$ {valor}");
            transacoes.Add(comprovante);
            Console.WriteLine(comprovante);
            SalvarComprovanteEmArquivo(comprovante);
        }
        else
        {
            Console.WriteLine("Valor inválido.");
        }
    }

    static void MostrarHistorico()
    {
        Console.WriteLine("=== Histórico de Transações ===");
        foreach (var transacao in transacoes)
            Console.WriteLine(transacao);
    }

    static string GerarComprovante(string tipoTransacao)
    {
        return $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {nomeCliente} - CPF: {cpfCliente} - {tipoTransacao}";
    }

    static void SalvarComprovanteEmArquivo(string comprovante)
    {
        string caminhoArquivo = "comprovantes.txt";
        using (StreamWriter sw = new StreamWriter(caminhoArquivo, true))
        {
            sw.WriteLine(comprovante);
        }
    }
}
