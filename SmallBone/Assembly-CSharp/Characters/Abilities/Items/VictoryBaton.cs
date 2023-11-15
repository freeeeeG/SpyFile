using System;
using Characters.Abilities.CharacterStat;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000D08 RID: 3336
	[Serializable]
	public sealed class VictoryBaton : Ability
	{
		// Token: 0x06004347 RID: 17223 RVA: 0x000C448B File Offset: 0x000C268B
		public override void Initialize()
		{
			base.Initialize();
			this._statBonusComponent.Initialize();
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x000C449E File Offset: 0x000C269E
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new VictoryBaton.Instance(owner, this);
		}

		// Token: 0x04003373 RID: 13171
		[SerializeField]
		[Subcomponent(typeof(StatBonusComponent))]
		private StatBonusComponent _statBonusComponent;

		// Token: 0x02000D09 RID: 3337
		public class Instance : AbilityInstance<VictoryBaton>
		{
			// Token: 0x0600434A RID: 17226 RVA: 0x000C44A7 File Offset: 0x000C26A7
			public Instance(Character owner, VictoryBaton ability) : base(owner, ability)
			{
			}

			// Token: 0x0600434B RID: 17227 RVA: 0x000C44B4 File Offset: 0x000C26B4
			protected override void OnAttach()
			{
				this.owner.ability.Add(this.ability._statBonusComponent.ability);
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}

			// Token: 0x0600434C RID: 17228 RVA: 0x000C4509 File Offset: 0x000C2709
			private void HandleOnKilled(ITarget target, ref Damage damage)
			{
				base.remainTime = this.ability.duration;
			}

			// Token: 0x0600434D RID: 17229 RVA: 0x000C451C File Offset: 0x000C271C
			protected override void OnDetach()
			{
				this.owner.ability.Remove(this.ability._statBonusComponent.ability);
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}
		}
	}
}
