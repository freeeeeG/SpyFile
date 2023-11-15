using System;
using Characters.Abilities.Weapons.Wizard;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Mage
{
	// Token: 0x02001006 RID: 4102
	public sealed class TryReduceMana : CharacterOperation
	{
		// Token: 0x06004F41 RID: 20289 RVA: 0x000EE5C7 File Offset: 0x000EC7C7
		public override void Run(Character owner)
		{
			this._passive.TryReduceMana(this._value);
		}

		// Token: 0x04003F63 RID: 16227
		[SerializeField]
		private WizardPassiveComponent _passive;

		// Token: 0x04003F64 RID: 16228
		[SerializeField]
		private float _value;
	}
}
