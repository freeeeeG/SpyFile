using System;
using System.Collections.Generic;

namespace AStar
{
	// Token: 0x020000FB RID: 251
	public class PathFinder
	{
		// Token: 0x060004BC RID: 1212 RVA: 0x00027FF4 File Offset: 0x000263F4
		public PathFinder(SearchParameters searchParameters)
		{
			this.searchParameters = searchParameters;
			this.InitializeNodes(searchParameters.Map);
			this.startNode = this.nodes[searchParameters.StartLocation.X, searchParameters.StartLocation.Y];
			this.startNode.State = NodeState.Open;
			if (searchParameters.EndLocation.X < this.nodes.GetLength(0) && searchParameters.EndLocation.Y < this.nodes.GetLength(1))
			{
				this.endNode = this.nodes[searchParameters.EndLocation.X, searchParameters.EndLocation.Y];
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000280AC File Offset: 0x000264AC
		public List<Point2> FindPath()
		{
			List<Point2> list = new List<Point2>();
			if (this.endNode != null)
			{
				bool flag = this.Search(this.startNode);
				if (flag)
				{
					Node parentNode = this.endNode;
					while (parentNode.ParentNode != null)
					{
						list.Add(parentNode.Location);
						parentNode = parentNode.ParentNode;
					}
					list.Reverse();
				}
			}
			return list;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00028110 File Offset: 0x00026510
		private void InitializeNodes(bool[,] map)
		{
			this.width = map.GetLength(0);
			this.height = map.GetLength(1);
			this.nodes = new Node[this.width, this.height];
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					this.nodes[j, i] = new Node(j, i, map[j, i], this.searchParameters.EndLocation);
				}
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000281A4 File Offset: 0x000265A4
		private bool Search(Node currentNode)
		{
			currentNode.State = NodeState.Closed;
			List<Node> adjacentWalkableNodes = this.GetAdjacentWalkableNodes(currentNode);
			adjacentWalkableNodes.Sort((Node node1, Node node2) => node1.F.CompareTo(node2.F));
			foreach (Node node in adjacentWalkableNodes)
			{
				if (node.Location == this.endNode.Location)
				{
					return true;
				}
				if (this.Search(node))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0002825C File Offset: 0x0002665C
		private List<Node> GetAdjacentWalkableNodes(Node fromNode)
		{
			List<Node> list = new List<Node>();
			IEnumerable<Point2> adjacentLocations = PathFinder.GetAdjacentLocations(fromNode.Location);
			foreach (Point2 point in adjacentLocations)
			{
				int x = point.X;
				int y = point.Y;
				if (x >= 0 && x < this.width && y >= 0 && y < this.height)
				{
					Node node = this.nodes[x, y];
					if (node.IsWalkable || node.Location == this.endNode.Location)
					{
						if (node.State != NodeState.Closed)
						{
							if (node.State == NodeState.Open)
							{
								float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
								float num = fromNode.G + traversalCost;
								if (num < node.G)
								{
									node.ParentNode = fromNode;
									list.Add(node);
								}
							}
							else
							{
								node.ParentNode = fromNode;
								node.State = NodeState.Open;
								list.Add(node);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000283C0 File Offset: 0x000267C0
		private static IEnumerable<Point2> GetAdjacentLocations(Point2 fromLocation)
		{
			return new Point2[]
			{
				new Point2(fromLocation.X - 1, fromLocation.Y),
				new Point2(fromLocation.X, fromLocation.Y + 1),
				new Point2(fromLocation.X + 1, fromLocation.Y),
				new Point2(fromLocation.X, fromLocation.Y - 1)
			};
		}

		// Token: 0x04000423 RID: 1059
		private int width;

		// Token: 0x04000424 RID: 1060
		private int height;

		// Token: 0x04000425 RID: 1061
		private Node[,] nodes;

		// Token: 0x04000426 RID: 1062
		private Node startNode;

		// Token: 0x04000427 RID: 1063
		private Node endNode;

		// Token: 0x04000428 RID: 1064
		private SearchParameters searchParameters;
	}
}
