using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000249 RID: 585
	public class BuffWhenStanding : MonoBehaviour
	{
		// Token: 0x06000CC2 RID: 3266 RVA: 0x0002E790 File Offset: 0x0002C990
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.stats = componentInParent.stats;
			this._lastFramePos = base.transform.position;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002E7C4 File Offset: 0x0002C9C4
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer >= this.secsPerTick)
			{
				this._timer -= this.secsPerTick;
				this.IncrementBuff();
			}
			if (this._lastFramePos != base.transform.position)
			{
				this.ResetBuff();
			}
			this._lastFramePos = base.transform.position;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0002E839 File Offset: 0x0002CA39
		private void ResetBuff()
		{
			this.stats[StatType.BulletDamage].AddMultiplierBonus((float)(-1 * this._ticks) * this.damageBoostPerTick);
			this._ticks = 0;
			this._timer = 0f;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0002E86E File Offset: 0x0002CA6E
		private void IncrementBuff()
		{
			if (this._ticks < this.maxTicks)
			{
				this.stats[StatType.BulletDamage].AddMultiplierBonus(this.damageBoostPerTick);
				this._ticks++;
			}
		}

		// Token: 0x040008F8 RID: 2296
		[SerializeField]
		private float damageBoostPerTick;

		// Token: 0x040008F9 RID: 2297
		[SerializeField]
		private float secsPerTick;

		// Token: 0x040008FA RID: 2298
		[SerializeField]
		private int maxTicks;

		// Token: 0x040008FB RID: 2299
		private Vector3 _lastFramePos;

		// Token: 0x040008FC RID: 2300
		private StatsHolder stats;

		// Token: 0x040008FD RID: 2301
		private int _ticks;

		// Token: 0x040008FE RID: 2302
		private float _timer;
	}
}
