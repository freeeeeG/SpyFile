using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000094 RID: 148
	public class XPath : ABPath
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x0002AC44 File Offset: 0x00028E44
		public new static XPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
		{
			XPath path = PathPool.GetPath<XPath>();
			path.Setup(start, end, callback);
			path.endingCondition = new ABPathEndingCondition(path);
			return path;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002AC60 File Offset: 0x00028E60
		protected override void Reset()
		{
			base.Reset();
			this.endingCondition = null;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0002AC6F File Offset: 0x00028E6F
		protected override bool EndPointGridGraphSpecialCase(GraphNode endNode)
		{
			return false;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002AC74 File Offset: 0x00028E74
		protected override void CompletePathIfStartIsValidTarget()
		{
			PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
			if (this.endingCondition.TargetFound(pathNode))
			{
				this.ChangeEndNode(this.startNode);
				this.Trace(pathNode);
				base.CompleteState = PathCompleteState.Complete;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002ACBC File Offset: 0x00028EBC
		private void ChangeEndNode(GraphNode target)
		{
			if (this.endNode != null && this.endNode != this.startNode)
			{
				PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
				pathNode.flag1 = (pathNode.flag2 = false);
			}
			this.endNode = target;
			this.endPoint = (Vector3)target.position;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002AD18 File Offset: 0x00028F18
		protected override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = base.searchedNodes;
				base.searchedNodes = searchedNodes + 1;
				if (this.endingCondition.TargetFound(this.currentR))
				{
					base.CompleteState = PathCompleteState.Complete;
					break;
				}
				this.currentR.node.Open(this, this.currentR, this.pathHandler);
				if (this.pathHandler.heap.isEmpty)
				{
					base.FailWithError("Searched whole area but could not find target");
					return;
				}
				this.currentR = this.pathHandler.heap.Remove();
				if (num > 500)
				{
					if (DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
					if (base.searchedNodes > 1000000)
					{
						throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
			if (base.CompleteState == PathCompleteState.Complete)
			{
				this.ChangeEndNode(this.currentR.node);
				this.Trace(this.currentR);
			}
		}

		// Token: 0x04000414 RID: 1044
		public PathEndingCondition endingCondition;
	}
}
