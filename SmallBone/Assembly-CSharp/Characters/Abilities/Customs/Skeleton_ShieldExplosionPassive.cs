using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D8E RID: 3470
	[Serializable]
	public class Skeleton_ShieldExplosionPassive : Ability
	{
		// Token: 0x060045DF RID: 17887 RVA: 0x000CA751 File Offset: 0x000C8951
		public override void Initialize()
		{
			base.Initialize();
			this._operation.Initialize();
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x000CA764 File Offset: 0x000C8964
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Skeleton_ShieldExplosionPassive.Instance(owner, this);
		}

		// Token: 0x0400350F RID: 13583
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operation;

		// Token: 0x04003510 RID: 13584
		[NonSerialized]
		public Skeleton_ShieldExplosionPassiveComponent component;

		// Token: 0x02000D8F RID: 3471
		public class Instance : AbilityInstance<Skeleton_ShieldExplosionPassive>
		{
			// Token: 0x060045E2 RID: 17890 RVA: 0x000CA76D File Offset: 0x000C896D
			public Instance(Character owner, Skeleton_ShieldExplosionPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x060045E3 RID: 17891 RVA: 0x000CA778 File Offset: 0x000C8978
			protected override void OnAttach()
			{
				this.owner.health.shield.onUpdate += this.OnShieldRemoveOrUpdate;
				this.owner.health.shield.onRemove += this.OnShieldRemoveOrUpdate;
			}

			// Token: 0x060045E4 RID: 17892 RVA: 0x000CA7C8 File Offset: 0x000C89C8
			protected override void OnDetach()
			{
				this.owner.health.shield.onUpdate -= this.OnShieldRemoveOrUpdate;
				this.owner.health.shield.onRemove -= this.OnShieldRemoveOrUpdate;
			}

			// Token: 0x060045E5 RID: 17893 RVA: 0x000CA817 File Offset: 0x000C8A17
			private void OnShieldRemoveOrUpdate(Shield.Instance shieldInstance)
			{
				this.ability.component.attackDamage = (float)shieldInstance.originalAmount;
				this.ability._operation.Run(this.owner);
			}
		}
	}
}
