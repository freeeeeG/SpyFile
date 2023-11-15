using System;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F6C RID: 3948
	public class BoundsAttackInfo : MonoBehaviour
	{
		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06004C99 RID: 19609 RVA: 0x000E3531 File Offset: 0x000E1731
		internal HitInfo hitInfo
		{
			get
			{
				return this._hitInfo;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x000E3539 File Offset: 0x000E1739
		internal OperationInfo.Subcomponents operationsToOwner
		{
			get
			{
				return this._operationsToOwner;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x000E3541 File Offset: 0x000E1741
		internal TargetedOperationInfo.Subcomponents operationInfo
		{
			get
			{
				return this._operationInfo;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x000E3549 File Offset: 0x000E1749
		internal BoundsAttackVisualEffect.Subcomponents effect
		{
			get
			{
				return this._effect;
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06004C9D RID: 19613 RVA: 0x000E3551 File Offset: 0x000E1751
		// (set) Token: 0x06004C9E RID: 19614 RVA: 0x000E3559 File Offset: 0x000E1759
		internal PushInfo pushInfo { get; private set; }

		// Token: 0x06004C9F RID: 19615 RVA: 0x000E3564 File Offset: 0x000E1764
		internal void Initialize()
		{
			this._operationsToOwner.Initialize();
			this._operationInfo.Initialize();
			foreach (TargetedOperationInfo targetedOperationInfo in this._operationInfo.components)
			{
				Knockback knockback = targetedOperationInfo.operation as Knockback;
				if (knockback != null)
				{
					this.pushInfo = knockback.pushInfo;
					return;
				}
				Smash smash = targetedOperationInfo.operation as Smash;
				if (smash != null)
				{
					this.pushInfo = smash.pushInfo;
				}
			}
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x000E35DF File Offset: 0x000E17DF
		internal void ApplyChrono(Character owner)
		{
			this._chronoToGlobe.ApplyGlobe();
			this._chronoToOwner.ApplyTo(owner);
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x000E35F8 File Offset: 0x000E17F8
		internal void ApplyChrono(Character owner, Character target)
		{
			this._chronoToGlobe.ApplyGlobe();
			this._chronoToOwner.ApplyTo(owner);
			this._chronoToTarget.ApplyTo(target);
		}

		// Token: 0x04003C4A RID: 15434
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003C4B RID: 15435
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003C4C RID: 15436
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003C4D RID: 15437
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003C4E RID: 15438
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operationsToOwner;

		// Token: 0x04003C4F RID: 15439
		[Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _operationInfo;

		// Token: 0x04003C50 RID: 15440
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private BoundsAttackVisualEffect.Subcomponents _effect;
	}
}
