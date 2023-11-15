using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000991 RID: 2449
	public interface IAbilityInstance
	{
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06003475 RID: 13429
		Character owner { get; }

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06003476 RID: 13430
		IAbility ability { get; }

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06003477 RID: 13431
		// (set) Token: 0x06003478 RID: 13432
		float remainTime { get; set; }

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06003479 RID: 13433
		bool attached { get; }

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600347A RID: 13434
		Sprite icon { get; }

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x0600347B RID: 13435
		float iconFillAmount { get; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x0600347C RID: 13436
		bool iconFillInversed { get; }

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600347D RID: 13437
		bool iconFillFlipped { get; }

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x0600347E RID: 13438
		int iconStacks { get; }

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x0600347F RID: 13439
		bool expired { get; }

		// Token: 0x06003480 RID: 13440
		void UpdateTime(float deltaTime);

		// Token: 0x06003481 RID: 13441
		void Refresh();

		// Token: 0x06003482 RID: 13442
		void Attach();

		// Token: 0x06003483 RID: 13443
		void Detach();
	}
}
