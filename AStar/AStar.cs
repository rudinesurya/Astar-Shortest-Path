using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    public class Node : IComparable<Node>
    {
        public Node parent = null;
        public double g = double.PositiveInfinity;
        public double h = double.PositiveInfinity;
        public double f = double.PositiveInfinity;
        public int r;
        public int c;

        public int CompareTo(Node obj)
        {
            double result = f - obj.f;

            if (result.Equals(0))
                return 0;
            else if (result > 0)
                return 1;
            else
                return -1;
        }

        public bool Equals(Node obj)
        {
            return r == obj.r && c == obj.c;
        }
    }

    public class AStar
    {
        int[,] map;
        int row;
        int col;

        PriorityQueue<Node> openList = new PriorityQueue<Node>();
        Dictionary<KeyValuePair<int,int>, Node> closedList = new Dictionary<KeyValuePair<int, int>, Node>();

        public AStar(int[,] map)
        {
            this.map = map;
            row = map.GetLength(0);
            col = map.GetLength(1);
        }

        public List<KeyValuePair<int, int>> GetPath(int r1, int c1, int r2, int c2)
        {
            List<KeyValuePair<int, int>> path = new List<KeyValuePair<int, int>>();
            
            closedList.Clear();

            Node goal = new Node();
            goal.r = r2;
            goal.c = c2;

            Node start = new Node();
            start.r = r1;
            start.c = c1;
            start.g = 0;
            start.h = GetHeuristicValue(r1, c1, r2, c2);
            start.f = start.g + start.h;
            start.parent = null;
            openList.Enqueue(start);

            bool goalFound = false;
            while (openList.Count > 0 && !goalFound)
            {
                var p = openList.Dequeue();
                if (closedList.ContainsKey(new KeyValuePair<int, int>(p.r,p.c)))
                {
                    continue;
                }

                if (p.Equals(goal))
                {
                    goal.parent = p;
                    goalFound = true;
                }

                closedList.Add(new KeyValuePair<int, int>(p.r, p.c), p);
                var neighs = GetNeighbours(p.r, p.c);
                for (int i = 0; i < neighs.Count; ++i)
                {
                    var v = neighs[i];
                    if (closedList.ContainsKey(new KeyValuePair<int, int>(v.Key, v.Value)))
                    {
                        continue;
                    }

                    Node newPath = new Node();
                    newPath.r = neighs[i].Key;
                    newPath.c = neighs[i].Value;
                    newPath.g = p.g + 1;
                    newPath.h = GetHeuristicValue(neighs[i].Key, neighs[i].Value, r2, c2);
                    newPath.f = newPath.g + newPath.h;
                    newPath.parent = p;

                    if (openList.Contains(newPath))
                    {
                        Console.WriteLine("XX");
                    }
                    else
                        openList.Enqueue(newPath);
                }
            }

            Node current = goal;
            while (true)
            {
                path.Add(new KeyValuePair<int, int>(current.r, current.c));

                if (current.parent == null)
                    break;
                else
                    current = current.parent;
            }

            return path;
        }

        private List<KeyValuePair<int, int>> GetNeighbours(int r, int c)
        {
            //check for out of bounds
            if (r >= row || r < 0)
                return null;
            if (c >= col || c < 0)
                return null;

            List<KeyValuePair<int, int>> result = new List<KeyValuePair<int, int>>();

            //j = row
            //i = col
            for (int j = -1; j < 2; ++j)
            {
                for (int i = -1; i < 2; ++i)
                {
                    if (i == j)
                        continue;

                    int neighR = r + j;
                    int neighC = c + i;

                    //check for out of bounds
                    if (neighR >= row || neighR < 0)
                        continue;
                    if (neighC >= col || neighC < 0)
                        continue;

                    if (map[neighR, neighC] == 0)
                        result.Add(new KeyValuePair<int, int>(neighR, neighC));
                }
            }

            return result;
        }

        private double GetHeuristicValue(int r1, int c1, int r2, int c2)
        {
            return Math.Sqrt(Math.Pow(c2 - c1, 2) + Math.Pow(r2 - r1, 2));
        }
    }
}
