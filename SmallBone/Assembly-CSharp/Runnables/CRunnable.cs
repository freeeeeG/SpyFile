using System;
using System.Collections;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002EB RID: 747
	public abstract class CRunnable : MonoBehaviour
	{
		// Token: 0x06000EBF RID: 3775
		public abstract IEnumerator CRun();

		// Token: 0x04000C32 RID: 3122
		public static readonly Type[] types = new Type[]
		{
			typeof(CharacterTranslateTo),
			typeof(FadeIn),
			typeof(FadeOut),
			typeof(GameFadeIn),
			typeof(GameFadeOut),
			typeof(TransformTranslateTo),
			typeof(WaitForTime),
			typeof(WaitForWeaponUpgrade),
			typeof(ShowHardmodeEndingCredit)
		};
	}
}
