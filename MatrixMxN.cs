using System;
using System.Collections.Generic;

namespace MNKSolve
{
    [Serializable]
    public class MatrixMxN
    {
        public int m;
        public int n;
        public List<MatrixCol> matrixRows = new List<MatrixCol>();

        public MatrixMxN()
        { }

        public MatrixMxN(int m, int n)
        {
            this.m = m;
            this.n = n;
            for (int i = 0; i < m; i++)
            {
                MatrixCol col = new MatrixCol();
                for (int j = 0; j < n; j++)
                {
                    col.Cols.Add(0.0);
                }
                matrixRows.Add(col);
            }
        }

        public static MatrixMxN Copy(MatrixMxN A)
        {
            try
            {
                MatrixMxN res = new MatrixMxN(A.m, A.n);
                for (int i = 0; i < A.m; i++)
                {
                    for (int j = 0; j < A.n; j++)
                    {
                        res.Set(i, j, A.Get(i, j));
                    }

                }
                return res;
            }
            catch
            {
                return null;
            }
        }

        public double Get(int i, int j)
        {
            try
            {
                return matrixRows[i].Cols[j];
            }
            catch { return 0.0; }
        }

        public void Set(int i, int j, double Value)
        {
            try
            {
                matrixRows[i].Cols[j] = Value;
            }
            catch { }
        }
        public static MatrixMxN Transponse(MatrixMxN A)
        {
            if (A == null) return null;
            try
            {
                MatrixMxN matrixMxN = new MatrixMxN(A.n, A.m);
                for (int i = 0; i < A.n; i++)
                {
                    for (int j = 0; j < A.m; j++)
                    {
                        matrixMxN.Set(i, j, A.Get(j, i));
                    }
                }
                return matrixMxN;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static double Minor(MatrixMxN A, int m1, int n1)
        {
            if (A == null) return double.NaN;
            if (A.m != A.n) return double.NaN;
            if (A.m <= 1) return double.NaN;
            // MatrixMxN minor = new MatrixMxN(A.m - 1, A.m - 1);
            MatrixMxN Ac = Copy(A);
            try
            {
                for (int i = 0; i < A.m; i++)
                {
                    Ac.matrixRows[i].Cols.RemoveAt(n1);
                }
                Ac.matrixRows.RemoveAt(m1);
                Ac.m -= 1;
                Ac.n -= 1;
                return Det(Ac);
            }
            catch (Exception e)
            {
                return double.NaN;
            }
        }

        public static double Det(MatrixMxN A)
        {

            if (A == null) throw new Exception("A == null");
            if (A.m != A.n) throw new Exception("not kvadratish");
            if (A.m == 1) return A.Get(0, 0);
            try
            {
                if (A.m == 2)
                {
                    return (A.Get(0, 0) * A.Get(1, 1) - A.Get(1, 0) * A.Get(0, 1));
                }
                double det = 0.0;
                for (int j = 0; j < A.m; j++)
                {
                    double t1 = Math.Pow(-1.0, j);
                    det += t1 * A.Get(0, j) * Minor(A, 0, j);
                }
                return det;
            }
            catch
            {
                return double.NaN;
            }
        }

        public static double Dopoln(MatrixMxN A, int i, int j)
        {
            if (A == null) throw new Exception("null!");
            try
            {
                return Math.Pow(-1.0,i+j)*Minor(A, i, j);
            }
            catch
            {
                int hh = 1;
            }
            return double.NaN;
        }

        public static MatrixMxN Souzn(MatrixMxN A)
        {
            if (A == null) return null;
            if (A.m != A.n) throw new Exception("nott kvadratish!");   
            try
            {
                MatrixMxN res = new MatrixMxN(A.m, A.n);
                for (int i = 0; i < A.m; i++)
                    for (int j = 0; j < A.n; j++)
                    {
                        res.Set(i, j, Dopoln(A, j, i));
                    }
                return res;
            }
            catch
            {
                return null;
            }
        }

        public static MatrixMxN ObratNaya(MatrixMxN A)
        {
            if (A == null) return null;
            double detA = Det(A);
            if (detA == double.NaN) return null;
            if (detA==0.0) return null;
            try
            {
                MatrixMxN res = Souzn(A);
                for (int i = 0; i < A.m; i++)
                    for (int j = 0; j < A.n; j++)
                    {
                        res.Set(i, j, res.Get(i, j) / detA);
                    }
                return res;
            }
            catch
            {
                return null;
            }
        }

        public static MatrixMxN Mul(MatrixMxN A, MatrixMxN B)
        {
            if (A == null) return null;
            if (B == null) return null;
            if (A.n != B.m) return null;
            try
            {
                MatrixMxN C = new MatrixMxN(A.m, B.n);
                for (int i = 0; i < A.m; i++)
                {
                    for (int j = 0; j < B.n; j++)
                    {
                        double sum = 0.0;
                        for (int r = 0; r < A.n; r++)
                        {
                            sum += (A.Get(i, r) * B.Get(r, j));
                        }
                        C.Set(i, j, sum);
                    }
                }
                return C;
            }
            catch { }
            return null;
        }
    }
}
