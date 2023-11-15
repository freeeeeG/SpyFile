using System;
using UnityEngine;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x02000967 RID: 2407
	public class Damage : Basic
	{
		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x060033E6 RID: 13286 RVA: 0x00099E1B File Offset: 0x0009801B
		public override float remainPercent
		{
			get
			{
				if (base.stacks <= 0)
				{
					return 1f - (float)this._stackedDamage / this._damagePerStack;
				}
				return 1f;
			}
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x00099E40 File Offset: 0x00098040
		protected virtual void OnEnable()
		{
			if (this._stackOnGive)
			{
				Character character = this._character;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.StackDamage));
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x00099E71 File Offset: 0x00098071
		protected virtual void OnDisable()
		{
			if (this._stackOnGive)
			{
				Character character = this._character;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(this.StackDamage));
			}
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x00099E40 File Offset: 0x00098040
		protected virtual void OnResume()
		{
			if (this._stackOnGive)
			{
				Character character = this._character;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.StackDamage));
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x00099EA4 File Offset: 0x000980A4
		private void StackDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (base.stacks == this._maxStacks)
			{
				return;
			}
			this._stackedDamage += damageDealt;
			if (this._stackedDamage >= (double)this._damagePerStack)
			{
				this._stackedDamage = 0.0;
				int stacks = base.stacks;
				base.stacks = stacks + 1;
			}
		}

		// Token: 0x04002A0A RID: 10762
		[SerializeField]
		protected float _damagePerStack;

		// Token: 0x04002A0B RID: 10763
		[SerializeField]
		[ReadOnly(true)]
		protected bool _stackOnGive;

		// Token: 0x04002A0C RID: 10764
		[SerializeField]
		[ReadOnly(true)]
		protected bool _stackOnTake;

		// Token: 0x04002A0D RID: 10765
		protected double _stackedDamage;
	}
}
