using System;
using System.Collections;
using flanne.Pickups;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000153 RID: 339
	public class MomentumRune : Rune
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x00024C88 File Offset: 0x00022E88
		private void OnXPPickup(object sender, object args)
		{
			if (this._timer <= 0f)
			{
				base.StartCoroutine(this.StartBuffCR());
				return;
			}
			this._timer = this.duration;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00024CB1 File Offset: 0x00022EB1
		protected override void Init()
		{
			this.stats = this.player.stats;
			this.AddObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00024CDB File Offset: 0x00022EDB
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnXPPickup), XPPickup.XPPickupEvent);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00024CF4 File Offset: 0x00022EF4
		private IEnumerator StartBuffCR()
		{
			this._timer = this.duration;
			this.stats[StatType.MoveSpeed].AddMultiplierBonus(this.moveSpeedBoostPerLevel * (float)this.level);
			while (this._timer > 0f)
			{
				yield return null;
				this._timer -= Time.deltaTime;
			}
			this.stats[StatType.MoveSpeed].AddMultiplierBonus(-1f * this.moveSpeedBoostPerLevel * (float)this.level);
			this._timer = 0f;
			yield break;
		}

		// Token: 0x0400067A RID: 1658
		[SerializeField]
		private float moveSpeedBoostPerLevel;

		// Token: 0x0400067B RID: 1659
		[SerializeField]
		private float duration;

		// Token: 0x0400067C RID: 1660
		private StatsHolder stats;

		// Token: 0x0400067D RID: 1661
		private float _timer;
	}
}
