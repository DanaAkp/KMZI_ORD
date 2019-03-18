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
                tblResult.Text = "";
                int p = int.Parse(tbMod.Text.Trim());

                int[] Koeff1 = GetVector(tbKoeff.Text.Trim().Split());//старший коэфф стоит последний, то есть его индекс обозначает его степень
                int[] Koeff2 = GetVector(tbKoeff2.Text.Trim().Split());

                tblResult.Text = DivisionPolinom(Koeff1, Koeff2, p);
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
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
        private string DivisionPolinom(int[] K1,int[] K2, int mod)
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
            string s = "Коэффициенты многочлена: ";
            Array.Reverse(result);
            Array.Reverse(K1);
            for (int i = 0; i < result.Length; i++)
            {
                s += result[i] + " ";
            }
            s += "\nКоэффициенты остатка: ";
            for (int i = 0; i < K1.Length; i++)
                s += K1[i] + " ";
            return s;
        }
        private int numOnMod(int num,int mod)
        {
            while (num < 0) num += mod;
            while (num > mod) num -= mod;
            return num;
        }
    }
}
