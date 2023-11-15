using System;
using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E6B RID: 3691
	public class SmashTo : TargetedCharacterOperation
	{
		// Token: 0x06004937 RID: 18743 RVA: 0x000D59B3 File Offset: 0x000D3BB3
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._onCollide.Initialize();
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x000D59CC File Offset: 0x000D3BCC
		private void OnEnd(Push push, Character from, Character to, Push.SmashEndType endType, RaycastHit2D? raycastHit, Movement.CollisionDirection direction)
		{
			if (endType != Push.SmashEndType.Collide)
			{
				return;
			}
			base.StartCoroutine(this._onCollide.CRun(from, to));
			Damage damage = from.stat.GetDamage((double)this._attackDamage.amount, raycastHit.Value.point, this._hitInfo);
			TargetStruct targetStruct = new TargetStruct(to);
			from.TryAttackCharacter(targetStruct, ref damage);
			this._effect.Spawn(to, push, raycastHit.Value, direction, damage, targetStruct);
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x000D5A54 File Offset: 0x000D3C54
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			Vector2 a;
			if (this._targetPlace != null)
			{
				a = MMMaths.RandomPointWithinBounds(this._targetPlace.bounds);
			}
			else
			{
				a = this._targetPoint.position;
			}
			Vector2 force = a - target.transform.position;
			target.movement.push.ApplySmash(owner, force, this._curve, this._ignoreOtherForce, this._expireOnGround, new Push.OnSmashEndDelegate(this.OnEnd));
		}

		// Token: 0x04003861 RID: 14433
		[Header("Destination")]
		[SerializeField]
		private Collider2D _targetPlace;

		// Token: 0x04003862 RID: 14434
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04003863 RID: 14435
		[Header("Force")]
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003864 RID: 14436
		[SerializeField]
		private bool _ignoreOtherForce = true;

		// Token: 0x04003865 RID: 14437
		[SerializeField]
		private bool _expireOnGround;

		// Token: 0x04003866 RID: 14438
		[Header("Hit")]
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04003867 RID: 14439
		[SmashAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private SmashAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003868 RID: 14440
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _onCollide;

		// Token: 0x04003869 RID: 14441
		private IAttackDamage _attackDamage;
	}
}
