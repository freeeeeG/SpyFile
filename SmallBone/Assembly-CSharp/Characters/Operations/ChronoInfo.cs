using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DB7 RID: 3511
	[Serializable]
	public class ChronoInfo
	{
		// Token: 0x060046AB RID: 18091 RVA: 0x000CD201 File Offset: 0x000CB401
		public void ApplyTo(Character character)
		{
			if (this._timeScale != 1f && this._duration > 0f)
			{
				character.chronometer.animation.AttachTimeScale(this, this._timeScale, this._duration);
			}
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x000CD23A File Offset: 0x000CB43A
		public void ApplyGlobe()
		{
			if (this._timeScale != 1f && this._duration > 0f)
			{
				Chronometer.global.AttachTimeScale(this, this._timeScale, this._duration);
			}
		}

		// Token: 0x04003581 RID: 13697
		[SerializeField]
		private float _timeScale = 1f;

		// Token: 0x04003582 RID: 13698
		[SerializeField]
		[FrameTime]
		private float _duration;
	}
}
