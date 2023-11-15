using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D0 RID: 464
	public class SingleShotModStatAction : Action
	{
		// Token: 0x06000A4F RID: 2639 RVA: 0x000281B3 File Offset: 0x000263B3
		public override void Init()
		{
			this.player = PlayerController.Instance;
			this.myGun = PlayerController.Instance.gun;
			this._isActive = false;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000281D8 File Offset: 0x000263D8
		public override void Activate(GameObject target)
		{
			if (this._isActive)
			{
				return;
			}
			StatsHolder stats = this.player.stats;
			if (stats == null)
			{
				Debug.LogWarning("Cannot apply stat mods. No stats holder on target game object.");
				return;
			}
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					stats[statChange.type].AddFlatBonus(statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					stats[statChange.type].AddMultiplierBonus(statChange.value);
				}
				else if (statChange.value < 0f)
				{
					stats[statChange.type].AddMultiplierReduction(1f + statChange.value);
				}
			}
			this.myGun.OnShoot.AddListener(new UnityAction(this.DeActivate));
			this._isActive = true;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000282C4 File Offset: 0x000264C4
		public void DeActivate()
		{
			if (!this._isActive)
			{
				return;
			}
			StatsHolder stats = this.player.stats;
			if (stats == null)
			{
				Debug.LogWarning("Cannot apply stat mods. No stats holder on target game object.");
				return;
			}
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					stats[statChange.type].AddFlatBonus(-1 * statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					stats[statChange.type].AddMultiplierBonus(-1f * statChange.value);
				}
				else if (statChange.value < 0f)
				{
					stats[statChange.type].AddMultiplierReduction(1f - statChange.value);
				}
			}
			this.myGun.OnShoot.RemoveListener(new UnityAction(this.DeActivate));
			this._isActive = false;
		}

		// Token: 0x0400074A RID: 1866
		[SerializeField]
		private StatChange[] statChanges;

		// Token: 0x0400074B RID: 1867
		private PlayerController player;

		// Token: 0x0400074C RID: 1868
		private Gun myGun;

		// Token: 0x0400074D RID: 1869
		private bool _isActive;
	}
}
