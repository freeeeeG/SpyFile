using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner
{
	// Token: 0x02000F69 RID: 3945
	public class OverrideBDVariableInRange : Operation
	{
		// Token: 0x06004C91 RID: 19601 RVA: 0x000E3368 File Offset: 0x000E1568
		public override void Run()
		{
			if (this._owner == null)
			{
				this._owner = base.GetComponentInParent<Character>();
			}
			SharedVariable variable = this._owner.GetComponent<BehaviorDesignerCommunicator>().GetVariable(this._variableName);
			if (variable == null)
			{
				return;
			}
			OverrideBDVariableInRange._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._owner.gameObject));
			List<Character> components = OverrideBDVariableInRange._overlapper.OverlapCollider(this._notifyRange).GetComponents<Character>(true);
			this._ownerPlatform = this._owner.movement.controller.collisionState.lastStandingCollider;
			foreach (Character character in components)
			{
				if (this.IsEqaulPlatform(character))
				{
					BehaviorDesignerCommunicator component = character.GetComponent<BehaviorDesignerCommunicator>();
					if (component != null)
					{
						component.SetVariable(this._variableName, variable);
					}
				}
			}
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x000E3464 File Offset: 0x000E1664
		private bool IsEqaulPlatform(Character target)
		{
			if (this._owner.movement.config.type == Movement.Config.Type.Static)
			{
				return true;
			}
			if (target.movement.config.type == Movement.Config.Type.Static)
			{
				return true;
			}
			Collider2D lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
			return lastStandingCollider != null && this._ownerPlatform != null && this._ownerPlatform == lastStandingCollider;
		}

		// Token: 0x04003C3F RID: 15423
		private static NonAllocOverlapper _overlapper = new NonAllocOverlapper(31);

		// Token: 0x04003C40 RID: 15424
		[SerializeField]
		[GetComponentInParent(false)]
		private Character _owner;

		// Token: 0x04003C41 RID: 15425
		[SerializeField]
		private Collider2D _notifyRange;

		// Token: 0x04003C42 RID: 15426
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04003C43 RID: 15427
		[SerializeField]
		private string _variableName;

		// Token: 0x04003C44 RID: 15428
		private Collider2D _ownerPlatform;
	}
}
