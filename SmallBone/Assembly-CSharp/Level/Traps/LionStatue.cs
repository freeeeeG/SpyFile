using System;
using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000667 RID: 1639
	public class LionStatue : MonoBehaviour
	{
		// Token: 0x060020DF RID: 8415 RVA: 0x00063486 File Offset: 0x00061686
		private void Awake()
		{
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00063495 File Offset: 0x00061695
		private IEnumerator CAttack()
		{
			float elapsed = -this._startTiming * this._interval;
			for (;;)
			{
				elapsed += Chronometer.global.deltaTime;
				if (elapsed >= this._interval)
				{
					this._attackAction.TryStart();
					elapsed -= this._interval;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04001BF6 RID: 7158
		[SerializeField]
		private Character _character;

		// Token: 0x04001BF7 RID: 7159
		[SerializeField]
		[Range(0f, 1f)]
		private float _startTiming;

		// Token: 0x04001BF8 RID: 7160
		[SerializeField]
		private float _interval = 4f;

		// Token: 0x04001BF9 RID: 7161
		[SerializeField]
		private Characters.Actions.Action _attackAction;
	}
}
