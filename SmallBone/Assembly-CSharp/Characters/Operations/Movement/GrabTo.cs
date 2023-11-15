using System;
using System.Collections;
using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E56 RID: 3670
	public sealed class GrabTo : TargetedCharacterOperation
	{
		// Token: 0x060048E4 RID: 18660 RVA: 0x000D486A File Offset: 0x000D2A6A
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._onCollide.Initialize();
			if (this._applyUnmoving)
			{
				this._statusInfo = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Unmoving);
			}
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x000D4898 File Offset: 0x000D2A98
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

		// Token: 0x060048E6 RID: 18662 RVA: 0x000D4920 File Offset: 0x000D2B20
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			if (target.movement.push.ignoreOtherForce)
			{
				return;
			}
			this._target = target;
			base.StartCoroutine(this.CRun(owner, target));
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x000D495D File Offset: 0x000D2B5D
		private IEnumerator CRun(Character owner, Character target)
		{
			float elapsed = 0f;
			Vector2 _targetPointing = Vector2.zero;
			if (this._targetPlace != null)
			{
				Vector2 vector = MMMaths.RandomPointWithinBounds(this._targetPlace.bounds);
				if (this._localCoordinate)
				{
					_targetPointing = vector - owner.transform.position;
				}
				this._targetPoint.position = vector;
			}
			while (elapsed < this._duration)
			{
				Vector2 force;
				if (this._targetPlace != null && this._localCoordinate)
				{
					force = owner.transform.position + _targetPointing - target.transform.position;
				}
				else
				{
					force = this._targetPoint.position - target.transform.position;
				}
				target.movement.push.ApplySmash(owner, force, this._curve, this._ignoreOtherForce, this._expireOnGround, new Push.OnSmashEndDelegate(this.OnEnd));
				if (target.status != null && !target.status.unmovable)
				{
					owner.GiveStatus(target, this._statusInfo);
				}
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x000D497C File Offset: 0x000D2B7C
		private void Dispose(Character target)
		{
			if (target == null || target.health == null || target.health.dead)
			{
				return;
			}
			if (this._applyUnmoving && target.status != null)
			{
				target.ability.Remove(target.status.unmoving);
			}
			target.movement.push.Expire();
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x000D49EB File Offset: 0x000D2BEB
		private void OnDisable()
		{
			this.Dispose(this._target);
		}

		// Token: 0x040037FA RID: 14330
		[SerializeField]
		private bool _localCoordinate = true;

		// Token: 0x040037FB RID: 14331
		[SerializeField]
		private float _duration;

		// Token: 0x040037FC RID: 14332
		[SerializeField]
		[Header("Destination")]
		private Collider2D _targetPlace;

		// Token: 0x040037FD RID: 14333
		[Information("필수", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x040037FE RID: 14334
		[Header("Force")]
		[SerializeField]
		private Curve _curve;

		// Token: 0x040037FF RID: 14335
		[SerializeField]
		private bool _ignoreOtherForce = true;

		// Token: 0x04003800 RID: 14336
		[SerializeField]
		private bool _expireOnGround;

		// Token: 0x04003801 RID: 14337
		[SerializeField]
		private bool _applyUnmoving;

		// Token: 0x04003802 RID: 14338
		[Header("Hit")]
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04003803 RID: 14339
		[SerializeField]
		[SmashAttackVisualEffect.SubcomponentAttribute]
		private SmashAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003804 RID: 14340
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _onCollide;

		// Token: 0x04003805 RID: 14341
		private IAttackDamage _attackDamage;

		// Token: 0x04003806 RID: 14342
		private CharacterStatus.ApplyInfo _statusInfo;

		// Token: 0x04003807 RID: 14343
		private Character _target;
	}
}
