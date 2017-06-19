using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
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

            string mask_asc = ConfigurationManager.AppSettings["mask"];

            AStarPathFinder solver = new AStarPathFinder(mask_asc);



            List<string> paths = solver.SolvePath(startLon, startLat, endLon, endLat);

         
        }
    }
}
