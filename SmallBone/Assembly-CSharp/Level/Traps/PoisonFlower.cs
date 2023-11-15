using System;
using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000672 RID: 1650
	public class PoisonFlower : MonoBehaviour
	{
		// Token: 0x06002108 RID: 8456 RVA: 0x00063A0A File Offset: 0x00061C0A
		private void Awake()
		{
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00063A19 File Offset: 0x00061C19
		private IEnumerator CAttack()
		{
			yield return Chronometer.global.WaitForSeconds(this._startTiming * this._interval);
			for (;;)
			{
				this._attackAction.TryStart();
				yield return Chronometer.global.WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x04001C21 RID: 7201
		[SerializeField]
		private Character _character;

		// Token: 0x04001C22 RID: 7202
		[SerializeField]
		[Range(0f, 1f)]
		private float _startTiming;

		// Token: 0x04001C23 RID: 7203
		[SerializeField]
		private float _interval = 4f;

		// Token: 0x04001C24 RID: 7204
		[SerializeField]
		private Characters.Actions.Action _attackAction;
	}
}
