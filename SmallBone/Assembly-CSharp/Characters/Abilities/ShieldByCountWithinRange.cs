using System;
using System.Linq;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AB8 RID: 2744
	[Serializable]
	public class ShieldByCountWithinRange : Ability
	{
		// Token: 0x0600387F RID: 14463 RVA: 0x000A6A5D File Offset: 0x000A4C5D
		public override void Initialize()
		{
			base.Initialize();
			if (this._overlapper == null)
			{
				this._overlapper = new NonAllocOverlapper(this._max);
			}
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x000A6A80 File Offset: 0x000A4C80
		private int GetCountWithinRange(GameObject gameObject)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(gameObject));
			this._range.enabled = true;
			this._overlapper.OverlapCollider(this._range);
			int num = this._overlapper.results.Where(delegate(Collider2D result)
			{
				Character component = result.GetComponent<Character>();
				return !(component == null) && (!this._statusCheck || ((!this._statusKinds[CharacterStatus.Kind.Burn] || component.status.burning) && (!this._statusKinds[CharacterStatus.Kind.Freeze] || component.status.freezed) && (!this._statusKinds[CharacterStatus.Kind.Poison] || component.status.poisoned) && (!this._statusKinds[CharacterStatus.Kind.Wound] || component.status.wounded) && (!this._statusKinds[CharacterStatus.Kind.Stun] || component.status.stuned))) && this._characterTypes[component.type];
			}).Count<Collider2D>();
			this._range.enabled = false;
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

		// Token: 0x06003881 RID: 14465 RVA: 0x000A6B11 File Offset: 0x000A4D11
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ShieldByCountWithinRange.Instance(owner, this);
		}

		// Token: 0x04002CFC RID: 11516
		public const float _overlapInterval = 0.25f;

		// Token: 0x04002CFD RID: 11517
		private NonAllocOverlapper _overlapper;

		// Token: 0x04002CFE RID: 11518
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04002CFF RID: 11519
		[SerializeField]
		private int _shieldAmountPerCount;

		// Token: 0x04002D00 RID: 11520
		[SerializeField]
		private TargetLayer _layer;

		// Token: 0x04002D01 RID: 11521
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x04002D02 RID: 11522
		[SerializeField]
		private bool _statusCheck;

		// Token: 0x04002D03 RID: 11523
		[SerializeField]
		private CharacterStatusKindBoolArray _statusKinds;

		// Token: 0x04002D04 RID: 11524
		[SerializeField]
		[Information("효과가 적용되기 시작하는 최소 개수", InformationAttribute.InformationType.Info, false)]
		private int _min;

		// Token: 0x04002D05 RID: 11525
		[SerializeField]
		private int _max;

		// Token: 0x02000AB9 RID: 2745
		public class Instance : AbilityInstance<ShieldByCountWithinRange>
		{
			// Token: 0x17000BC6 RID: 3014
			// (get) Token: 0x06003884 RID: 14468 RVA: 0x000A6BE0 File Offset: 0x000A4DE0
			public override int iconStacks
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x17000BC7 RID: 3015
			// (get) Token: 0x06003885 RID: 14469 RVA: 0x000A6BE8 File Offset: 0x000A4DE8
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

			// Token: 0x06003886 RID: 14470 RVA: 0x000A6BFB File Offset: 0x000A4DFB
			public Instance(Character owner, ShieldByCountWithinRange ability) : base(owner, ability)
			{
			}

			// Token: 0x06003887 RID: 14471 RVA: 0x000A6C05 File Offset: 0x000A4E05
			protected override void OnAttach()
			{
				this.UpdateShield();
			}

			// Token: 0x06003888 RID: 14472 RVA: 0x000A6C0D File Offset: 0x000A4E0D
			protected override void OnDetach()
			{
				this.owner.health.shield.Remove(this.ability);
			}

			// Token: 0x06003889 RID: 14473 RVA: 0x000A6C2B File Offset: 0x000A4E2B
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCheckTime -= deltaTime;
				if (this._remainCheckTime < 0f)
				{
					this._remainCheckTime += 0.25f;
					this.UpdateShield();
				}
			}

			// Token: 0x0600388A RID: 14474 RVA: 0x000A6C68 File Offset: 0x000A4E68
			private void UpdateShield()
			{
				this._count = this.ability.GetCountWithinRange(this.owner.gameObject);
				int num = this.ability._shieldAmountPerCount * this._count;
				this.owner.health.shield.AddOrUpdate(this.ability, (float)num, null);
			}

			// Token: 0x04002D06 RID: 11526
			private int _count;

			// Token: 0x04002D07 RID: 11527
			private float _remainCheckTime;
		}
	}
}
