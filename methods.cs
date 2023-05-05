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
                    add_text_to_log("\t   ", 0);
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
                S += (Main.Y3(i) + Main.Y3(i + step)) / 2.0 * step;
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








        string tochnost = "F10";
        public IList<DataPoint> Dif_Real(IList<DataPoint> Points, Func<double, double> dY_solved, double y0, double x0, double xn)
        {
            double xi, step = 0.01;
            Points = new List<DataPoint>();
            for (xi = x0; xi <= xn + step / 2; xi += step)
            {
                Points.Add(new DataPoint(xi, dY_solved(xi)));
            }

            return Points;
        }
        public IList<DataPoint> Dif_Euler(IList<DataPoint> Points, Func<double, double, double> dY, Func<double, double> dY_solved, double y0, double x0, double xn, double step)
        {
            double xi, yi = y0, delta = 0;
            Points = new List<DataPoint>();
            for (xi = x0; xi <= xn + step / 2; xi += step)
            {
                delta += Math.Abs(yi - dY_solved(xi));
                Points.Add(new DataPoint(xi, yi));
                yi += dY(xi, yi) * step;
            }
            delta /= (xn - x0) / step;
            add_text_to_log("Метод Эйлера\nСреднее отклонение: ", 0);
            add_text_to_log(delta.ToString(tochnost), 2);
            return Points;
        }
        public IList<DataPoint> Dif_Euler_Modify(IList<DataPoint> Points, Func<double, double, double> dY, Func<double, double> dY_solved, double y0, double x0, double xn, double step)
        {
            double xi, yi = y0, delta = 0;
            Points = new List<DataPoint>();
            for (xi = x0; xi <= xn + step / 2; xi += step)
            {
                delta += Math.Abs(yi - dY_solved(xi));
                Points.Add(new DataPoint(xi, yi));
                yi += dY(xi + step / 2, yi + dY(xi, yi) * step / 2) * step;
            }
            delta /= (xn - x0) / step;
            add_text_to_log("Метод Эйлера модифицированный\nСреднее отклонение: ", 0);
            add_text_to_log(delta.ToString(tochnost), 2);
            return Points;
        }
        public IList<DataPoint> Dif_Runge_Kutta_4(IList<DataPoint> Points, Func<double, double, double> dY, Func<double, double> dY_solved, double y0, double x0, double xn, double step)
        {
            double xi, yi = y0, k0, k1, k2, k3, delta = 0;
            Points = new List<DataPoint>();
            for (xi = x0; xi <= xn + step / 2; xi += step)
            {
                delta += Math.Abs(yi - dY_solved(xi));
                Points.Add(new DataPoint(xi, yi));
                k0 = dY(xi, yi);
                k1 = dY(xi + step / 2, yi + k0 * step / 2);
                k2 = dY(xi + step / 2, yi + k1 * step / 2);
                k3 = dY(xi + step, yi + k2 * step);
                yi += (k0 + 2 * k1 + 2 * k2 + k3) * step / 6;
            }
            delta /= (xn - x0) / step;
            add_text_to_log("Метод Рунге-Кутта 4\nСреднее отклонение: ", 0);
            add_text_to_log(delta.ToString(tochnost), 2);
            return Points;
        }
        public IList<DataPoint> Dif_Runge_Kutta_5(IList<DataPoint> Points, Func<double, double, double> dY, Func<double, double> dY_solved, double y0, double x0, double xn, double step)
        {
            double xi, yi = y0, k0, k1, k2, k3, k4, delta = 0;
            Points = new List<DataPoint>();
            for (xi = x0; xi <= xn + step / 2; xi += step)
            {
                delta += Math.Abs(yi - dY_solved(xi));
                Points.Add(new DataPoint(xi, yi));
                k0 = dY(xi, yi);
                k1 = dY(xi + step / 3, yi + k0 * step / 3);
                k2 = dY(xi + step / 3, yi + k0 * step / 6 + k1 * step / 6);
                k3 = dY(xi + step / 2, yi + k0 * step / 8 + k2 * step * 3 / 8);
                k4 = dY(xi + step,     yi + k0 * step / 2 - k2 * step * 3 / 2 + k3 * step * 2);
                yi += (k0 + 4 * k3 + k4) * step / 6;
            }
            delta /= (xn - x0) / step;
            add_text_to_log("Метод Рунге-Кутта 5\nСреднее отклонение: ", 0);
            add_text_to_log(delta.ToString(tochnost), 2);
            return Points;
        }
        public IList<DataPoint> Dif_Adams_Bashford_4(IList<DataPoint> Points, Func<double, double, double> dY, Func<double, double> dY_solved, double y0, double x0, double xn, double step)
        {
            double xi = x0, yi = y0, delta = 0, k0, k1, k2, k3, k4;
            int i;
            List<double> x = new List<double>() { 0, 0, 0, 0, 0 };
            List<double> y = new List<double>() { 0, 0, 0, 0, 0 };
            Points = new List<DataPoint>();

            for (i = 0; i < 4; i ++)
            {
                delta += Math.Abs(yi - dY_solved(xi));
                x[i] = xi;
                y[i] = yi;
                k0 = dY(xi, yi);
                k1 = dY(xi + step / 3, yi + k0 * step / 3);
                k2 = dY(xi + step / 3, yi + k0 * step / 6 + k1 * step / 6);
                k3 = dY(xi + step / 2, yi + k0 * step / 8 + k2 * step * 3 / 8);
                k4 = dY(xi + step, yi + k0 * step / 2 - k2 * step * 3 / 2 + k3 * step * 2);
                yi += (k0 + 4 * k3 + k4) * step / 6;
                xi += step;
            }
            y[4] = y[3];
            x[4] = x[3];

            while (x[4] < xn)
            {
                y[4] = y[3] + (55 * dY(x[3], y[3]) - 59 * dY(x[2], y[2]) + 37 * dY(x[1], y[1]) - 9 * dY(x[0], y[0])) * step / 24;
                x[4] += step;

                delta += Math.Abs(y[4] - dY_solved(x[4]));
                Points.Add(new DataPoint(x[4], y[4]));

                y[0] = y[1];
                y[1] = y[2];
                y[2] = y[3];
                y[3] = y[4];

                x[0] = x[1];
                x[1] = x[2];
                x[2] = x[3];
                x[3] = x[4];
            }
            delta /= (xn - x0) / step;
            add_text_to_log("Метод Адамса-Башфорта 4\nСреднее отклонение: ", 0);
            add_text_to_log(delta.ToString(tochnost), 2);
            return Points;
        }
        public IList<DataPoint> Dif_Adams_Moulton_4(IList<DataPoint> Points, Func<double, double, double> dY, Func<double, double> dY_solved, double y0, double x0, double xn, double step)
        {
            double xi = x0, yi = y0, delta = 0, k0, k1, k2, k3, k4;
            int i;
            List<double> x = new List<double>() { 0, 0, 0, 0, 0 };
            List<double> y = new List<double>() { 0, 0, 0, 0, 0 };
            Points = new List<DataPoint>();

            for (i = 0; i < 4; i++)
            {
                delta += Math.Abs(yi - dY_solved(xi));
                x[i] = xi;
                y[i] = yi;

                k0 = dY(xi, yi);
                k1 = dY(xi + step / 3, yi + k0 * step / 3);
                k2 = dY(xi + step / 3, yi + k0 * step / 6 + k1 * step / 6);
                k3 = dY(xi + step / 2, yi + k0 * step / 8 + k2 * step * 3 / 8);
                k4 = dY(xi + step, yi + k0 * step / 2 - k2 * step * 3 / 2 + k3 * step * 2);
                yi += (k0 + 4 * k3 + k4) * step / 6;
                xi += step;
            }

            y[4] = y[3];
            x[4] = x[3];

            while (x[4] < xn)
            {
                y[4] = y[3] + (55 * dY(x[3], y[3]) - 59 * dY(x[2], y[2]) + 37 * dY(x[1], y[1]) - 9 * dY(x[0], y[0])) * step / 24;

                x[4] += step;

                while (Math.Abs(y[4] - yi) > 0.0000000001)
                {
                    y[4] = y[3] + (9 * dY(x[4], y[4]) + 19 * dY(x[3], y[3]) - 5 * dY(x[2], y[2]) + dY(x[1], y[1])) * step / 24;
                    yi = y[i];
                }

                delta += Math.Abs(y[4] - dY_solved(x[4]));
                Points.Add(new DataPoint(x[4], y[4]));

                y[0] = y[1];
                y[1] = y[2];
                y[2] = y[3];
                y[3] = y[4];

                x[0] = x[1];
                x[1] = x[2];
                x[2] = x[3];
                x[3] = x[4];
            }
            delta /= (xn - x0) / step;
            add_text_to_log("Метод Адамса-Моултона 4\nСреднее отклонение: ", 0);
            add_text_to_log(delta.ToString(tochnost), 2);
            return Points;
        }
    }
}