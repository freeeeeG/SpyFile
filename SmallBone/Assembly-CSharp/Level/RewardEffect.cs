using System;
using FX;
using UnityEngine;

namespace Level
{
	// Token: 0x02000518 RID: 1304
	public class RewardEffect : MonoBehaviour
	{
		// Token: 0x060019B5 RID: 6581 RVA: 0x00002191 File Offset: 0x00000391
		public void Play(Rarity rarity)
		{
		}

		// Token: 0x0400167F RID: 5759
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001680 RID: 5760
		[SerializeField]
		private RewardEffect.Effect _unique;

		// Token: 0x04001681 RID: 5761
		[SerializeField]
		private RewardEffect.Effect _legendary;

		// Token: 0x02000519 RID: 1305
		[Serializable]
		private class Effect
		{
			// Token: 0x04001682 RID: 5762
			[SerializeField]
			internal RuntimeAnimatorController animator;

			// Token: 0x04001683 RID: 5763
			[SerializeField]
			internal SoundInfo soundInfo;
		}
	}
}
