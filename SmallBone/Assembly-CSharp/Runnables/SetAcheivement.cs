using System;
using Platforms;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002DB RID: 731
	public sealed class SetAcheivement : Runnable
	{
		// Token: 0x06000E9B RID: 3739 RVA: 0x0002D7BF File Offset: 0x0002B9BF
		public override void Run()
		{
			this._type.Set();
		}

		// Token: 0x04000C19 RID: 3097
		[SerializeField]
		private Achievement.Type _type;
	}
}
