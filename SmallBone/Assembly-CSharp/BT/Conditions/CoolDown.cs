using System;
using System.Collections;
using UnityEngine;

namespace BT.Conditions
{
	// Token: 0x0200142E RID: 5166
	public class CoolDown : Condition
	{
		// Token: 0x06006561 RID: 25953 RVA: 0x001257A6 File Offset: 0x001239A6
		private void OnEnable()
		{
			this._success = true;
		}

		// Token: 0x06006562 RID: 25954 RVA: 0x001257AF File Offset: 0x001239AF
		protected override bool Check(Context context)
		{
			if (this._success)
			{
				base.StartCoroutine(this.CCoolDown());
				return true;
			}
			return false;
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x001257C9 File Offset: 0x001239C9
		private IEnumerator CCoolDown()
		{
			this._success = false;
			float seconds = (this._time == 0f) ? this._timeOverrideValue.value : this._time;
			yield return Chronometer.global.WaitForSeconds(seconds);
			this._success = true;
			yield break;
		}

		// Token: 0x040051A7 RID: 20903
		[SerializeField]
		private CustomFloat _timeOverrideValue;

		// Token: 0x040051A8 RID: 20904
		[SerializeField]
		private float _time;

		// Token: 0x040051A9 RID: 20905
		private bool _success;
	}
}
