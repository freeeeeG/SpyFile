using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B67 RID: 2919
	public abstract class TriggerComponent : MonoBehaviour, ITrigger
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06003A6F RID: 14959
		public abstract float cooldownTime { get; }

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06003A70 RID: 14960
		public abstract float remainCooldownTime { get; }

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06003A71 RID: 14961
		// (remove) Token: 0x06003A72 RID: 14962
		public abstract event Action onTriggered;

		// Token: 0x06003A73 RID: 14963
		public abstract void Attach(Character character);

		// Token: 0x06003A74 RID: 14964
		public abstract void Detach();

		// Token: 0x06003A75 RID: 14965
		public abstract void UpdateTime(float deltaTime);

		// Token: 0x06003A76 RID: 14966 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x06003A77 RID: 14967
		public abstract void Refresh();

		// Token: 0x02000B68 RID: 2920
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06003A79 RID: 14969 RVA: 0x000ACAC4 File Offset: 0x000AACC4
			public SubcomponentAttribute() : base(true, TriggerComponent.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04002E73 RID: 11891
			public new static readonly Type[] types = new Type[]
			{
				typeof(OnActionComponent),
				typeof(OnApplyStatusComponent),
				typeof(OnChargeActionComponent),
				typeof(OnBackAttackComponent),
				typeof(OnBeginCombatComponent),
				typeof(OnConsumeShieldComponent),
				typeof(OnCooldownComponent),
				typeof(OnEnterMapComponent),
				typeof(OnEvadeComponent),
				typeof(OnFinishCombatComponent),
				typeof(OnGaugeFullComponent),
				typeof(OnGaugeEmptyComponent),
				typeof(OnGaveDamageStatusTargetComponent),
				typeof(OnValueGaugeValueReachedComponent),
				typeof(OnGaveDamageComponent),
				typeof(OnGroundedComponent),
				typeof(OnHealthChangedComponent),
				typeof(OnHealthValueComponent),
				typeof(OnHealedComponent),
				typeof(OnKilledComponent),
				typeof(OnStatusTargetKilledComponent),
				typeof(OnMarkMaxStackComponent),
				typeof(OnMoveComponent),
				typeof(OnSwapComponent),
				typeof(OnTookDamageComponent),
				typeof(OnUpdateComponent),
				typeof(OnJumpComponent),
				typeof(OnInscriptionItemDestroyedComponent),
				typeof(OnDashEvadeComponent),
				typeof(OnUseEssenceComponnet),
				typeof(OnActivateMapRewardComponent)
			};
		}
	}
}
