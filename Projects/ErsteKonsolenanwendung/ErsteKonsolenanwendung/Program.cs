using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErsteKonsolenanwendung
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;
            int alter;
            Console.WriteLine("Bitte Name eingeben: ");
            name = Console.ReadLine();
            Console.WriteLine("Bitte Alter eingeben");
            alter = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Ihr Name " + name + ", ihr Alter " + alter);
            Console.ReadKey();

        }
    }
}
