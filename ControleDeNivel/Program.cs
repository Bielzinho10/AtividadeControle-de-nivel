using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Informe a situação da Boia A: ");
        int boiaA = int.Parse(Console.ReadLine());
		Console.WriteLine("Informe a situação da Boia B");
        int boiaB = int.Parse(Console.ReadLine());
		Console.WriteLine("Informe a situação da Boia C");
        int boiaC = int.Parse(Console.ReadLine());

        if (boiaA == 0 && boiaB == 0 && boiaC == 0)
        {
            Console.WriteLine("Ligar válvula e desligar bomba");
        }
        else if (boiaA == 0 && boiaB == 0 && boiaC == 1)
        {
            Console.WriteLine("Ligar válvula e desligar bomba");
        }
        else if (boiaA == 0 && boiaB == 1 && boiaC == 0)
        {
            Console.WriteLine("Erro na válvula e na bomba");
        }
        else if (boiaA == 0 && boiaB == 1 && boiaC == 1)
        {
            Console.WriteLine("Erro na válvula e na bomba");
        }
        else if (boiaA == 1 && boiaB == 0 && boiaC == 0)
        {
            Console.WriteLine("Ligar válvula e bomba");
        }
        else if (boiaA == 1 && boiaB == 0 && boiaC == 1)
        {
            Console.WriteLine("Ligar válvula e desligar bomba");
        }
        else if (boiaA == 1 && boiaB == 1 && boiaC == 0)
        {
            Console.WriteLine("Desligar válvula e ligar bomba");
        }
        else if (boiaA == 1 && boiaB == 1 && boiaC == 1)
        {
            Console.WriteLine("Desligar válvula e desligar bomba");
        }
    }
}

