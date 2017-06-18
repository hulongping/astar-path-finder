using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace astar_path_finder
{
    class PriorityQueue<P, V> : IEnumerable
    {

        private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();

        
        
        public void Enqueue(P priority, V value)
        {
            Queue<V> q;
            //进队列
            if (!list.TryGetValue(priority, out q))
            {
                q = new Queue<V>();

                list.Add(priority, q);
            }

            q.Enqueue(value);
        }

        public V Dequeue()
        {
            // 出队列
            var pair = list.First();

            var v = pair.Value.Dequeue();

            if (pair.Value.Count == 0) 
                list.Remove(pair.Key);
            return v;
        }

        public bool IsEmpty
        {
            get { return !list.Any(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
