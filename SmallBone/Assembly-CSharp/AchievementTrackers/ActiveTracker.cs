using System;
using Platforms;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x02001435 RID: 5173
	public class ActiveTracker : MonoBehaviour
	{
		// Token: 0x0600657A RID: 25978 RVA: 0x00125A2A File Offset: 0x00123C2A
		public void Awake()
		{
			this._achievement.Set();
		}

		// Token: 0x040051B5 RID: 20917
		[SerializeField]
		private Achievement.Type _achievement;
	}
}
