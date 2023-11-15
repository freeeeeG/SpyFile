using System;
using UnityEngine;

namespace FX
{
	// Token: 0x02000228 RID: 552
	public class ClearTrailOnDisable : MonoBehaviour
	{
		// Token: 0x06000ADB RID: 2779 RVA: 0x0001D9F6 File Offset: 0x0001BBF6
		private void OnDisable()
		{
			this._trailRenderer.Clear();
		}

		// Token: 0x040008F3 RID: 2291
		[SerializeField]
		private TrailRenderer _trailRenderer;
	}
}
