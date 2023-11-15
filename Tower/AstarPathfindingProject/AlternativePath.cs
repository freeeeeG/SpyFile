using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000078 RID: 120
	[AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_alternative_path.php")]
	[Serializable]
	public class AlternativePath : MonoModifier
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00024AB7 File Offset: 0x00022CB7
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00024ABB File Offset: 0x00022CBB
		public override void Apply(Path p)
		{
			if (this == null)
			{
				return;
			}
			this.ApplyNow(p.path);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00024AD3 File Offset: 0x00022CD3
		protected void OnDestroy()
		{
			this.destroyed = true;
			this.ClearOnDestroy();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00024AE2 File Offset: 0x00022CE2
		private void ClearOnDestroy()
		{
			this.InversePrevious();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00024AEC File Offset: 0x00022CEC
		private void InversePrevious()
		{
			if (this.prevNodes != null)
			{
				bool flag = false;
				for (int i = 0; i < this.prevNodes.Count; i++)
				{
					if ((ulong)this.prevNodes[i].Penalty < (ulong)((long)this.prevPenalty))
					{
						flag = true;
						this.prevNodes[i].Penalty = 0U;
					}
					else
					{
						this.prevNodes[i].Penalty = (uint)((ulong)this.prevNodes[i].Penalty - (ulong)((long)this.prevPenalty));
					}
				}
				if (flag)
				{
					Debug.LogWarning("Penalty for some nodes has been reset while the AlternativePath modifier was active (possibly because of a graph update). Some penalties might be incorrect (they may be lower than expected for the affected nodes)");
				}
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00024B88 File Offset: 0x00022D88
		private void ApplyNow(List<GraphNode> nodes)
		{
			this.InversePrevious();
			this.prevNodes.Clear();
			if (this.destroyed)
			{
				return;
			}
			if (nodes != null)
			{
				for (int i = this.rnd.Next(this.randomStep); i < nodes.Count; i += this.rnd.Next(1, this.randomStep))
				{
					nodes[i].Penalty = (uint)((ulong)nodes[i].Penalty + (ulong)((long)this.penalty));
					this.prevNodes.Add(nodes[i]);
				}
			}
			this.prevPenalty = this.penalty;
		}

		// Token: 0x04000384 RID: 900
		public int penalty = 1000;

		// Token: 0x04000385 RID: 901
		public int randomStep = 10;

		// Token: 0x04000386 RID: 902
		private List<GraphNode> prevNodes = new List<GraphNode>();

		// Token: 0x04000387 RID: 903
		private int prevPenalty;

		// Token: 0x04000388 RID: 904
		private readonly Random rnd = new Random();

		// Token: 0x04000389 RID: 905
		private bool destroyed;
	}
}
