using System;
using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000656 RID: 1622
	public class Compressor : ControlableTrap
	{
		// Token: 0x0600209B RID: 8347 RVA: 0x00062843 File Offset: 0x00060A43
		private void Awake()
		{
			this._coroutine = this.CRun();
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00062851 File Offset: 0x00060A51
		public override void Activate()
		{
			base.StartCoroutine(this._coroutine);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00062860 File Offset: 0x00060A60
		private IEnumerator CRun()
		{
			yield return Chronometer.global.WaitForSeconds(this._interval * this._startTiming);
			for (;;)
			{
				this._action.TryStart();
				yield return Chronometer.global.WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0006286F File Offset: 0x00060A6F
		public override void Deactivate()
		{
			base.StopCoroutine(this._coroutine);
		}

		// Token: 0x04001BA1 RID: 7073
		[SerializeField]
		private Character _character;

		// Token: 0x04001BA2 RID: 7074
		[Range(0f, 1f)]
		[SerializeField]
		private float _startTiming;

		// Token: 0x04001BA3 RID: 7075
		[SerializeField]
		private float _interval;

		// Token: 0x04001BA4 RID: 7076
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04001BA5 RID: 7077
		private IEnumerator _coroutine;
	}
}
