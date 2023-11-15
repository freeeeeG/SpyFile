using System;
using System.Collections;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200014E RID: 334
	public class EtherealRune : Rune
	{
		// Token: 0x060008A1 RID: 2209 RVA: 0x00024474 File Offset: 0x00022674
		private void OnDeath(object sender, object args)
		{
			if (this._isActive)
			{
				return;
			}
			if ((sender as Health).gameObject.tag == "Enemy")
			{
				this._counter++;
				if (this._counter >= this.killsToActivate)
				{
					base.StartCoroutine(this.EtherealStateCR());
					this._counter = 0;
				}
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000244D6 File Offset: 0x000226D6
		protected override void Init()
		{
			this._isActive = false;
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000244F6 File Offset: 0x000226F6
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002450F File Offset: 0x0002270F
		private IEnumerator EtherealStateCR()
		{
			this._isActive = true;
			this.player.ammo.infiniteAmmo.Flip();
			yield return new WaitForSeconds(this.durationPerLevel * (float)this.level);
			this._isActive = false;
			this.player.ammo.infiniteAmmo.UnFlip();
			yield break;
		}

		// Token: 0x04000663 RID: 1635
		[SerializeField]
		private float durationPerLevel;

		// Token: 0x04000664 RID: 1636
		[SerializeField]
		private int killsToActivate;

		// Token: 0x04000665 RID: 1637
		private int _counter;

		// Token: 0x04000666 RID: 1638
		private bool _isActive;
	}
}
