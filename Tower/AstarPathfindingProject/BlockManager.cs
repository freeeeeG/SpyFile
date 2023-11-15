using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000098 RID: 152
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_block_manager.php")]
	public class BlockManager : VersionedMonoBehaviour
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x0002AED7 File Offset: 0x000290D7
		private void Start()
		{
			if (!AstarPath.active)
			{
				throw new Exception("No AstarPath object in the scene");
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002AEF0 File Offset: 0x000290F0
		public bool NodeContainsAnyOf(GraphNode node, List<SingleNodeBlocker> selector)
		{
			List<SingleNodeBlocker> list;
			if (!this.blocked.TryGetValue(node, out list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SingleNodeBlocker singleNodeBlocker = list[i];
				for (int j = 0; j < selector.Count; j++)
				{
					if (singleNodeBlocker == selector[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002AF48 File Offset: 0x00029148
		public bool NodeContainsAnyExcept(GraphNode node, List<SingleNodeBlocker> selector)
		{
			List<SingleNodeBlocker> list;
			if (!this.blocked.TryGetValue(node, out list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SingleNodeBlocker singleNodeBlocker = list[i];
				bool flag = false;
				for (int j = 0; j < selector.Count; j++)
				{
					if (singleNodeBlocker == selector[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002AFAC File Offset: 0x000291AC
		public void InternalBlock(GraphNode node, SingleNodeBlocker blocker)
		{
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate()
			{
				List<SingleNodeBlocker> list;
				if (!this.blocked.TryGetValue(node, out list))
				{
					list = (this.blocked[node] = ListPool<SingleNodeBlocker>.Claim());
				}
				list.Add(blocker);
			}, null));
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002AFF0 File Offset: 0x000291F0
		public void InternalUnblock(GraphNode node, SingleNodeBlocker blocker)
		{
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate()
			{
				List<SingleNodeBlocker> list;
				if (this.blocked.TryGetValue(node, out list))
				{
					list.Remove(blocker);
					if (list.Count == 0)
					{
						this.blocked.Remove(node);
						ListPool<SingleNodeBlocker>.Release(ref list);
					}
				}
			}, null));
		}

		// Token: 0x04000418 RID: 1048
		private Dictionary<GraphNode, List<SingleNodeBlocker>> blocked = new Dictionary<GraphNode, List<SingleNodeBlocker>>();

		// Token: 0x02000154 RID: 340
		public enum BlockMode
		{
			// Token: 0x040007C5 RID: 1989
			AllExceptSelector,
			// Token: 0x040007C6 RID: 1990
			OnlySelector
		}

		// Token: 0x02000155 RID: 341
		public class TraversalProvider : ITraversalProvider
		{
			// Token: 0x1700018D RID: 397
			// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00047D28 File Offset: 0x00045F28
			// (set) Token: 0x06000B54 RID: 2900 RVA: 0x00047D30 File Offset: 0x00045F30
			public BlockManager.BlockMode mode { get; private set; }

			// Token: 0x06000B55 RID: 2901 RVA: 0x00047D39 File Offset: 0x00045F39
			public TraversalProvider(BlockManager blockManager, BlockManager.BlockMode mode, List<SingleNodeBlocker> selector)
			{
				if (blockManager == null)
				{
					throw new ArgumentNullException("blockManager");
				}
				if (selector == null)
				{
					throw new ArgumentNullException("selector");
				}
				this.blockManager = blockManager;
				this.mode = mode;
				this.selector = selector;
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x00047D78 File Offset: 0x00045F78
			public bool CanTraverse(Path path, GraphNode node)
			{
				if (!node.Walkable || (path.enabledTags >> (int)node.Tag & 1) == 0)
				{
					return false;
				}
				if (this.mode == BlockManager.BlockMode.OnlySelector)
				{
					return !this.blockManager.NodeContainsAnyOf(node, this.selector);
				}
				return !this.blockManager.NodeContainsAnyExcept(node, this.selector);
			}

			// Token: 0x06000B57 RID: 2903 RVA: 0x00047DD7 File Offset: 0x00045FD7
			public uint GetTraversalCost(Path path, GraphNode node)
			{
				return path.GetTagPenalty((int)node.Tag) + node.Penalty;
			}

			// Token: 0x040007C7 RID: 1991
			private readonly BlockManager blockManager;

			// Token: 0x040007C9 RID: 1993
			private readonly List<SingleNodeBlocker> selector;
		}
	}
}
