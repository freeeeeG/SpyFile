﻿using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000055 RID: 85
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_navmesh_clamp.php")]
	public class NavmeshClamp : MonoBehaviour
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x00014B84 File Offset: 0x00012D84
		private void LateUpdate()
		{
			if (this.prevNode == null)
			{
				NNInfo nearest = AstarPath.active.GetNearest(base.transform.position);
				this.prevNode = nearest.node;
				this.prevPos = base.transform.position;
			}
			if (this.prevNode == null)
			{
				return;
			}
			if (this.prevNode != null)
			{
				IRaycastableGraph raycastableGraph = AstarData.GetGraph(this.prevNode) as IRaycastableGraph;
				if (raycastableGraph != null)
				{
					GraphHitInfo graphHitInfo;
					if (raycastableGraph.Linecast(this.prevPos, base.transform.position, out graphHitInfo, null, null))
					{
						graphHitInfo.point.y = base.transform.position.y;
						Vector3 vector = VectorMath.ClosestPointOnLine(graphHitInfo.tangentOrigin, graphHitInfo.tangentOrigin + graphHitInfo.tangent, base.transform.position);
						Vector3 vector2 = graphHitInfo.point;
						vector2 += Vector3.ClampMagnitude((Vector3)graphHitInfo.node.position - vector2, 0.008f);
						if (raycastableGraph.Linecast(vector2, vector, out graphHitInfo, null, null))
						{
							graphHitInfo.point.y = base.transform.position.y;
							base.transform.position = graphHitInfo.point;
						}
						else
						{
							vector.y = base.transform.position.y;
							base.transform.position = vector;
						}
					}
					this.prevNode = graphHitInfo.node;
				}
			}
			this.prevPos = base.transform.position;
		}

		// Token: 0x04000277 RID: 631
		private GraphNode prevNode;

		// Token: 0x04000278 RID: 632
		private Vector3 prevPos;
	}
}
