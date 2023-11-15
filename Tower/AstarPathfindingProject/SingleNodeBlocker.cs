using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000099 RID: 153
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_single_node_blocker.php")]
	public class SingleNodeBlocker : VersionedMonoBehaviour
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0002B047 File Offset: 0x00029247
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0002B04F File Offset: 0x0002924F
		public GraphNode lastBlocked { get; private set; }

		// Token: 0x06000727 RID: 1831 RVA: 0x0002B058 File Offset: 0x00029258
		public void BlockAtCurrentPosition()
		{
			this.BlockAt(base.transform.position);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002B06C File Offset: 0x0002926C
		public void BlockAt(Vector3 position)
		{
			this.Unblock();
			GraphNode node = AstarPath.active.GetNearest(position, NNConstraint.None).node;
			if (node != null)
			{
				this.Block(node);
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002B09F File Offset: 0x0002929F
		public void Block(GraphNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.manager.InternalBlock(node, this);
			this.lastBlocked = node;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0002B0C3 File Offset: 0x000292C3
		public void Unblock()
		{
			if (this.lastBlocked == null || this.lastBlocked.Destroyed)
			{
				this.lastBlocked = null;
				return;
			}
			this.manager.InternalUnblock(this.lastBlocked, this);
			this.lastBlocked = null;
		}

		// Token: 0x0400041A RID: 1050
		public BlockManager manager;
	}
}
