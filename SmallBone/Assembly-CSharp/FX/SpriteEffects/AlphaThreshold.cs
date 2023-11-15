using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x02000268 RID: 616
	public class AlphaThreshold : SpriteEffect
	{
		// Token: 0x06000C08 RID: 3080 RVA: 0x000212AB File Offset: 0x0001F4AB
		public AlphaThreshold(int priority, float value, float duration) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._value = value;
			if (duration == 0f)
			{
				duration = float.PositiveInfinity;
			}
			this._duration = duration;
			this._time = 0f;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000212E7 File Offset: 0x0001F4E7
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetFloat(SpriteEffect._alphaThreshold, this._value);
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00021317 File Offset: 0x0001F517
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime;
			return this._time < this._duration;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00021335 File Offset: 0x0001F535
		internal override void Expire()
		{
			this._time = this._duration;
		}

		// Token: 0x04000A0F RID: 2575
		private readonly MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A10 RID: 2576
		private readonly float _value;

		// Token: 0x04000A11 RID: 2577
		private readonly float _duration;

		// Token: 0x04000A12 RID: 2578
		private float _time;
	}
}
