using System.Linq;

namespace LinearAlgebra
{
    /// <summary>
    /// LU分解
    /// </summary>
    public struct MatrixLU
    {
        /// <summary>
        /// 下三角矩阵
        /// </summary>
        public Matrix L { get; set; }
        /// <summary>
        /// 上三角矩阵
        /// </summary>
        public Matrix U { get; set; }
        /// <summary>
        /// 奇偶性
        /// </summary>
        public int Parity { get; set; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public override string ToString()
        {
            return string.Join("\r\n", L, U);
        }
        public string ToString(string format)
        {
            return string.Join("\r\n", L.ToString(format), U.ToString(format));
        }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// QR分解
    /// </summary>
    public struct MatrixQR
    {
        /// <summary>
        /// Q矩阵（正交矩阵）
        /// </summary>
        public Matrix Q { get; set; }
        /// <summary>
        /// R矩阵（上三角矩阵）
        /// </summary>
        public Matrix R { get; set; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public override string ToString()
        {
            return string.Join("\r\n", Q, R);
        }
        public string ToString(string format)
        {
            return string.Join("\r\n", Q.ToString(format), R.ToString(format));
        }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// 特征值
    /// </summary>
    public struct MatrixEigenValue
    {
        /// <summary>
        /// 特征值的实部数组
        /// </summary>
        public double[] Real { get; set; }
        /// <summary>
        /// 特征值的虚部数组
        /// </summary>
        public double[] Imaginary { get; set; }
        /// <summary>
        /// 特征值的复数数组
        /// </summary>
        public Complex[] ComplexArray { get { return Real.Zip(Imaginary, (x, y) => new Complex(x, y)).ToArray(); } }
    }
}
