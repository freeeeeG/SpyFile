using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000061 RID: 97
	[Preserve]
	public class LayerGridGraph : GridGraph, IUpdatableGraph
	{
		// Token: 0x060004C0 RID: 1216 RVA: 0x000195D6 File Offset: 0x000177D6
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.RemoveGridGraphFromStatic();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000195E4 File Offset: 0x000177E4
		private void RemoveGridGraphFromStatic()
		{
			LevelGridNode.SetGridGraph(this.active.data.GetGraphIndex(this), null);
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x000195FD File Offset: 0x000177FD
		public override bool uniformWidthDepthGrid
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00019600 File Offset: 0x00017800
		public override int LayerCount
		{
			get
			{
				return this.layerCount;
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00019608 File Offset: 0x00017808
		public override int CountNodes()
		{
			if (this.nodes == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00019644 File Offset: 0x00017844
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					action(this.nodes[i]);
				}
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00019688 File Offset: 0x00017888
		protected override List<GraphNode> GetNodesInRegion(Bounds b, GraphUpdateShape shape)
		{
			IntRect rectFromBounds = base.GetRectFromBounds(b);
			if (this.nodes == null || !rectFromBounds.IsValid() || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				return ListPool<GraphNode>.Claim();
			}
			List<GraphNode> list = ListPool<GraphNode>.Claim(rectFromBounds.Width * rectFromBounds.Height * this.layerCount);
			for (int i = 0; i < this.layerCount; i++)
			{
				int num = i * this.width * this.depth;
				for (int j = rectFromBounds.xmin; j <= rectFromBounds.xmax; j++)
				{
					for (int k = rectFromBounds.ymin; k <= rectFromBounds.ymax; k++)
					{
						int num2 = num + k * this.width + j;
						GraphNode graphNode = this.nodes[num2];
						if (graphNode != null && b.Contains((Vector3)graphNode.position) && (shape == null || shape.Contains((Vector3)graphNode.position)))
						{
							list.Add(graphNode);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000197A0 File Offset: 0x000179A0
		public override List<GraphNode> GetNodesInRegion(IntRect rect)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			IntRect b = new IntRect(0, 0, this.width - 1, this.depth - 1);
			rect = IntRect.Intersection(rect, b);
			if (this.nodes == null || !rect.IsValid() || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				return list;
			}
			for (int i = 0; i < this.layerCount; i++)
			{
				int num = i * base.Width * base.Depth;
				for (int j = rect.ymin; j <= rect.ymax; j++)
				{
					int num2 = num + j * base.Width;
					for (int k = rect.xmin; k <= rect.xmax; k++)
					{
						GridNodeBase gridNodeBase = this.nodes[num2 + k];
						if (gridNodeBase != null)
						{
							list.Add(gridNodeBase);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00019884 File Offset: 0x00017A84
		public override int GetNodesInRegion(IntRect rect, GridNodeBase[] buffer)
		{
			IntRect b = new IntRect(0, 0, this.width - 1, this.depth - 1);
			rect = IntRect.Intersection(rect, b);
			if (this.nodes == null || !rect.IsValid() || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				return 0;
			}
			int num = 0;
			try
			{
				for (int i = 0; i < this.layerCount; i++)
				{
					int num2 = i * base.Width * base.Depth;
					for (int j = rect.ymin; j <= rect.ymax; j++)
					{
						int num3 = num2 + j * base.Width;
						for (int k = rect.xmin; k <= rect.xmax; k++)
						{
							GridNodeBase gridNodeBase = this.nodes[num3 + k];
							if (gridNodeBase != null)
							{
								buffer[num] = gridNodeBase;
								num++;
							}
						}
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException("Buffer is too small");
			}
			return num;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00019984 File Offset: 0x00017B84
		public override GridNodeBase GetNode(int x, int z)
		{
			if (x < 0 || z < 0 || x >= this.width || z >= this.depth)
			{
				return null;
			}
			return this.nodes[x + z * this.width];
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000199B4 File Offset: 0x00017BB4
		public GridNodeBase GetNode(int x, int z, int layer)
		{
			if (x < 0 || z < 0 || x >= this.width || z >= this.depth || layer < 0 || layer >= this.layerCount)
			{
				return null;
			}
			return this.nodes[x + z * this.width + layer * this.width * this.depth];
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00019A0C File Offset: 0x00017C0C
		void IUpdatableGraph.UpdateArea(GraphUpdateObject o)
		{
			if (this.nodes == null || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				Debug.LogWarning("The Grid Graph is not scanned, cannot update area ");
				return;
			}
			IntRect a;
			IntRect a2;
			IntRect intRect;
			bool flag;
			int num;
			base.CalculateAffectedRegions(o, out a, out a2, out intRect, out flag, out num);
			bool flag2 = o is LayerGridGraphUpdate && ((LayerGridGraphUpdate)o).recalculateNodes;
			bool flag3 = (o is LayerGridGraphUpdate) ? ((LayerGridGraphUpdate)o).preserveExistingNodes : (!o.resetPenaltyOnPhysics);
			if (o.trackChangedNodes && flag2)
			{
				Debug.LogError("Cannot track changed nodes when creating or deleting nodes.\nWill not update LayerGridGraph");
				return;
			}
			IntRect b = new IntRect(0, 0, this.width - 1, this.depth - 1);
			IntRect intRect2 = IntRect.Intersection(a2, b);
			if (!flag2)
			{
				for (int i = intRect2.xmin; i <= intRect2.xmax; i++)
				{
					for (int j = intRect2.ymin; j <= intRect2.ymax; j++)
					{
						for (int k = 0; k < this.layerCount; k++)
						{
							o.WillUpdateNode(this.nodes[k * this.width * this.depth + j * this.width + i]);
						}
					}
				}
			}
			if (o.updatePhysics && !o.modifyWalkability)
			{
				this.collision.Initialize(base.transform, this.nodeSize);
				intRect2 = IntRect.Intersection(intRect, b);
				for (int l = intRect2.xmin; l <= intRect2.xmax; l++)
				{
					for (int m = intRect2.ymin; m <= intRect2.ymax; m++)
					{
						this.RecalculateCell(l, m, !flag3, false);
					}
				}
				for (int n = intRect2.xmin; n <= intRect2.xmax; n++)
				{
					for (int num2 = intRect2.ymin; num2 <= intRect2.ymax; num2++)
					{
						this.CalculateConnections(n, num2);
					}
				}
			}
			intRect2 = IntRect.Intersection(a, b);
			for (int num3 = intRect2.xmin; num3 <= intRect2.xmax; num3++)
			{
				for (int num4 = intRect2.ymin; num4 <= intRect2.ymax; num4++)
				{
					for (int num5 = 0; num5 < this.layerCount; num5++)
					{
						int num6 = num5 * this.width * this.depth + num4 * this.width + num3;
						GridNodeBase gridNodeBase = this.nodes[num6];
						if (gridNodeBase != null)
						{
							if (flag)
							{
								gridNodeBase.Walkable = gridNodeBase.WalkableErosion;
								if (o.bounds.Contains((Vector3)gridNodeBase.position))
								{
									o.Apply(gridNodeBase);
								}
								gridNodeBase.WalkableErosion = gridNodeBase.Walkable;
							}
							else if (o.bounds.Contains((Vector3)gridNodeBase.position))
							{
								o.Apply(gridNodeBase);
							}
						}
					}
				}
			}
			if (flag && num == 0)
			{
				intRect2 = IntRect.Intersection(a2, b);
				for (int num7 = intRect2.xmin; num7 <= intRect2.xmax; num7++)
				{
					for (int num8 = intRect2.ymin; num8 <= intRect2.ymax; num8++)
					{
						this.CalculateConnections(num7, num8);
					}
				}
				return;
			}
			if (flag && num > 0)
			{
				IntRect a3 = IntRect.Union(a, intRect).Expand(num);
				IntRect intRect3 = a3.Expand(num);
				a3 = IntRect.Intersection(a3, b);
				intRect3 = IntRect.Intersection(intRect3, b);
				for (int num9 = intRect3.xmin; num9 <= intRect3.xmax; num9++)
				{
					for (int num10 = intRect3.ymin; num10 <= intRect3.ymax; num10++)
					{
						for (int num11 = 0; num11 < this.layerCount; num11++)
						{
							int num12 = num11 * this.width * this.depth + num10 * this.width + num9;
							GridNodeBase gridNodeBase2 = this.nodes[num12];
							if (gridNodeBase2 != null)
							{
								bool walkable = gridNodeBase2.Walkable;
								gridNodeBase2.Walkable = gridNodeBase2.WalkableErosion;
								if (!a3.Contains(num9, num10))
								{
									gridNodeBase2.TmpWalkable = walkable;
								}
							}
						}
					}
				}
				for (int num13 = intRect3.xmin; num13 <= intRect3.xmax; num13++)
				{
					for (int num14 = intRect3.ymin; num14 <= intRect3.ymax; num14++)
					{
						this.CalculateConnections(num13, num14);
					}
				}
				base.ErodeWalkableArea(intRect3.xmin, intRect3.ymin, intRect3.xmax + 1, intRect3.ymax + 1);
				for (int num15 = intRect3.xmin; num15 <= intRect3.xmax; num15++)
				{
					for (int num16 = intRect3.ymin; num16 <= intRect3.ymax; num16++)
					{
						if (!a3.Contains(num15, num16))
						{
							for (int num17 = 0; num17 < this.layerCount; num17++)
							{
								int num18 = num17 * this.width * this.depth + num16 * this.width + num15;
								GridNodeBase gridNodeBase3 = this.nodes[num18];
								if (gridNodeBase3 != null)
								{
									gridNodeBase3.Walkable = gridNodeBase3.TmpWalkable;
								}
							}
						}
					}
				}
				for (int num19 = intRect3.xmin; num19 <= intRect3.xmax; num19++)
				{
					for (int num20 = intRect3.ymin; num20 <= intRect3.ymax; num20++)
					{
						this.CalculateConnections(num19, num20);
					}
				}
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00019F90 File Offset: 0x00018190
		protected override IEnumerable<Progress> ScanInternal()
		{
			if (this.nodeSize <= 0f)
			{
				yield break;
			}
			base.UpdateTransform();
			if (this.width > 1024 || this.depth > 1024)
			{
				Debug.LogError("One of the grid's sides is longer than 1024 nodes");
				yield break;
			}
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			this.SetUpOffsetsAndCosts();
			LevelGridNode.SetGridGraph((int)this.graphIndex, this);
			this.maxClimb = Mathf.Clamp(this.maxClimb, 0f, this.characterHeight);
			this.collision = (this.collision ?? new GraphCollision());
			this.collision.Initialize(base.transform, this.nodeSize);
			int progressCounter = 0;
			this.layerCount = 1;
			GridNodeBase[] nodes = new LevelGridNode[this.width * this.depth * this.layerCount];
			this.nodes = nodes;
			int num;
			for (int z = 0; z < this.depth; z = num + 1)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new Progress(Mathf.Lerp(0f, 0.8f, (float)z / (float)this.depth), "Creating nodes");
				}
				progressCounter += this.width;
				for (int i = 0; i < this.width; i++)
				{
					this.RecalculateCell(i, z, true, true);
				}
				num = z;
			}
			for (int z = 0; z < this.depth; z = num + 1)
			{
				if (progressCounter >= 1000)
				{
					progressCounter = 0;
					yield return new Progress(Mathf.Lerp(0.8f, 0.9f, (float)z / (float)this.depth), "Calculating connections");
				}
				progressCounter += this.width;
				for (int j = 0; j < this.width; j++)
				{
					this.CalculateConnections(j, z);
				}
				num = z;
			}
			yield return new Progress(0.95f, "Calculating Erosion");
			for (int k = 0; k < this.nodes.Length; k++)
			{
				LevelGridNode levelGridNode = this.nodes[k] as LevelGridNode;
				if (levelGridNode != null && !levelGridNode.HasAnyGridConnections())
				{
					levelGridNode.Walkable = false;
					levelGridNode.WalkableErosion = levelGridNode.Walkable;
				}
			}
			this.ErodeWalkableArea();
			yield break;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00019FA0 File Offset: 0x000181A0
		protected static LayerGridGraph.HeightSample[] SampleHeights(GraphCollision collision, float mergeSpanRange, Vector3 position, out int numHits)
		{
			int num;
			RaycastHit[] array = collision.CheckHeightAll(position, out num);
			Array.Sort<RaycastHit>(array, 0, num, LayerGridGraph.comparer);
			if (num > LayerGridGraph.heightSampleBuffer.Length)
			{
				LayerGridGraph.heightSampleBuffer = new LayerGridGraph.HeightSample[Mathf.Max(LayerGridGraph.heightSampleBuffer.Length * 2, num)];
			}
			LayerGridGraph.HeightSample[] array2 = LayerGridGraph.heightSampleBuffer;
			if (num == 0)
			{
				LayerGridGraph.HeightSample[] array3 = array2;
				int num2 = 0;
				LayerGridGraph.HeightSample heightSample = new LayerGridGraph.HeightSample
				{
					position = position,
					height = float.PositiveInfinity,
					walkable = (!collision.unwalkableWhenNoGround && collision.Check(position))
				};
				array3[num2] = heightSample;
				numHits = 1;
				return array2;
			}
			int num3 = 0;
			for (int i = num - 1; i >= 0; i--)
			{
				if (i > 0 && array[i].distance - array[i - 1].distance <= mergeSpanRange)
				{
					i--;
				}
				LayerGridGraph.HeightSample[] array4 = array2;
				int num4 = num3;
				LayerGridGraph.HeightSample heightSample = new LayerGridGraph.HeightSample
				{
					position = array[i].point,
					hit = array[i],
					walkable = collision.Check(array[i].point),
					height = ((i > 0) ? (array[i].distance - array[i - 1].distance) : float.PositiveInfinity)
				};
				array4[num4] = heightSample;
				num3++;
			}
			numHits = num3;
			return array2;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001A104 File Offset: 0x00018304
		public override void RecalculateCell(int x, int z, bool resetPenalties = true, bool resetTags = true)
		{
			float num = Mathf.Cos(this.maxSlope * 0.017453292f);
			int num2;
			LayerGridGraph.HeightSample[] array = LayerGridGraph.SampleHeights(this.collision, this.mergeSpanRange, base.transform.Transform(new Vector3((float)x + 0.5f, 0f, (float)z + 0.5f)), out num2);
			if (num2 > this.layerCount)
			{
				if (num2 > 255)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Too many layers, a maximum of ",
						255.ToString(),
						" are allowed (required ",
						num2.ToString(),
						")"
					}));
					return;
				}
				this.AddLayers(num2 - this.layerCount);
			}
			int i;
			for (i = 0; i < num2; i++)
			{
				LayerGridGraph.HeightSample heightSample = array[i];
				int num3 = z * this.width + x + this.width * this.depth * i;
				LevelGridNode levelGridNode = this.nodes[num3] as LevelGridNode;
				bool flag = levelGridNode == null;
				if (flag)
				{
					if (this.nodes[num3] != null)
					{
						this.nodes[num3].Destroy();
					}
					levelGridNode = new LevelGridNode(this.active);
					this.nodes[num3] = levelGridNode;
					levelGridNode.NodeInGridIndex = z * this.width + x;
					levelGridNode.LayerCoordinateInGrid = i;
					levelGridNode.GraphIndex = this.graphIndex;
				}
				levelGridNode.position = (Int3)heightSample.position;
				levelGridNode.Walkable = heightSample.walkable;
				levelGridNode.WalkableErosion = levelGridNode.Walkable;
				if (flag || resetPenalties)
				{
					levelGridNode.Penalty = this.initialPenalty;
					if (this.penaltyPosition)
					{
						levelGridNode.Penalty += (uint)Mathf.RoundToInt(((float)levelGridNode.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
					}
				}
				if (flag || resetTags)
				{
					levelGridNode.Tag = 0U;
				}
				if (heightSample.hit.normal != Vector3.zero && (this.penaltyAngle || num > 0.0001f))
				{
					float num4 = Vector3.Dot(heightSample.hit.normal.normalized, this.collision.up);
					if (resetTags && this.penaltyAngle)
					{
						levelGridNode.Penalty += (uint)Mathf.RoundToInt((1f - num4) * this.penaltyAngleFactor);
					}
					if (num4 < num)
					{
						levelGridNode.Walkable = false;
					}
				}
				if (heightSample.height < this.characterHeight)
				{
					levelGridNode.Walkable = false;
				}
				levelGridNode.WalkableErosion = levelGridNode.Walkable;
			}
			while (i < this.layerCount)
			{
				int num5 = z * this.width + x + this.width * this.depth * i;
				if (this.nodes[num5] != null)
				{
					this.nodes[num5].Destroy();
				}
				this.nodes[num5] = null;
				i++;
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001A3E8 File Offset: 0x000185E8
		private void AddLayers(int count)
		{
			int num = this.layerCount + count;
			if (num > 255)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Too many layers, a maximum of ",
					255.ToString(),
					" are allowed (required ",
					num.ToString(),
					")"
				}));
				return;
			}
			Array nodes = this.nodes;
			this.nodes = new GridNodeBase[this.width * this.depth * num];
			nodes.CopyTo(this.nodes, 0);
			this.layerCount = num;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001A47C File Offset: 0x0001867C
		protected override bool ErosionAnyFalseConnections(GraphNode baseNode)
		{
			LevelGridNode levelGridNode = baseNode as LevelGridNode;
			if (this.neighbours == NumNeighbours.Six)
			{
				for (int i = 0; i < 6; i++)
				{
					if (!levelGridNode.HasConnectionInDirection(GridGraph.hexagonNeighbourIndices[i]))
					{
						return true;
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					if (!levelGridNode.HasConnectionInDirection(j))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001A4D0 File Offset: 0x000186D0
		public override void CalculateConnections(GridNodeBase baseNode)
		{
			LevelGridNode levelGridNode = baseNode as LevelGridNode;
			this.CalculateConnections(levelGridNode.XCoordinateInGrid, levelGridNode.ZCoordinateInGrid, levelGridNode.LayerCoordinateInGrid);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001A4FC File Offset: 0x000186FC
		[Obsolete("Use CalculateConnections(x,z,layerIndex) or CalculateConnections(node) instead")]
		public void CalculateConnections(int x, int z, int layerIndex, LevelGridNode node)
		{
			this.CalculateConnections(x, z, layerIndex);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001A508 File Offset: 0x00018708
		public override void CalculateConnections(int x, int z)
		{
			for (int i = 0; i < this.layerCount; i++)
			{
				this.CalculateConnections(x, z, i);
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001A530 File Offset: 0x00018730
		public void CalculateConnections(int x, int z, int layerIndex)
		{
			LevelGridNode levelGridNode = this.nodes[z * this.width + x + this.width * this.depth * layerIndex] as LevelGridNode;
			if (levelGridNode == null)
			{
				return;
			}
			levelGridNode.ResetAllGridConnections();
			if (!levelGridNode.Walkable)
			{
				return;
			}
			Vector3 vector = (Vector3)levelGridNode.position;
			Vector3 rhs = base.transform.WorldUpAtGraphPosition(vector);
			float num = Vector3.Dot(vector, rhs);
			float num2;
			if (layerIndex == this.layerCount - 1 || this.nodes[levelGridNode.NodeInGridIndex + this.width * this.depth * (layerIndex + 1)] == null)
			{
				num2 = float.PositiveInfinity;
			}
			else
			{
				num2 = Math.Abs(num - Vector3.Dot((Vector3)this.nodes[levelGridNode.NodeInGridIndex + this.width * this.depth * (layerIndex + 1)].position, rhs));
			}
			for (int i = 0; i < 4; i++)
			{
				int num3 = x + this.neighbourXOffsets[i];
				int num4 = z + this.neighbourZOffsets[i];
				if (num3 >= 0 && num4 >= 0 && num3 < this.width && num4 < this.depth)
				{
					int num5 = num4 * this.width + num3;
					int value = 255;
					for (int j = 0; j < this.layerCount; j++)
					{
						GraphNode graphNode = this.nodes[num5 + this.width * this.depth * j];
						if (graphNode != null && graphNode.Walkable)
						{
							float num6 = Vector3.Dot((Vector3)graphNode.position, rhs);
							float num7;
							if (j == this.layerCount - 1 || this.nodes[num5 + this.width * this.depth * (j + 1)] == null)
							{
								num7 = float.PositiveInfinity;
							}
							else
							{
								num7 = Math.Abs(num6 - Vector3.Dot((Vector3)this.nodes[num5 + this.width * this.depth * (j + 1)].position, rhs));
							}
							float num8 = Mathf.Max(num6, num);
							if (Mathf.Min(num6 + num7, num + num2) - num8 >= this.characterHeight && Mathf.Abs(num6 - num) <= this.maxClimb)
							{
								value = j;
							}
						}
					}
					levelGridNode.SetConnectionValue(i, value);
				}
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001A780 File Offset: 0x00018980
		public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
			{
				return default(NNInfoInternal);
			}
			Vector3 vector = base.transform.InverseTransform(position);
			float x = vector.x;
			float z = vector.z;
			int num = Mathf.Clamp((int)x, 0, this.width - 1);
			int num2 = Mathf.Clamp((int)z, 0, this.depth - 1);
			GridNodeBase nearestNode = this.GetNearestNode(position, num, num2, null);
			NNInfoInternal result = new NNInfoInternal(nearestNode);
			float y = base.transform.InverseTransform((Vector3)nearestNode.position).y;
			result.clampedPosition = base.transform.Transform(new Vector3(Mathf.Clamp(x, (float)num, (float)num + 1f), y, Mathf.Clamp(z, (float)num2, (float)num2 + 1f)));
			return result;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001A868 File Offset: 0x00018A68
		protected override GridNodeBase GetNearestFromGraphSpace(Vector3 positionGraphSpace)
		{
			if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
			{
				return null;
			}
			int x = (int)positionGraphSpace.x;
			float z = positionGraphSpace.z;
			int x2 = Mathf.Clamp(x, 0, this.width - 1);
			int z2 = Mathf.Clamp((int)z, 0, this.depth - 1);
			Vector3 position = base.transform.Transform(positionGraphSpace);
			return this.GetNearestNode(position, x2, z2, null);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001A8E4 File Offset: 0x00018AE4
		private GridNodeBase GetNearestNode(Vector3 position, int x, int z, NNConstraint constraint)
		{
			int num = this.width * z + x;
			float num2 = float.PositiveInfinity;
			GridNodeBase result = null;
			for (int i = 0; i < this.layerCount; i++)
			{
				GridNodeBase gridNodeBase = this.nodes[num + this.width * this.depth * i];
				if (gridNodeBase != null)
				{
					float sqrMagnitude = ((Vector3)gridNodeBase.position - position).sqrMagnitude;
					if (sqrMagnitude < num2 && (constraint == null || constraint.Suitable(gridNodeBase)))
					{
						num2 = sqrMagnitude;
						result = gridNodeBase;
					}
				}
			}
			return result;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001A96B File Offset: 0x00018B6B
		[Obsolete("Use node.HasConnectionInDirection instead")]
		public static bool CheckConnection(LevelGridNode node, int dir)
		{
			return node.HasConnectionInDirection(dir);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001A974 File Offset: 0x00018B74
		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(this.nodes.Length);
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001A9F0 File Offset: 0x00018BF0
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			GridNodeBase[] nodes = new LevelGridNode[num];
			this.nodes = nodes;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = new LevelGridNode(this.active);
					this.nodes[i].DeserializeNode(ctx);
				}
				else
				{
					this.nodes[i] = null;
				}
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001AA70 File Offset: 0x00018C70
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			base.UpdateTransform();
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			this.SetUpOffsetsAndCosts();
			LevelGridNode.SetGridGraph((int)this.graphIndex, this);
			if (this.nodes == null || this.nodes.Length == 0)
			{
				return;
			}
			if (this.width * this.depth * this.layerCount != this.nodes.Length)
			{
				Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
				GridNodeBase[] nodes = new LevelGridNode[0];
				this.nodes = nodes;
				return;
			}
			for (int i = 0; i < this.layerCount; i++)
			{
				for (int j = 0; j < this.depth; j++)
				{
					for (int k = 0; k < this.width; k++)
					{
						LevelGridNode levelGridNode = this.nodes[j * this.width + k + this.width * this.depth * i] as LevelGridNode;
						if (levelGridNode != null)
						{
							levelGridNode.NodeInGridIndex = j * this.width + k;
							levelGridNode.LayerCoordinateInGrid = i;
						}
					}
				}
			}
		}

		// Token: 0x040002DE RID: 734
		[JsonMember]
		internal int layerCount;

		// Token: 0x040002DF RID: 735
		[JsonMember]
		public float mergeSpanRange = 0.5f;

		// Token: 0x040002E0 RID: 736
		[JsonMember]
		public float characterHeight = 0.4f;

		// Token: 0x040002E1 RID: 737
		internal int lastScannedWidth;

		// Token: 0x040002E2 RID: 738
		internal int lastScannedDepth;

		// Token: 0x040002E3 RID: 739
		private static readonly LayerGridGraph.HitComparer comparer = new LayerGridGraph.HitComparer();

		// Token: 0x040002E4 RID: 740
		private static LayerGridGraph.HeightSample[] heightSampleBuffer = new LayerGridGraph.HeightSample[4];

		// Token: 0x0200012C RID: 300
		protected struct HeightSample
		{
			// Token: 0x04000707 RID: 1799
			public Vector3 position;

			// Token: 0x04000708 RID: 1800
			public RaycastHit hit;

			// Token: 0x04000709 RID: 1801
			public float height;

			// Token: 0x0400070A RID: 1802
			public bool walkable;
		}

		// Token: 0x0200012D RID: 301
		private class HitComparer : IComparer<RaycastHit>
		{
			// Token: 0x06000ACC RID: 2764 RVA: 0x0004482C File Offset: 0x00042A2C
			public int Compare(RaycastHit a, RaycastHit b)
			{
				return a.distance.CompareTo(b.distance);
			}
		}
	}
}
