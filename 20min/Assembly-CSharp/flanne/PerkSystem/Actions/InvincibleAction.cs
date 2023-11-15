using System;
using System.Collections;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B4 RID: 436
	public class InvincibleAction : Action
	{
		// Token: 0x06000A0A RID: 2570 RVA: 0x000278CF File Offset: 0x00025ACF
		public override void Init()
		{
			this.player = PlayerController.Instance;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000278DC File Offset: 0x00025ADC
		public override void Activate(GameObject target)
		{
			this.player.StartCoroutine(this.StartInvincible());
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000278F0 File Offset: 0x00025AF0
		private IEnumerator StartInvincible()
		{
			this.player.playerHealth.isInvincible.Flip();
			yield return new WaitForSeconds(this.duration);
			this.player.playerHealth.isInvincible.UnFlip();
			yield break;
		}

		// Token: 0x04000716 RID: 1814
		[SerializeField]
		private float duration;

		// Token: 0x04000717 RID: 1815
		private PlayerController player;
	}
}
