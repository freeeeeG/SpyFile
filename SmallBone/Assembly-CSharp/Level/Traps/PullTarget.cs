using System;
using System.Collections;
using Characters.AI;
using Characters.Movements;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000649 RID: 1609
	public class PullTarget : Trap
	{
		// Token: 0x06002059 RID: 8281 RVA: 0x000620BD File Offset: 0x000602BD
		private void OnEnable()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000620CC File Offset: 0x000602CC
		private IEnumerator CRun()
		{
			for (;;)
			{
				yield return null;
				if (!(this._controller.target == null) && Chronometer.global.timeScale != 0f)
				{
					this._controller.target.movement.push.ApplyKnockback(this._controller.character, this._pushInfo);
				}
			}
			yield break;
		}

		// Token: 0x04001B6F RID: 7023
		[SerializeField]
		private AIController _controller;

		// Token: 0x04001B70 RID: 7024
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);
	}
}
