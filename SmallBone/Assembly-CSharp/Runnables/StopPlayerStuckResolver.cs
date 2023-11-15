using System;
using System.Collections;
using Characters.Movements;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000334 RID: 820
	public sealed class StopPlayerStuckResolver : Runnable
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x0002F4B8 File Offset: 0x0002D6B8
		public override void Run()
		{
			if (this._duration == 0f)
			{
				this._remainTime = 2.1474836E+09f;
			}
			else
			{
				this._remainTime = this._duration;
			}
			this._stuckResolver = Singleton<Service>.Instance.levelManager.player.GetComponent<StuckResolver>();
			if (this._stuckResolver == null)
			{
				return;
			}
			this._cupdateReference.Stop();
			this._cupdateReference = this.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0002F531 File Offset: 0x0002D731
		private IEnumerator CUpdate()
		{
			this._stuckResolver.stop.Attach(this);
			while (this._remainTime > 0f)
			{
				yield return null;
				this._remainTime -= Chronometer.global.deltaTime;
			}
			this._stuckResolver.stop.Detach(this);
			yield break;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002F540 File Offset: 0x0002D740
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			if (this._stuckResolver == null)
			{
				return;
			}
			this._stuckResolver.stop.Detach(this);
		}

		// Token: 0x04000CD7 RID: 3287
		[SerializeField]
		private float _duration;

		// Token: 0x04000CD8 RID: 3288
		private float _remainTime;

		// Token: 0x04000CD9 RID: 3289
		private CoroutineReference _cupdateReference;

		// Token: 0x04000CDA RID: 3290
		private StuckResolver _stuckResolver;
	}
}
