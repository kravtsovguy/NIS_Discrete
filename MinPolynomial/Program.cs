using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinPolynomial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Polynomial.p = 2;
            Polynomial.m = 4;
            Polynomial.px = new Polynomial(1,1,0,0,1);
            Polynomial a = new Polynomial(0,1);
            Polynomial B = a.Pow(6);
            B = B.Prim();
            /*int x = 48%15;
            Console.WriteLine("10²");
            string a = "Adding substring" + "\xB3";
            Console.Write(a);
            Console.ReadLine();*/
            Console.WriteLine(B);
            Console.ReadLine();
        }
    }
}
