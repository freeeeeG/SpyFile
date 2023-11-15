using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000E0 RID: 224
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_turn_based_a_i.php")]
	public class TurnBasedAI : VersionedMonoBehaviour
	{
		// Token: 0x060009B0 RID: 2480 RVA: 0x0003EE7F File Offset: 0x0003D07F
		private void Start()
		{
			this.blocker.BlockAtCurrentPosition();
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0003EE8C File Offset: 0x0003D08C
		protected override void Awake()
		{
			base.Awake();
			this.traversalProvider = new BlockManager.TraversalProvider(this.blockManager, BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker>
			{
				this.blocker
			});
		}

		// Token: 0x040005B9 RID: 1465
		public int movementPoints = 2;

		// Token: 0x040005BA RID: 1466
		public BlockManager blockManager;

		// Token: 0x040005BB RID: 1467
		public SingleNodeBlocker blocker;

		// Token: 0x040005BC RID: 1468
		public GraphNode targetNode;

		// Token: 0x040005BD RID: 1469
		public BlockManager.TraversalProvider traversalProvider;
	}
}
