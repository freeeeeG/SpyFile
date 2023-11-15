using System;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DCF RID: 3535
	public class BoundsHeal : CharacterOperation
	{
		// Token: 0x06004702 RID: 18178 RVA: 0x000CE143 File Offset: 0x000CC343
		static BoundsHeal()
		{
			BoundsHeal._targetOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x000CE16A File Offset: 0x000CC36A
		private void Awake()
		{
			this._ToTargetOperations.Initialize();
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x000CE178 File Offset: 0x000CC378
		public override void Run(Character owner)
		{
			foreach (Character character in BoundsHeal._targetOverlapper.OverlapCollider(this._range).GetComponents<Character>(true))
			{
				character.health.Heal(this.GetAmount(character), true);
				this._ToTargetOperations.Run(character);
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x000CE1F4 File Offset: 0x000CC3F4
		private double GetAmount(Character target)
		{
			BoundsHeal.Type type = this._type;
			if (type == BoundsHeal.Type.Percent)
			{
				return (double)this._amount.value * target.health.maximumHealth * 0.01;
			}
			if (type != BoundsHeal.Type.Constnat)
			{
				return 0.0;
			}
			return (double)this._amount.value;
		}

		// Token: 0x040035DD RID: 13789
		[SerializeField]
		private BoundsHeal.Type _type;

		// Token: 0x040035DE RID: 13790
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x040035DF RID: 13791
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040035E0 RID: 13792
		[UnityEditor.Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _ToTargetOperations;

		// Token: 0x040035E1 RID: 13793
		private static readonly NonAllocOverlapper _targetOverlapper = new NonAllocOverlapper(15);

		// Token: 0x02000DD0 RID: 3536
		private enum Type
		{
			// Token: 0x040035E3 RID: 13795
			Percent,
			// Token: 0x040035E4 RID: 13796
			Constnat
		}
	}
}
