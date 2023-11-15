using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000E1 RID: 225
	public class FireRateAndPierceOnNotHurt : MonoBehaviour
	{
		// Token: 0x060006B4 RID: 1716 RVA: 0x0001E158 File Offset: 0x0001C358
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this.health = componentInParent.playerHealth;
			this.health.onHurt.AddListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001E1A5 File Offset: 0x0001C3A5
		private void OnDestroy()
		{
			this.health.onHurt.RemoveListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001E1C4 File Offset: 0x0001C3C4
		private void Update()
		{
			if (this._ticks < this.maxTicks)
			{
				this._timer += Time.deltaTime;
			}
			if (this._timer >= this.secsPerTick)
			{
				this._timer -= this.secsPerTick;
				this._ticks++;
				this.stats[StatType.FireRate].AddMultiplierBonus(this.fireRateBoostPerTick);
				this.stats[StatType.Piercing].AddFlatBonus(this.pierceBoostPerTick);
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001E250 File Offset: 0x0001C450
		private void OnHurt()
		{
			this.stats[StatType.FireRate].AddMultiplierBonus((float)(-1 * this._ticks) * this.fireRateBoostPerTick);
			this.stats[StatType.Piercing].AddFlatBonus(-1 * this._ticks * this.pierceBoostPerTick);
			this._ticks = 0;
			this._timer = 0f;
		}

		// Token: 0x04000480 RID: 1152
		[SerializeField]
		private float fireRateBoostPerTick;

		// Token: 0x04000481 RID: 1153
		[SerializeField]
		private int pierceBoostPerTick;

		// Token: 0x04000482 RID: 1154
		[SerializeField]
		private float secsPerTick;

		// Token: 0x04000483 RID: 1155
		[SerializeField]
		private int maxTicks;

		// Token: 0x04000484 RID: 1156
		private StatsHolder stats;

		// Token: 0x04000485 RID: 1157
		private PlayerHealth health;

		// Token: 0x04000486 RID: 1158
		private int _ticks;

		// Token: 0x04000487 RID: 1159
		private float _timer;
	}
}
