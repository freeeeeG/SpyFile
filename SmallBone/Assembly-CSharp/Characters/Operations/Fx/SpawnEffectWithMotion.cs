using System;
using FX;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F1F RID: 3871
	public class SpawnEffectWithMotion : CharacterOperation
	{
		// Token: 0x06004B85 RID: 19333 RVA: 0x000DE47D File Offset: 0x000DC67D
		private void Awake()
		{
			if (this._spawnPosition == null)
			{
				this._spawnPosition = base.transform;
			}
			this._info.loop = true;
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x000DE4A8 File Offset: 0x000DC6A8
		public override void Run(Character owner)
		{
			this._owner = owner;
			float duration = this._info.duration;
			this._info.duration = 0f;
			this._spawned = this._info.Spawn(this._spawnPosition.position, owner, 0f, 1f);
			owner.health.onTookDamage += new TookDamageDelegate(this.Health_onTookDamage);
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x000DE516 File Offset: 0x000DC716
		private void Health_onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._spawned.Stop();
			this._owner.health.onTookDamage -= new TookDamageDelegate(this.Health_onTookDamage);
		}

		// Token: 0x04003AC0 RID: 15040
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003AC1 RID: 15041
		[SerializeField]
		private EffectInfo _info;

		// Token: 0x04003AC2 RID: 15042
		private Character _owner;

		// Token: 0x04003AC3 RID: 15043
		private EffectPoolInstance _spawned;
	}
}
