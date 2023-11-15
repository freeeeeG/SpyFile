using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F82 RID: 3970
	public sealed class GlobalAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06004CFE RID: 19710 RVA: 0x000E4BAC File Offset: 0x000E2DAC
		// (remove) Token: 0x06004CFF RID: 19711 RVA: 0x000E4BE4 File Offset: 0x000E2DE4
		public event OnAttackHitDelegate onHit;

		// Token: 0x06004D00 RID: 19712 RVA: 0x000E4C19 File Offset: 0x000E2E19
		private void Awake()
		{
			Array.Sort<TargetedOperationInfo>(this._operationInfo.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x000E4C4C File Offset: 0x000E2E4C
		public override void Initialize()
		{
			base.Initialize();
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._operationInfo.Initialize();
			foreach (TargetedOperationInfo targetedOperationInfo in this._operationInfo.components)
			{
				Knockback knockback = targetedOperationInfo.operation as Knockback;
				if (knockback != null)
				{
					this._pushInfo = knockback.pushInfo;
					return;
				}
				Smash smash = targetedOperationInfo.operation as Smash;
				if (smash != null)
				{
					this._pushInfo = smash.pushInfo;
				}
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x000E4CD0 File Offset: 0x000E2ED0
		public override void Run(Character owner)
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			if (this._delay == 0f)
			{
				this.Attack(owner, allEnemies);
				return;
			}
			base.StartCoroutine(this.CRun(owner, allEnemies));
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x000E4D12 File Offset: 0x000E2F12
		private IEnumerator CRun(Character owner, List<Character> enemies)
		{
			yield return owner.chronometer.master.WaitForSeconds(this._delay);
			this.Attack(owner, enemies);
			yield break;
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x000E4D30 File Offset: 0x000E2F30
		private void Attack(Character owner, List<Character> enemies)
		{
			bool flag = false;
			foreach (Character character in enemies)
			{
				Target componentInChildren = character.GetComponentInChildren<Target>();
				if (!(componentInChildren == null) && character.gameObject.activeInHierarchy)
				{
					Bounds bounds = character.collider.bounds;
					Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds);
					Vector2 force = Vector2.zero;
					if (this._pushInfo != null)
					{
						ValueTuple<Vector2, Vector2> valueTuple = this._pushInfo.EvaluateTimeIndependent(owner, componentInChildren);
						force = valueTuple.Item1 + valueTuple.Item2;
					}
					if (character.liveAndActive && !(character == owner))
					{
						flag = true;
						this._chronoToTarget.ApplyTo(character);
						Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
						if (this._hitInfo.attackType != Damage.AttackType.None)
						{
							CommonResource.instance.hitParticle.Emit(componentInChildren.transform.position, bounds, force, true);
						}
						if (!character.cinematic.value)
						{
							flag = owner.TryAttackCharacter(componentInChildren, ref damage);
							if (flag)
							{
								if (base.gameObject.activeInHierarchy)
								{
									base.StartCoroutine(this._operationInfo.CRun(owner, character));
								}
								OnAttackHitDelegate onAttackHitDelegate = this.onHit;
								if (onAttackHitDelegate != null)
								{
									onAttackHitDelegate(componentInChildren, ref damage);
								}
								this._effect.Spawn(owner, bounds, damage, componentInChildren);
							}
							this._effect.Spawn(owner, bounds, damage, componentInChildren);
						}
					}
				}
			}
			if (flag)
			{
				this._chronoToGlobe.ApplyGlobe();
				this._chronoToOwner.ApplyTo(owner);
				if (this._operationToOwnerWhenHitInfo.components.Length != 0)
				{
					base.StartCoroutine(this._operationToOwnerWhenHitInfo.CRun(owner));
				}
			}
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x000E4F1C File Offset: 0x000E311C
		public override void Stop()
		{
			this._operationToOwnerWhenHitInfo.StopAll();
		}

		// Token: 0x04003CB9 RID: 15545
		[SerializeField]
		private float _delay;

		// Token: 0x04003CBA RID: 15546
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003CBB RID: 15547
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003CBC RID: 15548
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003CBD RID: 15549
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003CBE RID: 15550
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		internal OperationInfo.Subcomponents _operationToOwnerWhenHitInfo;

		// Token: 0x04003CBF RID: 15551
		[SerializeField]
		[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
		private int _maxHits = 512;

		// Token: 0x04003CC0 RID: 15552
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x04003CC1 RID: 15553
		[SerializeField]
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003CC2 RID: 15554
		private PushInfo _pushInfo;

		// Token: 0x04003CC3 RID: 15555
		private IAttackDamage _attackDamage;
	}
}
