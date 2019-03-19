using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KMZI_lib;

namespace KMZI_ORD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbRes.Text = "";
                int mod = int.Parse(tbMod.Text.Trim());

                int[] Koeff = GetVector(tbKoeff.Text.Trim().Split());//старший коэфф стоит последний, то есть его индекс обозначает его степень
                tbRes.Text += "" + GetOrd_f(Koeff, mod);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private int GetOrd_f(int[] K,int mod)
        {
            int d = (int)Math.Pow(mod, K.Length - 1) - 1;
            List<int> forNOK = new List<int>();
            Dictionary<int, int> f = Factorization(d);
            foreach(KeyValuePair<int,int> x in f)
            {
                int[] k1 = new int[d / (int)Math.Pow(x.Key, 1) + 1];
                if (ModPolinom(k1, K, mod) != new int[1] { 1 })
                {
                    forNOK.Add((int)Math.Pow(x.Key, x.Value));
                    break;
                }
                else for (int i = 2; i < x.Value; i++)
                    {
                        k1 = new int[d / (int)Math.Pow(x.Key, i) + 1];
                        if (ModPolinom(k1, K, mod) != new int[1] { 1 })
                        {
                            forNOK.Add((int)Math.Pow(x.Key, x.Value - i));
                            break;
                        }
                    }

            }
            int e = d;
            if (forNOK.Count == 1) e = forNOK[0];
            if (forNOK.Count > 1)
            {
                e = NOK(forNOK[0], forNOK[1]);
                for (int i = 2; i < forNOK.Count; i++) e = NOK(e, forNOK[i]);
            }
            return e;
        }

        private Dictionary<int, int> Factorization(int num)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();

            for(int i = 2; i <= num; i++)
            {
                if (Class1.CheckForSimplicity(i) && num%i==0)
                {
                    int c = 0;
                    while (num % i == 0)  { num /= i; c++; }
                    dic.Add(i, c);
                }
            }

            return dic;
        }
        private int NOK(int m,int n)
        {
            return m * n / (int)Class1.NOD(m, n);
        }
        private int[] GetVector(string[] s)
        {
            int[] ls = new int[s.Length];
            for(int i = s.Length - 1; i >= 0; i--)
            {
                ls[i] = int.Parse(s[i]);
            }
            Array.Reverse(ls, 0, ls.Length);
            return ls;
        }
        private int[] DivisionPolinom(int[] K1,int[] K2, int mod)
        {
          //  int deg = K1.Length - 1;
            int[] result = new int[(K1.Length - 1) - (K2.Length - 1) + 1];
            if (K2.Length > K1.Length) throw new Exception("Степень делимого многочлена должна быть выше делителя!");
            for(int i = K1.Length - 1; i >= 0; i--)
            {
                int[] buf = new int[K1.Length];
                if (i >= K2.Length - 1)
                {
                    result[i - (K2.Length - 1)] = K1[i];//разобраться с индексом, тк в рес должно быть меньше i
                    
                    for (int k = 0; k < K2.Length; k++)
                    {
                        buf[i - (K2.Length - 1) + k] = result[i - (K2.Length - 1)] * K2[k];
                    }
                    for (int j = K1.Length - 1; j >= 0; j--)
                    {
                        buf[j] = K1[j] - buf[j];
                        buf[j] = numOnMod(buf[j], mod);
                    }
                    K1 = buf;//deg = i + K2.Length - 1;
                }
            }
            Array.Reverse(result);
            Array.Reverse(K1);
            //string s = "Коэффициенты многочлена: ";
            //for (int i = 0; i < result.Length; i++)
            //{
            //    s += result[i] + " ";
            //}
            //s += "\nКоэффициенты остатка: ";
            //for (int i = 0; i < K1.Length; i++)
            //    s += K1[i] + " ";
            return result;
        }
        private int[] ModPolinom(int[] K1, int[] K2, int mod)
        {
            //  int deg = K1.Length - 1;
            int[] result = new int[(K1.Length - 1) - (K2.Length - 1) + 1];
            if (K2.Length > K1.Length) throw new Exception("Степень делимого многочлена должна быть выше делителя!");
            for (int i = K1.Length - 1; i >= 0; i--)
            {
                int[] buf = new int[K1.Length];
                if (i >= K2.Length - 1)
                {
                    result[i - (K2.Length - 1)] = K1[i];//разобраться с индексом, тк в рес должно быть меньше i

                    for (int k = 0; k < K2.Length; k++)
                    {
                        buf[i - (K2.Length - 1) + k] = result[i - (K2.Length - 1)] * K2[k];
                    }
                    for (int j = K1.Length - 1; j >= 0; j--)
                    {
                        buf[j] = K1[j] - buf[j];
                        buf[j] = numOnMod(buf[j], mod);
                    }
                    K1 = buf;//deg = i + K2.Length - 1;
                }
            }
            Array.Reverse(result);
            Array.Reverse(K1);
            //string s = "Коэффициенты многочлена: ";
            //for (int i = 0; i < result.Length; i++)
            //{
            //    s += result[i] + " ";
            //}
            //s += "\nКоэффициенты остатка: ";
            //for (int i = 0; i < K1.Length; i++)
            //    s += K1[i] + " ";
            return K1;
        }
        private int numOnMod(int num,int mod)
        {
            while (num < 0) num += mod;
            while (num > mod) num -= mod;
            return num;
        }
    }
}
