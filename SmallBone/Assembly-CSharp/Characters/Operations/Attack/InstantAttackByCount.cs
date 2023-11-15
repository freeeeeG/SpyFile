using System;
using System.Collections.Generic;
using FX.BoundsAttackVisualEffect;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F8D RID: 3981
	public sealed class InstantAttackByCount : CharacterOperation
	{
		// Token: 0x06004D41 RID: 19777 RVA: 0x000E60D0 File Offset: 0x000E42D0
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(this._maxHits);
			this._attackRange.enabled = false;
			Array.Sort<TargetedOperationInfo>(this._operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x000E6129 File Offset: 0x000E4329
		public override void Initialize()
		{
			base.Initialize();
			this._operationInfo.Initialize();
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x000E6148 File Offset: 0x000E4348
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._attackRange.enabled = true;
			Bounds bounds = this._attackRange.bounds;
			this._overlapper.OverlapCollider(this._attackRange);
			this._attackRange.enabled = false;
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				return;
			}
			if (this._adaptiveForce)
			{
				this._hitInfo.ChangeAdaptiveDamageAttribute(owner);
			}
			for (int i = 0; i < components.Count; i++)
			{
				Target target = components[i];
				if (!(target == null) && !(target.character == null) && !(target.character == owner) && target.character.liveAndActive)
				{
					Bounds bounds2 = target.collider.bounds;
					Bounds bounds3 = new Bounds
					{
						min = MMMaths.Max(bounds.min, bounds2.min),
						max = MMMaths.Min(bounds.max, bounds2.max)
					};
					Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds3);
					Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
					if (this._baseDamageMultiplierMaxCount != 0)
					{
						damage.percentMultiplier *= (double)(1f + this._multiplierCurve.Evaluate((float)components.Count / (float)this._baseDamageMultiplierMaxCount));
					}
					if (this._damageMultiplierMaxCount != 0)
					{
						damage.multiplier += (double)this._multiplierCurve.Evaluate((float)components.Count / (float)this._damageMultiplierMaxCount);
					}
					base.StartCoroutine(this._operationInfo.CRun(owner, target.character));
					this._effect.Spawn(owner, bounds3, damage, target);
					if (!target.character.cinematic.value)
					{
						owner.TryAttackCharacter(target, ref damage);
					}
				}
			}
		}

		// Token: 0x04003CFF RID: 15615
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003D00 RID: 15616
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04003D01 RID: 15617
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003D02 RID: 15618
		[SerializeField]
		private Collider2D _attackRange;

		// Token: 0x04003D03 RID: 15619
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x04003D04 RID: 15620
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003D05 RID: 15621
		[SerializeField]
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
		private int _maxHits = 512;

		// Token: 0x04003D06 RID: 15622
		[SerializeField]
		private int _damageMultiplierMaxCount;

		// Token: 0x04003D07 RID: 15623
		[SerializeField]
		private int _baseDamageMultiplierMaxCount;

		// Token: 0x04003D08 RID: 15624
		[SerializeField]
		private Curve _multiplierCurve;

		// Token: 0x04003D09 RID: 15625
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003D0A RID: 15626
		private IAttackDamage _attackDamage;
	}
}
