using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A23 RID: 2595
	[Serializable]
	public class EnhanceComboAction : Ability
	{
		// Token: 0x060036E6 RID: 14054 RVA: 0x000A274C File Offset: 0x000A094C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new EnhanceComboAction.Instance(owner, this);
		}

		// Token: 0x04002BE4 RID: 11236
		[SerializeField]
		private EnhanceableComboAction _enhanceableComboAction;

		// Token: 0x02000A24 RID: 2596
		public class Instance : AbilityInstance<EnhanceComboAction>
		{
			// Token: 0x060036E7 RID: 14055 RVA: 0x000A2755 File Offset: 0x000A0955
			public Instance(Character owner, EnhanceComboAction ability) : base(owner, ability)
			{
			}

			// Token: 0x060036E8 RID: 14056 RVA: 0x000A275F File Offset: 0x000A095F
			protected override void OnAttach()
			{
				this.ability._enhanceableComboAction.enhanced = true;
			}

			// Token: 0x060036E9 RID: 14057 RVA: 0x000A2772 File Offset: 0x000A0972
			protected override void OnDetach()
			{
				this.ability._enhanceableComboAction.enhanced = false;
			}
		}
	}
}
