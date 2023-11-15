using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000DE RID: 222
	public class DmgAndMSOnNotHurt : MonoBehaviour
	{
		// Token: 0x060006A6 RID: 1702 RVA: 0x0001DE34 File Offset: 0x0001C034
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.spriteTrail = componentInParent.playerSprite.GetComponentInChildren<SpriteTrail>();
			this.stats = componentInParent.stats;
			this.health = componentInParent.playerHealth;
			this.health.onHurt.AddListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001DE92 File Offset: 0x0001C092
		private void OnDestroy()
		{
			this.health.onHurt.RemoveListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001DEB0 File Offset: 0x0001C0B0
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
				this.stats[StatType.BulletDamage].AddMultiplierBonus(this.damageBoostPerTick);
				this.stats[StatType.MoveSpeed].AddMultiplierBonus(this.movespeedBoostPerTick);
				if (this._ticks >= this.maxTicks / 2)
				{
					this.spriteTrail.SetEnabled(true);
				}
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001DF58 File Offset: 0x0001C158
		private void OnHurt()
		{
			this.stats[StatType.BulletDamage].AddMultiplierBonus((float)(-1 * this._ticks) * this.damageBoostPerTick);
			this.stats[StatType.MoveSpeed].AddMultiplierBonus((float)(-1 * this._ticks) * this.movespeedBoostPerTick);
			this.spriteTrail.SetEnabled(false);
			this._ticks = 0;
			this._timer = 0f;
		}

		// Token: 0x04000472 RID: 1138
		[SerializeField]
		private float damageBoostPerTick;

		// Token: 0x04000473 RID: 1139
		[SerializeField]
		private float movespeedBoostPerTick;

		// Token: 0x04000474 RID: 1140
		[SerializeField]
		private float secsPerTick;

		// Token: 0x04000475 RID: 1141
		[SerializeField]
		private int maxTicks;

		// Token: 0x04000476 RID: 1142
		private SpriteTrail spriteTrail;

		// Token: 0x04000477 RID: 1143
		private StatsHolder stats;

		// Token: 0x04000478 RID: 1144
		private PlayerHealth health;

		// Token: 0x04000479 RID: 1145
		private int _ticks;

		// Token: 0x0400047A RID: 1146
		private float _timer;
	}
}
