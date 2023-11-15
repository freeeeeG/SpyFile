using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DA9 RID: 3497
	public class ApplyStatus : CharacterOperation
	{
		// Token: 0x06004669 RID: 18025 RVA: 0x000CB687 File Offset: 0x000C9887
		public override void Run(Character owner, Character target)
		{
			if (MMMaths.PercentChance(this._chance))
			{
				owner.GiveStatus(target, this._status);
			}
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x000CB6A4 File Offset: 0x000C98A4
		public override void Run(Character target)
		{
			if (MMMaths.PercentChance(this._chance))
			{
				CharacterStatus status = target.status;
				if (status == null)
				{
					return;
				}
				status.Apply(null, this._status);
			}
		}

		// Token: 0x04003551 RID: 13649
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x04003552 RID: 13650
		[Range(1f, 100f)]
		[SerializeField]
		private int _chance = 100;
	}
}
