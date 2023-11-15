using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EB4 RID: 3764
	public class ByAbility : CharacterOperation
	{
		// Token: 0x06004A0D RID: 18957 RVA: 0x000D8464 File Offset: 0x000D6664
		public override void Run(Character owner)
		{
			CharacterOperation.Subcomponents subcomponents;
			if (owner.ability.Contains(this._ability.ability))
			{
				subcomponents = this._onAttached;
			}
			else
			{
				subcomponents = this._onDetached;
			}
			if (subcomponents == null)
			{
				return;
			}
			subcomponents.Run(owner);
		}

		// Token: 0x04003943 RID: 14659
		[SerializeField]
		private AbilityComponent _ability;

		// Token: 0x04003944 RID: 14660
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onAttached;

		// Token: 0x04003945 RID: 14661
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _onDetached;
	}
}
