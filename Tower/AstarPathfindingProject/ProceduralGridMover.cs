using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200009F RID: 159
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_procedural_grid_mover.php")]
	public class ProceduralGridMover : VersionedMonoBehaviour
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0002CFC9 File Offset: 0x0002B1C9
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0002CFD1 File Offset: 0x0002B1D1
		public bool updatingGraph { get; private set; }

		// Token: 0x0600075B RID: 1883 RVA: 0x0002CFDC File Offset: 0x0002B1DC
		private void Start()
		{
			if (AstarPath.active == null)
			{
				throw new Exception("There is no AstarPath object in the scene");
			}
			if (this.graph == null)
			{
				if (this.graphIndex < 0)
				{
					throw new Exception("Graph index should not be negative");
				}
				if (this.graphIndex >= AstarPath.active.data.graphs.Length)
				{
					throw new Exception(string.Concat(new string[]
					{
						"The ProceduralGridMover was configured to use graph index ",
						this.graphIndex.ToString(),
						", but only ",
						AstarPath.active.data.graphs.Length.ToString(),
						" graphs exist"
					}));
				}
				this.graph = (AstarPath.active.data.graphs[this.graphIndex] as GridGraph);
				if (this.graph == null)
				{
					throw new Exception("The ProceduralGridMover was configured to use graph index " + this.graphIndex.ToString() + " but that graph either does not exist or is not a GridGraph or LayerGridGraph");
				}
			}
			this.UpdateGraph();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0002D0DC File Offset: 0x0002B2DC
		private void Update()
		{
			if (this.graph == null)
			{
				return;
			}
			Vector3 a = this.PointToGraphSpace(this.graph.center);
			Vector3 b = this.PointToGraphSpace(this.target.position);
			if (VectorMath.SqrDistanceXZ(a, b) > this.updateDistance * this.updateDistance)
			{
				this.UpdateGraph();
			}
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0002D130 File Offset: 0x0002B330
		private Vector3 PointToGraphSpace(Vector3 p)
		{
			return this.graph.transform.InverseTransform(p);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0002D144 File Offset: 0x0002B344
		public void UpdateGraph()
		{
			if (this.updatingGraph)
			{
				return;
			}
			this.updatingGraph = true;
			IEnumerator ie = this.UpdateGraphCoroutine();
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext context, bool force)
			{
				if (force)
				{
					while (ie.MoveNext())
					{
					}
				}
				bool flag;
				try
				{
					flag = !ie.MoveNext();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, this);
					flag = true;
				}
				if (flag)
				{
					this.updatingGraph = false;
				}
				return flag;
			}));
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002D195 File Offset: 0x0002B395
		private IEnumerator UpdateGraphCoroutine()
		{
			Vector3 vector = this.PointToGraphSpace(this.target.position) - this.PointToGraphSpace(this.graph.center);
			vector.x = Mathf.Round(vector.x);
			vector.z = Mathf.Round(vector.z);
			vector.y = 0f;
			if (vector == Vector3.zero)
			{
				yield break;
			}
			Int2 offset = new Int2(-Mathf.RoundToInt(vector.x), -Mathf.RoundToInt(vector.z));
			this.graph.center += this.graph.transform.TransformVector(vector);
			this.graph.UpdateTransform();
			int width = this.graph.width;
			int depth = this.graph.depth;
			int layers = this.graph.LayerCount;
			LayerGridGraph layerGridGraph = this.graph as LayerGridGraph;
			GridNodeBase[] nodes;
			if (layerGridGraph != null)
			{
				nodes = layerGridGraph.nodes;
			}
			else
			{
				nodes = this.graph.nodes;
			}
			if (this.buffer == null || this.buffer.Length != width * depth)
			{
				this.buffer = new GridNodeBase[width * depth];
			}
			if (Mathf.Abs(offset.x) <= width && Mathf.Abs(offset.y) <= depth)
			{
				IntRect recalculateRect = new IntRect(0, 0, offset.x, offset.y);
				if (recalculateRect.xmin > recalculateRect.xmax)
				{
					int xmax = recalculateRect.xmax;
					recalculateRect.xmax = width + recalculateRect.xmin;
					recalculateRect.xmin = width + xmax;
				}
				if (recalculateRect.ymin > recalculateRect.ymax)
				{
					int ymax = recalculateRect.ymax;
					recalculateRect.ymax = depth + recalculateRect.ymin;
					recalculateRect.ymin = depth + ymax;
				}
				IntRect connectionRect = recalculateRect.Expand(1);
				connectionRect = IntRect.Intersection(connectionRect, new IntRect(0, 0, width, depth));
				int num7;
				for (int i = 0; i < layers; i = num7 + 1)
				{
					int layerOffset = i * width * depth;
					for (int j = 0; j < depth; j++)
					{
						int num = j * width;
						int num2 = (j + offset.y + depth) % depth * width;
						for (int k = 0; k < width; k++)
						{
							this.buffer[num2 + (k + offset.x + width) % width] = nodes[layerOffset + num + k];
						}
					}
					yield return null;
					for (int l = 0; l < depth; l++)
					{
						int num3 = l * width;
						for (int m = 0; m < width; m++)
						{
							int num4 = num3 + m;
							GridNodeBase gridNodeBase = this.buffer[num4];
							if (gridNodeBase != null)
							{
								gridNodeBase.NodeInGridIndex = num4;
							}
							nodes[layerOffset + num4] = gridNodeBase;
						}
						int num5;
						int num6;
						if (l >= recalculateRect.ymin && l < recalculateRect.ymax)
						{
							num5 = 0;
							num6 = depth;
						}
						else
						{
							num5 = recalculateRect.xmin;
							num6 = recalculateRect.xmax;
						}
						for (int n = num5; n < num6; n++)
						{
							GridNodeBase gridNodeBase2 = this.buffer[num3 + n];
							if (gridNodeBase2 != null)
							{
								gridNodeBase2.ClearConnections(false);
							}
						}
					}
					yield return null;
					num7 = i;
				}
				int yieldEvery = 1000;
				int num8 = Mathf.Max(Mathf.Abs(offset.x), Mathf.Abs(offset.y)) * Mathf.Max(width, depth);
				yieldEvery = Mathf.Max(yieldEvery, num8 / 10);
				int counter = 0;
				for (int i = 0; i < depth; i = num7 + 1)
				{
					int num9;
					int num10;
					if (i >= recalculateRect.ymin && i < recalculateRect.ymax)
					{
						num9 = 0;
						num10 = width;
					}
					else
					{
						num9 = recalculateRect.xmin;
						num10 = recalculateRect.xmax;
					}
					for (int num11 = num9; num11 < num10; num11++)
					{
						this.graph.RecalculateCell(num11, i, false, false);
					}
					counter += num10 - num9;
					if (counter > yieldEvery)
					{
						counter = 0;
						yield return null;
					}
					num7 = i;
				}
				for (int i = 0; i < depth; i = num7 + 1)
				{
					int num12;
					int num13;
					if (i >= connectionRect.ymin && i < connectionRect.ymax)
					{
						num12 = 0;
						num13 = width;
					}
					else
					{
						num12 = connectionRect.xmin;
						num13 = connectionRect.xmax;
					}
					for (int num14 = num12; num14 < num13; num14++)
					{
						this.graph.CalculateConnections(num14, i);
					}
					counter += num13 - num12;
					if (counter > yieldEvery)
					{
						counter = 0;
						yield return null;
					}
					num7 = i;
				}
				yield return null;
				for (int num15 = 0; num15 < depth; num15++)
				{
					for (int num16 = 0; num16 < width; num16++)
					{
						if (num16 == 0 || num15 == 0 || num16 == width - 1 || num15 == depth - 1)
						{
							this.graph.CalculateConnections(num16, num15);
						}
					}
				}
			}
			else
			{
				int counter = Mathf.Max(depth * width / 20, 1000);
				int yieldEvery = 0;
				int num7;
				for (int i = 0; i < depth; i = num7 + 1)
				{
					for (int num17 = 0; num17 < width; num17++)
					{
						this.graph.RecalculateCell(num17, i, true, true);
					}
					yieldEvery += width;
					if (yieldEvery > counter)
					{
						yieldEvery = 0;
						yield return null;
					}
					num7 = i;
				}
				for (int i = 0; i < depth; i = num7 + 1)
				{
					for (int num18 = 0; num18 < width; num18++)
					{
						this.graph.CalculateConnections(num18, i);
					}
					yieldEvery += width;
					if (yieldEvery > counter)
					{
						yieldEvery = 0;
						yield return null;
					}
					num7 = i;
				}
			}
			yield break;
		}

		// Token: 0x0400042B RID: 1067
		public float updateDistance = 10f;

		// Token: 0x0400042C RID: 1068
		public Transform target;

		// Token: 0x0400042D RID: 1069
		private GridNodeBase[] buffer;

		// Token: 0x0400042F RID: 1071
		public GridGraph graph;

		// Token: 0x04000430 RID: 1072
		[HideInInspector]
		public int graphIndex;
	}
}
