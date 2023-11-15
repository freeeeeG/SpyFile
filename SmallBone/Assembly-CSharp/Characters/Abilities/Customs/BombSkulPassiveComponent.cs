using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D2E RID: 3374
	public class BombSkulPassiveComponent : AbilityComponent<BombSkulPassive>
	{
		// Token: 0x06004418 RID: 17432 RVA: 0x000C5E09 File Offset: 0x000C4009
		public void Explode()
		{
			this._ability.Explode();
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x000C5E16 File Offset: 0x000C4016
		public void RiskyUpgrade()
		{
			this._ability.RiskyUpgrade();
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x000C5E23 File Offset: 0x000C4023
		public void AddDamageStack(int amount)
		{
			this._ability.AddDamageStack(amount);
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x000C5E31 File Offset: 0x000C4031
		public void RegisterSmallBomb(OperationRunner smallBomb)
		{
			this._ability.RegisterSmallBomb(smallBomb);
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x000C5E3F File Offset: 0x000C403F
		private void OnDestroy()
		{
			this._ability.DetachEvent();
		}
	}
}
