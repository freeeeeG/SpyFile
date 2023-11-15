using System;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000261 RID: 609
	[CreateAssetMenu(fileName = "AIChangeAnimationSpecial", menuName = "AISpecials/AIChangeAnimationSpecial")]
	public class AIChangeAnimationSpecial : AISpecial
	{
		// Token: 0x06000D30 RID: 3376 RVA: 0x00030152 File Offset: 0x0002E352
		public override void Use(AIComponent ai, Transform target)
		{
			Animator animator = ai.animator;
			if (animator == null)
			{
				return;
			}
			animator.SetTrigger("Special");
		}
	}
}
