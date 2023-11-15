using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E51 RID: 3665
	public sealed class ClearGrabBoard : Operation
	{
		// Token: 0x060048DA RID: 18650 RVA: 0x000D4632 File Offset: 0x000D2832
		public override void Run()
		{
			this._grabBoard.Clear();
		}

		// Token: 0x040037E3 RID: 14307
		[SerializeField]
		private GrabBoard _grabBoard;
	}
}
