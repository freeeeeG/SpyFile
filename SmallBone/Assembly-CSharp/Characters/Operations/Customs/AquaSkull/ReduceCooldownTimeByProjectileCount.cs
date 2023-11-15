using System;
using Characters.Actions;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull
{
	// Token: 0x0200102C RID: 4140
	public class ReduceCooldownTimeByProjectileCount : Operation
	{
		// Token: 0x06004FC1 RID: 20417 RVA: 0x000F0AC0 File Offset: 0x000EECC0
		public override void Run()
		{
			int num = 0;
			foreach (Projectile projectile in this._projectilesToCount)
			{
				num += projectile.reusable.spawnedCount;
			}
			int num2 = Mathf.Clamp(num, 0, this._reducePercentByCount.Length - 1);
			Characters.Actions.Action[] actions = this._actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].cooldown.time.ReduceCooldownPercent(this._reducePercentByCount[num2] / 100f);
			}
		}

		// Token: 0x0400403B RID: 16443
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x0400403C RID: 16444
		[SerializeField]
		private Projectile[] _projectilesToCount;

		// Token: 0x0400403D RID: 16445
		[SerializeField]
		[Tooltip("0~100사이 값")]
		private float[] _reducePercentByCount;
	}
}
