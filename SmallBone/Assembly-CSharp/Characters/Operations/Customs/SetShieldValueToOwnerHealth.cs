using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FEB RID: 4075
	public class SetShieldValueToOwnerHealth : CharacterOperation
	{
		// Token: 0x06004EBC RID: 20156 RVA: 0x000EC389 File Offset: 0x000EA589
		public override void Run(Character owner)
		{
			this._shieldComponent.baseAbility.amount = (float)owner.health.maximumHealth;
		}

		// Token: 0x04003ED5 RID: 16085
		[SerializeField]
		private ShieldComponent _shieldComponent;
	}
}
