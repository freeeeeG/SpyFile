using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x0200026A RID: 618
	public class ColorOverlay : SpriteEffect
	{
		// Token: 0x06000C10 RID: 3088 RVA: 0x000213DB File Offset: 0x0001F5DB
		public ColorOverlay(int priority, Color color, float duration) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._color = color;
			if (duration == 0f)
			{
				duration = float.PositiveInfinity;
			}
			this._duration = duration;
			this._time = 0f;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00021417 File Offset: 0x0001F617
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetColor(SpriteEffect._overlayColor, this._color);
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00021447 File Offset: 0x0001F647
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			return this._time < this._duration;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00021465 File Offset: 0x0001F665
		internal override void Expire()
		{
			this._time = this._duration;
		}

		// Token: 0x04000A17 RID: 2583
		private readonly MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A18 RID: 2584
		private readonly Color _color;

		// Token: 0x04000A19 RID: 2585
		private readonly float _duration;

		// Token: 0x04000A1A RID: 2586
		private float _time;
	}
}
