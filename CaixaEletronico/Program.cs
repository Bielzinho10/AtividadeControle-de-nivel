using System;
using System.Collections.Generic;
using System.IO;

class CaixaEletronico
{
    static List<string> transacoes = new List<string>();
    static string nomeCliente, cpfCliente, senhaCliente;
    static decimal saldo = 1000.00m;
    static decimal limiteSaque = 500.00m;
    static decimal taxaTransferencia = 0.0005m;
    static bool autenticado = false;

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
                break;
            }
            else
            {
                Console.WriteLine("CPF incorreto. O CPF deve ter 11 dígitos e conter apenas números.");
            }
        }

        while (true)
        {
            Console.Write("Informe sua senha (6 dígitos): ");
            senhaCliente = Console.ReadLine();

            if (senhaCliente.Length == 6 && ulong.TryParse(senhaCliente, out _))
            {
                autenticado = true;
                break;
            }
            else
            {
                Console.WriteLine("Senha inválida. A senha deve ter 6 dígitos.");
            }
        }

        bool continuar = true;

        while (continuar && autenticado)
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
            Console.WriteLine("6. Aplicações Financeiras");
            Console.WriteLine("7. Sair");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out int opcao))
            {
                switch (opcao)
                {
                    case 1: RealizarTransacao(ref saldo, "depósito"); break;
                    case 2: RealizarSaque(ref saldo); break;
                    case 3: MostrarExtrato(); break;
                    case 4: RealizarTransferencia(ref saldo); break;
                    case 5: MostrarHistorico(); break;
                    case 6: AplicacoesFinanceiras(ref saldo); break;
                    case 7: continuar = false; break;
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

    static void RealizarSaque(ref decimal saldo)
    {
        Console.Write($"Informe o valor do saque (Limite: R$ {limiteSaque}): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
        {
            if (valor > saldo)
            {
                Console.WriteLine("Saldo insuficiente.");
                return;
            }
            if (valor > limiteSaque)
            {
                Console.WriteLine($"Valor excede o limite de saque diário de R$ {limiteSaque}.");
                return;
            }

            saldo -= valor;
            string comprovante = GerarComprovante($"saque: R$ {valor}");
            transacoes.Add(comprovante);
            Console.WriteLine(comprovante);
            SalvarComprovanteEmArquivo(comprovante);
            SalvarOperacaoEmArquivo("saque", valor);
        }
        else
        {
            Console.WriteLine("Valor inválido.");
        }
    }

    static void RealizarTransferencia(ref decimal saldo)
    {
        Console.Write("Informe o valor da transferência: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
        {
            decimal taxa = valor * taxaTransferencia;
            if (valor + taxa > saldo)
            {
                Console.WriteLine("Saldo insuficiente para transferência e taxa.");
                return;
            }

            saldo -= (valor + taxa);
            string comprovante = GerarComprovante($"transferência: R$ {valor} (taxa: R$ {taxa})");
            transacoes.Add(comprovante);
            Console.WriteLine(comprovante);
            SalvarComprovanteEmArquivo(comprovante);
            SalvarOperacaoEmArquivo("transferência", valor + taxa);
        }
        else
        {
            Console.WriteLine("Valor inválido.");
        }
    }

    static void AplicacoesFinanceiras(ref decimal saldo)
    {
        Console.WriteLine("Escolha uma aplicação:");
        Console.WriteLine("1. Poupança (rendimento de 0,56% mensal)");
        Console.WriteLine("2. CDB (rendimento de 90% do CDI)");
        Console.Write("Escolha uma opção: ");

        if (int.TryParse(Console.ReadLine(), out int opcaoAplicacao))
        {
            Console.Write("Informe o valor a ser aplicado: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valorAplicacao) && valorAplicacao > 0 && valorAplicacao <= saldo)
            {
                saldo -= valorAplicacao;

                decimal rendimento = 0;
                switch (opcaoAplicacao)
                {
                    case 1:
                        rendimento = valorAplicacao * 0.0056m;
                        break;
                    case 2:
                        rendimento = valorAplicacao * 0.9m * 0.1m;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        return;
                }

                Console.WriteLine($"Aplicação de R$ {valorAplicacao} realizada com rendimento estimado de R$ {rendimento}.");
                transacoes.Add(GerarComprovante($"aplicação: R$ {valorAplicacao}"));
            }
            else
            {
                Console.WriteLine("Valor inválido ou insuficiente.");
            }
        }
        else
        {
            Console.WriteLine("Opção inválida.");
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