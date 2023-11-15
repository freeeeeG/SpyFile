using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E9A RID: 3738
	[Serializable]
	public class ContainedInGrabBoard : ICondition
	{
		// Token: 0x060049D0 RID: 18896 RVA: 0x000D79A1 File Offset: 0x000D5BA1
		public bool Satisfied(Character character)
		{
			return this._grabBoard.HasInTargets(character);
		}

		// Token: 0x04003918 RID: 14616
		[SerializeField]
		private GrabBoard _grabBoard;
	}
}
