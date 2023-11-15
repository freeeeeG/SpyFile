using System;
using System.Collections;
using Level.Npc;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000302 RID: 770
	public sealed class WaitForWeaponUpgrade : CRunnable
	{
		// Token: 0x06000F1A RID: 3866 RVA: 0x0002E542 File Offset: 0x0002C742
		public override IEnumerator CRun()
		{
			yield return this._arachne.CUpgrade();
			yield break;
		}

		// Token: 0x04000C70 RID: 3184
		[SerializeField]
		private Arachne _arachne;
	}
}
