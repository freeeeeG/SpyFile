using System;
using Characters.Gear.Quintessences;
using UnityEngine;

namespace Characters.Operations.Essences
{
	// Token: 0x02000EAF RID: 3759
	public sealed class ApplyStatusFromEssenceOwner : CharacterOperation
	{
		// Token: 0x060049F7 RID: 18935 RVA: 0x000D7E2C File Offset: 0x000D602C
		public override void Run(Character owner, Character target)
		{
			if (MMMaths.PercentChance(this._chance))
			{
				this._essence.owner.GiveStatus(target, this._status);
			}
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x000D7E53 File Offset: 0x000D6053
		public override void Run(Character target)
		{
			if (MMMaths.PercentChance(this._chance))
			{
				CharacterStatus status = this._essence.owner.status;
				if (status == null)
				{
					return;
				}
				status.Apply(null, this._status);
			}
		}

		// Token: 0x0400392F RID: 14639
		[SerializeField]
		private Quintessence _essence;

		// Token: 0x04003930 RID: 14640
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x04003931 RID: 14641
		[Range(1f, 100f)]
		[SerializeField]
		private int _chance = 100;
	}
}
