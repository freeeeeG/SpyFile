using System;
using UnityEngine;
using Utils;

namespace Characters.Operations.GrabBorad.Custom
{
	// Token: 0x02000E93 RID: 3731
	public class CopyGrabBoard : CharacterOperation
	{
		// Token: 0x060049C6 RID: 18886 RVA: 0x000D7858 File Offset: 0x000D5A58
		public override void Run(Character owner)
		{
			this._to.targets.AddRange(this._from.targets);
		}

		// Token: 0x04003904 RID: 14596
		[SerializeField]
		private GrabBoard _to;

		// Token: 0x04003905 RID: 14597
		[SerializeField]
		private GrabBoard _from;
	}
}
