using System;
using System.Collections;
using Characters.Movements;
using Characters.Operations;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200077D RID: 1917
	public class GrabTo : CharacterHitOperation
	{
		// Token: 0x06002773 RID: 10099 RVA: 0x0007659E File Offset: 0x0007479E
		private void Awake()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._onCollide.Initialize();
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000765B8 File Offset: 0x000747B8
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

		// Token: 0x06002775 RID: 10101 RVA: 0x00076640 File Offset: 0x00074840
		private IEnumerator CRun(Character owner, Character target)
		{
			float elapsed = 0f;
			while (elapsed < this._duration)
			{
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
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x00076660 File Offset: 0x00074860
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
		{
			Character owner = projectile.owner;
			base.StartCoroutine(this.CRun(owner, target));
		}

		// Token: 0x0400218F RID: 8591
		[SerializeField]
		private float _duration;

		// Token: 0x04002190 RID: 8592
		[SerializeField]
		[Header("Destination")]
		private Collider2D _targetPlace;

		// Token: 0x04002191 RID: 8593
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002192 RID: 8594
		[SerializeField]
		[Header("Force")]
		private Curve _curve;

		// Token: 0x04002193 RID: 8595
		[SerializeField]
		private bool _ignoreOtherForce = true;

		// Token: 0x04002194 RID: 8596
		[SerializeField]
		private bool _expireOnGround;

		// Token: 0x04002195 RID: 8597
		[Header("Hit")]
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002196 RID: 8598
		[SmashAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private SmashAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04002197 RID: 8599
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _onCollide;

		// Token: 0x04002198 RID: 8600
		private IAttackDamage _attackDamage;
	}
}
