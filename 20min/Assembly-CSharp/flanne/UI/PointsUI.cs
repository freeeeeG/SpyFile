using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200020D RID: 525
	public class PointsUI : MonoBehaviour
	{
		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002C103 File Offset: 0x0002A303
		private void OnPointChanged(object sender, int pts)
		{
			this.Refresh(pts);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0002C10C File Offset: 0x0002A30C
		private void Start()
		{
			this.Refresh(PointsTracker.pts);
			PointsTracker.PointsChangedEvent = (EventHandler<int>)Delegate.Combine(PointsTracker.PointsChangedEvent, new EventHandler<int>(this.OnPointChanged));
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002C139 File Offset: 0x0002A339
		private void OnDestroy()
		{
			PointsTracker.PointsChangedEvent = (EventHandler<int>)Delegate.Remove(PointsTracker.PointsChangedEvent, new EventHandler<int>(this.OnPointChanged));
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0002C15B File Offset: 0x0002A35B
		private void Refresh(int pts)
		{
			this.ptsTMP.text = pts.ToString();
		}

		// Token: 0x04000845 RID: 2117
		[SerializeField]
		private TMP_Text ptsTMP;
	}
}
