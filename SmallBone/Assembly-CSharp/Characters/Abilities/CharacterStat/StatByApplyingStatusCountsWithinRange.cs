using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C7D RID: 3197
	[Serializable]
	public class StatByApplyingStatusCountsWithinRange : Ability
	{
		// Token: 0x0600413E RID: 16702 RVA: 0x000BD920 File Offset: 0x000BBB20
		public override void Initialize()
		{
			base.Initialize();
			if (this._overlapper == null)
			{
				this._overlapper = new NonAllocOverlapper(this._max);
			}
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x000BD944 File Offset: 0x000BBB44
		private int GetCountWithinRange(GameObject gameObject)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(gameObject));
			this._range.enabled = true;
			this._overlapper.OverlapCollider(this._range);
			List<Character> components = this._overlapper.GetComponents<Character>(true);
			int num = 0;
			foreach (Character character in components)
			{
				if (!(character.status == null) && character.status.IsApplying(this._targetStatusFilter))
				{
					num++;
				}
			}
			this._range.enabled = false;
			if (num > 0)
			{
				num += this._base;
			}
			if (num < this._min)
			{
				return 0;
			}
			if (num > this._max)
			{
				return this._max;
			}
			return num;
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x000BDA2C File Offset: 0x000BBC2C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatByApplyingStatusCountsWithinRange.Instance(owner, this);
		}

		// Token: 0x04003209 RID: 12809
		public const float _overlapInterval = 0.25f;

		// Token: 0x0400320A RID: 12810
		private NonAllocOverlapper _overlapper;

		// Token: 0x0400320B RID: 12811
		[SerializeField]
		private Collider2D _range;

		// Token: 0x0400320C RID: 12812
		[SerializeField]
		private Stat.Values _statPerCount;

		// Token: 0x0400320D RID: 12813
		[SerializeField]
		private TargetLayer _layer;

		// Token: 0x0400320E RID: 12814
		[SerializeField]
		private CharacterStatusKindBoolArray _targetStatusFilter;

		// Token: 0x0400320F RID: 12815
		[Information("스탯 배수에 적용할 기본 배수", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private int _base;

		// Token: 0x04003210 RID: 12816
		[Information("효과가 적용되기 시작하는 최소 개수", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private int _min;

		// Token: 0x04003211 RID: 12817
		[SerializeField]
		private int _max;

		// Token: 0x02000C7E RID: 3198
		public class Instance : AbilityInstance<StatByApplyingStatusCountsWithinRange>
		{
			// Token: 0x17000DBC RID: 3516
			// (get) Token: 0x06004142 RID: 16706 RVA: 0x000BDA35 File Offset: 0x000BBC35
			public override int iconStacks
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x17000DBD RID: 3517
			// (get) Token: 0x06004143 RID: 16707 RVA: 0x000BDA3D File Offset: 0x000BBC3D
			public override Sprite icon
			{
				get
				{
					if (this._count <= 0)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x06004144 RID: 16708 RVA: 0x000BDA50 File Offset: 0x000BBC50
			public Instance(Character owner, StatByApplyingStatusCountsWithinRange ability) : base(owner, ability)
			{
				this._stat = ability._statPerCount.Clone();
			}

			// Token: 0x06004145 RID: 16709 RVA: 0x000BDA6B File Offset: 0x000BBC6B
			protected override void OnAttach()
			{
				this.UpdateStat();
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x06004146 RID: 16710 RVA: 0x000BDA89 File Offset: 0x000BBC89
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004147 RID: 16711 RVA: 0x000BDAA1 File Offset: 0x000BBCA1
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCheckTime -= deltaTime;
				if (this._remainCheckTime < 0f)
				{
					this._remainCheckTime += 0.25f;
					this.UpdateStat();
				}
			}

			// Token: 0x06004148 RID: 16712 RVA: 0x000BDAE0 File Offset: 0x000BBCE0
			private void UpdateStat()
			{
				this._count = this.ability.GetCountWithinRange(this.owner.gameObject);
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					double num = (double)this._count * this.ability._statPerCount.values[i].value;
					if (this.ability._statPerCount.values[i].categoryIndex == Stat.Category.Percent.index)
					{
						num += 1.0;
					}
					this._stat.values[i].value = num;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003212 RID: 12818
			private Stat.Values _stat;

			// Token: 0x04003213 RID: 12819
			private int _count;

			// Token: 0x04003214 RID: 12820
			private float _remainCheckTime;
		}
	}
}
