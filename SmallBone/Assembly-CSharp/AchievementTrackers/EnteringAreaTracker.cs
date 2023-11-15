using System;
using Characters;
using Platforms;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x02001438 RID: 5176
	public class EnteringAreaTracker : MonoBehaviour
	{
		// Token: 0x06006586 RID: 25990 RVA: 0x00125D4A File Offset: 0x00123F4A
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.GetComponent<Character>() == null)
			{
				return;
			}
			this._achievement.Set();
		}

		// Token: 0x040051BE RID: 20926
		[SerializeField]
		private Collider2D _area;

		// Token: 0x040051BF RID: 20927
		[SerializeField]
		private Achievement.Type _achievement;
	}
}
