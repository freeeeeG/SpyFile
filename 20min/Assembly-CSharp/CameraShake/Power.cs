using System;

namespace CameraShake
{
	// Token: 0x0200005C RID: 92
	public static class Power
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x000169A5 File Offset: 0x00014BA5
		public static float Evaluate(float value, Degree degree)
		{
			switch (degree)
			{
			case Degree.Linear:
				return value;
			case Degree.Quadratic:
				return value * value;
			case Degree.Cubic:
				return value * value * value;
			case Degree.Quadric:
				return value * value * value * value;
			default:
				return value;
			}
		}
	}
}
