using System;
using BehaviorDesigner.Runtime;
using FX;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E31 RID: 3633
	public sealed class SetBDCharacterVariableInRange : CharacterOperation
	{
		// Token: 0x06004873 RID: 18547 RVA: 0x000D29E4 File Offset: 0x000D0BE4
		public override void Run(Character owner)
		{
			this._overlapper = new NonAllocOverlapper(31);
			this._range.enabled = true;
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(owner.gameObject));
			foreach (Character character in this._overlapper.OverlapCollider(this._range).GetComponents<Character>(true))
			{
				BehaviorDesignerCommunicator component = character.GetComponent<BehaviorDesignerCommunicator>();
				if (component != null)
				{
					if (component.GetVariable(this.variableName) == null)
					{
						break;
					}
					component.SetVariable(this.variableName, this._variable);
					this._effect.Spawn(component.transform.position, 0f, 1f);
				}
			}
		}

		// Token: 0x0400377F RID: 14207
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04003780 RID: 14208
		[SerializeField]
		private TargetLayer _targetLayer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04003781 RID: 14209
		[SerializeField]
		private string variableName;

		// Token: 0x04003782 RID: 14210
		[SubclassSelector]
		[SerializeReference]
		private SharedVariable _variable;

		// Token: 0x04003783 RID: 14211
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x04003784 RID: 14212
		private NonAllocOverlapper _overlapper;
	}
}
