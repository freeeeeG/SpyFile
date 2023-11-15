using System;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CBC RID: 3260
	[Serializable]
	public sealed class FightersBelt : Ability
	{
		// Token: 0x06004226 RID: 16934 RVA: 0x000C0A15 File Offset: 0x000BEC15
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new FightersBelt.Instance(owner, this);
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x000C0A1E File Offset: 0x000BEC1E
		public override void Initialize()
		{
			base.Initialize();
			this._onStartAction.Initialize();
			if (this._overlapper == null)
			{
				this._overlapper = new NonAllocOverlapper(128);
			}
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x000C0A4C File Offset: 0x000BEC4C
		private int GetCountWithinRange(GameObject gameObject)
		{
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(gameObject));
			this._overlapper.OverlapCollider(this._range);
			ReadonlyBoundedList<Collider2D> results = this._overlapper.results;
			int num = 0;
			using (IEnumerator<Collider2D> enumerator = results.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Target target;
					if (enumerator.Current.TryGetComponent<Target>(out target) && !(target.character == null))
					{
						Character character = target.character;
						if (this._targetCharacterTypeFilter[character.type])
						{
							num++;
						}
					}
				}
			}
			if (results == null)
			{
				return 0;
			}
			if (num > this._max)
			{
				return this._max;
			}
			return num;
		}

		// Token: 0x040032A9 RID: 12969
		private NonAllocOverlapper _overlapper;

		// Token: 0x040032AA RID: 12970
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040032AB RID: 12971
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x040032AC RID: 12972
		[SerializeField]
		private CharacterTypeBoolArray _targetCharacterTypeFilter;

		// Token: 0x040032AD RID: 12973
		[SerializeField]
		private int _max;

		// Token: 0x040032AE RID: 12974
		[Header("충격파")]
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x040032AF RID: 12975
		[SerializeField]
		private float _cooldownTimeReducementPerTarget;

		// Token: 0x040032B0 RID: 12976
		[SerializeField]
		private ActionTypeBoolArray _triggerActionFilter;

		// Token: 0x040032B1 RID: 12977
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onStartAction;

		// Token: 0x02000CBD RID: 3261
		public sealed class Instance : AbilityInstance<FightersBelt>
		{
			// Token: 0x17000DD6 RID: 3542
			// (get) Token: 0x0600422A RID: 16938 RVA: 0x000C0B18 File Offset: 0x000BED18
			public override int iconStacks
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x17000DD7 RID: 3543
			// (get) Token: 0x0600422B RID: 16939 RVA: 0x000C0B20 File Offset: 0x000BED20
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

			// Token: 0x0600422C RID: 16940 RVA: 0x000C0B33 File Offset: 0x000BED33
			public Instance(Character owner, FightersBelt ability) : base(owner, ability)
			{
			}

			// Token: 0x0600422D RID: 16941 RVA: 0x000C0B3D File Offset: 0x000BED3D
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x0600422E RID: 16942 RVA: 0x000C0B58 File Offset: 0x000BED58
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (!this.ability._triggerActionFilter[action.type])
				{
					return;
				}
				this._count = this.ability.GetCountWithinRange(this.owner.gameObject);
				this._remainCooldownTime = this.ability._cooldownTime - this.ability._cooldownTimeReducementPerTarget * (float)this._count;
				this.owner.StartCoroutine(this.ability._onStartAction.CRun(this.owner));
			}

			// Token: 0x0600422F RID: 16943 RVA: 0x000C0BEF File Offset: 0x000BEDEF
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06004230 RID: 16944 RVA: 0x000C0C06 File Offset: 0x000BEE06
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x040032B2 RID: 12978
			private int _count;

			// Token: 0x040032B3 RID: 12979
			private float _remainCooldownTime;
		}
	}
}
