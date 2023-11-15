using System;
using Characters.Movements;
using Characters.Operations.Movement;
using FX.CastAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F78 RID: 3960
	public class CastAttackInfo : MonoBehaviour
	{
		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06004CD7 RID: 19671 RVA: 0x000E41F8 File Offset: 0x000E23F8
		internal HitInfo hitInfo
		{
			get
			{
				return this._hitInfo;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06004CD8 RID: 19672 RVA: 0x000E4200 File Offset: 0x000E2400
		internal OperationInfo.Subcomponents operationsToOwner
		{
			get
			{
				return this._operationsToOwner;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06004CD9 RID: 19673 RVA: 0x000E4208 File Offset: 0x000E2408
		internal TargetedOperationInfo.Subcomponents operationsToCharacter
		{
			get
			{
				return this._operationsToCharacter;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06004CDA RID: 19674 RVA: 0x000E4210 File Offset: 0x000E2410
		internal CastAttackVisualEffect.Subcomponents effect
		{
			get
			{
				return this._effect;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06004CDB RID: 19675 RVA: 0x000E4218 File Offset: 0x000E2418
		// (set) Token: 0x06004CDC RID: 19676 RVA: 0x000E4220 File Offset: 0x000E2420
		internal PushInfo pushInfo { get; private set; }

		// Token: 0x06004CDD RID: 19677 RVA: 0x000E422C File Offset: 0x000E242C
		internal void Initialize()
		{
			this._operationsToOwner.Initialize();
			this._operationsToCharacter.Initialize();
			foreach (TargetedOperationInfo targetedOperationInfo in this.operationsToCharacter.components)
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

		// Token: 0x06004CDE RID: 19678 RVA: 0x000E42A7 File Offset: 0x000E24A7
		internal void ApplyChrono(Character owner)
		{
			this._chronoToGlobe.ApplyGlobe();
			this._chronoToOwner.ApplyTo(owner);
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x000E42C0 File Offset: 0x000E24C0
		internal void ApplyChrono(Character owner, Character target)
		{
			this._chronoToGlobe.ApplyGlobe();
			this._chronoToOwner.ApplyTo(owner);
			this._chronoToTarget.ApplyTo(target);
		}

		// Token: 0x04003C78 RID: 15480
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

		// Token: 0x04003C79 RID: 15481
		[SerializeField]
		private ChronoInfo _chronoToGlobe;

		// Token: 0x04003C7A RID: 15482
		[SerializeField]
		private ChronoInfo _chronoToOwner;

		// Token: 0x04003C7B RID: 15483
		[SerializeField]
		private ChronoInfo _chronoToTarget;

		// Token: 0x04003C7C RID: 15484
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operationsToOwner;

		// Token: 0x04003C7D RID: 15485
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operationsToCharacter;

		// Token: 0x04003C7E RID: 15486
		[CastAttackVisualEffect.SubcomponentAttribute]
		[SerializeField]
		private CastAttackVisualEffect.Subcomponents _effect;
	}
}
