using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE3 RID: 3811
	public sealed class ToCirclePoint : Policy
	{
		// Token: 0x06004AC4 RID: 19140 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x000DAB90 File Offset: 0x000D8D90
		public override Vector2 GetPosition()
		{
			float f = this._angleRange.value * 0.017453292f;
			Vector2 b = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * this._radius;
			return this._center.transform.position + b;
		}

		// Token: 0x040039E4 RID: 14820
		[SerializeField]
		private Transform _center;

		// Token: 0x040039E5 RID: 14821
		[SerializeField]
		private CustomAngle _angleRange;

		// Token: 0x040039E6 RID: 14822
		[SerializeField]
		private float _radius;
	}
}
