using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x02000273 RID: 627
	public class Invulnerable : SpriteEffect
	{
		// Token: 0x06000C42 RID: 3138 RVA: 0x00021B07 File Offset: 0x0001FD07
		public Invulnerable(int priority, float flickInterval, float duration = float.PositiveInfinity) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._flickInterval = flickInterval;
			this._duration = duration;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00021B2C File Offset: 0x0001FD2C
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			Color color = this._propertyBlock.GetColor(SpriteEffect._baseColor);
			color.a = (this._transparent ? 0.3f : 0.7f);
			this._propertyBlock.SetColor(SpriteEffect._baseColor, color);
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00021B90 File Offset: 0x0001FD90
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			this._flickerTime -= deltaTime;
			if (this._flickerTime <= 0f)
			{
				this._flickerTime = this._flickInterval;
				this._transparent = !this._transparent;
			}
			return this._time < this._duration;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00021BEF File Offset: 0x0001FDEF
		internal override void Expire()
		{
			this._time = this._duration;
		}

		// Token: 0x04000A45 RID: 2629
		private readonly MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A46 RID: 2630
		private readonly float _flickInterval;

		// Token: 0x04000A47 RID: 2631
		private readonly float _duration;

		// Token: 0x04000A48 RID: 2632
		private float _flickerTime;

		// Token: 0x04000A49 RID: 2633
		private float _time;

		// Token: 0x04000A4A RID: 2634
		private bool _transparent;
	}
}
