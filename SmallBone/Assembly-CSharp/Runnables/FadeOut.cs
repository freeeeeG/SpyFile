using System;
using System.Collections;
using Services;
using Singletons;

namespace Runnables
{
	// Token: 0x020002F4 RID: 756
	public class FadeOut : CRunnable
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002DFAE File Offset: 0x0002C1AE
		public override IEnumerator CRun()
		{
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			yield break;
		}
	}
}
