using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FBC RID: 4028
	public class ResetGlobalTransformToLocal : Operation
	{
		// Token: 0x06004E13 RID: 19987 RVA: 0x000E97CB File Offset: 0x000E79CB
		public override void Run()
		{
			this._transformHolder.ResetChildrenToLocal();
		}

		// Token: 0x04003E0F RID: 15887
		[SerializeField]
		private GlobalTransformHolder _transformHolder;
	}
}
