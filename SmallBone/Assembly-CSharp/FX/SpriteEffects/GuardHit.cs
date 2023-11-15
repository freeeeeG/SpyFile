using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x02000272 RID: 626
	public sealed class GuardHit : SpriteEffect
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x00021A00 File Offset: 0x0001FC00
		public GuardHit(int priority = 0, float duration = 0.2f) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._duration = duration;
			this._time = 0f;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00021A6F File Offset: 0x0001FC6F
		internal void Reset()
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._time = 0f;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00021A88 File Offset: 0x0001FC88
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetColor(SpriteEffect._overlayColor, Color.LerpUnclamped(this._startColor, this._endColor, this._time / this._duration));
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00021ADB File Offset: 0x0001FCDB
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			return this._time < this._duration;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00021AF9 File Offset: 0x0001FCF9
		internal override void Expire()
		{
			this._time = this._duration;
		}

		// Token: 0x04000A40 RID: 2624
		private readonly float _duration;

		// Token: 0x04000A41 RID: 2625
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A42 RID: 2626
		private float _time;

		// Token: 0x04000A43 RID: 2627
		private Color _startColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000A44 RID: 2628
		private Color _endColor = new Color(1f, 1f, 1f, 0f);
	}
}
