using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E6C RID: 3692
	public sealed class StartMovingGrabbedTarget : CharacterOperation
	{
		// Token: 0x0600493B RID: 18747 RVA: 0x000D5B08 File Offset: 0x000D3D08
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._onCollide.Initialize();
			this._statusInfo = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Unmoving);
			this._relatedDirectionDict = new Dictionary<Target, Vector2>(this._grabBoard.maxTargetCount);
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x000D5B43 File Offset: 0x000D3D43
		public override void Run(Character owner)
		{
			this._relatedDirectionDict.Clear();
			if (this._grabber == null)
			{
				this._grabber = owner.transform;
			}
			base.StartCoroutine(this.Grab(owner));
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x000D5B78 File Offset: 0x000D3D78
		private IEnumerator Grab(Character owner)
		{
			this._grabRunning = true;
			float elapsed = 0f;
			while (elapsed < this._duration)
			{
				foreach (Target target in this._grabBoard.targets)
				{
					if (target == null)
					{
						Debug.Log("Grabbed Target is null");
					}
					else
					{
						if (this._targetPlace != null && !this._relatedDirectionDict.ContainsKey(target))
						{
							Vector2 value = MMMaths.RandomPointWithinBounds(this._targetPlace.bounds) - this._grabber.position;
							this._relatedDirectionDict.Add(target, value);
						}
						if (target.character.status != null && !target.character.status.unmovable)
						{
							owner.GiveStatus(target.character, this._statusInfo);
						}
						Vector2 force;
						if (this._targetPlace != null)
						{
							force = this._grabber.position + this._relatedDirectionDict[target] - target.transform.position;
						}
						else
						{
							force = this._targetTransform.position - target.transform.position;
						}
						target.character.movement.push.ApplySmash(owner, force, this._curve, this._ignoreOtherForce, this._expireOnGround, new Push.OnSmashEndDelegate(this.OnEnd));
					}
				}
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			this.Dispose();
			yield break;
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x000D5B8E File Offset: 0x000D3D8E
		public override void Stop()
		{
			base.Stop();
			this.Dispose();
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x000D5B9C File Offset: 0x000D3D9C
		private void Dispose()
		{
			if (!this._grabRunning)
			{
				return;
			}
			this._grabRunning = false;
			this._relatedDirectionDict.Clear();
			foreach (Target target in this._grabBoard.targets)
			{
				if (!(target == null) && !(target.character == null) && !(target.character.health == null) && !target.character.health.dead)
				{
					if (target.character.status != null)
					{
						target.character.ability.Remove(target.character.status.unmoving);
					}
					target.character.movement.push.Expire();
				}
			}
			base.StopAllCoroutines();
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x000D5C9C File Offset: 0x000D3E9C
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

		// Token: 0x06004941 RID: 18753 RVA: 0x000D5D24 File Offset: 0x000D3F24
		private void OnDisable()
		{
			if (this._grabRunning)
			{
				this.Stop();
			}
		}

		// Token: 0x0400386A RID: 14442
		[SerializeField]
		private GrabBoard _grabBoard;

		// Token: 0x0400386B RID: 14443
		[Information("끌고가는 주체, 비어있으면 캐릭터", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Transform _grabber;

		// Token: 0x0400386C RID: 14444
		[SerializeField]
		private float _duration;

		// Token: 0x0400386D RID: 14445
		[Header("Destination")]
		[Information("둘 중 하나만, 범위가 우선순위가 높음", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Collider2D _targetPlace;

		// Token: 0x0400386E RID: 14446
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x0400386F RID: 14447
		[Header("Force")]
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003870 RID: 14448
		[SerializeField]
		private bool _ignoreOtherForce = true;

		// Token: 0x04003871 RID: 14449
		[SerializeField]
		private bool _expireOnGround;

		// Token: 0x04003872 RID: 14450
		[SerializeField]
		[Header("Hit")]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04003873 RID: 14451
		[SerializeField]
		[SmashAttackVisualEffect.SubcomponentAttribute]
		private SmashAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003874 RID: 14452
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _onCollide;

		// Token: 0x04003875 RID: 14453
		private IAttackDamage _attackDamage;

		// Token: 0x04003876 RID: 14454
		private CharacterStatus.ApplyInfo _statusInfo;

		// Token: 0x04003877 RID: 14455
		private Dictionary<Target, Vector2> _relatedDirectionDict;

		// Token: 0x04003878 RID: 14456
		private bool _grabRunning;
	}
}
