using System;
using System.Collections;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000147 RID: 327
	public class BlessingRune : Rune
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x00023D75 File Offset: 0x00021F75
		protected override void Init()
		{
			base.StartCoroutine(this.WaitToApplyCR());
			this.powerupGenerator = PowerupGenerator.Instance;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00023D8F File Offset: 0x00021F8F
		private IEnumerator WaitToApplyCR()
		{
			yield return new WaitForSeconds(0.1f);
			PlayerController.Instance.playerPerks.Equip(this.holyShieldPowerup);
			this.powerupGenerator.RemoveFromPool(this.holyShieldPowerup);
			this.player.GetComponentInChildren<PreventDamage>().cooldownTime -= this.cdrPerLevel * (float)this.level;
			yield break;
		}

		// Token: 0x04000644 RID: 1604
		[SerializeField]
		private Powerup holyShieldPowerup;

		// Token: 0x04000645 RID: 1605
		[SerializeField]
		private float cdrPerLevel;

		// Token: 0x04000646 RID: 1606
		private PowerupGenerator powerupGenerator;
	}
}
