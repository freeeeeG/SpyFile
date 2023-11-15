using System;
using System.Collections;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x0200055E RID: 1374
	public sealed class TimeRemain : Leaf
	{
		// Token: 0x06001B1A RID: 6938 RVA: 0x00054481 File Offset: 0x00052681
		private void Awake()
		{
			if (this._target == null)
			{
				this.CheckTime();
				return;
			}
			this._target.onSpawn += this.CheckTime;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x000544AF File Offset: 0x000526AF
		protected override bool Check(EnemyWave wave)
		{
			return this._spawnable;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x000544B7 File Offset: 0x000526B7
		private void CheckTime()
		{
			base.StartCoroutine("CCheckTime");
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000544C5 File Offset: 0x000526C5
		private IEnumerator CCheckTime()
		{
			yield return Chronometer.global.WaitForSeconds(this._time);
			this._spawnable = true;
			yield break;
		}

		// Token: 0x0400174B RID: 5963
		[SerializeField]
		private EnemyWave _target;

		// Token: 0x0400174C RID: 5964
		[SerializeField]
		private float _time;

		// Token: 0x0400174D RID: 5965
		private bool _spawnable;
	}
}
