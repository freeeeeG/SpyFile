using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008EA RID: 2282
	public sealed class AttachAbility : QuintessenceEffect
	{
		// Token: 0x060030CA RID: 12490 RVA: 0x00092056 File Offset: 0x00090256
		private void Awake()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x00092064 File Offset: 0x00090264
		protected override void OnInvoke(Quintessence quintessence)
		{
			Character owner = quintessence.owner;
			owner.ability.Add(this._abilityComponent.ability);
			if (owner != this._owner)
			{
				quintessence.onDropped += this.Detach;
			}
			this._owner = owner;
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x000920B6 File Offset: 0x000902B6
		private void Detach()
		{
			this._owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0400283D RID: 10301
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x0400283E RID: 10302
		private Character _owner;
	}
}
