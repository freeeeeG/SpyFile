using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EAB RID: 3755
	[Serializable]
	public class InGrabBoard : IScope
	{
		// Token: 0x060049ED RID: 18925 RVA: 0x000D7CBC File Offset: 0x000D5EBC
		public List<Character> GetEnemyList()
		{
			List<Character> list = new List<Character>();
			foreach (Target target in this._grabBoard.targets)
			{
				list.Add(target.character);
			}
			return list;
		}

		// Token: 0x04003928 RID: 14632
		[SerializeField]
		private GrabBoard _grabBoard;
	}
}
