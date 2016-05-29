using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinPolynomial
{
    class Program
    {
        static Polynomial a = new Polynomial(0, 1);

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Polynomial.p = 2;
            Polynomial.m = 4;
            Polynomial.px = new Polynomial(1,1,0,0,1);
            Polynomial B = a.Pow(6);
            B = B.Prim();
            var list = FindR(B);
            /*Polynomial res = new Polynomial(1);
            foreach (var pol in list)
            {
                res *= (new Polynomial(0, 1) - pol);
                //res = res.Prim();
            }*/
            var koeffs = new List<Polynomial>();
            for (int i = 0; i<=list.Count; i++)
            {
                koeffs.Add(CNK(list,i));
            }
            koeffs.Reverse();
            var res = new Polynomial(koeffs.Select(x=>x[0]).ToArray());
            Console.WriteLine("Минимальный полином для элемента В="+B);
            Console.WriteLine(res);
            Console.ReadLine();
        }

        static List<Polynomial> FindR(Polynomial B)
        {
            int r = 0;
            Polynomial c = B;
            var list = new List<Polynomial>();
            do
            {
                list.Add(c);
                r++;
                c = B.Pow((uint)Math.Pow(Polynomial.p, r)).Prim();
            } while (!c.Equals(B));
            //list[2] = new Polynomial(1, 1, 0, 1);
            return list;
        }

        static Polynomial CNK(List<Polynomial> arr, int k)
        {
            if(k==0) return new Polynomial(1);
            hs = new HashSet<int>();
            res = new Polynomial(0);
            var l = Do(arr, new List<Polynomial>(), k);
            foreach (var item in l)
            {
                res += item;
            }
            return res;
        }

        static HashSet<int> hs = new HashSet<int>();
        static Polynomial res = new Polynomial(0);
        static List<Polynomial> Do(List<Polynomial> source, List<Polynomial> arr, int k)
        {
            if (k <= 0)
            {
                int pp = 0;
                var p = new Polynomial(1);
                foreach (var item in arr)
                {
                    p *= item;
                    pp ^= item.GetHashCode();
                }
                p = p.Prim();
                if(hs.Contains(pp)) return new List<Polynomial>();
                hs.Add(pp);
                return new List<Polynomial>() {p};
            }
            var list = new List<Polynomial>();
            for (int i = 0; i < source.Count; i++)
            {
                var ar = new List<Polynomial>(source);
                ar.RemoveAt(i);
                var l = new List<Polynomial>(arr);
                l.Add(source[i]);

                list.AddRange(Do(ar,l, k - 1));
            }
            return list;
        } 
    }
}
