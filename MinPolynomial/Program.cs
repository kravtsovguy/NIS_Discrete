﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinPolynomial
{
    class Program
    {
        static uint deg_b = 1;
        static void ReadData()
        {
            var s = File.ReadAllLines("data.txt");
            for (int i = 0; i < s.Length; i++)
            {
                var t = s[i].Split('=');
                if (t[0] == "p")
                {
                    Polynomial.p = int.Parse(t[1]);
                }
                if (t[0] == "n")
                {

                }
                if (t[0] == "m")
                {
                    Polynomial.m = int.Parse(t[1]);
                }
                if (t[0] == "px")
                {
                    Polynomial.px = new Polynomial(t[1].Split(' ').Reverse().Select(x=>int.Parse(x)).ToArray());
                }
                if (t[0] == "deg")
                {
                    deg_b = uint.Parse(t[1]);
                }
            }
        }

        static void Main(string[] args)
        {


            Console.OutputEncoding = System.Text.Encoding.Unicode;

            //Считывание данных
            ReadData();

            //Простое число
            //Polynomial.p = 2;

            //Максимальная степень мнохочлена
            //Polynomial.m = 4;

            //Примитивный полином p(x)
            //Polynomial.px = new Polynomial(1,1,0,0,1);

            //Генератор
            Polynomial a = new Polynomial(0, 1);

            //Элемент Бета, для которого будем находить минимальный полином
            
            Polynomial B = a.Pow(deg_b).Prim();

            //Массив степеней элемента B до равенства B^(p^r)=B (вычисление r)
            var list = FindR(B);

            //Коэффициенты у минимального полинома
            var koeffs = new List<Polynomial>();
            for (int i = 0; i<=list.Count; i++)
            {
                koeffs.Add(CNK(list,i));
            }

            //Минимальный полином
            koeffs.Reverse();
            var res = new Polynomial(koeffs.Select(x=>x[0]).ToArray());

            //Вывод минимального полинома
            Console.WriteLine("p={0}, m={1}, a=x, B=a^{2}",Polynomial.p,Polynomial.m,deg_b);
            Console.WriteLine("p(x)={0}",Polynomial.px);
            Console.WriteLine();
            Console.WriteLine("Минимальный полином для элемента В={0}:",B);
            Console.WriteLine(res);
            Console.ReadLine();
        }

        /// <summary>
        /// Нахождение r, то есть такое, что B^(p^r)=B
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
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
            return list;
        }

        /// <summary>
        /// C(M,k) - сумма всех произведений элементов всех сочетаний из M по k элементов в каждом сочетании.
        /// </summary>
        /// <param name="arr">Множество</param>
        /// <param name="k">Количество элементов</param>
        /// <returns></returns>
        static Polynomial CNK(List<Polynomial> arr, int k)
        {
            if(k==0) return new Polynomial(1);
            hs = new HashSet<int>();
            var res = new Polynomial(0);
            var l = Do(arr, new List<Polynomial>(), k);
            foreach (var item in l)
            {
                res += item;
            }
            return res;
        }

        static HashSet<int> hs = new HashSet<int>();
        static List<Polynomial> pol = new List<Polynomial>();
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
