using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Re.Grid;
using System.IO;

namespace astar_path_finder
{
    class Program
    {
        static void Main(string[] args)
        {


            double startLon = 122.12;
            double startLat = 31;
            double endLon = 116;
            double endLat = 6;


            List<string> paths = new AStarPathFinder().SolvePath(startLon, startLat, endLon, endLat);

         
        }
    }
}
