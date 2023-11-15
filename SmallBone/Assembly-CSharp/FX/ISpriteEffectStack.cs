using System;
using FX.SpriteEffects;
using UnityEngine;

namespace FX
{
	// Token: 0x02000240 RID: 576
	public interface ISpriteEffectStack
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000B53 RID: 2899
		SpriteRenderer mainRenderer { get; }

		// Token: 0x06000B54 RID: 2900
		void Add(SpriteEffect effect);

		// Token: 0x06000B55 RID: 2901
		bool Contains(SpriteEffect effect);

		// Token: 0x06000B56 RID: 2902
		bool Remove(SpriteEffect effect);
	}
}
