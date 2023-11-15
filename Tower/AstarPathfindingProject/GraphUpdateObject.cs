using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000019 RID: 25
	public class GraphUpdateObject
	{
		// Token: 0x1700007A RID: 122
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00008813 File Offset: 0x00006A13
		[Obsolete("Not necessary anymore")]
		public bool requiresFloodFill
		{
			set
			{
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00008818 File Offset: 0x00006A18
		public GraphUpdateStage stage
		{
			get
			{
				switch (this.internalStage)
				{
				case -3:
					return GraphUpdateStage.Aborted;
				case -1:
					return GraphUpdateStage.Created;
				case 0:
					return GraphUpdateStage.Applied;
				}
				return GraphUpdateStage.Pending;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008850 File Offset: 0x00006A50
		public virtual void WillUpdateNode(GraphNode node)
		{
			if (this.trackChangedNodes && node != null)
			{
				if (this.changedNodes == null)
				{
					this.changedNodes = ListPool<GraphNode>.Claim();
					this.backupData = ListPool<uint>.Claim();
					this.backupPositionData = ListPool<Int3>.Claim();
				}
				this.changedNodes.Add(node);
				this.backupPositionData.Add(node.position);
				this.backupData.Add(node.Penalty);
				this.backupData.Add(node.Flags);
				GridNode gridNode = node as GridNode;
				if (gridNode != null)
				{
					this.backupData.Add((uint)gridNode.InternalGridFlags);
				}
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000088F4 File Offset: 0x00006AF4
		public virtual void RevertFromBackup()
		{
			if (!this.trackChangedNodes)
			{
				throw new InvalidOperationException("Changed nodes have not been tracked, cannot revert from backup. Please set trackChangedNodes to true before applying the update.");
			}
			if (this.changedNodes == null)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < this.changedNodes.Count; i++)
			{
				this.changedNodes[i].Penalty = this.backupData[num];
				num++;
				int hierarchicalNodeIndex = this.changedNodes[i].HierarchicalNodeIndex;
				this.changedNodes[i].Flags = this.backupData[num];
				this.changedNodes[i].HierarchicalNodeIndex = hierarchicalNodeIndex;
				num++;
				GridNode gridNode = this.changedNodes[i] as GridNode;
				if (gridNode != null)
				{
					gridNode.InternalGridFlags = (ushort)this.backupData[num];
					num++;
				}
				this.changedNodes[i].position = this.backupPositionData[i];
				this.changedNodes[i].SetConnectivityDirty();
			}
			ListPool<GraphNode>.Release(ref this.changedNodes);
			ListPool<uint>.Release(ref this.backupData);
			ListPool<Int3>.Release(ref this.backupPositionData);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008A20 File Offset: 0x00006C20
		public virtual void Apply(GraphNode node)
		{
			if (this.shape == null || this.shape.Contains(node))
			{
				node.Penalty = (uint)((ulong)node.Penalty + (ulong)((long)this.addPenalty));
				if (this.modifyWalkability)
				{
					node.Walkable = this.setWalkability;
				}
				if (this.modifyTag)
				{
					node.Tag = (uint)this.setTag;
				}
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008A81 File Offset: 0x00006C81
		public GraphUpdateObject()
		{
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008AB0 File Offset: 0x00006CB0
		public GraphUpdateObject(Bounds b)
		{
			this.bounds = b;
		}

		// Token: 0x040000FD RID: 253
		public Bounds bounds;

		// Token: 0x040000FE RID: 254
		public bool updatePhysics = true;

		// Token: 0x040000FF RID: 255
		public bool resetPenaltyOnPhysics = true;

		// Token: 0x04000100 RID: 256
		public bool updateErosion = true;

		// Token: 0x04000101 RID: 257
		public NNConstraint nnConstraint = NNConstraint.None;

		// Token: 0x04000102 RID: 258
		public int addPenalty;

		// Token: 0x04000103 RID: 259
		public bool modifyWalkability;

		// Token: 0x04000104 RID: 260
		public bool setWalkability;

		// Token: 0x04000105 RID: 261
		public bool modifyTag;

		// Token: 0x04000106 RID: 262
		public int setTag;

		// Token: 0x04000107 RID: 263
		public bool trackChangedNodes;

		// Token: 0x04000108 RID: 264
		public List<GraphNode> changedNodes;

		// Token: 0x04000109 RID: 265
		private List<uint> backupData;

		// Token: 0x0400010A RID: 266
		private List<Int3> backupPositionData;

		// Token: 0x0400010B RID: 267
		public GraphUpdateShape shape;

		// Token: 0x0400010C RID: 268
		internal int internalStage = -1;

		// Token: 0x0400010D RID: 269
		internal const int STAGE_CREATED = -1;

		// Token: 0x0400010E RID: 270
		internal const int STAGE_PENDING = -2;

		// Token: 0x0400010F RID: 271
		internal const int STAGE_ABORTED = -3;

		// Token: 0x04000110 RID: 272
		internal const int STAGE_APPLIED = 0;
	}
}
