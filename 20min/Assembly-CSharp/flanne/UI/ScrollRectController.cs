using System;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000212 RID: 530
	public class ScrollRectController : MonoBehaviour
	{
		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002C56D File Offset: 0x0002A76D
		public void IncrementScrollbar()
		{
			this.scrollBar.value += this.scrollbarIncrements;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0002C587 File Offset: 0x0002A787
		public void DecrementScrollbar()
		{
			this.scrollBar.value -= this.scrollbarIncrements;
		}

		// Token: 0x0400084D RID: 2125
		[SerializeField]
		private Scrollbar scrollBar;

		// Token: 0x0400084E RID: 2126
		[Range(0f, 1f)]
		[SerializeField]
		private float scrollbarIncrements;
	}
}
