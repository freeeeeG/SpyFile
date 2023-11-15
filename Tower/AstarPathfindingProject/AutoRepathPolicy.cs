using System;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class AutoRepathPolicy
	{
		// Token: 0x0600028A RID: 650 RVA: 0x0000E188 File Offset: 0x0000C388
		public virtual bool ShouldRecalculatePath(Vector3 position, float radius, Vector3 destination)
		{
			if (this.mode == AutoRepathPolicy.Mode.Never || float.IsPositiveInfinity(destination.x))
			{
				return false;
			}
			float num = Time.time - this.lastRepathTime;
			if (this.mode == AutoRepathPolicy.Mode.EveryNSeconds)
			{
				return num >= this.period;
			}
			float num2 = (destination - this.lastDestination).sqrMagnitude / Mathf.Max((position - this.lastDestination).sqrMagnitude, radius * radius) * (this.sensitivity * this.sensitivity);
			return num2 > 1f || float.IsNaN(num2) || num >= this.maximumPeriod * (1f - Mathf.Sqrt(num2));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000E23A File Offset: 0x0000C43A
		public virtual void Reset()
		{
			this.lastRepathTime = float.NegativeInfinity;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000E247 File Offset: 0x0000C447
		public virtual void DidRecalculatePath(Vector3 destination)
		{
			this.lastRepathTime = Time.time;
			this.lastDestination = destination;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000E25C File Offset: 0x0000C45C
		public void DrawGizmos(Vector3 position, float radius)
		{
			if (this.visualizeSensitivity && !float.IsPositiveInfinity(this.lastDestination.x))
			{
				float radius2 = Mathf.Sqrt(Mathf.Max((position - this.lastDestination).sqrMagnitude, radius * radius) / (this.sensitivity * this.sensitivity));
				Draw.Gizmos.CircleXZ(this.lastDestination, radius2, Color.magenta, 0f, 6.2831855f);
			}
		}

		// Token: 0x040001AD RID: 429
		public AutoRepathPolicy.Mode mode = AutoRepathPolicy.Mode.Dynamic;

		// Token: 0x040001AE RID: 430
		[FormerlySerializedAs("interval")]
		public float period = 0.5f;

		// Token: 0x040001AF RID: 431
		public float sensitivity = 10f;

		// Token: 0x040001B0 RID: 432
		[FormerlySerializedAs("maximumInterval")]
		public float maximumPeriod = 2f;

		// Token: 0x040001B1 RID: 433
		public bool visualizeSensitivity;

		// Token: 0x040001B2 RID: 434
		private Vector3 lastDestination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x040001B3 RID: 435
		private float lastRepathTime = float.NegativeInfinity;

		// Token: 0x02000112 RID: 274
		public enum Mode
		{
			// Token: 0x040006B9 RID: 1721
			Never,
			// Token: 0x040006BA RID: 1722
			EveryNSeconds,
			// Token: 0x040006BB RID: 1723
			Dynamic
		}
	}
}
