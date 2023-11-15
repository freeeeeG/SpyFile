using System;
using Characters.Actions;
using FX;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D63 RID: 3427
	[Serializable]
	public class MagesManaBracelet : Ability
	{
		// Token: 0x0600451A RID: 17690 RVA: 0x000C8AA9 File Offset: 0x000C6CA9
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new MagesManaBracelet.Instance(owner, this);
		}

		// Token: 0x0400349D RID: 13469
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x0400349E RID: 13470
		[SerializeField]
		private EffectInfo _buffEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400349F RID: 13471
		[SerializeField]
		private float _buffDuration;

		// Token: 0x040034A0 RID: 13472
		[SerializeField]
		private int _count;

		// Token: 0x02000D64 RID: 3428
		public class Instance : AbilityInstance<MagesManaBracelet>
		{
			// Token: 0x17000E5C RID: 3676
			// (get) Token: 0x0600451C RID: 17692 RVA: 0x000C8ACC File Offset: 0x000C6CCC
			public override int iconStacks
			{
				get
				{
					return this._currentCount;
				}
			}

			// Token: 0x17000E5D RID: 3677
			// (get) Token: 0x0600451D RID: 17693 RVA: 0x000C8AD4 File Offset: 0x000C6CD4
			public override float iconFillAmount
			{
				get
				{
					if (!this._buffAttached)
					{
						return 1f;
					}
					return 1f - this._remainBuffTime / this.ability._buffDuration;
				}
			}

			// Token: 0x0600451E RID: 17694 RVA: 0x000C8AFC File Offset: 0x000C6CFC
			public Instance(Character owner, MagesManaBracelet ability) : base(owner, ability)
			{
			}

			// Token: 0x0600451F RID: 17695 RVA: 0x000C8B06 File Offset: 0x000C6D06
			protected override void OnAttach()
			{
				this._buffAttached = true;
				this.owner.onStartAction += this.OnOwnerStartAction;
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x06004520 RID: 17696 RVA: 0x000C8B44 File Offset: 0x000C6D44
			protected override void OnDetach()
			{
				this._buffAttached = false;
				this.owner.onStartAction -= this.OnOwnerStartAction;
				this.owner.stat.DetachValues(this.ability._stat);
				this.DetachBuff();
			}

			// Token: 0x06004521 RID: 17697 RVA: 0x000C8B90 File Offset: 0x000C6D90
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainBuffTime -= deltaTime;
				if (this._remainBuffTime < 0f)
				{
					this.DetachBuff();
				}
			}

			// Token: 0x06004522 RID: 17698 RVA: 0x000C8BBC File Offset: 0x000C6DBC
			private void AttachBuff()
			{
				this._remainBuffTime = this.ability._buffDuration;
				this._currentCount = 0;
				if (this._buffAttached)
				{
					return;
				}
				this._buffAttached = true;
				this.owner.stat.AttachValues(this.ability._stat);
				this._buffEffect = ((this.ability._buffEffect == null) ? null : this.ability._buffEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
			}

			// Token: 0x06004523 RID: 17699 RVA: 0x000C8C54 File Offset: 0x000C6E54
			private void DetachBuff()
			{
				this._buffAttached = false;
				this.owner.stat.DetachValues(this.ability._stat);
				if (this._buffEffect != null)
				{
					this._buffEffect.Stop();
					this._buffEffect = null;
				}
			}

			// Token: 0x06004524 RID: 17700 RVA: 0x000C8CA4 File Offset: 0x000C6EA4
			private void OnOwnerStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Skill)
				{
					return;
				}
				if (action.cooldown.usedByStreak)
				{
					return;
				}
				this._currentCount++;
				if (this._currentCount == this.ability._count)
				{
					this.AttachBuff();
				}
			}

			// Token: 0x040034A1 RID: 13473
			private EffectPoolInstance _buffEffect;

			// Token: 0x040034A2 RID: 13474
			private float _remainBuffTime;

			// Token: 0x040034A3 RID: 13475
			private int _currentCount;

			// Token: 0x040034A4 RID: 13476
			private bool _buffAttached;
		}
	}
}
