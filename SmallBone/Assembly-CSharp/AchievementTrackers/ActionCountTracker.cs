using System;
using System.Collections;
using Platforms;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x02001433 RID: 5171
	public class ActionCountTracker : MonoBehaviour
	{
		// Token: 0x06006571 RID: 25969 RVA: 0x00125958 File Offset: 0x00123B58
		public void AddCount()
		{
			this._currentCount++;
			if (this._timeout > 0)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.CTimeout());
			}
			if (this._currentCount < this._count)
			{
				return;
			}
			this._achievement.Set();
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x001259A9 File Offset: 0x00123BA9
		private IEnumerator CTimeout()
		{
			yield return Chronometer.global.WaitForSeconds((float)this._timeout);
			this._currentCount = 0;
			yield break;
		}

		// Token: 0x040051AE RID: 20910
		[SerializeField]
		private Achievement.Type _achievement;

		// Token: 0x040051AF RID: 20911
		[SerializeField]
		private int _count;

		// Token: 0x040051B0 RID: 20912
		[SerializeField]
		private int _timeout;

		// Token: 0x040051B1 RID: 20913
		private int _currentCount;
	}
}
