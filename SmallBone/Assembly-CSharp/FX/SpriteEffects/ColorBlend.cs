using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x02000269 RID: 617
	public class ColorBlend : SpriteEffect
	{
		// Token: 0x06000C0C RID: 3084 RVA: 0x00021343 File Offset: 0x0001F543
		public ColorBlend(int priority, Color color, float duration) : base(priority)
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

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002137F File Offset: 0x0001F57F
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetColor(SpriteEffect._baseColor, this._color);
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000213AF File Offset: 0x0001F5AF
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			return this._time < this._duration;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000213CD File Offset: 0x0001F5CD
		internal override void Expire()
		{
			this._time = this._duration;
		}

		// Token: 0x04000A13 RID: 2579
		private readonly MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A14 RID: 2580
		private readonly Color _color;

		// Token: 0x04000A15 RID: 2581
		private readonly float _duration;

		// Token: 0x04000A16 RID: 2582
		private float _time;
	}
}
