using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EA9 RID: 3753
	[Serializable]
	public class InBDVariable : IScope
	{
		// Token: 0x060049E9 RID: 18921 RVA: 0x000D7C40 File Offset: 0x000D5E40
		public List<Character> GetEnemyList()
		{
			this._enemise.Clear();
			Character value = this._communicator.GetVariable<SharedCharacter>(this._variableName).Value;
			if (value != null)
			{
				this._enemise.Add(value);
			}
			return this._enemise;
		}

		// Token: 0x04003925 RID: 14629
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04003926 RID: 14630
		[SerializeField]
		private string _variableName = "Target";

		// Token: 0x04003927 RID: 14631
		private List<Character> _enemise = new List<Character>();
	}
}
