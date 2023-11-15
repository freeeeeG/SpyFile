using System;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F90 RID: 3984
	public sealed class PlayerAttack : CharacterOperation
	{
		// Token: 0x06004D4C RID: 19788 RVA: 0x000E643A File Offset: 0x000E463A
		private void Awake()
		{
			Array.Sort<TargetedOperationInfo>(this._targetWhenHit.components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x000E646B File Offset: 0x000E466B
		public override void Run(Character owner)
		{
			this.Attack(owner);
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x000E6474 File Offset: 0x000E4674
		public override void Initialize()
		{
			base.Initialize();
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			this._ownerWhenHit.Initialize();
			this._targetWhenHit.Initialize();
			foreach (TargetedOperationInfo targetedOperationInfo in this._targetWhenHit.components)
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

		// Token: 0x06004D4F RID: 19791 RVA: 0x000E6504 File Offset: 0x000E4704
		private void Attack(Character owner)
		{
			Target component = Singleton<Service>.Instance.levelManager.player.collider.GetComponent<Target>();
			bool flag = false;
			if (component == null)
			{
				return;
			}
			Vector2 hitPoint = MMMaths.RandomPointWithinBounds(component.collider.bounds);
			Vector2 force = Vector2.zero;
			if (this._pushInfo != null)
			{
				ValueTuple<Vector2, Vector2> valueTuple = this._pushInfo.EvaluateTimeIndependent(owner, component);
				force = valueTuple.Item1 + valueTuple.Item2;
			}
			if (this._adaptiveForce)
			{
				this._hitInfo.ChangeAdaptiveDamageAttribute(owner);
			}
			if (component.character != null)
			{
				if (!component.character.liveAndActive || component.character == owner)
				{
					return;
				}
				if (component.character.cinematic.value)
				{
					return;
				}
				this._chronoToTarget.ApplyTo(component.character);
				Damage damage = owner.stat.GetDamage((double)this._attackDamage.amount, hitPoint, this._hitInfo);
				if (this._hitInfo.attackType != Damage.AttackType.None)
				{
					CommonResource.instance.hitParticle.Emit(component.transform.position, component.collider.bounds, force, true);
				}
				flag = owner.TryAttackCharacter(component, ref damage);
				if (flag)
				{
					base.StartCoroutine(this._targetWhenHit.CRun(owner, component.character));
					this._effect.Spawn(owner, component.collider.bounds, damage, component);
				}
			}
			if (flag)
			{
				this._chronoToGlobe.ApplyGlobe();
				this._chronoToOwner.ApplyTo(owner);
				if (this._ownerWhenHit.components.Length != 0)
				{
					base.StartCoroutine(this._ownerWhenHit.CRun(owner));
				}
			}
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x000E66B2 File Offset: 0x000E48B2
		public override void Stop()
		{
			this._ownerWhenHit.StopAll();
		}

		// Token: 0x04003D0F RID: 15631
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003D10 RID: 15632
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003D11 RID: 15633
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003D12 RID: 15634
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003D13 RID: 15635
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003D14 RID: 15636
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _ownerWhenHit;

		// Token: 0x04003D15 RID: 15637
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _targetWhenHit;

		// Token: 0x04003D16 RID: 15638
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect;

		// Token: 0x04003D17 RID: 15639
		private IAttackDamage _attackDamage;

		// Token: 0x04003D18 RID: 15640
		private PushInfo _pushInfo;
	}
}
