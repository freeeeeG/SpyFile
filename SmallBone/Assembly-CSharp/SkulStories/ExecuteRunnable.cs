using System;
using Runnables;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x020000FD RID: 253
	public sealed class ExecuteRunnable : Event
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x0000F6BD File Offset: 0x0000D8BD
		public override void Run()
		{
			this._runnable.Run();
		}

		// Token: 0x040003B8 RID: 952
		[SerializeField]
		[Runnable.SubcomponentAttribute]
		private Runnable _runnable;
	}
}
