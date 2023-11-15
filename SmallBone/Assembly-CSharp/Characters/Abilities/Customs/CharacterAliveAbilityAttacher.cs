using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D12 RID: 3346
	public sealed class CharacterAliveAbilityAttacher : AbilityAttacher
	{
		// Token: 0x06004381 RID: 17281 RVA: 0x000C4A3B File Offset: 0x000C2C3B
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x000C4A48 File Offset: 0x000C2C48
		public override void StartAttach()
		{
			if (this._target.health.dead)
			{
				return;
			}
			base.owner.ability.Add(this._abilityComponent.ability);
			this._target.health.onDiedTryCatch += this.StopAttach;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x000C4AA1 File Offset: 0x000C2CA1
		public override void StopAttach()
		{
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x04003392 RID: 13202
		[SerializeField]
		private Character _target;

		// Token: 0x04003393 RID: 13203
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;
	}
}
