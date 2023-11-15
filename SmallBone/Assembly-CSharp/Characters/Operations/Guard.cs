using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E1A RID: 3610
	public class Guard : CharacterOperation
	{
		// Token: 0x0600480B RID: 18443 RVA: 0x000D1C47 File Offset: 0x000CFE47
		public override void Run(Character owner)
		{
			this._guard.Initialize(owner);
			this._guard.GuardUp();
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CExpire(owner));
			}
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x000D1C7B File Offset: 0x000CFE7B
		private IEnumerator CExpire(Character owner)
		{
			yield return owner.chronometer.master.WaitForSeconds(this._duration);
			this.Stop();
			yield break;
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x000D1C91 File Offset: 0x000CFE91
		public override void Stop()
		{
			this._guard.GuardDown();
		}

		// Token: 0x04003727 RID: 14119
		[SerializeField]
		private Guard _guard;

		// Token: 0x04003728 RID: 14120
		[SerializeField]
		private float _duration;
	}
}
