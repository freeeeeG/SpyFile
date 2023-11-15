using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000091 RID: 145
	public class FloodPathTracer : ABPath
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x000296BE File Offset: 0x000278BE
		protected override bool hasEndPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000296C9 File Offset: 0x000278C9
		public static FloodPathTracer Construct(Vector3 start, FloodPath flood, OnPathDelegate callback = null)
		{
			FloodPathTracer path = PathPool.GetPath<FloodPathTracer>();
			path.Setup(start, flood, callback);
			return path;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000296D9 File Offset: 0x000278D9
		protected void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
		{
			this.flood = flood;
			if (flood == null || flood.PipelineState < PathState.Returned)
			{
				throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
			}
			base.Setup(start, flood.originalStartPoint, callback);
			this.nnConstraint = new FloodPathConstraint(flood);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00029713 File Offset: 0x00027913
		protected override void Reset()
		{
			base.Reset();
			this.flood = null;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00029722 File Offset: 0x00027922
		protected override void Initialize()
		{
			if (this.startNode != null && this.flood.HasPathTo(this.startNode))
			{
				this.Trace(this.startNode);
				base.CompleteState = PathCompleteState.Complete;
				return;
			}
			base.FailWithError("Could not find valid start node");
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002975E File Offset: 0x0002795E
		protected override void CalculateStep(long targetTick)
		{
			if (base.CompleteState != PathCompleteState.Complete)
			{
				throw new Exception("Something went wrong. At this point the path should be completed");
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00029774 File Offset: 0x00027974
		public void Trace(GraphNode from)
		{
			GraphNode graphNode = from;
			int num = 0;
			while (graphNode != null)
			{
				this.path.Add(graphNode);
				this.vectorPath.Add((Vector3)graphNode.position);
				graphNode = this.flood.GetParent(graphNode);
				num++;
				if (num > 1024)
				{
					Debug.LogWarning("Inifinity loop? >1024 node path. Remove this message if you really have that long paths (FloodPathTracer.cs, Trace function)");
					return;
				}
			}
		}

		// Token: 0x040003FD RID: 1021
		protected FloodPath flood;
	}
}
