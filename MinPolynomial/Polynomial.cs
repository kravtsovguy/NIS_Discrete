using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MinPolynomial
{
    class Polynomial
    {
        public static int p;
        public static int m;

        /// <summary>
        /// Примитивный полином
        /// </summary>
        public static Polynomial px;

        /// <summary>
        /// Коэффициенты полинома по возрастанию степеней
        /// </summary>
        public readonly int[] koeffs;

        /// <summary>
        /// Количество коэффициентов
        /// </summary>
        public int N => koeffs.Length;

        /// <summary>
        /// Доступ к определенному коэффициенту
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int this[int x]
        {
            get
            {
                return koeffs[x];
            }

            set
            {
                koeffs[x] = value;
            }
        }

        /// <summary>
        /// Старший коэфф.
        /// </summary>
        public int Last => koeffs.Last();

        public Polynomial(params int[] koeffs)
        {
            this.koeffs = koeffs;
        }

        /// <summary>
        /// Создание полинома только со старшим коэфф k*x^m
        /// </summary>
        /// <param name="k">Коэфф</param>
        /// <param name="m">Степень</param>
        /// <returns></returns>
        public static Polynomial Create(int k, int m)
        {
            var arr = new int[m + 1];
            arr[m] = k;
            return new Polynomial(arr);
        }


        /// <summary>
        /// Сложение полиномов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            var m = Math.Max(a.N, b.N);
            var c = new Polynomial(new int[m]);
            for (int i = 0; i < m; i++)
            {
                if (i < a.N) c[i] += a[i];
                if (i < b.N) c[i] += b[i];
            }
            return c.Trim();
        }

        /// <summary>
        /// Умножение полиномов
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            var m = a.N + b.N - 1;
            var c = new Polynomial(new int[m]);
            for (int i = 0; i < a.N; i++)
            {
                for (int j = 0; j < b.N; j++)
                {
                    c[i + j] += a[i]*b[j];
                }
            }
            return c.Trim();
        }


        /// <summary>
        /// Приводим полином к нужному виду
        /// </summary>
        /// <returns></returns>
        public Polynomial Trim()
        {
            var a = this;
            var c = new Polynomial(a.koeffs);
            int t = 0;
            for (int i = 0; i < c.N; i++)
            {
                c[i] %= p;
                if (c[i] < 0) c[i] +=p;
                if (c[i] == 0) t++;
                else t = 0;
            }
            c = new Polynomial(c.koeffs.Take(c.N - t).ToArray());
            return c;
        }

        /// <summary>
        /// Умножение полинома на число
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial operator *(int a, Polynomial b)
        {
            var c = new Polynomial(a)*b;
            return c;
        }

        /// <summary>
        /// Вычитание одного полинома из другого
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            var c = a + (-1)*b;
            return c;
        }

        /// <summary>
        /// Деление одного полинома на другой
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            var t = a.N - b.N;
            if(t<0) return new Polynomial(0);
            var c = Polynomial.Create(a.Last/b.Last, t);
            var g = a - b*c;
            return c + g/b;
        }


        /// <summary>
        /// Остаток от деления одного полинома на другой
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Polynomial operator %(Polynomial a, Polynomial b)
        {
            var c = a / b;
            return a - b * c;
        }

        /// <summary>
        /// Остаток на деления полинома на примитивный полином
        /// </summary>
        /// <returns></returns>
        public Polynomial Prim()
        {
            return this%px;
        }


        /// <summary>
        /// Обратный полином
        /// </summary>
        /// <returns></returns>
        public Polynomial Obr()
        {
            var a = this;
            var c = new Polynomial(a.koeffs);
            var list = new List<Polynomial>();
            list.Add(c);
            int t = 0;
            do
            {
                c *= a;
                t++;
                list.Add(c);
            } while (!c.Equals(new Polynomial(1)));
            return list[list.Count-2];
        }

        /// <summary>
        /// Возведение полинома в степень
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Polynomial Pow(uint k)
        {
            k %= (uint)Math.Pow(p, m) - 1;
            var a = this;
            var c = new Polynomial(1);
            for (int i = 0; i < k; i++)
            {
                c *= a;
            }
            return c;
        }

        /// <summary>
        /// Сравнение полиномов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var p = obj as Polynomial;
            if ((Object)p == null)
            {
                return false;
            }

            return koeffs.SequenceEqual(p.koeffs);
        }

        public override int GetHashCode()
        {
            int result = 0;
            int shift = 0;
            for (int i = 0; i < koeffs.Length; i++)
            {
                shift = (shift + 11) % 21;
                result ^= (koeffs[i] + 1024) << shift;
            }
            return result;
        }

        /// <summary>
        /// Вывод полинома в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < N; i++)
            {
                var a = i == 0 ? this[i].ToString() : $"{this[i].ToString()}x^{i}+";
                s = a + s;
            }
            return s;
        }
    }
}
