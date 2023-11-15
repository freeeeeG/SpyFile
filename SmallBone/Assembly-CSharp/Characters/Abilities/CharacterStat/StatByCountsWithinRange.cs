using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C80 RID: 3200
	[Serializable]
	public class StatByCountsWithinRange : Ability
	{
		// Token: 0x0600414A RID: 16714 RVA: 0x000BDB9B File Offset: 0x000BBD9B
		public override void Initialize()
		{
			base.Initialize();
			if (this._overlapper == null)
			{
				this._overlapper = new NonAllocOverlapper(128);
			}
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x000BDBBC File Offset: 0x000BBDBC
		private int GetCountWithinRange(GameObject gameObject)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(gameObject));
			this._overlapper.OverlapCollider(this._range);
			ReadonlyBoundedList<Collider2D> results = this._overlapper.results;
			int num = 0;
			foreach (Collider2D collider2D in results)
			{
				Target component = collider2D.GetComponent<Target>();
				if (!(component == null) && !(component.character == null))
				{
					Character character = component.character;
					if ((!this._statusCheck || (!(character.status == null) && character.status.IsApplying(this._statusKinds))) && this._characterTypes[character.type])
					{
						num++;
					}
				}
			}
			if (results == null)
			{
				return 0;
			}
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

		// Token: 0x0600414C RID: 16716 RVA: 0x000BDCD0 File Offset: 0x000BBED0
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatByCountsWithinRange.Instance(owner, this);
		}

		// Token: 0x04003215 RID: 12821
		public const float _overlapInterval = 0.15f;

		// Token: 0x04003216 RID: 12822
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003217 RID: 12823
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04003218 RID: 12824
		[SerializeField]
		private Stat.Values _statPerCount;

		// Token: 0x04003219 RID: 12825
		[SerializeField]
		private TargetLayer _layer;

		// Token: 0x0400321A RID: 12826
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x0400321B RID: 12827
		[SerializeField]
		private bool _statusCheck;

		// Token: 0x0400321C RID: 12828
		[SerializeField]
		private CharacterStatusKindBoolArray _statusKinds;

		// Token: 0x0400321D RID: 12829
		[SerializeField]
		[Information("스탯 배수에 적용할 기본 배수", InformationAttribute.InformationType.Info, false)]
		private int _base;

		// Token: 0x0400321E RID: 12830
		[Information("효과가 적용되기 시작하는 최소 개수", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private int _min;

		// Token: 0x0400321F RID: 12831
		[SerializeField]
		private int _max;

		// Token: 0x02000C81 RID: 3201
		public class Instance : AbilityInstance<StatByCountsWithinRange>
		{
			// Token: 0x17000DBE RID: 3518
			// (get) Token: 0x0600414E RID: 16718 RVA: 0x000BDCD9 File Offset: 0x000BBED9
			public override int iconStacks
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x17000DBF RID: 3519
			// (get) Token: 0x0600414F RID: 16719 RVA: 0x000BDCE1 File Offset: 0x000BBEE1
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

			// Token: 0x06004150 RID: 16720 RVA: 0x000BDCF4 File Offset: 0x000BBEF4
			public Instance(Character owner, StatByCountsWithinRange ability) : base(owner, ability)
			{
				this._stat = ability._statPerCount.Clone();
			}

			// Token: 0x06004151 RID: 16721 RVA: 0x000BDD0F File Offset: 0x000BBF0F
			protected override void OnAttach()
			{
				this.UpdateStat();
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x06004152 RID: 16722 RVA: 0x000BDD2D File Offset: 0x000BBF2D
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004153 RID: 16723 RVA: 0x000BDD45 File Offset: 0x000BBF45
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this.UpdateStat();
			}

			// Token: 0x06004154 RID: 16724 RVA: 0x000BDD54 File Offset: 0x000BBF54
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

			// Token: 0x04003220 RID: 12832
			private Stat.Values _stat;

			// Token: 0x04003221 RID: 12833
			private int _count;

			// Token: 0x04003222 RID: 12834
			private float _remainCheckTime;
		}
	}
}
