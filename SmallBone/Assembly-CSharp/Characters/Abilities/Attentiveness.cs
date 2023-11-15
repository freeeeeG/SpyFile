using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A0D RID: 2573
	[Serializable]
	public class Attentiveness : Ability
	{
		// Token: 0x06003695 RID: 13973 RVA: 0x000A177F File Offset: 0x0009F97F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Attentiveness.Instance(owner, this);
		}

		// Token: 0x04002BB0 RID: 11184
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04002BB1 RID: 11185
		[SerializeField]
		private AttackTypeBoolArray _types;

		// Token: 0x04002BB2 RID: 11186
		[SerializeField]
		private float attackDamage;

		// Token: 0x04002BB3 RID: 11187
		[SerializeField]
		private HitInfo hitInfo;

		// Token: 0x02000A0E RID: 2574
		public class Instance : AbilityInstance<Attentiveness>
		{
			// Token: 0x06003697 RID: 13975 RVA: 0x000A1788 File Offset: 0x0009F988
			public Instance(Character owner, Attentiveness ability) : base(owner, ability)
			{
			}

			// Token: 0x06003698 RID: 13976 RVA: 0x000A179C File Offset: 0x0009F99C
			protected override void OnAttach()
			{
				base.remainTime = this.ability.duration;
				this.owner.chronometer.animation.AttachTimeScale(this, 0f);
				this.owner.stat.AttachValues(this.ability._stat);
				this.owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
				this._count = 0;
			}

			// Token: 0x06003699 RID: 13977 RVA: 0x000A1814 File Offset: 0x0009FA14
			private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this.ability._types[originalDamage.attackType])
				{
					return;
				}
				this._count++;
				if (this._count >= this._maxcount)
				{
					this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
					this.TakeDamage();
					this.owner.ability.Remove(this);
				}
			}

			// Token: 0x0600369A RID: 13978 RVA: 0x000A188C File Offset: 0x0009FA8C
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
				this.owner.stat.DetachValues(this.ability._stat);
				this.owner.chronometer.animation.DetachTimeScale(this);
			}

			// Token: 0x0600369B RID: 13979 RVA: 0x000A18E7 File Offset: 0x0009FAE7
			public override void Refresh()
			{
				this._count = 0;
				base.remainTime = this.ability.duration;
			}

			// Token: 0x0600369C RID: 13980 RVA: 0x000A1904 File Offset: 0x0009FB04
			private void TakeDamage()
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				Damage damage = player.stat.GetDamage((double)this.ability.attackDamage, this.owner.transform.position, this.ability.hitInfo);
				player.Attack(this.owner, ref damage);
			}

			// Token: 0x04002BB4 RID: 11188
			private int _count;

			// Token: 0x04002BB5 RID: 11189
			private int _maxcount = 1;
		}
	}
}
