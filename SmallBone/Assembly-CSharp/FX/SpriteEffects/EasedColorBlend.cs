using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x0200026B RID: 619
	public class EasedColorBlend : SpriteEffect
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x00021474 File Offset: 0x0001F674
		public EasedColorBlend(int priority, Color startColor, Color endColor, Curve curve) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._startColor = startColor;
			this._endColor = endColor;
			this._curve = curve;
			this._time = 0f;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000214E0 File Offset: 0x0001F6E0
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetColor(SpriteEffect._baseColor, Color.Lerp(this._startColor, this._endColor, this._curve.Evaluate(this._time)));
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00021537 File Offset: 0x0001F737
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			return this._time < this._curve.duration;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0002155A File Offset: 0x0001F75A
		internal override void Expire()
		{
			this._time = this._curve.duration;
		}

		// Token: 0x04000A1B RID: 2587
		private readonly MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A1C RID: 2588
		private readonly Color _startColor = Color.white;

		// Token: 0x04000A1D RID: 2589
		private readonly Color _endColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04000A1E RID: 2590
		private readonly Curve _curve;

		// Token: 0x04000A1F RID: 2591
		private float _time;
	}
}
