using System;
using System.Collections;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x02000179 RID: 377
	public class RefundAmmoWhileStillBuff : Buff
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x000263D8 File Offset: 0x000245D8
		public override void OnAttach()
		{
			this.AddObserver(new Action<object, object>(this.OnCheckshouldConumeAmmo), Ammo.ShouldConsumeAmmoCheck);
			this._checkStillCoroutine = this.CheckStillCR();
			PlayerController.Instance.StartCoroutine(this._checkStillCoroutine);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002640E File Offset: 0x0002460E
		public override void OnUnattach()
		{
			this.RemoveObserver(new Action<object, object>(this.OnCheckshouldConumeAmmo), Ammo.ShouldConsumeAmmoCheck);
			PlayerController.Instance.StopCoroutine(this._checkStillCoroutine);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00026438 File Offset: 0x00024638
		private void OnCheckshouldConumeAmmo(object sender, object args)
		{
			BaseException ex = args as BaseException;
			if (this._lastFramePos == this._thisFramePos && Random.Range(0f, 1f) < this.chanceToRefund)
			{
				ex.FlipToggle();
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002647C File Offset: 0x0002467C
		private IEnumerator CheckStillCR()
		{
			PlayerController player = PlayerController.Instance;
			this._lastFramePos = player.transform.position;
			this._thisFramePos = player.transform.position;
			for (;;)
			{
				yield return new WaitForFixedUpdate();
				this._lastFramePos = this._thisFramePos;
				this._thisFramePos = player.transform.position;
			}
			yield break;
		}

		// Token: 0x040006C0 RID: 1728
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToRefund;

		// Token: 0x040006C1 RID: 1729
		[NonSerialized]
		private IEnumerator _checkStillCoroutine;

		// Token: 0x040006C2 RID: 1730
		[NonSerialized]
		private Vector3 _lastFramePos;

		// Token: 0x040006C3 RID: 1731
		[NonSerialized]
		private Vector3 _thisFramePos;
	}
}
