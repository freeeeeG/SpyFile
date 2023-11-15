using System;
using System.Collections;
using Services;
using Singletons;

namespace Runnables
{
	// Token: 0x020002F2 RID: 754
	public class FadeIn : CRunnable
	{
		// Token: 0x06000EDA RID: 3802 RVA: 0x0002DF42 File Offset: 0x0002C142
		public override IEnumerator CRun()
		{
			yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
			yield break;
		}
	}
}
