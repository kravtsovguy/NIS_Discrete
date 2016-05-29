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
        public static int p = 2;
        public static int m = 4;
        public static Polynomial px;

        public readonly int[] koeffs;

        public int N => koeffs.Length;

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

        public int Last => koeffs.Last();

        public Polynomial(params int[] koeffs)
        {
            this.koeffs = koeffs;
        }

        public static Polynomial Create(int k, int m)
        {
            var arr = new int[m + 1];
            arr[m] = k;
            return new Polynomial(arr);
        }


        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            var m = Math.Max(a.N, b.N);
            var c = new Polynomial(new int[m]);
            for (int i = 0; i < m; i++)
            {
                if (i < a.N) c[i] += a[i];
                if (i < b.N) c[i] += b[i];
                //c[i] %= p;
            }
            return c.Trim();
        }
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            var m = a.N + b.N - 1;
            var c = new Polynomial(new int[m]);
            for (int i = 0; i < a.N; i++)
            {
                for (int j = 0; j < b.N; j++)
                {
                    c[i + j] += a[i]*b[j];
                    //c[i + j] %= p;
                }
            }
            return c.Trim();
        }

        public Polynomial Trim()
        {
            var a = this;
            var c = new Polynomial(a.koeffs);
            /*for (int i = m+1; i < c.N; i++)
            {
                    c[i%(m+1)] += c[i];
                    c[i] = 0;
            }*/
            int t = 0;
            for (int i = 0; i < c.N; i++)
            {
                c[i] %= p;
                if (c[i] < 0) c[i] +=p;
                if (c[i] == 0) t++;
                else t = 0;
            }
            c = new Polynomial(c.koeffs.Take(c.N - t).ToArray());
            //c %= px;
            return c;
        }

        public static Polynomial operator *(int a, Polynomial b)
        {
            var c = new Polynomial(a)*b;
            return c;
        }

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            var c = a + (-1)*b;
            return c;
        }


        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            var t = a.N - b.N;
            if(t<0) return new Polynomial(0);
            var c = Polynomial.Create(a.Last/b.Last, t);
            var g = a - b*c;
            return c + g/b;
            /*var c = b.Obr();
            var cc = new Polynomial(0,1).Pow(6);
            var g = c*cc;
            var gg = b*g;
            var r = cc - gg;
            return c;*/
        }

        public static Polynomial operator %(Polynomial a, Polynomial b)
        {
            var c = a / b;
            return a - b * c;
        }

        public Polynomial Prim()
        {
            return this%px;
        }

        public Polynomial Obr()
        {
            var a = this;
            var c = new Polynomial(a.koeffs);
            List<Polynomial> list = new List<Polynomial>();
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

        public Polynomial Pow(uint k)
        {
            var a = this;
            var c = new Polynomial(1);
            for (int i = 0; i < k; i++)
            {
                c *= a;
            }
            return c;
        }

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
            return koeffs.GetHashCode();
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < N; i++)
            {
                //if(this[i]==0) continue;
                var a = i == 0 ? this[i].ToString() : $"{this[i].ToString()}x^{i}+";
                s = a + s;
            }
            return s;
        }
    }
}
