using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexHeap
{
    class Expend
    {
        static readonly double SQRT3 = Math.Sqrt(3);
        public IEnumerable<string> Print(double R, double r)
        {
            double k = R / r;
            Position vertexAlign = new Position { X = 0, Y = 0 };
            Position centerAlign = new Position { X = SQRT3 / 2, Y = 0.5 };
            var list1 = Compare(k, vertexAlign);
            var list2 = Compare(k, centerAlign);
            return list1.Concat(list2);
        }
        private IEnumerable<string> Compare(double k, Position origin)
        {
            // Use k+2 to include all boundary data.
            var groups= CalcDistance(k + 2, origin)
                // Use k as a threshold.
                .Where(x => x.Distance <= k)
                .OrderBy(x => x.Distance)
                .GroupBy(x => $"{x.Distance:0.00}");
            int total = 0;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach(var group in groups)
            {
                total += group.Count();
                // Record the total vertices inside the circle.
                dict.Add(group.Key, total);
                double third = double.Parse(group.Key) / 3;
                int removed = 0;
                foreach(var item in dict)
                {
                    // Remove the core with 1/3 radiance.
                    if (double.Parse(item.Key) < third)
                        removed = item.Value;
                    else
                        break;
                }
                yield return $"{group.Key}\t{total}\t{total - removed}";
            }
        }
        private IEnumerable<PositionDetail> CalcDistance(double k, Position origin)
        {
            int mBoundary = (int)(k / SQRT3) + 1;
            int nBoundary = (int)(k / 3) + 1;
            double square = k * k;
            for(int m = -mBoundary; m <= mBoundary; m ++)
            {
                for(int n = -nBoundary; n <= nBoundary; n++)
                {
                    // The coordinate is (m*sqrt3, n*2)
                    Position vertex1 = new Position { X = m*SQRT3, Y = 3 * n };
                    double dist1 = DistSquare(vertex1, origin);
                    if (dist1 <= square)
                        yield return new PositionDetail
                        {
                            XRational = "0",
                            XIrational = m.ToString(),
                            YRational = (3 * n).ToString(),
                            YIrational = "0",
                            Distance = Math.Sqrt(dist1)
                        };
                    // The coordinate is ((m+1/2)*sqrt3, (n+/1/2)*sqrt3)
                    Position vertex2 = new Position { X = (0.5 + m)*SQRT3, Y = (0.5 + n) * 3 };
                    double dist2 = DistSquare(vertex2, origin);
                    if (dist2 < square)
                        yield return new PositionDetail
                        {
                            XRational = "0",
                            XIrational = (0.5 + m).ToString(),
                            YRational = ((0.5 + n) * 3).ToString(),
                            YIrational = "0",
                            Distance = Math.Sqrt(dist2)
                        };
                }
            }
        }
        private double DistSquare(Position p1, Position p2)
        {
            double xDiff = p1.X - p2.X;
            double yDiff = p1.Y - p2.Y;
            return xDiff * xDiff + yDiff * yDiff;
        }
    }
    struct Position
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    struct PositionDetail
    {
        public string XRational { get; set; }
        public string XIrational { get; set; }
        public string YRational { get; set; }
        public string YIrational { get; set; }
        public double Distance { get; set; }
    }
}
