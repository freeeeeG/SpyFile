using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000ABF RID: 2751
	[Serializable]
	public class StatusLevelBonus : Ability
	{
		// Token: 0x060038A0 RID: 14496 RVA: 0x000A6FF0 File Offset: 0x000A51F0
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatusLevelBonus.Instance(owner, this);
		}

		// Token: 0x04002D0E RID: 11534
		[SerializeField]
		private int _level = 1;

		// Token: 0x02000AC0 RID: 2752
		public class Instance : AbilityInstance<StatusLevelBonus>
		{
			// Token: 0x060038A2 RID: 14498 RVA: 0x000A7008 File Offset: 0x000A5208
			public Instance(Character owner, StatusLevelBonus ability) : base(owner, ability)
			{
				this._characterStatus = owner.GetComponent<CharacterStatus>();
			}

			// Token: 0x060038A3 RID: 14499 RVA: 0x000A701E File Offset: 0x000A521E
			protected override void OnAttach()
			{
				this._characterStatus.gradeBonuses.Add(this, this.ability._level);
			}

			// Token: 0x060038A4 RID: 14500 RVA: 0x000A703C File Offset: 0x000A523C
			protected override void OnDetach()
			{
				this._characterStatus.gradeBonuses.Remove(this);
			}

			// Token: 0x04002D0F RID: 11535
			private CharacterStatus _characterStatus;
		}
	}
}
