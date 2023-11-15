using System;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000328 RID: 808
	public sealed class LoadChapter : Runnable
	{
		// Token: 0x06000F85 RID: 3973 RVA: 0x0002F1CC File Offset: 0x0002D3CC
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.Load(this._chapter);
		}

		// Token: 0x04000CC1 RID: 3265
		[SerializeField]
		private Chapter.Type _chapter;
	}
}
