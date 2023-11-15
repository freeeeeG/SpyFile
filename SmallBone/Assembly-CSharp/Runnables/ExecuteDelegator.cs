using System;
using Characters;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200031F RID: 799
	[Serializable]
	public class ExecuteDelegator : IHitEvent, IStatusEvent
	{
		// Token: 0x06000F69 RID: 3945 RVA: 0x0002EF90 File Offset: 0x0002D190
		public void OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			Runnable[] runnables = this._runnables;
			for (int i = 0; i < runnables.Length; i++)
			{
				runnables[i].Run();
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0002EFBC File Offset: 0x0002D1BC
		public void Apply(Character owner, Character target)
		{
			Runnable[] runnables = this._runnables;
			for (int i = 0; i < runnables.Length; i++)
			{
				runnables[i].Run();
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00002191 File Offset: 0x00000391
		public void Release(Character owner, Character target)
		{
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0002EFE6 File Offset: 0x0002D1E6
		void IHitEvent.OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this.OnHit(originalDamage, tookDamage, damageDealt);
		}

		// Token: 0x04000CB3 RID: 3251
		[SerializeField]
		private Runnable[] _runnables;
	}
}
