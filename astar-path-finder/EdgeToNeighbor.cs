using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar_path_finder
{
    public class EdgeToNeighbor
    {
        public EdgeToNeighbor(Node neighbor) : this(neighbor, 0)
        {

        }

        public virtual double Cost { get; private set; }

        public virtual Node Neighbor { get; private set; }

        public EdgeToNeighbor(Node neighbor, double cost)
        {
            Cost = cost;
            Neighbor = neighbor;
        }

        public override string ToString()
        {
            return string.Format("Neighbor = {0} | Cost = {1}", Neighbor.Key, Cost);
        }
    }
}
