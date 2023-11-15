using System;
using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200065E RID: 1630
	public class FlameFlower : MonoBehaviour
	{
		// Token: 0x060020B9 RID: 8377 RVA: 0x00062EBE File Offset: 0x000610BE
		private void Awake()
		{
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00062ECD File Offset: 0x000610CD
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

		// Token: 0x04001BC8 RID: 7112
		[SerializeField]
		private Character _character;

		// Token: 0x04001BC9 RID: 7113
		[SerializeField]
		private GameObject _horizontalBody;

		// Token: 0x04001BCA RID: 7114
		[SerializeField]
		private GameObject _verticalBody;

		// Token: 0x04001BCB RID: 7115
		[SerializeField]
		[Range(0f, 1f)]
		private float _startTiming;

		// Token: 0x04001BCC RID: 7116
		[SerializeField]
		private float _interval = 4f;

		// Token: 0x04001BCD RID: 7117
		[SerializeField]
		private Characters.Actions.Action _attackAction;
	}
}
