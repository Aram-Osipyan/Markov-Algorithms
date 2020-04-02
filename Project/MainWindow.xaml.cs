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
using System.Text.RegularExpressions;
namespace TA
{
    public static class StringExtension
    {
        public static string Replace(this string self,
                                          string oldString, string newString,
                                          bool firstOccurrenceOnly = false)
        {
            if (!firstOccurrenceOnly)
                return self.Replace(oldString, newString);

            int pos = self.IndexOf(oldString);
            if (pos < 0)
                return self;

            return self.Substring(0, pos) + newString
                   + self.Substring(pos + oldString.Length);
        }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int MAX_OP = 1000;
        public MainWindow()
        {
            InitializeComponent();
        }
        

        //функция выполняющая при нажатии на Compile
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            outbox.Text = " ";
            string algorithm = algbox.Text;//введенный алгоритм
            string word = inptbox.Text;//введенное слово

            //регулярное выражение котороое распазнает алгоритм 
            // в Groups[1] - то что нужно заменить
            // в Groups[3] - то на что заменить
            // в Groups[2] - подозрение на точку(последняя замена)
            Regex reg = new Regex(@"(\w+)\s*=>\s*(\.?)\s*(\w+)");

            outbox.Text = word;

            while (i<MAX_OP)// если i превысит MAX_OP, то выводит сообщение 
            {
                i++;
                var outc = false;//проверка на, то есть ли точка
                var nomatch = true;// проверка на то, что нет совпадений 
                foreach (Match item in reg.Matches(algorithm))// проходит по всем подстановкам
                {
                    word = "_" + word;// ставлю пробельный символ в начало строки 
                    if (word.Contains(item.Groups[1].Value))//есть ли пробельные символы
                    {
                        nomatch = false;// есть совпадение
                        outbox.Text += " => ";
                        outc = item.Groups[2].Value == ".";// последняя ли итерация                        
                        word = word.Replace(item.Groups[1].Value,  item.Groups[3].Value,true);// заменяет первое вхождение

                        word = Regex.Replace(word, "_", "");//удаляет все пробельные символы из строки
                        outbox.Text += word ;
                        break;
                    }
                    
                }

                if (outc || nomatch)
                    break;
            }
           
            if (i== MAX_OP)
            {
                outbox.Text = "видимо у вас что-то зациклилось ;(";
            }

        }
    }
}
