using OxyPlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace WpfApplication1
{
    public class methods
    {

        public string log;
        private void add_text_to_log(string text, int endl)
        {
            log += text;
            log += " ";
            for (int i = 0; i < endl; i++)
                log += "\n";
        }
        public IList<DataPoint> tab(IList<DataPoint> Points, List<double> xi, List<double> yi)
        {
            Points = new List<DataPoint>();
            int n = xi.Count;
            for (int i = 0; i < n; i++)
                Points.Add(new DataPoint(xi[i], yi[i]));

            return Points;
        }
        private double Get_Lagrange(double xi, List<double> x, List<double> y, double n)
        {
            double Val = 0;
            for (int i = 0; i < n; i++)
            {
                double P = 1;
                double znam = 1;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        P *= (xi - x[j]) / (x[i] - x[j]);
                        znam *= (x[i] - x[j]);
                    }
                    else continue;
                }
                Val += P * y[i];
            }
            return Val;
        }
        public IList<DataPoint> Lagrange(IList<DataPoint> Points, List<double> x, List<double> y, double x0, double xn, bool is_der)
        {
            int n = x.Count;
            double xi;
            Points = new List<DataPoint>();
            for (xi = x0; xi < xn; xi += 0.05) //находим график многочлена
            {
                Points.Add(new DataPoint(xi, Get_Lagrange(xi, x, y, n)));
            }
            if (is_der)
                for (int i = 1; i < n - 1; i++)  //находим производные для многочлена
                {
                    log += Get_der_1(x[i]).ToString("F4");
                    add_text_to_log("\t   ",0);
                    add_text_to_log(Get_der_2(x[i]).ToString("F4"), 1);
                }

            double Get_der_1(double xi)
            {
                double der = (Get_Lagrange(xi + 0.0000001, x, y, n) - Get_Lagrange(xi - 0.0000001, x, y, n)) / 0.0000002;
                return der;
            }
            double Get_der_2(double xi)
            {
                double der = (Get_der_1(xi + 0.0000001) - Get_der_1(xi - 0.0000001)) / 0.0000002;
                return der;
            }

            return Points;

        }
        private double Get_Newton(double x, List<double> MasX, List<double> MasY, int n)
        {
            double step = MasX[1] - MasX[0];
            List<List<double>> dy = new List<List<double>>(n);
            dy.Add(MasY);

            // подсчитываем dy
            for (int i = 1; i < n; i++)
            {
                List<double> row = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                dy.Add(row);
                for (int j = 0; j < n - i; j++)
                {
                    dy[i][j] = dy[i - 1][j + 1] - dy[i - 1][j];
                }
            }

            // вычисляем результирующий y
            double q = (x - MasX[0]) / step; // см. формулу
            double result = MasY[0]; // результат (y) 

            double mult_q = 1; // произведение из q*(q-1)*(q-2)*(q-n)
            double fact = 1;  // факториал

            for (int i = 1; i < n; i++)
            {
                fact *= i;
                mult_q *= (q - i + 1);

                result += mult_q / fact * dy[i][0];
            }

            return result;
        }
        public IList<DataPoint> Newton(IList<DataPoint> Points, List<double> x, List<double> y, double x0, double xn)
        {
            int n = x.Count;
            double xi;
            Points = new List<DataPoint>();
            for (xi = x0; xi < xn; xi += 0.05) //находим график многочлена
            {
                Points.Add(new DataPoint(xi, Get_Newton(xi, x, y, n)));
            }
            for (int i = 1; i < n - 1; i++)  //находим производные для многочлена
            {
                double der = (Get_Newton(x[i] + 0.0000001, x, y, n) - Get_Newton(x[i] - 0.0000001, x, y, n)) / 0.0000002;
                add_text_to_log(der.ToString("F4"), 1);
            }
            return Points;
        }

        public double Newton_Cotes_3_My(List<double> x, List<double> y)
        {
            int n = 4;
            int count = x.Count;
            double h = (x[1] - x[0]) / 3;
            double S = 0, xi;
            List<double> c = new List<double>() { 1, 3, 3, 1 };

            for (int i = 0; i < count - 1; i++)
            {
                for (int k = 0; k < n; k++)
                {
                    xi = x[i] + h * k;
                    S += c[k] * Get_Lagrange(xi, x, y, count);
                }
            }
            S *= (x[1] - x[0]) / 8;
            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 2);
            return S;

        }
        public double Newton_Cotes_6_My(List<double> x, List<double> y)
        {
            int n = 7;
            int count = x.Count;
            double h = (x[1] - x[0]) / 6;
            double S = 0, xi;
            List<double> c = new List<double>() { 41, 216, 27, 272, 27, 216, 41 };

            for (int i = 0; i < count - 1; i++)
            {

                for (int k = 0; k < n; k++)
                {
                    xi = x[i] + h * k;
                    S += c[k] * Get_Lagrange(xi, x, y, count);
                }
            }
            S *= (x[1] - x[0]) / 840;
            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }
        public double Newton_Cotes_3(List<double> x, List<double> y)
        {
            int n = 4;
            int count = x.Count;
            double h = (x[1] - x[0]) / 3;
            double S = 0, xi;
            List<double> c = new List<double>() { 1, 3, 3, 1 };

            for (int i = 0; i < count - 1; i++)
            {

                for (int k = 0; k < n; k++)
                {
                    xi = x[i] + h * k;
                    S += c[k] * Main.Y3(xi);
                }
            }
            S *= (x[1] - x[0]) / 8;
            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }
        public double Newton_Cotes_6(List<double> x, List<double> y)
        {
            int n = 7;
            int count = x.Count;
            double h = (x[1] - x[0]) / 6;
            double S = 0, xi;
            List<double> c = new List<double>() { 41, 216, 27, 272, 27, 216, 41 };

            for (int i = 0; i < count - 1; i++)
            {

                for (int k = 0; k < n; k++)
                {
                    xi = x[i] + h * k;
                    S += c[k] * Main.Y3(xi);
                }
            }
            S *= (x[1] - x[0]) / 840;
            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }
        public double Left_square(double a, double b, double step)
        {
            double S = 0;

            for (double i = a; i < b; i += step)
            {
                S += Main.Y3(i) * step;
            }

            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }
        public double Right_square(double a, double b, double step)
        {
            double S = 0;
            
            for (double i = a + step; i < b + step / 2; i += step)
            {
                S += Main.Y3(i) * step;
            }

            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }

        public double Center_square(double a, double b, double step)
        {
            double S = 0;

            for (double i = a + step / 2; i < b; i += step)
            {
                S += Main.Y3(i) * step;
            }

            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }
        public double Trapecia(double a, double b, double step)
        {
            double S = 0;

            for (double i = a; i < b; i += step)
            {
                S += (Main.Y3(i) + Main.Y3(i+ step)) / 2.0 * step;
            }

            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 1);
            return S;
        }
        public double Simpson(List<double> x, List<double> y)
        {
            int n = 3;
            int count = x.Count;
            double h = (x[1] - x[0]) / 2;
            double S = 0, xi;
            List<double> c = new List<double>() { 1, 4, 1 };

            for (int i = 0; i < count - 1; i++)
            {

                for (int k = 0; k < n; k++)
                {
                    xi = x[i] + h * k;
                    S += c[k] * Main.Y3(xi);
                }
            }
            S *= (x[1] - x[0]) / 6;
            add_text_to_log("I = ", 0);
            add_text_to_log(S.ToString("F5"), 4);
            return S;
        }














        public IList<DataPoint> Line_spline(IList<DataPoint> Points, List<double> x, List<double> y)
        {
            add_text_to_log("Линейный сплайн", 1);
            int n = x.Count;
            double xi;
            Points = new List<DataPoint>();
            for (int i = 0; i < n - 1; ++i) //проход по всем точкам
            {
                for (xi = x[i]; xi < x[i + 1]; xi += 0.05) //проход по отрезку между точкой и следующей
                {
                    Points.Add(new DataPoint(xi, y[i] + (y[i + 1] - y[i]) / (x[i + 1] - x[i]) * (xi - x[i])));
                }

                double coef = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
                add_text_to_log(coef.ToString("F3"), 1);
            }
            add_text_to_log("", 1);
            return Points;
        }

        public class spline
        {
            public double b, c, d, x, y;
        };
        List<spline> splines;
        public void Build_spline(List<double> x, List<double> y)
        {
            int n = x.Count;

            splines = new List<spline>(n);

            for (int i = 0; i < n; ++i)
            {
                splines.Add(new spline());
                splines[i].x = x[i];
                splines[i].y = y[i];
            }

            splines[0].c = 0;

            // Решение СЛАУ относительно коэффициентов сплайнов c[i] методом прогонки для трехдиагональных матриц
            // Вычисление прогоночных коэффициентов - прямой ход метода прогонки

            List<double> alpha = new List<double>(n);
            List<double> beta = new List<double>(n);
            for (int i = 1; i < n; i++)
            {
                alpha.Add(0);
                beta.Add(0);
            }
            double A = 0, B, C = 0, F = 0, h_i, h_i1, z;

            for (int i = 1; i < n - 1; i++)
            {
                h_i = x[i] - x[i - 1];
                h_i1 = x[i + 1] - x[i];
                A = h_i;
                C = 2 * (h_i + h_i1);
                B = h_i1;
                F = 6 * ((y[i + 1] - y[i]) / h_i1 - (y[i] - y[i - 1]) / h_i);
                z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }

            splines[n - 1].c = (F - A * beta[n - 2]) / (C + A * alpha[n - 2]);

            // Нахождение решения - обратный ход метода прогонки
            for (int i = n - 2; i > 0; i--)
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];


            // По известным коэффициентам c[i] находим значения b[i] и d[i]
            for (int i = n - 1; i > 0; i--)
            {
                h_i = x[i] - x[i - 1];
                splines[i].d = (splines[i].c - splines[i - 1].c) / h_i;
                splines[i].b = h_i * (2 * splines[i].c + splines[i - 1].c) / 6 + (y[i] - y[i - 1]) / h_i;
            }

        }
        public IList<DataPoint> Cube_spline(IList<DataPoint> Points, List<double> x, List<double> y)
        {
            add_text_to_log("Кубический сплайн", 1);
            Points = new List<DataPoint>();
            Build_spline(x, y);
            int n = x.Count;
            add_text_to_log("Коэффициенты b, c, d:", 1);
            for (int i = 1; i < n; i++)
            {
                add_text_to_log(splines[i].b.ToString("F3"), 0);
                add_text_to_log(splines[i].c.ToString("F3"), 0);
                add_text_to_log(splines[i].d.ToString("F3"), 1);
            }

            spline s = new spline();
            for (double xi = x[0]; xi <= x[n - 1]; xi += 0.05)
            {
                int i = 0, j = n - 1;
                while (i + 1 < j) //поиск сплана
                {
                    int k = i + (j - i) / 2;
                    if (xi <= splines[k].x)
                        j = k;
                    else
                        i = k;
                }

                s = splines[j];

                double dx = (xi - s.x);
                Points.Add(new DataPoint(xi, s.y + (s.b + (s.c / 2 + s.d * dx / 6) * dx) * dx)); //получаем точку
            }
            return Points;
        }
        public IList<DataPoint> Line_approx(IList<DataPoint> Points, List<double> x, List<double> y)
        {
            add_text_to_log("Линейная апроксимация", 1);
            int n = x.Count;
            double xi;
            Points = new List<DataPoint>();
            double sumx = 0, sumy = 0, sumx2 = 0, sumxy = 0, a, b;
            for (int i = 0; i < n; i++)
            {
                sumx += x[i];
                sumy += y[i];
                sumx2 += x[i] * x[i];
                sumxy += x[i] * y[i];
            }
            b = (n * sumxy - (sumx * sumy)) / (n * sumx2 - sumx * sumx);
            a = (sumy - b * sumx) / n;

            add_text_to_log("Коэффициенты a, b:", 1);
            add_text_to_log(a.ToString("F3"), 0);
            add_text_to_log(b.ToString("F3"), 1);

            for (xi = x[0]; xi < x[n - 1]; xi += 0.05)
            {
                Points.Add(new DataPoint(xi, a + b * xi));
            }
            double summa = 0, oshibka, delta;
            for (int i = 0; i < n; i++)
            {
                delta = y[i] - a - b * x[i];
                summa += Math.Pow(delta, 2);
            }
            oshibka = (summa / (n + 1));
            add_text_to_log("Ошибка:", 1);
            add_text_to_log(oshibka.ToString("F3"), 1);
            add_text_to_log("", 1);

            return Points;
        }
        public IList<DataPoint> Exp_approx(IList<DataPoint> Points, List<double> x, List<double> y)
        {
            add_text_to_log("Апроксимация функцией e^(a+bx)", 1);
            int n = x.Count;
            double xi;
            Points = new List<DataPoint>();
            double sumx = 0, sumy = 0, sumx2 = 0, sumxy = 0, a, b;
            for (int i = 0; i < n; i++)
            {
                if (y[i] > 0) //патамушта аппроксимирующая функция положительная
                {
                    sumx += x[i];
                    sumy += Math.Log(y[i]);
                    sumx2 += x[i] * x[i];
                    sumxy += x[i] * Math.Log(y[i]);
                }
            }
            b = (n * sumxy - (sumx * sumy)) / (n * sumx2 - sumx * sumx);
            a = (sumy - b * sumx) / n;

            add_text_to_log("Коэффициенты a, b:", 1);
            add_text_to_log(a.ToString("F3"), 0);
            add_text_to_log(b.ToString("F3"), 1);

            for (xi = x[0]; xi < x[n - 1]; xi += 0.05)
            {
                Points.Add(new DataPoint(xi, Math.Exp(a + b * xi)));
            }

            double summa = 0, oshibka, delta;
            for (int i = 0; i < n; i++)
            {
                delta = y[i] - Math.Exp(a + b * x[i]);
                summa += Math.Pow(delta, 2);
            }
            oshibka = (summa / (n + 1));
            add_text_to_log("Ошибка:", 1);
            add_text_to_log(oshibka.ToString("F3"), 1);
            add_text_to_log("", 1);

            return Points;
        }
    }
}
