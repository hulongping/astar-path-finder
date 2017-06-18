using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace astar_path_finder
{
    interface IHasNeighbours<N>
    {
        IEnumerable<N> Neighbours { get; }
    }
}
