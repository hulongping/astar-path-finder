using System;
using System.Collections.Generic;
using System.Linq;
using Re.Grid;
using System.Configuration;



namespace astar_path_finder
{
    class AStarPathFinder
    {
        public List<string> SolvePath(double startLon,double startLat,double endLon,double endLat)
        {


            string mask_asc = ConfigurationManager.AppSettings["mask"];
            AscFile asc = new AscFile(mask_asc);
            asc.ReadIntValues();
            asc.Dispose();

            Graph oceanGrapc = new Graph();
            FillOceanGraphWithMask(oceanGrapc, asc,80,-80);


            int startRowKey = asc.getYIndex(startLat);
            int startColoumKey = asc.getXIndex(startLon);
            int endRowKey = asc.getYIndex(endLat);
            int endColumnKey = asc.getXIndex(endLon);

            string startPortKey = startRowKey + "-" + startColoumKey;
            string destinationPortKey = endRowKey + "-" + endColumnKey;


            Node startPort = oceanGrapc.Nodes[startPortKey];

            Node destinationPort = oceanGrapc.Nodes[destinationPortKey];

            Func<Node, Node, double> distance = (node1, node2) =>
                                                   node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                       etn => etn.Neighbor.Key == node2.Key).Cost;
            Func<Node, double> haversineEstimation =
                   n => Haversine.Distance(n, destinationPort, DistanceType.km);

            Path<Node> shortestPath = FindPath(startPort, destinationPort, distance, haversineEstimation);

            List<string> pathString = new List<string>();

            foreach (Path<Node> path in shortestPath.Reverse())
            {
                if (path.PreviousSteps != null)
                {

                    Node step = path.LastStep;
                    string line =(step.Longitude.ToString("#.####") + "," + step.Latitude.ToString("#.####") + "," + path.TotalCost.ToString("#.#"));

                    pathString.Add(line);
                  
                }
            }

            return pathString;


        }

       


        private void FillOceanGraphWithMask(Graph graph, AscFile asc,double maxLat,double minLat)
        {

            int startRow = asc.getYIndex(maxLat);
            int endRow = asc.getYIndex(minLat);

            DistanceType distanceType = DistanceType.km;
            AscHeader header = asc.header;
            int[,] values = asc.values;
            int nodata = (int)header.NodataValue;

            //添加海洋上的控制节点和无向图
            for(int i= startRow; i< endRow; i++)
            {
              
                for(int j = 0; j < header.ColumnCount; j++)
                {
                    int v = values[i, j];
                    if (v != nodata)
                    {
                        double lat = asc.getLatitudeByIdx(i);
                        double lon = asc.getLongitudeByIdx(j);
                        string ckey = i + "-" + j;
                        Node cNode = new Node(ckey,null, lat, lon);
                        if (!graph.Contains(cNode))
                        {
                            graph.AddNode(cNode);
                        }
                       

                        //点的8个方向扩散
                        for (int rowstep = -1; rowstep <= 1; rowstep += 1)
                        {
                            for(int colstep = -1; colstep <= 1; colstep += 1)
                            {
                                int rowI = i + rowstep;
                                int colJ = j + colstep;

                                //上下纬度不要超过-90到90
                                if (rowI>=0 && rowI < header.RowCount)
                                {
                                    //这样就保证了这个球体是连续的了，可以横跨本初子午线
                                    if (colJ == -1)
                                    {
                                        colJ = -1 + header.ColumnCount;
                                    }
                                    else if (colJ == header.ColumnCount)
                                    {
                                        colJ = 0;
                                    }

                                    int evalue = values[rowI, colJ];
                                    string ekey = rowI + "-" + colJ;
                                    //下一个方向的节点也是
                                    if (evalue != nodata && ekey != cNode.Key)
                                    {
                                        double elat = asc.getLatitudeByIdx(rowI);
                                        double elon = asc.getLongitudeByIdx(colJ);
                                        //初始化这个节点
                                        Node enode = new Node(ekey, null, elat, elon);
                                        if (!graph.Contains(ekey))
                                        {                                          
                                            graph.AddNode(enode);
                                        }
                                        graph.AddDirectedEdge(cNode.Key, enode.Key, Haversine.Distance(cNode, enode, distanceType));
                                    }

                                    

                                }
                            }
                        }                                     
                    }
                }
            }        
        }





      

        /// <summary>
        /// This is the method responsible for finding the shortest path between a Start and Destination cities using the A*
        /// search algorithm.
        /// </summary>
        /// <typeparam name="TNode">The Node type</typeparam>
        /// <param name="start">Start city</param>
        /// <param name="destination">Destination city</param>
        /// <param name="distance">Function which tells us the exact distance between two neighbours.</param>
        /// <param name="estimate">Function which tells us the estimated distance between the last node on a proposed path and the
        /// destination node.</param>
        /// <returns></returns>
         public Path<TNode> FindPath<TNode>(
            TNode start,
            TNode destination,
            Func<TNode, TNode, double> distance,
            Func<TNode, double> estimate) where TNode : IHasNeighbours<TNode>
        {
            var closed = new HashSet<TNode>();

            var queue = new PriorityQueue<double, Path<TNode>>();

            queue.Enqueue(0, new Path<TNode>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                    continue;

                if (path.LastStep.Equals(destination))
                    return path;

                closed.Add(path.LastStep);

                foreach (TNode n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);

                    var newPath = path.AddStep(n, d);

                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }

            }

            return null;
        }

       
    }

    sealed partial class Node : IHasNeighbours<Node>
    {
        public IEnumerable<Node> Neighbours
        {
            get
            {
                List<Node> nodes = new List<Node>();

                foreach (EdgeToNeighbor etn in Neighbors)
                {
                    nodes.Add(etn.Neighbor);
                }

                return nodes;
            }
        }
    }
}
