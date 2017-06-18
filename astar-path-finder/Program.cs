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

            //ReSampleAscFile re = new ReSampleAscFile(@"D:\ocean.asc", @"D:\ocean4.asc");
            //re.ReSample(4);
          
            List<string> paths = new AStarPathFinder().SolvePath();

            var stream = new FileStream(@"D:\path.csv", FileMode.Create);
            StreamWriter _writer = new StreamWriter(stream, Encoding.Default);
            _writer.WriteLine("lon,lat,cost");

            for(int i = 0; i < paths.Count; i++)
            {
                _writer.WriteLine(paths[i]);
            }
            _writer.Flush();
            _writer.Close();
        }
    }
}
