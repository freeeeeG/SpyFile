using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E9 RID: 233
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x000269E8 File Offset: 0x00024DE8
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00026A14 File Offset: 0x00024E14
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe key = this.curve[length - 1];
			key.time -= this.m_Range;
			Keyframe key2 = this.curve[0];
			key2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(key);
			this.m_InternalLoopingCurve.AddKey(key2);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00026ACC File Offset: 0x00024ECC
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x040003F0 RID: 1008
		public AnimationCurve curve;

		// Token: 0x040003F1 RID: 1009
		[SerializeField]
		private bool m_Loop;

		// Token: 0x040003F2 RID: 1010
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x040003F3 RID: 1011
		[SerializeField]
		private float m_Range;

		// Token: 0x040003F4 RID: 1012
		private AnimationCurve m_InternalLoopingCurve;
	}
}
