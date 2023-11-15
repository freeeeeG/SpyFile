using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200006C RID: 108
	[JsonOptIn]
	[Preserve]
	public class PointGraph : NavGraph, IUpdatableGraph
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001FEE9 File Offset: 0x0001E0E9
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0001FEF1 File Offset: 0x0001E0F1
		public int nodeCount { get; protected set; }

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001FEFA File Offset: 0x0001E0FA
		public override int CountNodes()
		{
			return this.nodeCount;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001FF04 File Offset: 0x0001E104
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			int nodeCount = this.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001FF3B File Offset: 0x0001E13B
		public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			return this.GetNearestInternal(position, constraint, true);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001FF46 File Offset: 0x0001E146
		public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return this.GetNearestInternal(position, constraint, false);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001FF54 File Offset: 0x0001E154
		private NNInfoInternal GetNearestInternal(Vector3 position, NNConstraint constraint, bool fastCheck)
		{
			if (this.nodes == null)
			{
				return default(NNInfoInternal);
			}
			Int3 @int = (Int3)position;
			if (!this.optimizeForSparseGraph)
			{
				float num = (constraint == null || constraint.constrainDistance) ? AstarPath.active.maxNearestNodeDistanceSqr : float.PositiveInfinity;
				num *= 1000000f;
				NNInfoInternal nninfoInternal = new NNInfoInternal(null);
				long num2 = long.MaxValue;
				long num3 = long.MaxValue;
				for (int i = 0; i < this.nodeCount; i++)
				{
					PointNode pointNode = this.nodes[i];
					long sqrMagnitudeLong = (@int - pointNode.position).sqrMagnitudeLong;
					if (sqrMagnitudeLong < num2)
					{
						num2 = sqrMagnitudeLong;
						nninfoInternal.node = pointNode;
					}
					if (sqrMagnitudeLong < num3 && (float)sqrMagnitudeLong < num && (constraint == null || constraint.Suitable(pointNode)))
					{
						num3 = sqrMagnitudeLong;
						nninfoInternal.constrainedNode = pointNode;
					}
				}
				if (!fastCheck)
				{
					nninfoInternal.node = nninfoInternal.constrainedNode;
				}
				nninfoInternal.UpdateInfo();
				return nninfoInternal;
			}
			if (this.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Node)
			{
				return new NNInfoInternal(this.lookupTree.GetNearest(@int, fastCheck ? null : constraint));
			}
			GraphNode nearestConnection = this.lookupTree.GetNearestConnection(@int, fastCheck ? null : constraint, this.maximumConnectionLength);
			if (nearestConnection == null)
			{
				return default(NNInfoInternal);
			}
			return this.FindClosestConnectionPoint(nearestConnection as PointNode, position);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000200A8 File Offset: 0x0001E2A8
		private NNInfoInternal FindClosestConnectionPoint(PointNode node, Vector3 position)
		{
			Vector3 clampedPosition = (Vector3)node.position;
			Connection[] connections = node.connections;
			Vector3 vector = (Vector3)node.position;
			float num = float.PositiveInfinity;
			if (connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					Vector3 lineEnd = ((Vector3)connections[i].node.position + vector) * 0.5f;
					Vector3 vector2 = VectorMath.ClosestPointOnSegment(vector, lineEnd, position);
					float sqrMagnitude = (vector2 - position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						clampedPosition = vector2;
					}
				}
			}
			return new NNInfoInternal
			{
				node = node,
				clampedPosition = clampedPosition
			};
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002015B File Offset: 0x0001E35B
		public PointNode AddNode(Int3 position)
		{
			return this.AddNode<PointNode>(new PointNode(this.active), position);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00020170 File Offset: 0x0001E370
		public T AddNode<T>(T node, Int3 position) where T : PointNode
		{
			if (this.nodes == null || this.nodeCount == this.nodes.Length)
			{
				PointNode[] array = new PointNode[(this.nodes != null) ? Math.Max(this.nodes.Length + 4, this.nodes.Length * 2) : 4];
				if (this.nodes != null)
				{
					this.nodes.CopyTo(array, 0);
				}
				this.nodes = array;
			}
			node.SetPosition(position);
			node.GraphIndex = this.graphIndex;
			node.Walkable = true;
			this.nodes[this.nodeCount] = node;
			int nodeCount = this.nodeCount;
			this.nodeCount = nodeCount + 1;
			if (this.optimizeForSparseGraph)
			{
				this.AddToLookup(node);
			}
			return node;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0002023C File Offset: 0x0001E43C
		protected static int CountChildren(Transform tr)
		{
			int num = 0;
			foreach (object obj in tr)
			{
				Transform tr2 = (Transform)obj;
				num++;
				num += PointGraph.CountChildren(tr2);
			}
			return num;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0002029C File Offset: 0x0001E49C
		protected void AddChildren(ref int c, Transform tr)
		{
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				this.nodes[c].position = (Int3)transform.position;
				this.nodes[c].Walkable = true;
				this.nodes[c].gameObject = transform.gameObject;
				c++;
				this.AddChildren(ref c, transform);
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00020334 File Offset: 0x0001E534
		public void RebuildNodeLookup()
		{
			if (!this.optimizeForSparseGraph || this.nodes == null)
			{
				this.lookupTree = new PointKDTree();
			}
			else
			{
				PointKDTree pointKDTree = this.lookupTree;
				GraphNode[] array = this.nodes;
				pointKDTree.Rebuild(array, 0, this.nodeCount);
			}
			this.RebuildConnectionDistanceLookup();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00020380 File Offset: 0x0001E580
		public void RebuildConnectionDistanceLookup()
		{
			this.maximumConnectionLength = 0L;
			if (this.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Connection)
			{
				for (int i = 0; i < this.nodeCount; i++)
				{
					PointNode pointNode = this.nodes[i];
					Connection[] connections = pointNode.connections;
					if (connections != null)
					{
						for (int j = 0; j < connections.Length; j++)
						{
							long sqrMagnitudeLong = (pointNode.position - connections[j].node.position).sqrMagnitudeLong;
							this.RegisterConnectionLength(sqrMagnitudeLong);
						}
					}
				}
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000203FF File Offset: 0x0001E5FF
		private void AddToLookup(PointNode node)
		{
			this.lookupTree.Add(node);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0002040D File Offset: 0x0001E60D
		public void RegisterConnectionLength(long sqrLength)
		{
			this.maximumConnectionLength = Math.Max(this.maximumConnectionLength, sqrLength);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00020424 File Offset: 0x0001E624
		protected virtual PointNode[] CreateNodes(int count)
		{
			PointNode[] array = new PointNode[count];
			for (int i = 0; i < this.nodeCount; i++)
			{
				array[i] = new PointNode(this.active);
			}
			return array;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00020458 File Offset: 0x0001E658
		protected override IEnumerable<Progress> ScanInternal()
		{
			yield return new Progress(0f, "Searching for GameObjects");
			if (this.root == null)
			{
				GameObject[] gos = (this.searchTag != null) ? GameObject.FindGameObjectsWithTag(this.searchTag) : null;
				if (gos == null)
				{
					this.nodes = new PointNode[0];
					this.nodeCount = 0;
				}
				else
				{
					yield return new Progress(0.1f, "Creating nodes");
					this.nodeCount = gos.Length;
					this.nodes = this.CreateNodes(this.nodeCount);
					for (int i = 0; i < gos.Length; i++)
					{
						this.nodes[i].position = (Int3)gos[i].transform.position;
						this.nodes[i].Walkable = true;
						this.nodes[i].gameObject = gos[i].gameObject;
					}
				}
				gos = null;
			}
			else
			{
				if (!this.recursive)
				{
					this.nodeCount = this.root.childCount;
					this.nodes = this.CreateNodes(this.nodeCount);
					int num = 0;
					using (IEnumerator enumerator = this.root.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Transform transform = (Transform)obj;
							this.nodes[num].position = (Int3)transform.position;
							this.nodes[num].Walkable = true;
							this.nodes[num].gameObject = transform.gameObject;
							num++;
						}
						goto IL_24C;
					}
				}
				this.nodeCount = PointGraph.CountChildren(this.root);
				this.nodes = this.CreateNodes(this.nodeCount);
				int num2 = 0;
				this.AddChildren(ref num2, this.root);
			}
			IL_24C:
			yield return new Progress(0.15f, "Building node lookup");
			this.RebuildNodeLookup();
			foreach (Progress progress in this.ConnectNodesAsync())
			{
				yield return progress.MapTo(0.15f, 0.95f, null);
			}
			IEnumerator<Progress> enumerator2 = null;
			yield return new Progress(0.95f, "Building connection distances");
			this.RebuildConnectionDistanceLookup();
			yield break;
			yield break;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00020468 File Offset: 0x0001E668
		public void ConnectNodes()
		{
			IEnumerator<Progress> enumerator = this.ConnectNodesAsync().GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
			this.RebuildConnectionDistanceLookup();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0002048F File Offset: 0x0001E68F
		private IEnumerable<Progress> ConnectNodesAsync()
		{
			if (this.maxDistance >= 0f)
			{
				List<Connection> connections = new List<Connection>();
				List<GraphNode> candidateConnections = new List<GraphNode>();
				long maxSquaredRange;
				if (this.maxDistance == 0f && (this.limits.x == 0f || this.limits.y == 0f || this.limits.z == 0f))
				{
					maxSquaredRange = long.MaxValue;
				}
				else
				{
					maxSquaredRange = (long)(Mathf.Max(this.limits.x, Mathf.Max(this.limits.y, Mathf.Max(this.limits.z, this.maxDistance))) * 1000f) + 1L;
					maxSquaredRange *= maxSquaredRange;
				}
				int num3;
				for (int i = 0; i < this.nodeCount; i = num3 + 1)
				{
					if (i % 512 == 0)
					{
						yield return new Progress((float)i / (float)this.nodeCount, "Connecting nodes");
					}
					connections.Clear();
					PointNode pointNode = this.nodes[i];
					if (this.optimizeForSparseGraph)
					{
						candidateConnections.Clear();
						this.lookupTree.GetInRange(pointNode.position, maxSquaredRange, candidateConnections);
						for (int j = 0; j < candidateConnections.Count; j++)
						{
							PointNode pointNode2 = candidateConnections[j] as PointNode;
							float num;
							if (pointNode2 != pointNode && this.IsValidConnection(pointNode, pointNode2, out num))
							{
								connections.Add(new Connection(pointNode2, (uint)Mathf.RoundToInt(num * 1000f), byte.MaxValue));
							}
						}
					}
					else
					{
						for (int k = 0; k < this.nodeCount; k++)
						{
							if (i != k)
							{
								PointNode pointNode3 = this.nodes[k];
								float num2;
								if (this.IsValidConnection(pointNode, pointNode3, out num2))
								{
									connections.Add(new Connection(pointNode3, (uint)Mathf.RoundToInt(num2 * 1000f), byte.MaxValue));
								}
							}
						}
					}
					pointNode.connections = connections.ToArray();
					pointNode.SetConnectivityDirty();
					num3 = i;
				}
				connections = null;
				candidateConnections = null;
			}
			yield break;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000204A0 File Offset: 0x0001E6A0
		public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
		{
			dist = 0f;
			if (!a.Walkable || !b.Walkable)
			{
				return false;
			}
			Vector3 vector = (Vector3)(b.position - a.position);
			if ((!Mathf.Approximately(this.limits.x, 0f) && Mathf.Abs(vector.x) > this.limits.x) || (!Mathf.Approximately(this.limits.y, 0f) && Mathf.Abs(vector.y) > this.limits.y) || (!Mathf.Approximately(this.limits.z, 0f) && Mathf.Abs(vector.z) > this.limits.z))
			{
				return false;
			}
			dist = vector.magnitude;
			if (this.maxDistance != 0f && dist >= this.maxDistance)
			{
				return false;
			}
			if (!this.raycast)
			{
				return true;
			}
			Ray ray = new Ray((Vector3)a.position, vector);
			Ray ray2 = new Ray((Vector3)b.position, -vector);
			if (this.use2DPhysics)
			{
				if (this.thickRaycast)
				{
					return !Physics2D.CircleCast(ray.origin, this.thickRaycastRadius, ray.direction, dist, this.mask) && !Physics2D.CircleCast(ray2.origin, this.thickRaycastRadius, ray2.direction, dist, this.mask);
				}
				return !Physics2D.Linecast((Vector3)a.position, (Vector3)b.position, this.mask) && !Physics2D.Linecast((Vector3)b.position, (Vector3)a.position, this.mask);
			}
			else
			{
				if (this.thickRaycast)
				{
					return !Physics.SphereCast(ray, this.thickRaycastRadius, dist, this.mask) && !Physics.SphereCast(ray2, this.thickRaycastRadius, dist, this.mask);
				}
				return !Physics.Linecast((Vector3)a.position, (Vector3)b.position, this.mask) && !Physics.Linecast((Vector3)b.position, (Vector3)a.position, this.mask);
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00020755 File Offset: 0x0001E955
		GraphUpdateThreading IUpdatableGraph.CanUpdateAsync(GraphUpdateObject o)
		{
			return GraphUpdateThreading.UnityThread;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00020758 File Offset: 0x0001E958
		void IUpdatableGraph.UpdateAreaInit(GraphUpdateObject o)
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002075A File Offset: 0x0001E95A
		void IUpdatableGraph.UpdateAreaPost(GraphUpdateObject o)
		{
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0002075C File Offset: 0x0001E95C
		void IUpdatableGraph.UpdateArea(GraphUpdateObject guo)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodeCount; i++)
			{
				PointNode pointNode = this.nodes[i];
				if (guo.bounds.Contains((Vector3)pointNode.position))
				{
					guo.WillUpdateNode(pointNode);
					guo.Apply(pointNode);
				}
			}
			if (guo.updatePhysics)
			{
				Bounds bounds = guo.bounds;
				if (this.thickRaycast)
				{
					bounds.Expand(this.thickRaycastRadius * 2f);
				}
				List<Connection> list = ListPool<Connection>.Claim();
				for (int j = 0; j < this.nodeCount; j++)
				{
					PointNode pointNode2 = this.nodes[j];
					Vector3 a = (Vector3)pointNode2.position;
					List<Connection> list2 = null;
					for (int k = 0; k < this.nodeCount; k++)
					{
						if (k != j)
						{
							Vector3 b = (Vector3)this.nodes[k].position;
							if (VectorMath.SegmentIntersectsBounds(bounds, a, b))
							{
								PointNode pointNode3 = this.nodes[k];
								bool flag = pointNode2.ContainsConnection(pointNode3);
								float num;
								bool flag2 = this.IsValidConnection(pointNode2, pointNode3, out num);
								if (list2 == null && flag != flag2)
								{
									list.Clear();
									list2 = list;
									list2.AddRange(pointNode2.connections);
								}
								if (!flag && flag2)
								{
									uint cost = (uint)Mathf.RoundToInt(num * 1000f);
									list2.Add(new Connection(pointNode3, cost, byte.MaxValue));
									this.RegisterConnectionLength((pointNode3.position - pointNode2.position).sqrMagnitudeLong);
								}
								else if (flag && !flag2)
								{
									for (int l = 0; l < list2.Count; l++)
									{
										if (list2[l].node == pointNode3)
										{
											list2.RemoveAt(l);
											break;
										}
									}
								}
							}
						}
					}
					if (list2 != null)
					{
						pointNode2.connections = list2.ToArray();
						pointNode2.SetConnectivityDirty();
					}
				}
				ListPool<Connection>.Release(ref list);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00020956 File Offset: 0x0001EB56
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			this.RebuildNodeLookup();
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0002095E File Offset: 0x0001EB5E
		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			base.RelocateNodes(deltaMatrix);
			this.RebuildNodeLookup();
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00020970 File Offset: 0x0001EB70
		protected override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.root = (ctx.DeserializeUnityObject() as Transform);
			this.searchTag = ctx.reader.ReadString();
			this.maxDistance = ctx.reader.ReadSingle();
			this.limits = ctx.DeserializeVector3();
			this.raycast = ctx.reader.ReadBoolean();
			this.use2DPhysics = ctx.reader.ReadBoolean();
			this.thickRaycast = ctx.reader.ReadBoolean();
			this.thickRaycastRadius = ctx.reader.ReadSingle();
			this.recursive = ctx.reader.ReadBoolean();
			ctx.reader.ReadBoolean();
			this.mask = ctx.reader.ReadInt32();
			this.optimizeForSparseGraph = ctx.reader.ReadBoolean();
			ctx.reader.ReadBoolean();
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00020A58 File Offset: 0x0001EC58
		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
			}
			ctx.writer.Write(this.nodeCount);
			for (int i = 0; i < this.nodeCount; i++)
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

		// Token: 0x060005C2 RID: 1474 RVA: 0x00020AD0 File Offset: 0x0001ECD0
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			this.nodes = new PointNode[num];
			this.nodeCount = num;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = new PointNode(this.active);
					this.nodes[i].DeserializeNode(ctx);
				}
			}
		}

		// Token: 0x04000320 RID: 800
		[JsonMember]
		public Transform root;

		// Token: 0x04000321 RID: 801
		[JsonMember]
		public string searchTag;

		// Token: 0x04000322 RID: 802
		[JsonMember]
		public float maxDistance;

		// Token: 0x04000323 RID: 803
		[JsonMember]
		public Vector3 limits;

		// Token: 0x04000324 RID: 804
		[JsonMember]
		public bool raycast = true;

		// Token: 0x04000325 RID: 805
		[JsonMember]
		public bool use2DPhysics;

		// Token: 0x04000326 RID: 806
		[JsonMember]
		public bool thickRaycast;

		// Token: 0x04000327 RID: 807
		[JsonMember]
		public float thickRaycastRadius = 1f;

		// Token: 0x04000328 RID: 808
		[JsonMember]
		public bool recursive = true;

		// Token: 0x04000329 RID: 809
		[JsonMember]
		public LayerMask mask;

		// Token: 0x0400032A RID: 810
		[JsonMember]
		public bool optimizeForSparseGraph;

		// Token: 0x0400032B RID: 811
		private PointKDTree lookupTree = new PointKDTree();

		// Token: 0x0400032C RID: 812
		private long maximumConnectionLength;

		// Token: 0x0400032D RID: 813
		public PointNode[] nodes;

		// Token: 0x0400032E RID: 814
		[JsonMember]
		public PointGraph.NodeDistanceMode nearestNodeDistanceMode;

		// Token: 0x02000134 RID: 308
		public enum NodeDistanceMode
		{
			// Token: 0x04000728 RID: 1832
			Node,
			// Token: 0x04000729 RID: 1833
			Connection
		}
	}
}
