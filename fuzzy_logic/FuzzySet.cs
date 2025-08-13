using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuzzy_logic
{
    public class FuzzySet
    {
        public string Name { get; }
        public double[] Points { get; }


        public FuzzySet(string name, params double[] points)
        {
            Name = name;
            Points = points;
        }

        public double GetMembership(double x)
        {
            if (Points.Length == 3)
                return Triangular(x, Points[0], Points[1], Points[2]);
            else
                return Trapezoidal(x, Points[0], Points[1], Points[2], Points[3]);
        }

        private double Triangular(double x, double a, double b, double c)
        {
            if (x <= a || x >= c) return 0;
            if (x == b) return 1;
            if (x > a && x < b) return (x - a) / (b - a);
            return (c - x) / (c - b);
        }

        private double Trapezoidal(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d) return 0;
            if (x >= b && x <= c) return 1;
            if (x > a && x < b) return (x - a) / (b - a);
            return (d - x) / (d - c);
        }

        public double GetCentroid()
        {
            if (Points.Length == 3)
            {
                // Üçgen için centroid: (a + b + c) / 3
                double a = Points[0], b = Points[1], c = Points[2];
                return (a + b + c) / 3.0;
            }
            else if (Points.Length == 4)
            {
                // Yamuk için centroid: analitik formül
                double a = Points[0], b = Points[1], c = Points[2], d = Points[3];
                // Yamuk centroid formülü: (a + b + c + d) * (b - a + d - c) / (3 * (b - a + d - c + c - b))
                double numerator = (d * d + c * d + c * c - a * a - a * b - b * b);
                double denominator = 3 * (d + c - a - b);
                return denominator == 0 ? (a + b + c + d) / 4.0 : numerator / denominator;
            }

            return 0; // Belirsiz durum
        }
    }

}





