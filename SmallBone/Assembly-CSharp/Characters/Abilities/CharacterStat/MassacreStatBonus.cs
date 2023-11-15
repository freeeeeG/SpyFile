using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C93 RID: 3219
	[Serializable]
	public sealed class MassacreStatBonus : Ability
	{
		// Token: 0x06004187 RID: 16775 RVA: 0x000BE991 File Offset: 0x000BCB91
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new MassacreStatBonus.Instance(owner, this);
		}

		// Token: 0x0400323D RID: 12861
		[SerializeField]
		private Stat.Values _statValues;

		// Token: 0x0400323E RID: 12862
		[SerializeField]
		private CharacterTypeBoolArray _targetFilter;

		// Token: 0x0400323F RID: 12863
		[SerializeField]
		private float _recoverTime;

		// Token: 0x02000C94 RID: 3220
		public class Instance : AbilityInstance<MassacreStatBonus>
		{
			// Token: 0x06004189 RID: 16777 RVA: 0x000BE99A File Offset: 0x000BCB9A
			public Instance(Character owner, MassacreStatBonus ability) : base(owner, ability)
			{
			}

			// Token: 0x0600418A RID: 16778 RVA: 0x000BE9A4 File Offset: 0x000BCBA4
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this.ability._statValues);
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}

			// Token: 0x0600418B RID: 16779 RVA: 0x000BE9F4 File Offset: 0x000BCBF4
			private void HandleOnKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._targetFilter[character.type])
				{
					return;
				}
				base.remainTime += this.ability._recoverTime;
			}

			// Token: 0x0600418C RID: 16780 RVA: 0x000BEA44 File Offset: 0x000BCC44
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._statValues);
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}
		}
	}
}
