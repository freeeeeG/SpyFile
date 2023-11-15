using System;
using UnityEngine;

namespace Characters.Abilities.Statuses
{
	// Token: 0x02000B70 RID: 2928
	public sealed class OverrideGigantBossStatusEffect : MonoBehaviour
	{
		// Token: 0x06003B0C RID: 15116 RVA: 0x000AE234 File Offset: 0x000AC434
		private void Start()
		{
			this._character.status.freeze.effectHandler.Dispose();
			this._character.status.freeze.effectHandler = StatusEffect.CopyFrom(this._gigantEnemyFreeze, this._character);
			this._character.status.burn.effectHandler.Dispose();
			this._character.status.burn.effectHandler = StatusEffect.CopyFrom(this._burn, this._character);
			this._character.status.poison.effectHandler.Dispose();
			this._character.status.poison.effectHandler = StatusEffect.CopyFrom(this._poison, this._character);
			this._character.status.wound.effectHandler.Dispose();
			this._character.status.wound.effectHandler = StatusEffect.CopyFrom(this._wound, this._character);
			this._character.status.stun.effectHandler.Dispose();
			this._character.status.stun.effectHandler = StatusEffect.CopyFrom(this._stun, this._character);
		}

		// Token: 0x04002EB1 RID: 11953
		[SerializeField]
		private Character _character;

		// Token: 0x04002EB2 RID: 11954
		[SerializeField]
		private StatusEffect.GigantEnemyFreeze _gigantEnemyFreeze;

		// Token: 0x04002EB3 RID: 11955
		[SerializeField]
		private StatusEffect.Burn _burn;

		// Token: 0x04002EB4 RID: 11956
		[SerializeField]
		private StatusEffect.Poison _poison;

		// Token: 0x04002EB5 RID: 11957
		[SerializeField]
		private StatusEffect.Wound _wound;

		// Token: 0x04002EB6 RID: 11958
		[SerializeField]
		private StatusEffect.Stun _stun;
	}
}
