using System;
using UnityEngine;

namespace Characters.Abilities.Decorators
{
	// Token: 0x02000B9E RID: 2974
	[Serializable]
	public sealed class RandomAbilities : Ability
	{
		// Token: 0x06003D88 RID: 15752 RVA: 0x000B2EF8 File Offset: 0x000B10F8
		public override void Initialize()
		{
			base.Initialize();
			this._abilityComponents.Initialize();
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x000B2F0B File Offset: 0x000B110B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RandomAbilities.Instance(owner, this);
		}

		// Token: 0x04002F96 RID: 12182
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x04002F97 RID: 12183
		[SerializeField]
		private bool _removeAbilityOnRefresh;

		// Token: 0x02000B9F RID: 2975
		public sealed class Instance : AbilityInstance<RandomAbilities>
		{
			// Token: 0x06003D8B RID: 15755 RVA: 0x000B2F14 File Offset: 0x000B1114
			public Instance(Character owner, RandomAbilities ability) : base(owner, ability)
			{
			}

			// Token: 0x06003D8C RID: 15756 RVA: 0x000B2F1E File Offset: 0x000B111E
			protected override void OnAttach()
			{
				this._target = this.ability._abilityComponents.components.Random<AbilityComponent>();
				this.owner.ability.Add(this._target.ability);
			}

			// Token: 0x06003D8D RID: 15757 RVA: 0x000B2F58 File Offset: 0x000B1158
			public override void Refresh()
			{
				base.Refresh();
				if (this.ability._removeAbilityOnRefresh)
				{
					this.owner.ability.Remove(this._target.ability);
				}
				this._target = this.ability._abilityComponents.components.Random<AbilityComponent>();
				this.owner.ability.Add(this._target.ability);
			}

			// Token: 0x06003D8E RID: 15758 RVA: 0x000B2FCB File Offset: 0x000B11CB
			protected override void OnDetach()
			{
				this.owner.ability.Remove(this._target.ability);
			}

			// Token: 0x04002F98 RID: 12184
			private AbilityComponent _target;
		}
	}
}
