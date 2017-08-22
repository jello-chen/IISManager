using System;
using System.Collections.Generic;
using System.Linq;
using IISManager.Core;

namespace IISManager.Workbench.Common
{
    public class ModelBuilder
    {
        public static List<IISTreeNode> Build()
        {
            var rootName = $"{Environment.MachineName}({Environment.UserDomainName}\\{Environment.UserName})";
            var rootNode = new IISTreeNode { Id = -1, Text = rootName, HasContextMenu = false, Nodes = new List<IISTreeNode>() };

            var controller = IISController.GetController();

            var appPools = controller.GetAllPoolInfos();
            var appPoolNode = new IISTreeNode { Id = -2, Text = "Application Pools", HasContextMenu = false, Nodes = new List<IISTreeNode>() };
            appPoolNode.Nodes.AddRange(appPools.Data.Select((a, i) => new IISTreeNode { Id = appPoolNode.Id - i - 1, Text = a.Name, HasContextMenu = false }));
            rootNode.Nodes.Add(appPoolNode);

            var sites = controller.GetAllSites();
            var siteNode = new IISTreeNode { Id = 0, Text = "Web Sites", HasContextMenu = false, Nodes = new List<IISTreeNode>() };
            siteNode.Nodes.AddRange(sites.Data.Select(s => new IISTreeNode { Id = s.ID, Text = s.Name, HasContextMenu = true, IsSite = true, Status = (int)s.State }));
            rootNode.Nodes.Add(siteNode);

            var nodes = new List<IISTreeNode>();
            nodes.Add(rootNode);
            return nodes;
        }
    }
    public class IISTreeNode
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool IsSite { get; set; }
        public int Status { get; set; }
        public bool HasContextMenu { get; set; }
        public List<IISTreeNode> Nodes { get; set; }
    }
}
