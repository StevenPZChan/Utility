using System;
using System.Globalization;
namespace LinearAlgebra
{
    /// <summary>
    /// 复数类
    /// </summary>
    public struct Complex : IEquatable<Complex>, IFormattable
    {
        /// <summary>
        /// 实部
        /// </summary>
        public double Real { get { return real; } set { real = value; } }
        /// <summary>
        /// 虚部
        /// </summary>
        public double Imaginary { get { return imaginary; } set { imaginary = value; } }

        private const double eps = 1e-9;
        private const double Radian90 = Math.PI / 2.0;
        private double real;
        private double imaginary;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_real">实部</param>
        /// <param name="_imaginary">虚部</param>
        public Complex(double _real, double _imaginary) { real = _real; imaginary = _imaginary; }

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Complex Add(Complex other)
        {
            return new Complex(real + other.real, imaginary + other.imaginary);
        }

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Complex Subtract(Complex other)
        {
            return new Complex(real - other.real, imaginary - other.imaginary);
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Complex Multiply(Complex other)
        {
            double x = real * other.real - imaginary * other.imaginary;
            double y = real * other.imaginary + imaginary * other.real;
            return new Complex(x, y);
        }

        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Complex Divide(Complex other)
        {
            double e, f, x, y;
            if (Math.Abs(other.real) >= Math.Abs(other.imaginary))
            {
                e = other.imaginary / other.real;
                f = other.real + e * other.real;
                x = (real + imaginary * e) / f;
                y = (imaginary - real * e) / f;
            }
            else
            {
                e = other.real / other.imaginary;
                f = other.imaginary + e * other.real;
                x = (real * e + imaginary) / f;
                y = (imaginary * e - real) / f;
            }
            return new Complex(x, y);
        }

        /// <summary>
        /// 数乘方
        /// </summary>
        /// <param name="Exponent">指数</param>
        /// <returns></returns>
        public Complex Pow(double Exponent)
        {
            double r, t;
            if ((real == 0.0) && (imaginary == 0.0)) return new Complex(0.0, 0.0);
            if (real == 0.0)//幂运算公式中的三角函数运算
            {
                if (imaginary > 0) t = Radian90;
                else t = -Radian90;
            }
            else
            {
                if (real > 0)
                    t = Math.Atan2(imaginary, real);
                else
                {
                    if (imaginary >= 0)
                        t = Math.Atan2(imaginary, real) + Math.PI;
                    else
                        t = Math.Atan2(imaginary, real) - Math.PI;
                }
            }
            r = Math.Exp(Exponent * Math.Log(Math.Sqrt(QuadraticSum())));
            double u = Exponent * t;
            return new Complex(r * Math.Cos(u), r * Math.Sin(u));
        }

        /// <summary>
        /// 矩阵乘方
        /// </summary>
        /// <param name="Exponent">矩阵底</param>
        /// <param name="n">指数</param>
        /// <returns></returns>
        public Complex Pow(Complex Exponent, int n)
        {
            double r, s, u, v;
            if (real == 0.0)
            {
                if (imaginary == 0.0) return new Complex(0.0, 0.0);
                s = Radian90 * (Math.Abs(imaginary) / imaginary + 4.0 * n);
            }
            else
            {
                s = 2.0 * Math.PI * n + Math.Atan2(imaginary, real);
                if (real < 0)
                {
                    if (imaginary > 0) s += Math.PI;
                    else s -= Math.PI;
                }
            }
            //求幂运算公式
            r = 0.5 * Math.Log(QuadraticSum());
            v = Exponent.real * r + Exponent.imaginary * s;
            u = Math.Exp(Exponent.real * r - Exponent.imaginary * s);
            return new Complex(u * Math.Cos(v), u * Math.Sin(v));
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <returns></returns>
        public double Abs()
        {
            double x = Math.Abs(real);
            double y = Math.Abs(imaginary);
            if (real == 0.0) return y;
            if (imaginary == 0.0) return x;
            if (x > y)
                return x * Math.Sqrt(1.0 + Square(y / x));
            return y * Math.Sqrt(1.0 + Square(x / y));
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <returns></returns>
        public Complex Log()
        {
            double p = Math.Log(Math.Sqrt(QuadraticSum()));
            return new Complex(p, Math.Atan2(imaginary, real));
        }

        private static double Square(double x)
        {
            return x * x;
        }

        private double QuadraticSum()
        {
            return real * real + imaginary * imaginary;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static implicit operator Complex(double x) { return new Complex(x, 0.0); }
        public static Complex operator +(Complex z1, Complex z2)
        {
            return z1.Add(z2);
        }
        public static Complex operator -(Complex z1, Complex z2)
        {
            return z1.Subtract(z2);
        }
        public static Complex operator *(Complex z1, Complex z2)
        {
            return z1.Multiply(z2);
        }
        public static Complex operator /(Complex z1, Complex z2)
        {
            return z1.Divide(z2);
        }
        public static bool operator ==(Complex z1, Complex z2)
        {
            return z1.Equals(z2);
        }
        public static bool operator !=(Complex z1, Complex z2)
        {
            return !z1.Equals(z2);
        }
        public override string ToString()
        {
            if (real != 0.0)
            {
                if (imaginary > 0.0)
                    return string.Concat(real.ToString(), " + ", imaginary.ToString(), "i");
                else if (imaginary < 0.0)
                    return string.Concat(real.ToString(), " - ", Math.Abs(imaginary).ToString(), "i");
                else return real.ToString();
            }
            else return string.Concat(imaginary.ToString(), "i");
        }
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Complex)obj);
        }
        public override int GetHashCode()
        {
            return (int)Math.Sqrt(real * real + imaginary * imaginary);
        }
        public bool Equals(Complex other)
        {
            return Math.Abs(real - other.real) < eps && Math.Abs(imaginary - other.imaginary) < eps;
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (real != 0.0)
            {
                if (imaginary > 0.0)
                    return string.Concat(real.ToString(format, formatProvider), " + ", imaginary.ToString(format, formatProvider), "i");
                else if (imaginary < 0.0)
                    return string.Concat(real.ToString(format, formatProvider), " - ", Math.Abs(imaginary).ToString(format, formatProvider), "i");
                else return real.ToString(format, formatProvider);
            }
            else return string.Concat(imaginary.ToString(format, formatProvider), "i");
        }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}
