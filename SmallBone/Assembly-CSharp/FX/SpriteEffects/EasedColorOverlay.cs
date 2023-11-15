using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x0200026C RID: 620
	public class EasedColorOverlay : SpriteEffect
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x00021570 File Offset: 0x0001F770
		public EasedColorOverlay(int priority, Color startColor, Color endColor, Curve curve) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._startColor = startColor;
			this._endColor = endColor;
			this._curve = curve;
			this._time = 0f;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000215DC File Offset: 0x0001F7DC
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetColor(SpriteEffect._overlayColor, Color.Lerp(this._startColor, this._endColor, this._curve.Evaluate(this._time)));
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00021633 File Offset: 0x0001F833
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			return this._time < this._curve.duration;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00021656 File Offset: 0x0001F856
		internal override void Expire()
		{
			this._time = this._curve.duration;
		}

		// Token: 0x04000A20 RID: 2592
		private readonly MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A21 RID: 2593
		private readonly Color _startColor = Color.white;

		// Token: 0x04000A22 RID: 2594
		private readonly Color _endColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04000A23 RID: 2595
		private readonly Curve _curve;

		// Token: 0x04000A24 RID: 2596
		private float _time;
	}
}
