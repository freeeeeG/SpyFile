using System;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200032C RID: 812
	public sealed class RunRunnables : Runnable
	{
		// Token: 0x06000F8E RID: 3982 RVA: 0x0002F224 File Offset: 0x0002D424
		public override void Run()
		{
			Runnable[] runnables = this._runnables;
			for (int i = 0; i < runnables.Length; i++)
			{
				runnables[i].Run();
			}
		}

		// Token: 0x04000CC5 RID: 3269
		[SerializeField]
		private Runnable[] _runnables;
	}
}
