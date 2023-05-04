
namespace WpfApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using OxyPlot;
    using OxyPlot.Series;

    public class Main
    {
        int exercise = 3;

        methods metod = new methods();

        List<double> Xi = new List<double>(); //точки
        List<double> Yi = new List<double>();

        List<double> Xiline = new List<double>(); //график
        List<double> Yiline = new List<double>();

        List<List<double>> tochki = new List<List<double>>(m); //точки внутри отрезков
        List<List<double>> znachenia = new List<List<double>>(m);

        readonly static double a = 17, b = 4, k = 1;
        readonly static int c = 4, d = 20, m = 16, n = 4;

        public string some_pice_of_shit;
        public IList<DataPoint> Points1 { get; private set; }        public IList<DataPoint> Points2 { get; private set; }
        public IList<DataPoint> Points3 { get; private set; }        public IList<DataPoint> Points4 { get; private set; }
        public IList<DataPoint> Points5 { get; private set; }        public IList<DataPoint> Points6 { get; private set; }
        public IList<DataPoint> Points7 { get; private set; }        public IList<DataPoint> Points8 { get; private set; }

        public static double Y3(double x)
        {
            return Math.Pow(Math.Log(x), a / b) * Math.Sin(k * x);
        }

        void ex1()
        {
            for (int i = 0; i <= m; i++) //добавляем точки
            {
                Xi.Add(c + (d - c) * (double)i / m);
                Yi.Add(Y3(Xi[i]));
            }

            for (double i = 0; i <= m; i += 0.05) //добавляем график
            {
                double x = c + (d - c) * i / m;
                Xiline.Add(x);
                Yiline.Add(Y3(x));
            }

            metod.log += "Производные исходной функции:\n";

            for (int i = 0; i < m; i++)  //бьём отрезки между точками на m отрезков
            {
                tochki.Add(new List<double>());
                znachenia.Add(new List<double>());
                for (double j = 0; j <= n; j++)
                {
                    double x = Xi[i] + (Xi[i + 1] - Xi[i]) * j / n;
                    tochki[i].Add(x);
                    znachenia[i].Add(Y3(x));
                    if (j != 0 && j < n) //если это не крайние точки, то ищем их производные
                    {
                        metod.log += Get_der_1(x).ToString("F4");
                        metod.log += "\t   ";
                        metod.log += Get_der_2(x).ToString("F4");
                        metod.log += "\n";
                    }
                }
            }

            double Get_der_1(double x) //находим производные для функции
            {
                double der = (Y3(x + 0.0000001) - Y3(x - 0.0000001)) / 0.0000002;
                return der;
            }
            double Get_der_2(double x)
            {
                double der = (Get_der_1(x + 0.0000001) - Get_der_1(x - 0.0000001)) / 0.0000002;
                return der;
            }

            metod.log += "\nПроизводные многочлена:\n";

            Points1 = metod.tab(Points1, Xi, Yi);
            Points2 = metod.tab(Points2, Xiline, Yiline);
            Points3 = metod.Lagrange(Points3, tochki[0], znachenia[0], Xi[0], Xi[1], true);
            Points4 = metod.Lagrange(Points4, tochki[1], znachenia[1], Xi[1], Xi[2], true);
            Points5 = metod.Lagrange(Points5, tochki[2], znachenia[2], Xi[2], Xi[3], true);
            Points6 = metod.Lagrange(Points6, tochki[3], znachenia[3], Xi[3], Xi[4], true);
            Points7 = metod.Lagrange(Points7, tochki[4], znachenia[4], Xi[4], Xi[5], true);
            Points8 = metod.Lagrange(Points8, tochki[5], znachenia[5], Xi[5], Xi[6], true);
        }

        void ex2()
        {
            for (int i = 0; i <= m; i++) //добавляем точки
            {
                Xi.Add(c + (d - c) * (double)i / m);
                Yi.Add(Y3(Xi[i]));
            }

            for (double i = 0; i <= m; i += 0.05) //добавляем график
            {
                double x = c + (d - c) * i / m;
                Xiline.Add(x);
                Yiline.Add(Y3(x));
            }

            metod.log += "Производные исходной функции:\n";

            for (int i = 0; i < m; i++)  //бьём отрезки между точками на n отрезков
            {
                tochki.Add(new List<double>());
                znachenia.Add(new List<double>());
                for (double j = 0; j <= n; j++)
                {
                    double x = Xi[i] + (Xi[i + 1] - Xi[i]) * j / n;
                    tochki[i].Add(x);
                    znachenia[i].Add(Y3(x));
                    if (j != 0 && j < n) //если это не крайние точки, то ищем их производные
                        Push_der(x);
                }
            }

            void Push_der(double x) //находим производные для функции
            {
                double der = (Y3(x + 0.0000001) - Y3(x - 0.0000001)) / 0.0000002;
                metod.log += der.ToString("F4");
                metod.log += "\n";
            }

            metod.log += "\nПроизводные многочлена:\n";

            Points1 = metod.tab(Points1, Xi, Yi);
            Points2 = metod.tab(Points2, Xiline, Yiline);
            Points3 = metod.Newton(Points3, tochki[0], znachenia[0], Xi[0], Xi[1]);
            Points4 = metod.Newton(Points4, tochki[1], znachenia[1], Xi[1], Xi[2]);
            Points5 = metod.Newton(Points5, tochki[2], znachenia[2], Xi[2], Xi[3]);
            Points6 = metod.Newton(Points6, tochki[3], znachenia[3], Xi[3], Xi[4]);
            Points7 = metod.Newton(Points7, tochki[4], znachenia[4], Xi[4], Xi[5]);
            Points8 = metod.Newton(Points8, tochki[5], znachenia[5], Xi[5], Xi[6]);
        }

        void ex3()
        {
            for (int i = 0; i <= m; i++) //добавляем точки
            {
                Xi.Add(c + (d - c) * (double)i / m);
                Yi.Add(Y3(Xi[i]));
            }

            for (double i = 0; i <= m; i += 0.05) //добавляем график
            {
                double x = c + (d - c) * i / m;
                Xiline.Add(x);
                Yiline.Add(Y3(x));
            }

            for (int i = 0; i < m; i++)  //бьём отрезки между точками на n отрезков
            {
                tochki.Add(new List<double>());
                znachenia.Add(new List<double>());
                for (double j = 0; j <= n; j++)
                {
                    double x = Xi[i] + (Xi[i + 1] - Xi[i]) * j / n;
                    tochki[i].Add(x);
                    znachenia[i].Add(Y3(x));
                }
            }

            double step = 0.000001;

            metod.log += "Значение интеграла методом Ньютона-Котеса n = 3:\n";
            metod.log += "Число точек: "; metod.log += (m * 3).ToString(); metod.log += "\n";
            metod.Newton_Cotes_3(Xi, Yi);
            metod.log += "\nЗначение интеграла методом Ньютона-Котеса n = 6:\n";
            metod.log += "Число точек: "; metod.log += (m * 6).ToString(); metod.log += "\n";
            metod.Newton_Cotes_6(Xi, Yi);
            metod.log += "\nЗначение интеграла методом левых прямоугольников:\n";
            metod.log += "Число точек: "; metod.log += Math.Round((d - c)/ step).ToString(); metod.log += "\n";
            metod.Left_square(c, d, step);
            metod.log += "\nЗначение интеграла методом правых прямоугольников:\n";
            metod.log += "Число точек: "; metod.log += Math.Round((d - c) / step).ToString(); metod.log += "\n";
            metod.Right_square(c, d, step);
            metod.log += "\nЗначение интеграла методом центральных прямоугольников:\n";
            metod.log += "Число точек: "; metod.log += Math.Round((d - c) / step).ToString(); metod.log += "\n";
            metod.Center_square(c, d, step);
            metod.log += "\nЗначение интеграла методом трапеций:\n";
            metod.log += "Число точек: "; metod.log += Math.Round((d - c) / step).ToString(); metod.log += "\n";
            metod.Trapecia(c, d, step);
            metod.log += "\nЗначение интеграла методом Симпсона:\n";
            metod.log += "Число точек: "; metod.log += ((m + 1) * 2).ToString(); metod.log += "\n";
            metod.Simpson(Xi, Yi);

            List<double> Xi2 = new List<double>(); //еще точки
            List<double> Yi2 = new List<double>();

            int count = 24;
            for (int i = 0; i <= count; i++) //добавляем
            {
                Xi2.Add(c + (d - c) * (double)i / count);
                Yi2.Add(Y3(Xi2[i]));
            }

            metod.log += "Значение интеграла методом Ньютона-Котеса n = 3 в моем переосмыслении:\n";
            metod.log += "Число точек: "; metod.log += (count + 1).ToString(); metod.log += "\n";
            metod.Newton_Cotes_3_My(Xi2, Yi2);
            metod.log += "Значение интеграла методом Ньютона-Котеса n = 6 в моем переосмыслении:\n";
            metod.log += "Число точек: "; metod.log += (count + 1).ToString(); metod.log += "\n";
            metod.Newton_Cotes_6_My(Xi2, Yi2);

            Points1 = metod.tab(Points1, Xi, Yi);
            Points2 = metod.tab(Points2, Xiline, Yiline);
            Points3 = metod.Lagrange(Points3, tochki[0], znachenia[0], Xi[0], Xi[1], false);
            Points4 = metod.Lagrange(Points4, tochki[1], znachenia[1], Xi[1], Xi[2], false);
            Points5 = metod.Lagrange(Points5, tochki[2], znachenia[2], Xi[2], Xi[3], false);
            Points6 = metod.Lagrange(Points6, tochki[3], znachenia[3], Xi[3], Xi[4], false);
            Points7 = metod.Lagrange(Points7, tochki[4], znachenia[4], Xi[4], Xi[5], false);
            Points8 = metod.Lagrange(Points8, tochki[5], znachenia[5], Xi[5], Xi[6], false);

        }
        
       

        public Main()
        {
            if (exercise == 1)
                ex1();

            if (exercise == 2)
                ex2();

            if (exercise == 3)
                ex3();

            /*
            if (exercise == 4)
                ex4();

            if (exercise == 5)
                ex5();

            if (exercise == 6)
                ex6();
            */
            some_pice_of_shit += metod.log;
        }

    }

}
