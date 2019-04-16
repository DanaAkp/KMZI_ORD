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
using KMZI_lib_int;

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
            int[] vs = new int[]{ mod-1,1};
            if (vs.Length >= K.Length)
            {
                int[] vc = ModPolinom(vs, K, mod);
                if (K.Length == 2 && vc[0] == 0 && vc[1] == 0) return 1;
            }
            int d = (int)Math.Pow(mod, K.Length - 1) - 1;
            List<int> forNOK = new List<int>();
            Dictionary<int, int> f = KMZI_int.Factorization(d);
            if (f.Count == 1 && f.ElementAt(0).Value == 1)
            {
                return d;
            }
            foreach(KeyValuePair<int,int> x in f)
            {
                int[] k1 = new int[d / x.Key + 1];
                k1[k1.Length - 1] = 1;
                if (K.Length > k1.Length)
                    forNOK.Add((int)Math.Pow(x.Key, x.Value));
                else
                {
                    int[] res = ModPolinom(k1, K, mod);int counter = 0;
                    for (int i = 1; i < res.Length; i++) if (res[i] != 0) counter++; 
                    
                    if (counter!=0 || res[0]!=1)
                    {
                        forNOK.Add((int)Math.Pow(x.Key, x.Value));
                    }
                    else
                        for (int i = 2; i <= x.Value; i++)
                        {
                            k1 = new int[d / (int)Math.Pow(x.Key, i) + 1];
                            k1[k1.Length - 1] = 1;
                            if (K.Length > k1.Length)
                                forNOK.Add((int)Math.Pow(x.Key, x.Value));
                            else
                            {
                                res = ModPolinom(k1, K, mod); counter = 0;
                                for (int j = 1; j < res.Length; j++) if (res[j] != 0) counter++;

                                if (counter != 0 || res[0] != 1)
                                {
                                    forNOK.Add((int)Math.Pow(x.Key, x.Value - 1));
                                }
                            }
                        }
                }
                

            }
            int e = d;
            if (forNOK.Count == 1) e = forNOK[0];
            if (forNOK.Count > 1)
            {
                e = KMZI_int.NOK(forNOK[0], forNOK[1]);
                for (int i = 2; i < forNOK.Count; i++) e = KMZI_int.NOK(e, forNOK[i]);
            }
            return e;
        }
        
        static private int[] GetVector(string[] s)
        {
            int[] ls = new int[s.Length];
            for(int i = s.Length - 1; i >= 0; i--)
            {
                ls[i] = int.Parse(s[i]);
            }
            Array.Reverse(ls, 0, ls.Length);
            return ls;
        }
        private int[] ModPolinom(int[] K1, int[] K2, int mod)
        {
            if (K2.Length > K1.Length) throw new Exception("Степень делимого многочлена должна быть выше делителя!");
            int[] result = new int[(K1.Length - 1) - (K2.Length - 1) + 1];
            for (int i = K1.Length - 1; i >= 0; i--)
            {
                int[] buf = new int[K1.Length];
                if (i >= K2.Length - 1)
                {
                    result[i - (K2.Length - 1)] = K1[i];

                    for (int k = 0; k < K2.Length; k++)
                    {
                        buf[i - (K2.Length - 1) + k] = result[i - (K2.Length - 1)] * K2[k];
                    }
                    for (int j = K1.Length - 1; j >= 0; j--)
                    {
                        buf[j] = K1[j] - buf[j];
                        buf[j] = KMZI_int.numOnMod(buf[j], mod);
                    }
                    K1 = buf;
                }
            }
            Array.Reverse(result);
            return K1;
        }
    }
}
