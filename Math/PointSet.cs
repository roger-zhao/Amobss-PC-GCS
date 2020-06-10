using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// =============================================
// Author:		Maciej Krawczyk
// Creation date: 29.05.2017
// =============================================

namespace LeastSquares
{
    //represents a single point in two-dimensional coordinate system
    struct Point 
    {
        public double x { get; set; }
        public double y { get; set; }
        public Point(double x, double y) //constructor
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }
    }

    //represents a line in the form of y = ax + b
    struct StraightLine
    {
        public double a { get; set; }
        public double b { get; set; }
        public StraightLine(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public override string ToString()
        {
            return string.Format("y = {0:0.0000}*x + {1:0.0000}", a, b);
        }
    }

    //represents a collection of points stacked in list, has method for estimate straight line using least squares
    class PointSet 
    {
        List<Point> pointsList;

        //constructors
        public PointSet()
        {
            pointsList = new List<Point>();
        }

        public PointSet(IEnumerable<Point> points)
        {
            pointsList = new List<Point>(points);
        }

        //returns estimated line based on actual content of PointSet
        public StraightLine FindApproximateSolution()
        {
            double[] derivative_a = new double[3];
            double[] derivative_b = new double[3];
            for (int i = 0; i < pointsList.Count; i++)
            {
                derivative_a[0] += pointsList[i].x * pointsList[i].x;
                derivative_a[1] += pointsList[i].x;
                derivative_a[2] += pointsList[i].x * pointsList[i].y;
                derivative_b[0] += pointsList[i].x;
                derivative_b[1] += 1;
                derivative_b[2] += pointsList[i].y;
            }

            double determinant = derivative_a[0] * derivative_b[1] - derivative_a[1] * derivative_b[0];
            double determinant_x = derivative_a[2] * derivative_b[1] - derivative_a[1] * derivative_b[2];
            double determinant_y = derivative_a[0] * derivative_b[2] - derivative_a[2] * derivative_b[0];
            //if (determinant == 0)
            //    throw new Exception("No solution exists");
            return new StraightLine(determinant_x / determinant, determinant_y / determinant);
        }

        public int Count
        {
            get
            {
                return pointsList.Count;
            }
        }

        public void Add(Point item)
        {
            pointsList.Add(item);
        }

        public void Clear()
        {
            pointsList.Clear();
        }

        public bool Contains(Point item)
        {
            return pointsList.Contains(item);
        }

        public void CopyTo(Point[] array, int arrayIndex)
        {
            pointsList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return pointsList.GetEnumerator();
        }

        public bool Remove(Point item)
        {
            return pointsList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            pointsList.RemoveAt(index);
        }
    }
}
