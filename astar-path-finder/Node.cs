using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar_path_finder
{
    public partial class Node
    {
        #region 私有变量

       

        #endregion

        #region 公共属性

      
        public string Key { get; private set; }

        /// <summary>
        /// Returns the TNode's Data.
        /// </summary>
        public object Data { get; set; }

        
        public AdjacencyList Neighbors { get; private set; }

        public Node PathParent { get; set; }

       
        public int X { get; set; }

       
        public int Y { get; set; }

      
        public double Latitude { get; set; }

     
        public double Longitude { get; set; }

        #endregion

        #region Constructors

        public Node(string key, object data) : this(key, data, null)
        {
        }

        public Node(string key, object data, int x, int y) : this(key, data, x, y, null)
        {
        }

        public Node(string key, object data, double latitude, double longitude)
            : this(key, data, latitude, longitude, null)
        {
        }

        public Node(string key, object data, AdjacencyList neighbors)
        {
            Key = key;
            Data = data;

            if (neighbors == null)
            {
                Neighbors = new AdjacencyList();
            }
            else
            {
                Neighbors = neighbors;
            }
        }

        public Node(string key, object data, int x, int y, AdjacencyList neighbors)
        {
            Key = key;
            Data = data;
            X = x;
            Y = y;

            if (neighbors == null)
            {
                Neighbors = new AdjacencyList();
            }
            else
            {
                Neighbors = neighbors;
            }
        }

        public Node(string key, object data, double latitude, double longitude, AdjacencyList neighbors)
        {
            Key = key;
            Data = data;
            Latitude = latitude;
            Longitude = longitude;

            if (neighbors == null)
            {
                Neighbors = new AdjacencyList();
            }
            else
            {
                Neighbors = neighbors;
            }
        }

        #endregion

        #region Public Methods

        #region Add Methods

      
        internal void AddDirected(Node n)
        {
            AddDirected(new EdgeToNeighbor(n));
        }

       
        internal void AddDirected(Node n, int cost)
        {
            AddDirected(new EdgeToNeighbor(n, cost));
        }

       
        internal void AddDirected(Node n, double cost)
        {
            AddDirected(new EdgeToNeighbor(n, cost));
        }

        
        internal void AddDirected(EdgeToNeighbor e)
        {
            Neighbors.Add(e);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Key = {0} | X, Y = [{1}, {2}] | Latitude, Longitude = [{3}, {4}] | Data = {5}",
                Key, X, Y, Latitude, Longitude, Data);
        }

        #endregion
    }
}
