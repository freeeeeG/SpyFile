using System;
using System.Collections;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009F6 RID: 2550
	public sealed class MaxStatStackAbilityAttacher : AbilityAttacher
	{
		// Token: 0x06003632 RID: 13874 RVA: 0x000A0BBC File Offset: 0x0009EDBC
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000A0BC9 File Offset: 0x0009EDC9
		public override void StartAttach()
		{
			this._cUpdateReference = this.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000A0BDD File Offset: 0x0009EDDD
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			this._cUpdateReference.Stop();
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000A0C15 File Offset: 0x0009EE15
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				if (base.owner.ability.Contains(this._stackableStatBonusComponent.ability) && this._stackableStatBonusComponent.isMax)
				{
					if (!this._attached)
					{
						this._attached = true;
						base.owner.ability.Add(this._abilityComponent.ability);
					}
				}
				else
				{
					this._attached = false;
					base.owner.ability.Remove(this._abilityComponent.ability);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B82 RID: 11138
		[SerializeField]
		private StackableStatBonusComponent _stackableStatBonusComponent;

		// Token: 0x04002B83 RID: 11139
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B84 RID: 11140
		private CoroutineReference _cUpdateReference;

		// Token: 0x04002B85 RID: 11141
		private bool _attached;
	}
}
