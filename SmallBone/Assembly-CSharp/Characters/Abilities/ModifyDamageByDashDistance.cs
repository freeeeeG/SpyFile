using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A62 RID: 2658
	[Serializable]
	public sealed class ModifyDamageByDashDistance : Ability
	{
		// Token: 0x06003785 RID: 14213 RVA: 0x000A3C03 File Offset: 0x000A1E03
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByDashDistance.Instance(owner, this);
		}

		// Token: 0x04002C32 RID: 11314
		[SerializeField]
		private float _buffDuration;

		// Token: 0x04002C33 RID: 11315
		[SerializeField]
		private float _maxDistance;

		// Token: 0x04002C34 RID: 11316
		[SerializeField]
		[Information("Percent, 100% = 100", InformationAttribute.InformationType.Info, false)]
		private float _maxStatBonusValue;

		// Token: 0x02000A63 RID: 2659
		private class Instance : AbilityInstance<ModifyDamageByDashDistance>
		{
			// Token: 0x17000BB4 RID: 2996
			// (get) Token: 0x06003787 RID: 14215 RVA: 0x000A3C0C File Offset: 0x000A1E0C
			public override Sprite icon
			{
				get
				{
					if (!this._attached)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000BB5 RID: 2997
			// (get) Token: 0x06003788 RID: 14216 RVA: 0x000A3C1E File Offset: 0x000A1E1E
			public override float iconFillAmount
			{
				get
				{
					return this._buffRemainTime / this.ability._buffDuration;
				}
			}

			// Token: 0x17000BB6 RID: 2998
			// (get) Token: 0x06003789 RID: 14217 RVA: 0x000A3C32 File Offset: 0x000A1E32
			public override int iconStacks
			{
				get
				{
					return (int)this._attachedValue;
				}
			}

			// Token: 0x0600378A RID: 14218 RVA: 0x000A3C3B File Offset: 0x000A1E3B
			public Instance(Character owner, ModifyDamageByDashDistance ability) : base(owner, ability)
			{
			}

			// Token: 0x0600378B RID: 14219 RVA: 0x000A3C74 File Offset: 0x000A1E74
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.OnStartAction;
				this.owner.onEndAction += this.OnEndAction;
				this._buffRemainTime = 0f;
				this._attached = false;
			}

			// Token: 0x0600378C RID: 14220 RVA: 0x000A3CC1 File Offset: 0x000A1EC1
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (!this._attached)
				{
					return;
				}
				this._buffRemainTime -= deltaTime;
				if (this._buffRemainTime <= 0f)
				{
					this.DetachStat();
				}
			}

			// Token: 0x0600378D RID: 14221 RVA: 0x000A3CF4 File Offset: 0x000A1EF4
			private void OnStartAction(Characters.Actions.Action obj)
			{
				if (this._dashing && obj.type != Characters.Actions.Action.Type.Dash)
				{
					this._dashing = false;
					this._end = this.owner.transform.position.x;
					this.AttachStat();
				}
				if (obj.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				this._dashing = true;
				this._start = this.owner.transform.position.x;
			}

			// Token: 0x0600378E RID: 14222 RVA: 0x000A3D64 File Offset: 0x000A1F64
			private void OnEndAction(Characters.Actions.Action obj)
			{
				if (obj.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				this._dashing = false;
				this._end = this.owner.transform.position.x;
				this.AttachStat();
			}

			// Token: 0x0600378F RID: 14223 RVA: 0x000A3D97 File Offset: 0x000A1F97
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.OnStartAction;
				this.owner.onEndAction -= this.OnEndAction;
				this.DetachStat();
			}

			// Token: 0x06003790 RID: 14224 RVA: 0x000A3DD0 File Offset: 0x000A1FD0
			private void AttachStat()
			{
				float num = Mathf.Abs(this._start - this._end);
				this._attachedValue = Mathf.Lerp(0f, this.ability._maxStatBonusValue, num / this.ability._maxDistance);
				for (int i = 0; i < this._stats.values.Length; i++)
				{
					this._stats.values[i].value = (double)this._attachedValue;
				}
				this.owner.stat.AttachOrUpdateValues(this._stats);
				this._attached = true;
				this._buffRemainTime = this.ability._buffDuration;
			}

			// Token: 0x06003791 RID: 14225 RVA: 0x000A3E77 File Offset: 0x000A2077
			private void DetachStat()
			{
				this.owner.stat.DetachValues(this._stats);
				this._attached = false;
			}

			// Token: 0x04002C35 RID: 11317
			private bool _dashing;

			// Token: 0x04002C36 RID: 11318
			private float _start;

			// Token: 0x04002C37 RID: 11319
			private float _end;

			// Token: 0x04002C38 RID: 11320
			private float _buffRemainTime;

			// Token: 0x04002C39 RID: 11321
			private float _attachedValue;

			// Token: 0x04002C3A RID: 11322
			private Stat.Values _stats = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.AttackDamage, 1.0)
			});

			// Token: 0x04002C3B RID: 11323
			private bool _attached;
		}
	}
}
