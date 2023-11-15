using System;
using System.Collections;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200014D RID: 333
	public class ElementalShieldRune : Rune
	{
		// Token: 0x0600089B RID: 2203 RVA: 0x00024390 File Offset: 0x00022590
		protected override void Init()
		{
			this.AddObserver(new Action<object, object>(this.OnInflict), BurnSystem.InflictBurnEvent);
			this.AddObserver(new Action<object, object>(this.OnInflict), FreezeSystem.InflictFreezeEvent);
			this.inflictCounter = 0;
			this.disableInflictGain = false;
			this.playerFlasher = this.player.GetComponentInChildren<PlayerFlasher>();
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x000243EA File Offset: 0x000225EA
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnInflict), BurnSystem.InflictBurnEvent);
			this.RemoveObserver(new Action<object, object>(this.OnInflict), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002441A File Offset: 0x0002261A
		private void OnInflict(object sender, object args)
		{
			if (this.disableInflictGain)
			{
				return;
			}
			this.inflictCounter++;
			if (this.inflictCounter >= this.inflictsToActivate)
			{
				this.inflictCounter = 0;
				base.StartCoroutine(this.StartInvinCR());
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00024455 File Offset: 0x00022655
		private IEnumerator StartInvinCR()
		{
			this.disableInflictGain = true;
			this.player.playerHealth.isInvincible.Flip();
			this.playerFlasher.Flash();
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			yield return new WaitForSeconds(this.secondsPerLevel * (float)this.level);
			this.player.playerHealth.isInvincible.UnFlip();
			this.playerFlasher.StopFlash();
			base.StartCoroutine(this.WaitForCooldownCR());
			yield break;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00024464 File Offset: 0x00022664
		private IEnumerator WaitForCooldownCR()
		{
			yield return new WaitForSeconds(this.cooldown);
			this.disableInflictGain = false;
			yield break;
		}

		// Token: 0x0400065C RID: 1628
		[SerializeField]
		private float secondsPerLevel;

		// Token: 0x0400065D RID: 1629
		[SerializeField]
		private float cooldown;

		// Token: 0x0400065E RID: 1630
		[SerializeField]
		private int inflictsToActivate;

		// Token: 0x0400065F RID: 1631
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000660 RID: 1632
		private PlayerFlasher playerFlasher;

		// Token: 0x04000661 RID: 1633
		private int inflictCounter;

		// Token: 0x04000662 RID: 1634
		private bool disableInflictGain;
	}
}
