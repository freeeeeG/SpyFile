using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200004C RID: 76
	public abstract class GraphNode
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600037A RID: 890 RVA: 0x000134FD File Offset: 0x000116FD
		public NavGraph Graph
		{
			get
			{
				if (!this.Destroyed)
				{
					return AstarData.GetGraph(this);
				}
				return null;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001350F File Offset: 0x0001170F
		protected GraphNode(AstarPath astar)
		{
			if (astar != null)
			{
				this.nodeIndex = astar.GetNewNodeIndex();
				astar.InitializeNode(this);
				return;
			}
			throw new Exception("No active AstarPath object to bind to");
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00013538 File Offset: 0x00011738
		public void Destroy()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ClearConnections(true);
			if (AstarPath.active != null)
			{
				AstarPath.active.DestroyNode(this);
			}
			this.NodeIndex = 268435454;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0001356D File Offset: 0x0001176D
		public bool Destroyed
		{
			get
			{
				return this.NodeIndex == 268435454;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0001357C File Offset: 0x0001177C
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0001358A File Offset: 0x0001178A
		public int NodeIndex
		{
			get
			{
				return this.nodeIndex & 268435455;
			}
			private set
			{
				this.nodeIndex = ((this.nodeIndex & -268435456) | value);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000380 RID: 896 RVA: 0x000135A0 File Offset: 0x000117A0
		// (set) Token: 0x06000381 RID: 897 RVA: 0x000135B1 File Offset: 0x000117B1
		internal bool TemporaryFlag1
		{
			get
			{
				return (this.nodeIndex & 268435456) != 0;
			}
			set
			{
				this.nodeIndex = ((this.nodeIndex & -268435457) | (value ? 268435456 : 0));
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000382 RID: 898 RVA: 0x000135D1 File Offset: 0x000117D1
		// (set) Token: 0x06000383 RID: 899 RVA: 0x000135E2 File Offset: 0x000117E2
		internal bool TemporaryFlag2
		{
			get
			{
				return (this.nodeIndex & 536870912) != 0;
			}
			set
			{
				this.nodeIndex = ((this.nodeIndex & -536870913) | (value ? 536870912 : 0));
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00013602 File Offset: 0x00011802
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0001360A File Offset: 0x0001180A
		public uint Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00013613 File Offset: 0x00011813
		// (set) Token: 0x06000387 RID: 903 RVA: 0x0001361B File Offset: 0x0001181B
		public uint Penalty
		{
			get
			{
				return this.penalty;
			}
			set
			{
				if (value > 16777215U)
				{
					Debug.LogWarning("Very high penalty applied. Are you sure negative values haven't underflowed?\nPenalty values this high could with long paths cause overflows and in some cases infinity loops because of that.\nPenalty value applied: " + value.ToString());
				}
				this.penalty = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00013642 File Offset: 0x00011842
		// (set) Token: 0x06000389 RID: 905 RVA: 0x0001364F File Offset: 0x0001184F
		public bool Walkable
		{
			get
			{
				return (this.flags & 1U) > 0U;
			}
			set
			{
				this.flags = ((this.flags & 4294967294U) | (value ? 1U : 0U));
				AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00013678 File Offset: 0x00011878
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00013688 File Offset: 0x00011888
		internal int HierarchicalNodeIndex
		{
			get
			{
				return (int)((this.flags & 262142U) >> 1);
			}
			set
			{
				this.flags = ((this.flags & 4294705153U) | (uint)((uint)value << 1));
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000136A0 File Offset: 0x000118A0
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000136B1 File Offset: 0x000118B1
		internal bool IsHierarchicalNodeDirty
		{
			get
			{
				return (this.flags & 262144U) > 0U;
			}
			set
			{
				this.flags = ((this.flags & 4294705151U) | (value ? 1U : 0U) << 18);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000136D0 File Offset: 0x000118D0
		public uint Area
		{
			get
			{
				return AstarPath.active.hierarchicalGraph.GetConnectedComponent(this.HierarchicalNodeIndex);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600038F RID: 911 RVA: 0x000136E7 File Offset: 0x000118E7
		// (set) Token: 0x06000390 RID: 912 RVA: 0x000136F8 File Offset: 0x000118F8
		public uint GraphIndex
		{
			get
			{
				return (this.flags & 4278190080U) >> 24;
			}
			set
			{
				this.flags = ((this.flags & 16777215U) | value << 24);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00013711 File Offset: 0x00011911
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00013722 File Offset: 0x00011922
		public uint Tag
		{
			get
			{
				return (this.flags & 16252928U) >> 19;
			}
			set
			{
				this.flags = ((this.flags & 4278714367U) | (value << 19 & 16252928U));
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00013741 File Offset: 0x00011941
		public void SetConnectivityDirty()
		{
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00013753 File Offset: 0x00011953
		[Obsolete("This method is deprecated because it never did anything, you can safely remove any calls to this method")]
		public void RecalculateConnectionCosts()
		{
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00013758 File Offset: 0x00011958
		public virtual void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
			this.GetConnections(delegate(GraphNode other)
			{
				PathNode pathNode2 = handler.GetPathNode(other);
				if (pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
				{
					other.UpdateRecursiveG(path, pathNode2, handler);
				}
			});
		}

		// Token: 0x06000396 RID: 918
		public abstract void GetConnections(Action<GraphNode> action);

		// Token: 0x06000397 RID: 919
		public abstract void AddConnection(GraphNode node, uint cost);

		// Token: 0x06000398 RID: 920
		public abstract void RemoveConnection(GraphNode node);

		// Token: 0x06000399 RID: 921
		public abstract void ClearConnections(bool alsoReverse);

		// Token: 0x0600039A RID: 922 RVA: 0x000137BC File Offset: 0x000119BC
		public virtual bool ContainsConnection(GraphNode node)
		{
			bool contains = false;
			this.GetConnections(delegate(GraphNode neighbour)
			{
				contains |= (neighbour == node);
			});
			return contains;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000137F5 File Offset: 0x000119F5
		public virtual bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			return false;
		}

		// Token: 0x0600039C RID: 924
		public abstract void Open(Path path, PathNode pathNode, PathHandler handler);

		// Token: 0x0600039D RID: 925 RVA: 0x000137F8 File Offset: 0x000119F8
		public virtual float SurfaceArea()
		{
			return 0f;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000137FF File Offset: 0x000119FF
		public virtual Vector3 RandomPointOnSurface()
		{
			return (Vector3)this.position;
		}

		// Token: 0x0600039F RID: 927
		public abstract Vector3 ClosestPointOnNode(Vector3 p);

		// Token: 0x060003A0 RID: 928 RVA: 0x0001380C File Offset: 0x00011A0C
		public virtual int GetGizmoHashCode()
		{
			return this.position.GetHashCode() ^ (int)(19U * this.Penalty) ^ (int)(41U * (this.flags & 4294443009U));
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00013839 File Offset: 0x00011A39
		public virtual void SerializeNode(GraphSerializationContext ctx)
		{
			ctx.writer.Write(this.Penalty);
			ctx.writer.Write(this.Flags & 4294443009U);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00013864 File Offset: 0x00011A64
		public virtual void DeserializeNode(GraphSerializationContext ctx)
		{
			this.Penalty = ctx.reader.ReadUInt32();
			this.Flags = ((ctx.reader.ReadUInt32() & 4294443009U) | (this.Flags & 524286U));
			this.GraphIndex = ctx.graphIndex;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000138B2 File Offset: 0x00011AB2
		public virtual void SerializeReferences(GraphSerializationContext ctx)
		{
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000138B4 File Offset: 0x00011AB4
		public virtual void DeserializeReferences(GraphSerializationContext ctx)
		{
		}

		// Token: 0x04000232 RID: 562
		private int nodeIndex;

		// Token: 0x04000233 RID: 563
		protected uint flags;

		// Token: 0x04000234 RID: 564
		private uint penalty;

		// Token: 0x04000235 RID: 565
		private const int NodeIndexMask = 268435455;

		// Token: 0x04000236 RID: 566
		private const int DestroyedNodeIndex = 268435454;

		// Token: 0x04000237 RID: 567
		private const int TemporaryFlag1Mask = 268435456;

		// Token: 0x04000238 RID: 568
		private const int TemporaryFlag2Mask = 536870912;

		// Token: 0x04000239 RID: 569
		public Int3 position;

		// Token: 0x0400023A RID: 570
		private const int FlagsWalkableOffset = 0;

		// Token: 0x0400023B RID: 571
		private const uint FlagsWalkableMask = 1U;

		// Token: 0x0400023C RID: 572
		private const int FlagsHierarchicalIndexOffset = 1;

		// Token: 0x0400023D RID: 573
		private const uint HierarchicalIndexMask = 262142U;

		// Token: 0x0400023E RID: 574
		private const int HierarchicalDirtyOffset = 18;

		// Token: 0x0400023F RID: 575
		private const uint HierarchicalDirtyMask = 262144U;

		// Token: 0x04000240 RID: 576
		private const int FlagsGraphOffset = 24;

		// Token: 0x04000241 RID: 577
		private const uint FlagsGraphMask = 4278190080U;

		// Token: 0x04000242 RID: 578
		public const uint MaxHierarchicalNodeIndex = 131071U;

		// Token: 0x04000243 RID: 579
		public const uint MaxGraphIndex = 255U;

		// Token: 0x04000244 RID: 580
		private const int FlagsTagOffset = 19;

		// Token: 0x04000245 RID: 581
		private const uint FlagsTagMask = 16252928U;
	}
}
