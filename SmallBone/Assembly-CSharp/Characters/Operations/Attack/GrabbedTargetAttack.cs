using System;
using System.Collections;
using System.Collections.Generic;
using GameResources;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F85 RID: 3973
	public sealed class GrabbedTargetAttack : CharacterOperation, IAttack
	{
		// Token: 0x140000BA RID: 186
		// (add) Token: 0x06004D10 RID: 19728 RVA: 0x000E4FFC File Offset: 0x000E31FC
		// (remove) Token: 0x06004D11 RID: 19729 RVA: 0x000E5034 File Offset: 0x000E3234
		public event OnAttackHitDelegate onHit;

		// Token: 0x06004D12 RID: 19730 RVA: 0x000E5069 File Offset: 0x000E3269
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._attackAndEffect.Initialize();
			this.results = new List<Target>(64);
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x000E508F File Offset: 0x000E328F
		public override void Stop()
		{
			this._attackAndEffect.StopAllOperationsToOwner();
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x000E509C File Offset: 0x000E329C
		public override void Run(Character owner)
		{
			if (this._grabBoard == null)
			{
				return;
			}
			this.results.Clear();
			if (this._grabbedTarget)
			{
				this.results.AddRange(this._grabBoard.targets);
			}
			if (this._failedTarget)
			{
				this.results.AddRange(this._grabBoard.failTargets);
			}
			for (int i = 0; i < this.results.Count; i++)
			{
				Target target = this.results[i];
				if (target == null)
				{
					Debug.LogError("Target is null in GrabbedTargetAttack");
					return;
				}
				if (this._attackAndEffect.noDelay)
				{
					foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in this._attackAndEffect.components)
					{
						this.Attack(owner, target.collider.bounds, target, boundsAttackInfoSequence.attackInfo);
					}
				}
				else
				{
					target.StartCoroutine(this.CAttack(owner, target.character.collider.bounds, target));
				}
			}
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x000E51A4 File Offset: 0x000E33A4
		private void Attack(Character owner, Bounds bounds, Target target, BoundsAttackInfo attackInfo)
		{
			if (target == null)
			{
				Debug.LogError("Target is null in GrabbedTargetAttack");
				return;
			}
			if (!target.isActiveAndEnabled)
			{
				return;
			}
			Bounds bounds2 = bounds;
			Bounds bounds3 = target.collider.bounds;
			Bounds bounds4 = new Bounds
			{
				min = MMMaths.Max(bounds2.min, bounds3.min),
				max = MMMaths.Min(bounds2.max, bounds3.max)
			};
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(bounds4);
			Vector2 force = Vector2.zero;
			if (attackInfo.pushInfo != null)
			{
				ValueTuple<Vector2, Vector2> valueTuple = attackInfo.pushInfo.EvaluateTimeIndependent(owner, target);
				force = valueTuple.Item1 + valueTuple.Item2;
			}
			if (!(target.character != null))
			{
				return;
			}
			if (!target.character.liveAndActive || target.character == owner)
			{
				return;
			}
			if (target.character.cinematic.value)
			{
				return;
			}
			attackInfo.ApplyChrono(owner, target.character);
			if (attackInfo.operationsToOwner.components.Length != 0)
			{
				owner.StartCoroutine(attackInfo.operationsToOwner.CRun(owner));
			}
			Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, attackInfo.hitInfo);
			if (attackInfo.hitInfo.attackType != Damage.AttackType.None)
			{
				CommonResource.instance.hitParticle.Emit(target.transform.position, target.collider.bounds, force, true);
			}
			if (owner.TryAttackCharacter(target, ref damage))
			{
				owner.StartCoroutine(attackInfo.operationInfo.CRun(owner, target.character));
				OnAttackHitDelegate onAttackHitDelegate = this.onHit;
				if (onAttackHitDelegate != null)
				{
					onAttackHitDelegate(target, ref damage);
				}
				attackInfo.effect.Spawn(owner, bounds4, damage, target);
			}
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x000E538A File Offset: 0x000E358A
		private IEnumerator CAttack(Character owner, Bounds bounds, Target target)
		{
			int index = 0;
			float time = 0f;
			while (this != null && index < this._attackAndEffect.components.Length)
			{
				BoundsAttackInfoSequence boundsAttackInfoSequence;
				while (index < this._attackAndEffect.components.Length && time >= (boundsAttackInfoSequence = this._attackAndEffect.components[index]).timeToTrigger)
				{
					this.Attack(owner, bounds, target, boundsAttackInfoSequence.attackInfo);
					int num = index;
					index = num + 1;
				}
				yield return null;
				time += owner.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x04003CCB RID: 15563
		[UnityEditor.Subcomponent(typeof(BoundsAttackInfoSequence))]
		[SerializeField]
		private BoundsAttackInfoSequence.Subcomponents _attackAndEffect;

		// Token: 0x04003CCC RID: 15564
		[SerializeField]
		private GrabBoard _grabBoard;

		// Token: 0x04003CCD RID: 15565
		[SerializeField]
		private bool _grabbedTarget = true;

		// Token: 0x04003CCE RID: 15566
		[SerializeField]
		private bool _failedTarget;

		// Token: 0x04003CCF RID: 15567
		private IAttackDamage _attackDamage;

		// Token: 0x04003CD1 RID: 15569
		private List<Target> results;
	}
}
