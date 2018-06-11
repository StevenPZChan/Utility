using System;
using System.Collections;
using System.Collections.Generic;
using LinearAlgebra.MatrixAlgebra;
namespace LinearAlgebra.VectorAlgebra
{
    /// <summary>
    /// 向量类型
    /// </summary>
    public enum VectorType
    {
        /// <summary>
        /// 行向量
        /// </summary>
        Row,
        /// <summary>
        /// 列向量
        /// </summary>
        Column
    };

    /// <summary>
    /// 向量类
    /// </summary>
    public sealed class Vector : IEquatable<Vector>, IEnumerable<double>
    {
        /// <summary>
        /// 向量元素
        /// </summary>
        public double[] Elements { get { return elements; } set { elements = value; } }
        /// <summary>
        /// 向量类型
        /// </summary>
        public VectorType vectorType { get { return vType; } set { vType = value; } }
        /// <summary>
        /// 向量元素个数
        /// </summary>
        public int Count { get { return elements.Length; } }
        /// <summary>
        /// 返回向量元素
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public double this[int index] { get { return elements[index]; } set { elements[index] = value; } }

        private double[] elements;
        private VectorType vType;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_elements">向量元素</param>
        /// <param name="_vectorType">向量类型</param>
        public Vector(double[] _elements, VectorType _vectorType)
        {
            elements = _elements;
            vType = _vectorType;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static Vector operator +(Vector X)
        {
            return new Vector(X.elements.CopyVector(), X.vType);
        }
        public static Vector operator -(Vector X)
        {
            return VectorComputation.UnaryMinus(X);
        }
        public static Vector operator +(Vector X, Vector Y)
        {
            return VectorComputation.Add(X, Y);
        }
        public static Vector operator -(Vector X, Vector Y)
        {
            return VectorComputation.Subtract(X, Y);
        }
        public static Vector operator *(Vector X, Vector Y)
        {
            return VectorComputation.Multiply(X, Y);
        }
        public static Vector operator /(Vector X, Vector Y)
        {
            return VectorComputation.Divide(X, Y);
        }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// 向量函数
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Vector Map(Func<double, double> func)
        {
            return VectorComputation.Map(this, func);
        }

        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static double DotProduct(Vector X, Vector Y)
        {
            return VectorComputation.DotProduct(X, Y);
        }

        /// <summary>
        /// 行向量乘矩阵
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Vector RowVecMulMat(Vector X, Matrix Y)
        {
            return VectorMatrixComputation.RowVectorMultiplyMatrix(X, Y);
        }

        /// <summary>
        /// 列向量乘矩阵
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Matrix ColVecMulMat(Vector X, Matrix Y)
        {
            return VectorMatrixComputation.ColumnVectorMultiplyMatrix(X, Y);
        }

        /// <summary>
        /// 矩阵乘行向量
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Matrix MatMulRowVec(Matrix X, Vector Y)
        {
            return VectorMatrixComputation.MatrixMultiplyRowVector(X, Y);
        }

        /// <summary>
        /// 矩阵乘列向量
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Vector MatMulColVec(Matrix X, Vector Y)
        {
            return VectorMatrixComputation.MatrixMultiplyColumnVector(X, Y);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public override string ToString()
        {
            if (vType == VectorType.Row) return string.Join("  ", elements);
            else return string.Join("\r\n", elements);
        }
        public bool Equals(Vector other)
        {
            const double eps = 1e-09;
            if (vType != other.vType) return false;
            int n = Count;
            if (n != other.Count) return false;
            unsafe
            {
                fixed (double* x = elements)
                fixed (double* y = other.elements)
                    for (int i = 0; i < n; i++)
                        if (Math.Abs(x[i] - y[i]) > eps)
                            return false;
            }
            return true;
        }
        public IEnumerator<double> GetEnumerator()
        {
            foreach (var item in elements) yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in elements) yield return item;
        }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// 生成向量
        /// </summary>
        /// <param name="_vectorType"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static Vector Range(VectorType _vectorType, double start, int count, double step = 1.0)
        {
            var v = new double[count];
            for (int i = 0; i < count; i++, start += step)
                v[i] = start;
            return new Vector(v, _vectorType);
        }
    }
}
