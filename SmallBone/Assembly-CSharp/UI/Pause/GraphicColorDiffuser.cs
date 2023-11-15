using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x02000419 RID: 1049
	public class GraphicColorDiffuser : Graphic
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0003CDE4 File Offset: 0x0003AFE4
		// (set) Token: 0x060013E4 RID: 5092 RVA: 0x0003CDEC File Offset: 0x0003AFEC
		public override Color color
		{
			get
			{
				return base.color;
			}
			set
			{
				base.color = value;
				Graphic[] graphics = this._graphics;
				for (int i = 0; i < graphics.Length; i++)
				{
					graphics[i].color = value;
				}
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0003CE20 File Offset: 0x0003B020
		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			Graphic[] graphics = this._graphics;
			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			}
		}

		// Token: 0x040010E6 RID: 4326
		[SerializeField]
		private Graphic[] _graphics;
	}
}
