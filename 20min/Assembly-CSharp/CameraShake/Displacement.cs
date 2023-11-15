using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000056 RID: 86
	[Serializable]
	public struct Displacement
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0001610D File Offset: 0x0001430D
		public Displacement(Vector3 position, Vector3 eulerAngles)
		{
			this.position = position;
			this.eulerAngles = eulerAngles;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001611D File Offset: 0x0001431D
		public Displacement(Vector3 position)
		{
			this.position = position;
			this.eulerAngles = Vector3.zero;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00016131 File Offset: 0x00014331
		public static Displacement Zero
		{
			get
			{
				return new Displacement(Vector3.zero, Vector3.zero);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00016142 File Offset: 0x00014342
		public static Displacement operator +(Displacement a, Displacement b)
		{
			return new Displacement(a.position + b.position, b.eulerAngles + a.eulerAngles);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001616B File Offset: 0x0001436B
		public static Displacement operator -(Displacement a, Displacement b)
		{
			return new Displacement(a.position - b.position, b.eulerAngles - a.eulerAngles);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00016194 File Offset: 0x00014394
		public static Displacement operator -(Displacement disp)
		{
			return new Displacement(-disp.position, -disp.eulerAngles);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000161B1 File Offset: 0x000143B1
		public static Displacement operator *(Displacement coords, float number)
		{
			return new Displacement(coords.position * number, coords.eulerAngles * number);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000161D0 File Offset: 0x000143D0
		public static Displacement operator *(float number, Displacement coords)
		{
			return coords * number;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000161D9 File Offset: 0x000143D9
		public static Displacement operator /(Displacement coords, float number)
		{
			return new Displacement(coords.position / number, coords.eulerAngles / number);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000161F8 File Offset: 0x000143F8
		public static Displacement Scale(Displacement a, Displacement b)
		{
			return new Displacement(Vector3.Scale(a.position, b.position), Vector3.Scale(b.eulerAngles, a.eulerAngles));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00016221 File Offset: 0x00014421
		public static Displacement Lerp(Displacement a, Displacement b, float t)
		{
			return new Displacement(Vector3.Lerp(a.position, b.position, t), Vector3.Lerp(a.eulerAngles, b.eulerAngles, t));
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001624C File Offset: 0x0001444C
		public Displacement ScaledBy(float posScale, float rotScale)
		{
			return new Displacement(this.position * posScale, this.eulerAngles * rotScale);
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0001626B File Offset: 0x0001446B
		public Displacement Normalized
		{
			get
			{
				return new Displacement(this.position.normalized, this.eulerAngles.normalized);
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00016288 File Offset: 0x00014488
		public static Displacement InsideUnitSpheres()
		{
			return new Displacement(Random.insideUnitSphere, Random.insideUnitSphere);
		}

		// Token: 0x0400022E RID: 558
		public Vector3 position;

		// Token: 0x0400022F RID: 559
		public Vector3 eulerAngles;
	}
}
