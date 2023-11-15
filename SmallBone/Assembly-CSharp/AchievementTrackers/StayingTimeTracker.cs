using System;
using System.Collections;
using Platforms;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x0200143C RID: 5180
	public class StayingTimeTracker : MonoBehaviour
	{
		// Token: 0x06006598 RID: 26008 RVA: 0x0012612F File Offset: 0x0012432F
		private IEnumerator Start()
		{
			yield return Chronometer.global.WaitForSeconds(this._time);
			this._achievement.Set();
			yield break;
		}

		// Token: 0x040051C8 RID: 20936
		[SerializeField]
		private float _time;

		// Token: 0x040051C9 RID: 20937
		[SerializeField]
		private Achievement.Type _achievement;
	}
}
