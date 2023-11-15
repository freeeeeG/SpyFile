using System;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000322 RID: 802
	[Serializable]
	public class SetBDTargetToAttacker : IHitEvent
	{
		// Token: 0x06000F71 RID: 3953 RVA: 0x0002EFF4 File Offset: 0x0002D1F4
		public void OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			Character character = originalDamage.attacker.character;
			if (character == null || character.collider == null || originalDamage.attacker.character.health == null)
			{
				return;
			}
			if (this._layermask == 1 << character.gameObject.layer)
			{
				this._behaviorDesignerCommunicator.SetVariable<SharedCharacter>(this._variableName, character);
			}
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002F06C File Offset: 0x0002D26C
		void IHitEvent.OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this.OnHit(originalDamage, tookDamage, damageDealt);
		}

		// Token: 0x04000CB4 RID: 3252
		[SerializeField]
		private BehaviorDesignerCommunicator _behaviorDesignerCommunicator;

		// Token: 0x04000CB5 RID: 3253
		[SerializeField]
		private LayerMask _layermask;

		// Token: 0x04000CB6 RID: 3254
		[SerializeField]
		private string _variableName;
	}
}
