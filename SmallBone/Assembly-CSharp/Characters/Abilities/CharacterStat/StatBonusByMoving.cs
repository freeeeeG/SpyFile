using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C59 RID: 3161
	[Serializable]
	public class StatBonusByMoving : Ability
	{
		// Token: 0x060040B5 RID: 16565 RVA: 0x000BC051 File Offset: 0x000BA251
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByMoving.Instance(owner, this);
		}

		// Token: 0x040031B6 RID: 12726
		[Tooltip("손실이 없다고 가정할 때, 이 거리만큼 이동하면 스탯이 최대치에 도달함 (반드시 0보다 커야함)")]
		[SerializeField]
		private float _distanceToMax = 1f;

		// Token: 0x040031B7 RID: 12727
		[SerializeField]
		[Tooltip("매 초 이 수치만큼 이동 거리를 잃음")]
		private float _lossPerSecond;

		// Token: 0x040031B8 RID: 12728
		[Space]
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x040031B9 RID: 12729
		[SerializeField]
		[Space]
		private Stat.Values _maxStat;

		// Token: 0x02000C5A RID: 3162
		public class Instance : AbilityInstance<StatBonusByMoving>
		{
			// Token: 0x17000DA4 RID: 3492
			// (get) Token: 0x060040B7 RID: 16567 RVA: 0x000BC06D File Offset: 0x000BA26D
			public override Sprite icon
			{
				get
				{
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000DA5 RID: 3493
			// (get) Token: 0x060040B8 RID: 16568 RVA: 0x000BC07A File Offset: 0x000BA27A
			public override float iconFillAmount
			{
				get
				{
					return this._movedDistance / this.ability._distanceToMax;
				}
			}

			// Token: 0x060040B9 RID: 16569 RVA: 0x000BC08E File Offset: 0x000BA28E
			public Instance(Character owner, StatBonusByMoving ability) : base(owner, ability)
			{
				this._stat = ability._maxStat.Clone();
			}

			// Token: 0x060040BA RID: 16570 RVA: 0x000BC0A9 File Offset: 0x000BA2A9
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.owner.movement.onMoved += this.OnMoved;
			}

			// Token: 0x060040BB RID: 16571 RVA: 0x000BC0DD File Offset: 0x000BA2DD
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.owner.movement.onMoved -= this.OnMoved;
			}

			// Token: 0x060040BC RID: 16572 RVA: 0x000BC111 File Offset: 0x000BA311
			private void OnMoved(Vector2 amount)
			{
				this._movedDistance += Mathf.Abs(amount.x);
				if (this._movedDistance > this.ability._distanceToMax)
				{
					this._movedDistance = this.ability._distanceToMax;
				}
			}

			// Token: 0x060040BD RID: 16573 RVA: 0x000BC150 File Offset: 0x000BA350
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._movedDistance -= this.ability._lossPerSecond * deltaTime;
				if (this._movedDistance < 0f)
				{
					this._movedDistance = 0f;
				}
				this._remainUpdateTime -= deltaTime;
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime = 0.2f;
					this.UpdateStat();
				}
			}

			// Token: 0x060040BE RID: 16574 RVA: 0x000BC1C4 File Offset: 0x000BA3C4
			public void UpdateStat()
			{
				if (this._movedDistance == this._movedDistanceBefore)
				{
					return;
				}
				Stat.Value[] values = this._stat.values;
				float num = this._movedDistance / this.ability._distanceToMax;
				if (this.ability._gauge != null)
				{
					this.ability._gauge.Set(num * 100f);
				}
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._maxStat.values[i].GetMultipliedValue(num);
				}
				this.owner.stat.SetNeedUpdate();
				this._movedDistanceBefore = this._movedDistance;
			}

			// Token: 0x040031BA RID: 12730
			private const float _updateInterval = 0.2f;

			// Token: 0x040031BB RID: 12731
			private float _remainUpdateTime;

			// Token: 0x040031BC RID: 12732
			private Stat.Values _stat;

			// Token: 0x040031BD RID: 12733
			private float _movedDistance;

			// Token: 0x040031BE RID: 12734
			private float _movedDistanceBefore;
		}
	}
}
