using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000052 RID: 82
	public static class Attenuator
	{
		// Token: 0x06000415 RID: 1045 RVA: 0x00015958 File Offset: 0x00013B58
		public static float Strength(Attenuator.StrengthAttenuationParams pars, Vector3 sourcePosition, Vector3 cameraPosition)
		{
			Vector3 b = cameraPosition - sourcePosition;
			float magnitude = Vector3.Scale(pars.axesMultiplier, b).magnitude;
			return Power.Evaluate(Mathf.Clamp01(1f - (magnitude - pars.clippingDistance) / pars.falloffScale), pars.falloffDegree);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000159A8 File Offset: 0x00013BA8
		public static Displacement Direction(Vector3 sourcePosition, Vector3 cameraPosition, Quaternion cameraRotation)
		{
			Displacement zero = Displacement.Zero;
			zero.position = (cameraPosition - sourcePosition).normalized;
			zero.position = Quaternion.Inverse(cameraRotation) * zero.position;
			zero.eulerAngles.x = zero.position.z;
			zero.eulerAngles.y = zero.position.x;
			zero.eulerAngles.z = -zero.position.x;
			return zero;
		}

		// Token: 0x0200028E RID: 654
		[Serializable]
		public class StrengthAttenuationParams
		{
			// Token: 0x04000A1C RID: 2588
			[Tooltip("Radius in which shake doesn't lose strength.")]
			public float clippingDistance = 10f;

			// Token: 0x04000A1D RID: 2589
			[Tooltip("How fast strength falls with distance.")]
			public float falloffScale = 50f;

			// Token: 0x04000A1E RID: 2590
			[Tooltip("Power of the falloff function.")]
			public Degree falloffDegree = Degree.Quadratic;

			// Token: 0x04000A1F RID: 2591
			[Tooltip("Contribution of each axis to distance. E. g. (1, 1, 0) for a 2D game in XY plane.")]
			public Vector3 axesMultiplier = Vector3.one;
		}
	}
}
