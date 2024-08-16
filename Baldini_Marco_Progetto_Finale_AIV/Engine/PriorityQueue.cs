﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baldini_Marco_Progetto_Finale_AIV
{
    class PriorityQueue
    {
        Dictionary<Node, int> items;
        public bool IsEmpty { get => items.Count == 0;  }

        public PriorityQueue()
        {
            items = new Dictionary<Node, int>();
        }

        public void Enqueue(Node n, int priority)
        {
            if (!items.ContainsKey(n))
            {
                items.Add(n, priority);
            }
        }

        public Node Dequeue()
        {
            int lowestPriority = int.MaxValue;
            Node node = null;

            foreach (Node item in items.Keys)
            {
                int currentPriority = items[item];

                if(currentPriority < lowestPriority)
                {
                    lowestPriority = currentPriority;
                    node = item;
                }
            }

            items.Remove(node);

            return node;
        }
    }
}
