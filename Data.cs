using System;
using System.Collections.Generic;
using System.Linq;
using CG.Web.MegaApiClient;

namespace micro
{
    class Data
    {
        public static void ListAll(MegaApiClient client){
            IEnumerable<INode> nodes = client.GetNodes();
            INode parent = nodes.Single(n => n.Type == NodeType.Root);
            DisplayNodesRecursive(nodes, parent);
        }

        public static void DisplayNodesRecursive(IEnumerable<INode> nodes, INode parent, int level = 0)
        {
            IEnumerable<INode> children = nodes.Where(x => x.ParentId == parent.Id);
            foreach (INode child in children)
            {
                string infos = $"- {child.Name, -10} - {child.Size} bytes - {child.CreationDate}";
                Console.WriteLine(infos.PadLeft(infos.Length + level, '\t'));
                if (child.Type == NodeType.Directory)
                    DisplayNodesRecursive(nodes, child, level + 1);
            }
        }
    }
}