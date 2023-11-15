using System;
using FX;
using UnityEngine;

namespace Characters.Marks
{
	// Token: 0x02000818 RID: 2072
	[CreateAssetMenu]
	public class MarkInfo : ScriptableObject
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x0008385F File Offset: 0x00081A5F
		public int maxStack
		{
			get
			{
				return this._maxStack;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002AA7 RID: 10919 RVA: 0x00083867 File Offset: 0x00081A67
		public Sprite[] stackImages
		{
			get
			{
				return this._stackImages;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x0008386F File Offset: 0x00081A6F
		public EffectInfo.AttachInfo attachInfo
		{
			get
			{
				return this._attachInfo;
			}
		}

		// Token: 0x04002452 RID: 9298
		public MarkInfo.OnStackDelegate onStack;

		// Token: 0x04002453 RID: 9299
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002454 RID: 9300
		[SerializeField]
		protected Sprite[] _stackImages;

		// Token: 0x04002455 RID: 9301
		[SerializeField]
		private EffectInfo.AttachInfo _attachInfo = new EffectInfo.AttachInfo(true, false, 9, EffectInfo.AttachInfo.Pivot.Top);

		// Token: 0x02000819 RID: 2073
		// (Invoke) Token: 0x06002AAB RID: 10923
		public delegate void OnStackDelegate(Mark mark, float stack);
	}
}
