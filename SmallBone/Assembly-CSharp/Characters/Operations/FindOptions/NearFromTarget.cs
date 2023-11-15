using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EA1 RID: 3745
	[Serializable]
	public class NearFromTarget : ICondition
	{
		// Token: 0x060049DD RID: 18909 RVA: 0x000D7A74 File Offset: 0x000D5C74
		public bool Satisfied(Character character)
		{
			Character value = this._communicator.GetVariable<SharedCharacter>(this._targetName).Value;
			if (value == null)
			{
				return false;
			}
			float num = Vector2.Distance(value.transform.position, character.transform.position);
			return this._maxDistance.value > num;
		}

		// Token: 0x0400391E RID: 14622
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x0400391F RID: 14623
		[SerializeField]
		private string _targetName = "Target";

		// Token: 0x04003920 RID: 14624
		[SerializeField]
		private CustomFloat _maxDistance;
	}
}
