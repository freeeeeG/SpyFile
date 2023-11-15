using System;
using System.Collections;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200014C RID: 332
	public class ElementalBarrageRune : Rune
	{
		// Token: 0x06000893 RID: 2195 RVA: 0x0002423D File Offset: 0x0002243D
		protected override void Init()
		{
			this.AddObserver(new Action<object, object>(this.OnInflict), BurnSystem.InflictBurnEvent);
			this.AddObserver(new Action<object, object>(this.OnInflict), FreezeSystem.InflictFreezeEvent);
			this.inflictCounter = 0;
			this.disableInflictGain = false;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002427B File Offset: 0x0002247B
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnInflict), BurnSystem.InflictBurnEvent);
			this.RemoveObserver(new Action<object, object>(this.OnInflict), FreezeSystem.InflictFreezeEvent);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000242AB File Offset: 0x000224AB
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
				base.StartCoroutine(this.StartBonusCR());
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000242E6 File Offset: 0x000224E6
		private void ActivateBonus()
		{
			this.player.stats[StatType.FireRate].AddMultiplierBonus(this.bonusStatMulti);
			this.player.stats[StatType.FireRate].AddMultiplierBonus(this.bonusStatMulti);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00024320 File Offset: 0x00022520
		private void DeactivateBonus()
		{
			this.player.stats[StatType.FireRate].AddMultiplierBonus(-1f * this.bonusStatMulti);
			this.player.stats[StatType.FireRate].AddMultiplierBonus(-1f * this.bonusStatMulti);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00024371 File Offset: 0x00022571
		private IEnumerator StartBonusCR()
		{
			this.disableInflictGain = true;
			this.ActivateBonus();
			yield return new WaitForSeconds(this.secondsPerLevel * (float)this.level);
			this.DeactivateBonus();
			base.StartCoroutine(this.WaitForCooldownCR());
			yield break;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00024380 File Offset: 0x00022580
		private IEnumerator WaitForCooldownCR()
		{
			yield return new WaitForSeconds(this.cooldown);
			this.disableInflictGain = false;
			yield break;
		}

		// Token: 0x04000656 RID: 1622
		[SerializeField]
		private float bonusStatMulti;

		// Token: 0x04000657 RID: 1623
		[SerializeField]
		private float secondsPerLevel;

		// Token: 0x04000658 RID: 1624
		[SerializeField]
		private float cooldown;

		// Token: 0x04000659 RID: 1625
		[SerializeField]
		private int inflictsToActivate;

		// Token: 0x0400065A RID: 1626
		private int inflictCounter;

		// Token: 0x0400065B RID: 1627
		private bool disableInflictGain;
	}
}
