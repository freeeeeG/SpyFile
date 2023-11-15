using System;
using System.Collections;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x0200055C RID: 1372
	public sealed class TimeOut : Leaf
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x000543C8 File Offset: 0x000525C8
		private void OnEnable()
		{
			base.StartCoroutine("CTimeOut");
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000543D6 File Offset: 0x000525D6
		protected override bool Check(EnemyWave wave)
		{
			return this._remainTime < 0f;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x000543E5 File Offset: 0x000525E5
		private IEnumerator CTimeOut()
		{
			this._remainTime = this._timeOut;
			while (this._remainTime > 0f)
			{
				yield return null;
				this._remainTime -= Chronometer.global.deltaTime;
			}
			yield break;
		}

		// Token: 0x04001746 RID: 5958
		[SerializeField]
		private float _timeOut;

		// Token: 0x04001747 RID: 5959
		private float _remainTime;
	}
}
