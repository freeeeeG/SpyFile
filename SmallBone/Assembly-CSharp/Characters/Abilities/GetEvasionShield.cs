using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A32 RID: 2610
	[Serializable]
	public sealed class GetEvasionShield : Ability
	{
		// Token: 0x06003705 RID: 14085 RVA: 0x000A2A98 File Offset: 0x000A0C98
		public override void Initialize()
		{
			base.Initialize();
			this._onEvade.Initialize();
			this._onBroken.Initialize();
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x000A2AB6 File Offset: 0x000A0CB6
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetEvasionShield.Instance(owner, this);
		}

		// Token: 0x04002BEF RID: 11247
		[SerializeField]
		private int _amount;

		// Token: 0x04002BF0 RID: 11248
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onEvade;

		// Token: 0x04002BF1 RID: 11249
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onBroken;

		// Token: 0x02000A33 RID: 2611
		public sealed class Instance : AbilityInstance<GetEvasionShield>
		{
			// Token: 0x06003708 RID: 14088 RVA: 0x000A2ABF File Offset: 0x000A0CBF
			public Instance(Character owner, GetEvasionShield ability) : base(owner, ability)
			{
			}

			// Token: 0x06003709 RID: 14089 RVA: 0x000A2AC9 File Offset: 0x000A0CC9
			protected override void OnAttach()
			{
				this._remainAmount = this.ability._amount;
				this.owner.evasion.Attach(this);
				this.owner.onEvade += this.OnEvade;
			}

			// Token: 0x0600370A RID: 14090 RVA: 0x000A2B04 File Offset: 0x000A0D04
			private void OnEvade(ref Damage damage)
			{
				this.ability._onEvade.Run(this.owner);
				this._remainAmount--;
				if (this._remainAmount <= 0)
				{
					this.owner.ability.Remove(this);
				}
			}

			// Token: 0x0600370B RID: 14091 RVA: 0x000A2B50 File Offset: 0x000A0D50
			protected override void OnDetach()
			{
				this.ability._onBroken.Run(this.owner);
				this.owner.evasion.Detach(this);
				this.owner.onEvade -= this.OnEvade;
			}

			// Token: 0x04002BF2 RID: 11250
			private int _remainAmount;
		}
	}
}
