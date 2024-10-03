using System;

class CaixaEletronico
{
    static void Main()
    {
        decimal saldo = 1000.00m;
        const decimal limiteSaque = 500.00m;
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== Caixa Eletrônico ===");
            Console.WriteLine("1. Depósito");
            Console.WriteLine("2. Saque");
            Console.WriteLine("3. Extrato");
            Console.WriteLine("4. Transferência");
            Console.WriteLine("5. Sair");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out int opcao))
            {
                switch (opcao)
                {
                    case 1: // Depósito
                        RealizarDeposito(ref saldo);
                        break;

                    case 2: // Saque
                        RealizarSaque(ref saldo, limiteSaque);
                        break;

                    case 3: // Extrato
                        Console.WriteLine($"Saldo atual: R$ {saldo}");
                        break;

                    case 4: // Transferência
                        RealizarTransferencia(ref saldo);
                        break;

                    case 5: // Sair
                        continuar = false;
                        Console.WriteLine("Saindo do caixa eletrônico. Até mais!");
                        break;

                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }

            if (continuar)
            {
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static void RealizarDeposito(ref decimal saldo)
    {
        Console.Write("Informe o valor do depósito: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal deposito) && deposito > 0)
        {
            saldo += deposito;
            Console.WriteLine($"Depósito de R$ {deposito} realizado com sucesso.");
        }
        else
        {
            Console.WriteLine("Valor inválido. O depósito deve ser maior que zero.");
        }
    }

    static void RealizarSaque(ref decimal saldo, decimal limiteSaque)
    {
        Console.Write("Informe o valor do saque: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal saque) && saque > 0 && saque <= limiteSaque && saque <= saldo)
        {
            saldo -= saque;
            Console.WriteLine($"Saque de R$ {saque} realizado com sucesso.");
        }
        else
        {
            Console.WriteLine(saque > limiteSaque ? 
                $"O valor do saque não pode ultrapassar R$ {limiteSaque}." : 
                "Saldo insuficiente ou valor inválido.");
        }
    }

    static void RealizarTransferencia(ref decimal saldo)
    {
        Console.Write("Informe o valor da transferência: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal transferencia) && transferencia > 0 && transferencia <= saldo)
        {
            saldo -= transferencia;
            Console.WriteLine($"Transferência de R$ {transferencia} realizada com sucesso.");
        }
        else
        {
            Console.WriteLine("Saldo insuficiente ou valor inválido.");
        }
    }
}
