using System;
class Program
{
    static void Main(string[] args)
    {
        int opcion = 2;
        switch (opcion)
        {
            case 1:
                Console.WriteLine("Opción 1");
                break;
            case 2:
                Console.WriteLine("Opción 2");
                break;
            default:
                Console.WriteLine("Otra opción");
                break;
        }
        Console.ReadKey();
    }
} 
