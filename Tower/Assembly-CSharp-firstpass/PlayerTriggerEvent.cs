using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000005 RID: 5
	public class PlayerTriggerEvent : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002300 File Offset: 0x00000500
		private void OnTriggerEnter(Collider other)
		{
			if (this.volumeLayerMask == (this.volumeLayerMask | 1 << other.gameObject.layer))
			{
				this.highlighterTrigger.ChangeTriggeringState(true);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002337 File Offset: 0x00000537
		private void OnTriggerExit(Collider other)
		{
			if (this.volumeLayerMask == (this.volumeLayerMask | 1 << other.gameObject.layer))
			{
				this.highlighterTrigger.ChangeTriggeringState(false);
			}
		}

		// Token: 0x04000006 RID: 6
		public HighlighterTrigger highlighterTrigger;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		private LayerMask volumeLayerMask;
	}
}
