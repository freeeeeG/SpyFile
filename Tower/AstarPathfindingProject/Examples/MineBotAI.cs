using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F2 RID: 242
	[RequireComponent(typeof(Seeker))]
	[Obsolete("This script has been replaced by Pathfinding.Examples.MineBotAnimation. Any uses of this script in the Unity editor will be automatically replaced by one AIPath component and one MineBotAnimation component.")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_mine_bot_a_i.php")]
	public class MineBotAI : AIPath
	{
		// Token: 0x04000634 RID: 1588
		public Animation anim;

		// Token: 0x04000635 RID: 1589
		public float sleepVelocity = 0.4f;

		// Token: 0x04000636 RID: 1590
		public float animationSpeed = 0.2f;

		// Token: 0x04000637 RID: 1591
		public GameObject endOfPathEffect;
	}
}
