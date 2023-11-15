using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200024D RID: 589
	[Serializable]
	public class PositionNoise
	{
		// Token: 0x06000B96 RID: 2966 RVA: 0x0001FE9C File Offset: 0x0001E09C
		public Vector3 Evaluate()
		{
			switch (this._method)
			{
			case PositionNoise.Method.InsideCircle:
				return UnityEngine.Random.insideUnitCircle * this._value;
			case PositionNoise.Method.Horizontal:
				return new Vector3(UnityEngine.Random.Range(-this._value, this._value), 0f);
			case PositionNoise.Method.Vertical:
				return new Vector3(0f, UnityEngine.Random.Range(-this._value, this._value));
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0001FF1C File Offset: 0x0001E11C
		public Vector2 EvaluateAsVector2()
		{
			switch (this._method)
			{
			case PositionNoise.Method.InsideCircle:
				return UnityEngine.Random.insideUnitCircle * this._value;
			case PositionNoise.Method.Horizontal:
				return new Vector2(UnityEngine.Random.Range(-this._value, this._value), 0f);
			case PositionNoise.Method.Vertical:
				return new Vector2(0f, UnityEngine.Random.Range(-this._value, this._value));
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x0400099D RID: 2461
		[SerializeField]
		private PositionNoise.Method _method;

		// Token: 0x0400099E RID: 2462
		[SerializeField]
		private float _value;

		// Token: 0x0200024E RID: 590
		public enum Method
		{
			// Token: 0x040009A0 RID: 2464
			None,
			// Token: 0x040009A1 RID: 2465
			InsideCircle,
			// Token: 0x040009A2 RID: 2466
			Horizontal,
			// Token: 0x040009A3 RID: 2467
			Vertical
		}
	}
}
