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

        while (true)
        {
            Console.Write("Informe seu CPF (somente números): ");
            cpfCliente = Console.ReadLine();

            if (cpfCliente.Length == 11 && ulong.TryParse(cpfCliente, out _))
            {
                break; // CPF válido, sai do loop
            }
            else
            {
                Console.WriteLine("CPF incorreto. O CPF deve ter 11 dígitos e conter apenas números.");
            }
        }

        decimal saldo = 1000.00m;
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            int largura = 50;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=".PadLeft(largura / 2, '='));
            Console.WriteLine("Caixa Eletronico");
            Console.WriteLine("=".PadLeft(largura / 2, '='));
            Console.ResetColor();

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
                    case 3: MostrarExtrato(); break;
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
            SalvarOperacaoEmArquivo(tipo, valor); 
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

    static void MostrarExtrato()
    {
        string caminhoExtrato = "extrato.txt";
        using (StreamWriter sw = new StreamWriter(caminhoExtrato, false)) 
        {
            sw.WriteLine($"=== Extrato Bancário - {nomeCliente} - CPF: {cpfCliente} ===");
            Console.WriteLine($"=== Extrato Bancário - {nomeCliente} - CPF: {cpfCliente} ===");
            sw.WriteLine($"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            Console.WriteLine($"Data: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sw.WriteLine();
            Console.WriteLine();
            foreach (var transacao in transacoes)
            {
                sw.WriteLine(transacao);
                Console.WriteLine(transacao);
            }
        }
        Console.WriteLine($"Extrato bancário salvo em: {caminhoExtrato}");
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
             Console.WriteLine(comprovante);
        }
    }

    static void SalvarOperacaoEmArquivo(string tipo, decimal valor)
    {
        string caminhoOperacao = "operacoes.txt";
        using (StreamWriter sw = new StreamWriter(caminhoOperacao, true))
        {
            sw.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {nomeCliente} - CPF: {cpfCliente} - {tipo}: R$ {valor}");
            Console.WriteLine($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {nomeCliente} - CPF: {cpfCliente} - {tipo}: R$ {valor}");
        }
    }
}
