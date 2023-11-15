using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Debuffs
{
	// Token: 0x02000BA6 RID: 2982
	[Serializable]
	public sealed class DotDamage : Ability
	{
		// Token: 0x06003D9F RID: 15775 RVA: 0x000B31D5 File Offset: 0x000B13D5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new DotDamage.Instance(owner, this);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x000B31DE File Offset: 0x000B13DE
		public override void Initialize()
		{
			base.Initialize();
			this._onCharacterHit.Initialize();
		}

		// Token: 0x04002F9F RID: 12191
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002FA0 RID: 12192
		[SerializeField]
		private Transform _onHitPoint;

		// Token: 0x04002FA1 RID: 12193
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _onCharacterHit;

		// Token: 0x04002FA2 RID: 12194
		[SerializeField]
		private CustomFloat _baseDamage;

		// Token: 0x04002FA3 RID: 12195
		[SerializeField]
		private Character _attacker;

		// Token: 0x04002FA4 RID: 12196
		[SerializeField]
		private HitInfo _hitInfo;

		// Token: 0x04002FA5 RID: 12197
		[SerializeField]
		private float _tickInterval;

		// Token: 0x02000BA7 RID: 2983
		public class Instance : AbilityInstance<DotDamage>
		{
			// Token: 0x06003DA2 RID: 15778 RVA: 0x000B31F1 File Offset: 0x000B13F1
			public Instance(Character owner, DotDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DA3 RID: 15779 RVA: 0x000B31FB File Offset: 0x000B13FB
			protected override void OnAttach()
			{
				if (this.ability._attacker == null)
				{
					this.ability._attacker = Singleton<Service>.Instance.levelManager.player;
				}
			}

			// Token: 0x06003DA4 RID: 15780 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x06003DA5 RID: 15781 RVA: 0x000B322C File Offset: 0x000B142C
			private void GiveDamage()
			{
				Target component = this.owner.collider.GetComponent<Target>();
				if (component == null)
				{
					return;
				}
				Damage damage = this.owner.stat.GetDamage((double)this.ability._baseDamage.value, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), this.ability._hitInfo);
				bool flag = this.ability._attacker.TryAttackCharacter(component, ref damage);
				this.ability._positionInfo.Attach(component, this.ability._onHitPoint);
				if (flag)
				{
					this.ability._attacker.StartCoroutine(this.ability._onCharacterHit.CRun(this.ability._attacker, this.owner));
				}
			}

			// Token: 0x06003DA6 RID: 15782 RVA: 0x000B32FC File Offset: 0x000B14FC
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTimeToNextTick -= deltaTime;
				if (this._remainTimeToNextTick < 0f)
				{
					this._remainTimeToNextTick += this.ability._tickInterval;
					this.GiveDamage();
				}
			}

			// Token: 0x04002FA6 RID: 12198
			private float _remainTimeToNextTick;
		}
	}
}
