using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexHeap
{
    class Search
    {
        static readonly double SQRT3 = Math.Sqrt(3);
        public IEnumerable<string> Print(double R, double r)
        {
            double k = R / r;
            Position vertexMatch = new Position { X = 0, Y = 0 };
            Position centerMatch = new Position { X = SQRT3 / 2, Y = 0.5 };
            var list1 = Compare(k, vertexMatch);
            var list2 = Compare(k, centerMatch);
            return list1.Concat(list2);
        }
        private IEnumerable<string> Compare(double k, Position origin)
        {
            var groups= CalcDistance(k + 1, origin)
                .Where(x => x.Distance <= k)
                .OrderBy(x => x.Distance)
                .GroupBy(x => $"{x.Distance:0.00}");
            int total = 0;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach(var group in groups)
            {
                total += group.Count();
                dict.Add(group.Key, total);
                double third = double.Parse(group.Key) / 3;
                int removed = 0;
                foreach(var item in dict)
                {
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
                    Position vertexMatch = new Position { X = m*SQRT3, Y = 3 * n };
                    double distVertexMatch = DistSquare(vertexMatch, origin);
                    if (distVertexMatch <= square)
                        yield return new PositionDetail
                        {
                            XRational = "0",
                            XIrational = m.ToString(),
                            YRational = (3 * n).ToString(),
                            YIrational = "0",
                            Distance = Math.Sqrt(distVertexMatch)
                        };
                    Position centerMatch = new Position { X = (0.5 + m)*SQRT3, Y = (0.5 + n) * 3 };
                    double distCenterMatch = DistSquare(centerMatch, origin);
                    if (distCenterMatch < square)
                        yield return new PositionDetail
                        {
                            XRational = "0",
                            XIrational = (0.5 + m).ToString(),
                            YRational = ((0.5 + n) * 3).ToString(),
                            YIrational = "0",
                            Distance=Math.Sqrt(distCenterMatch)
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
        private string DetailToString(PositionDetail detail)
        {
            return $"{detail.XRational}_{detail.XIrational}\t{detail.YRational}_{detail.YIrational}\t{detail.Distance:0.00}";
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
