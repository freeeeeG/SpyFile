using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200005A RID: 90
	public abstract class NavGraph : IGraphInternals
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00015211 File Offset: 0x00013411
		internal bool exists
		{
			get
			{
				return this.active != null;
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00015220 File Offset: 0x00013420
		public virtual int CountNodes()
		{
			int count = 0;
			this.GetNodes(delegate(GraphNode node)
			{
				int count = count;
				count++;
			});
			return count;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00015254 File Offset: 0x00013454
		public void GetNodes(Func<GraphNode, bool> action)
		{
			bool cont = true;
			this.GetNodes(delegate(GraphNode node)
			{
				if (cont)
				{
					cont &= action(node);
				}
			});
		}

		// Token: 0x0600043E RID: 1086
		public abstract void GetNodes(Action<GraphNode> action);

		// Token: 0x0600043F RID: 1087 RVA: 0x00015287 File Offset: 0x00013487
		[Obsolete("Use the transform field (only available on some graph types) instead", true)]
		public void SetMatrix(Matrix4x4 m)
		{
			this.matrix = m;
			this.inverseMatrix = m.inverse;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001529D File Offset: 0x0001349D
		[Obsolete("Use RelocateNodes(Matrix4x4) instead. To keep the same behavior you can call RelocateNodes(newMatrix * oldMatrix.inverse).")]
		public void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
		{
			this.RelocateNodes(newMatrix * oldMatrix.inverse);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000152B2 File Offset: 0x000134B2
		protected void AssertSafeToUpdateGraph()
		{
			if (!this.active.IsAnyWorkItemInProgress && !this.active.isScanning)
			{
				throw new Exception("Trying to update graphs when it is not safe to do so. Graph updates must be done inside a work item or when a graph is being scanned. See AstarPath.AddWorkItem");
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000152DC File Offset: 0x000134DC
		public virtual void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			this.GetNodes(delegate(GraphNode node)
			{
				node.position = (Int3)deltaMatrix.MultiplyPoint((Vector3)node.position);
			});
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00015308 File Offset: 0x00013508
		public NNInfoInternal GetNearest(Vector3 position)
		{
			return this.GetNearest(position, NNConstraint.None);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00015316 File Offset: 0x00013516
		public NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint)
		{
			return this.GetNearest(position, constraint, null);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00015324 File Offset: 0x00013524
		public virtual NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			float maxDistSqr = (constraint == null || constraint.constrainDistance) ? AstarPath.active.maxNearestNodeDistanceSqr : float.PositiveInfinity;
			float minDist = float.PositiveInfinity;
			GraphNode minNode = null;
			float minConstDist = float.PositiveInfinity;
			GraphNode minConstNode = null;
			this.GetNodes(delegate(GraphNode node)
			{
				float sqrMagnitude = (position - (Vector3)node.position).sqrMagnitude;
				if (sqrMagnitude < minDist)
				{
					minDist = sqrMagnitude;
					minNode = node;
				}
				if (sqrMagnitude < minConstDist && sqrMagnitude < maxDistSqr && (constraint == null || constraint.Suitable(node)))
				{
					minConstDist = sqrMagnitude;
					minConstNode = node;
				}
			});
			NNInfoInternal result = new NNInfoInternal(minNode);
			result.constrainedNode = minConstNode;
			if (minConstNode != null)
			{
				result.constClampedPosition = (Vector3)minConstNode.position;
			}
			else if (minNode != null)
			{
				result.constrainedNode = minNode;
				result.constClampedPosition = (Vector3)minNode.position;
			}
			return result;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001540F File Offset: 0x0001360F
		public virtual NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return this.GetNearest(position, constraint);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00015419 File Offset: 0x00013619
		protected virtual void OnDestroy()
		{
			this.DestroyAllNodes();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00015421 File Offset: 0x00013621
		protected virtual void DestroyAllNodes()
		{
			this.GetNodes(delegate(GraphNode node)
			{
				node.Destroy();
			});
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00015448 File Offset: 0x00013648
		[Obsolete("Use AstarPath.Scan instead")]
		public void ScanGraph()
		{
			this.Scan();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00015450 File Offset: 0x00013650
		public void Scan()
		{
			this.active.Scan(this);
		}

		// Token: 0x0600044B RID: 1099
		protected abstract IEnumerable<Progress> ScanInternal();

		// Token: 0x0600044C RID: 1100 RVA: 0x0001545E File Offset: 0x0001365E
		protected virtual void SerializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00015460 File Offset: 0x00013660
		protected virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00015462 File Offset: 0x00013662
		protected virtual void PostDeserialization(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00015464 File Offset: 0x00013664
		protected virtual void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			this.guid = new Guid(ctx.reader.ReadBytes(16));
			this.initialPenalty = ctx.reader.ReadUInt32();
			this.open = ctx.reader.ReadBoolean();
			this.name = ctx.reader.ReadString();
			this.drawGizmos = ctx.reader.ReadBoolean();
			this.infoScreenOpen = ctx.reader.ReadBoolean();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000154E0 File Offset: 0x000136E0
		public virtual void OnDrawGizmos(RetainedGizmos gizmos, bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			RetainedGizmos.Hasher hasher = new RetainedGizmos.Hasher(this.active);
			this.GetNodes(delegate(GraphNode node)
			{
				hasher.HashNode(node);
			});
			if (!gizmos.Draw(hasher))
			{
				using (GraphGizmoHelper gizmoHelper = gizmos.GetGizmoHelper(this.active, hasher))
				{
					this.GetNodes(new Action<GraphNode>(gizmoHelper.DrawConnections));
				}
			}
			if (this.active.showUnwalkableNodes)
			{
				this.DrawUnwalkableNodes(this.active.unwalkableNodeDebugSize);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00015588 File Offset: 0x00013788
		protected void DrawUnwalkableNodes(float size)
		{
			Gizmos.color = AstarColor.UnwalkableNode;
			this.GetNodes(delegate(GraphNode node)
			{
				if (!node.Walkable)
				{
					Gizmos.DrawCube((Vector3)node.position, Vector3.one * size);
				}
			});
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000155BE File Offset: 0x000137BE
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x000155C6 File Offset: 0x000137C6
		string IGraphInternals.SerializedEditorSettings
		{
			get
			{
				return this.serializedEditorSettings;
			}
			set
			{
				this.serializedEditorSettings = value;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000155CF File Offset: 0x000137CF
		void IGraphInternals.OnDestroy()
		{
			this.OnDestroy();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000155D7 File Offset: 0x000137D7
		void IGraphInternals.DestroyAllNodes()
		{
			this.DestroyAllNodes();
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000155DF File Offset: 0x000137DF
		IEnumerable<Progress> IGraphInternals.ScanInternal()
		{
			return this.ScanInternal();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000155E7 File Offset: 0x000137E7
		void IGraphInternals.SerializeExtraInfo(GraphSerializationContext ctx)
		{
			this.SerializeExtraInfo(ctx);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000155F0 File Offset: 0x000137F0
		void IGraphInternals.DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			this.DeserializeExtraInfo(ctx);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000155F9 File Offset: 0x000137F9
		void IGraphInternals.PostDeserialization(GraphSerializationContext ctx)
		{
			this.PostDeserialization(ctx);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00015602 File Offset: 0x00013802
		void IGraphInternals.DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			this.DeserializeSettingsCompatibility(ctx);
		}

		// Token: 0x04000285 RID: 645
		public AstarPath active;

		// Token: 0x04000286 RID: 646
		[JsonMember]
		public Guid guid;

		// Token: 0x04000287 RID: 647
		[JsonMember]
		public uint initialPenalty;

		// Token: 0x04000288 RID: 648
		[JsonMember]
		public bool open;

		// Token: 0x04000289 RID: 649
		public uint graphIndex;

		// Token: 0x0400028A RID: 650
		[JsonMember]
		public string name;

		// Token: 0x0400028B RID: 651
		[JsonMember]
		public bool drawGizmos = true;

		// Token: 0x0400028C RID: 652
		[JsonMember]
		public bool infoScreenOpen;

		// Token: 0x0400028D RID: 653
		[JsonMember]
		private string serializedEditorSettings;

		// Token: 0x0400028E RID: 654
		[Obsolete("Use the transform field (only available on some graph types) instead", true)]
		public Matrix4x4 matrix = Matrix4x4.identity;

		// Token: 0x0400028F RID: 655
		[Obsolete("Use the transform field (only available on some graph types) instead", true)]
		public Matrix4x4 inverseMatrix = Matrix4x4.identity;
	}
}
