using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000D9 RID: 217
	public class AnimationRect : MonoBehaviour
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000E23C File Offset: 0x0000C43C
		private void OnEnable()
		{
			base.enabled = false;
		}

		// Token: 0x040002F0 RID: 752
		public float timeAnimScale = 0.3f;

		// Token: 0x040002F1 RID: 753
		public float timeDelayScale = 0.05f;

		// Token: 0x040002F2 RID: 754
		public float timeAnimRight = 0.3f;

		// Token: 0x040002F3 RID: 755
		public float timeDelayRight = 0.05f;

		// Token: 0x040002F4 RID: 756
		public float timeDelayRightNext;

		// Token: 0x040002F5 RID: 757
		public float timeAnimLeft = 0.3f;

		// Token: 0x040002F6 RID: 758
		public float timeDelayLeft = 0.05f;

		// Token: 0x040002F7 RID: 759
		public float timeDelayLeftNext;

		// Token: 0x040002F8 RID: 760
		public float timeAnimTop = 0.3f;

		// Token: 0x040002F9 RID: 761
		public float timeDelayTop = 0.05f;

		// Token: 0x040002FA RID: 762
		public float timeDelayTopNext;

		// Token: 0x040002FB RID: 763
		public float timeAnimBot = 0.3f;

		// Token: 0x040002FC RID: 764
		public float timeDelayBot = 0.05f;

		// Token: 0x040002FD RID: 765
		public float timeDelayBotNext;

		// Token: 0x040002FE RID: 766
		public RectTransform[] rectAnimScale;

		// Token: 0x040002FF RID: 767
		public RectTransform[] rectAnimRight;

		// Token: 0x04000300 RID: 768
		public RectTransform[] rectAnimLeft;

		// Token: 0x04000301 RID: 769
		public RectTransform[] rectAnimTop;

		// Token: 0x04000302 RID: 770
		public RectTransform[] rectAnimBot;

		// Token: 0x04000303 RID: 771
		private Vector3 scaleStart = new Vector3(0f, 0f, 0f);
	}
}
