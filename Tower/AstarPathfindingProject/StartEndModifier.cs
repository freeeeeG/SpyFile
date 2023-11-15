﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000080 RID: 128
	[Serializable]
	public class StartEndModifier : PathModifier
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00026657 File Offset: 0x00024857
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002665C File Offset: 0x0002485C
		public override void Apply(Path _p)
		{
			ABPath abpath = _p as ABPath;
			if (abpath == null || abpath.vectorPath.Count == 0)
			{
				return;
			}
			bool flag = false;
			if (abpath.vectorPath.Count == 1 && !this.addPoints)
			{
				abpath.vectorPath.Add(abpath.vectorPath[0]);
				flag = true;
			}
			bool flag2;
			int num;
			Vector3 vector = this.Snap(abpath, this.exactStartPoint, true, out flag2, out num);
			bool flag3;
			int num2;
			Vector3 vector2 = this.Snap(abpath, this.exactEndPoint, false, out flag3, out num2);
			if (flag)
			{
				if (num == num2)
				{
					flag2 = false;
					flag3 = false;
				}
				else
				{
					flag2 = false;
				}
			}
			if ((flag2 || this.addPoints) && this.exactStartPoint != StartEndModifier.Exactness.SnapToNode)
			{
				abpath.vectorPath.Insert(0, vector);
			}
			else
			{
				abpath.vectorPath[0] = vector;
			}
			if ((flag3 || this.addPoints) && this.exactEndPoint != StartEndModifier.Exactness.SnapToNode)
			{
				abpath.vectorPath.Add(vector2);
				return;
			}
			abpath.vectorPath[abpath.vectorPath.Count - 1] = vector2;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00026758 File Offset: 0x00024958
		private Vector3 Snap(ABPath path, StartEndModifier.Exactness mode, bool start, out bool forceAddPoint, out int closestConnectionIndex)
		{
			int num = start ? 0 : (path.path.Count - 1);
			GraphNode graphNode = path.path[num];
			Vector3 vector = (Vector3)graphNode.position;
			closestConnectionIndex = 0;
			forceAddPoint = false;
			switch (mode)
			{
			case StartEndModifier.Exactness.SnapToNode:
				return vector;
			case StartEndModifier.Exactness.Original:
			case StartEndModifier.Exactness.Interpolate:
			case StartEndModifier.Exactness.NodeConnection:
			{
				Vector3 vector2;
				if (start)
				{
					vector2 = ((this.adjustStartPoint != null) ? this.adjustStartPoint() : path.originalStartPoint);
				}
				else
				{
					vector2 = path.originalEndPoint;
				}
				switch (mode)
				{
				case StartEndModifier.Exactness.Original:
					return this.GetClampedPoint(vector, vector2, graphNode);
				case StartEndModifier.Exactness.Interpolate:
				{
					GraphNode graphNode2 = path.path[Mathf.Clamp(num + (start ? 1 : -1), 0, path.path.Count - 1)];
					return VectorMath.ClosestPointOnSegment(vector, (Vector3)graphNode2.position, vector2);
				}
				case StartEndModifier.Exactness.NodeConnection:
				{
					this.connectionBuffer = (this.connectionBuffer ?? new List<GraphNode>());
					Action<GraphNode> action;
					if ((action = this.connectionBufferAddDelegate) == null)
					{
						action = new Action<GraphNode>(this.connectionBuffer.Add);
					}
					this.connectionBufferAddDelegate = action;
					GraphNode graphNode2 = path.path[Mathf.Clamp(num + (start ? 1 : -1), 0, path.path.Count - 1)];
					graphNode.GetConnections(this.connectionBufferAddDelegate);
					Vector3 result = vector;
					float num2 = float.PositiveInfinity;
					for (int i = this.connectionBuffer.Count - 1; i >= 0; i--)
					{
						GraphNode graphNode3 = this.connectionBuffer[i];
						if (path.CanTraverse(graphNode3))
						{
							Vector3 vector3 = VectorMath.ClosestPointOnSegment(vector, (Vector3)graphNode3.position, vector2);
							float sqrMagnitude = (vector3 - vector2).sqrMagnitude;
							if (sqrMagnitude < num2)
							{
								result = vector3;
								num2 = sqrMagnitude;
								closestConnectionIndex = i;
								forceAddPoint = (graphNode3 != graphNode2);
							}
						}
					}
					this.connectionBuffer.Clear();
					return result;
				}
				}
				throw new ArgumentException("Cannot reach this point, but the compiler is not smart enough to realize that.");
			}
			case StartEndModifier.Exactness.ClosestOnNode:
				if (!start)
				{
					return path.endPoint;
				}
				return path.startPoint;
			default:
				throw new ArgumentException("Invalid mode");
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00026970 File Offset: 0x00024B70
		protected Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
		{
			Vector3 vector = to;
			RaycastHit raycastHit;
			if (this.useRaycasting && Physics.Linecast(from, to, out raycastHit, this.mask))
			{
				vector = raycastHit.point;
			}
			if (this.useGraphRaycasting && hint != null)
			{
				IRaycastableGraph raycastableGraph = AstarData.GetGraph(hint) as IRaycastableGraph;
				GraphHitInfo graphHitInfo;
				if (raycastableGraph != null && raycastableGraph.Linecast(from, vector, out graphHitInfo, null, null))
				{
					vector = graphHitInfo.point;
				}
			}
			return vector;
		}

		// Token: 0x040003AB RID: 939
		public bool addPoints;

		// Token: 0x040003AC RID: 940
		public StartEndModifier.Exactness exactStartPoint = StartEndModifier.Exactness.ClosestOnNode;

		// Token: 0x040003AD RID: 941
		public StartEndModifier.Exactness exactEndPoint = StartEndModifier.Exactness.ClosestOnNode;

		// Token: 0x040003AE RID: 942
		public Func<Vector3> adjustStartPoint;

		// Token: 0x040003AF RID: 943
		public bool useRaycasting;

		// Token: 0x040003B0 RID: 944
		public LayerMask mask = -1;

		// Token: 0x040003B1 RID: 945
		public bool useGraphRaycasting;

		// Token: 0x040003B2 RID: 946
		private List<GraphNode> connectionBuffer;

		// Token: 0x040003B3 RID: 947
		private Action<GraphNode> connectionBufferAddDelegate;

		// Token: 0x0200014F RID: 335
		public enum Exactness
		{
			// Token: 0x040007AE RID: 1966
			SnapToNode,
			// Token: 0x040007AF RID: 1967
			Original,
			// Token: 0x040007B0 RID: 1968
			Interpolate,
			// Token: 0x040007B1 RID: 1969
			ClosestOnNode,
			// Token: 0x040007B2 RID: 1970
			NodeConnection
		}
	}
}
