using System;
using UnityEngine;

namespace StylizedWater
{
	// Token: 0x0200007C RID: 124
	public static class GerstnerWaveDisplacement
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x00008254 File Offset: 0x00006454
		private static Vector3 GerstnerWave(Vector3 position, float steepness, float wavelength, float speed, float direction)
		{
			direction = direction * 2f - 1f;
			Vector2 normalized = new Vector2(Mathf.Cos(3.1415927f * direction), Mathf.Sin(3.1415927f * direction)).normalized;
			float num = 6.2831855f / wavelength;
			float num2 = steepness / num;
			float f = num * (Vector2.Dot(normalized, new Vector2(position.x, position.z)) - speed * Time.time);
			return new Vector3(normalized.x * num2 * Mathf.Cos(f), num2 * Mathf.Sin(f), normalized.y * num2 * Mathf.Cos(f));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000082F4 File Offset: 0x000064F4
		public static Vector3 GetWaveDisplacement(Vector3 position, float steepness, float wavelength, float speed, float[] directions)
		{
			return Vector3.zero + GerstnerWaveDisplacement.GerstnerWave(position, steepness, wavelength, speed, directions[0]) + GerstnerWaveDisplacement.GerstnerWave(position, steepness, wavelength, speed, directions[1]) + GerstnerWaveDisplacement.GerstnerWave(position, steepness, wavelength, speed, directions[2]) + GerstnerWaveDisplacement.GerstnerWave(position, steepness, wavelength, speed, directions[3]);
		}
	}
}
