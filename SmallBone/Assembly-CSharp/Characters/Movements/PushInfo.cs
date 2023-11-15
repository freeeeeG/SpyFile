using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Movements
{
	// Token: 0x0200080F RID: 2063
	[Serializable]
	public class PushInfo
	{
		// Token: 0x06002A6B RID: 10859 RVA: 0x00082E56 File Offset: 0x00081056
		internal ValueTuple<Vector2, Vector2> Evaluate(Transform from, ITarget to)
		{
			return new ValueTuple<Vector2, Vector2>(this.force1.Evaluate(from, to), this.force2.Evaluate(from, to));
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x00082E77 File Offset: 0x00081077
		internal ValueTuple<Vector2, Vector2> Evaluate(IProjectile from, ITarget to)
		{
			return new ValueTuple<Vector2, Vector2>(this.force1.Evaluate(from, to), this.force2.Evaluate(from, to));
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x00082E98 File Offset: 0x00081098
		internal ValueTuple<Vector2, Vector2> Evaluate(Character from, ITarget to)
		{
			return new ValueTuple<Vector2, Vector2>(this.force1.Evaluate(from, to), this.force2.Evaluate(from, to));
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00082EBC File Offset: 0x000810BC
		internal ValueTuple<Vector2, Vector2> EvaluateTimeIndependent(Character from, ITarget to)
		{
			Vector2 vector = this.force1.Evaluate(from, to);
			if (this.curve1.duration > 0f)
			{
				vector /= this.curve1.duration;
			}
			Vector2 vector2 = this.force2.Evaluate(from, to);
			if (this.curve2.duration > 0f)
			{
				vector2 /= this.curve2.duration;
			}
			return new ValueTuple<Vector2, Vector2>(vector, vector2);
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x00082F34 File Offset: 0x00081134
		public PushInfo()
		{
			this.ignoreOtherForce = false;
			this.expireOnGround = false;
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x00082F4A File Offset: 0x0008114A
		public PushInfo(bool ignoreOtherForce = false, bool expireOnGround = false)
		{
			this.ignoreOtherForce = ignoreOtherForce;
			this.expireOnGround = expireOnGround;
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x00082F60 File Offset: 0x00081160
		public PushInfo(PushForce pushForce, Curve curve, bool ignoreOtherForce = false, bool expireOnGround = false)
		{
			this.ignoreOtherForce = ignoreOtherForce;
			this.expireOnGround = expireOnGround;
			this.force1 = pushForce;
			this.curve1 = curve;
			this.force2 = new PushForce();
			this.curve2 = Curve.empty;
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x00082F9B File Offset: 0x0008119B
		public PushInfo(PushForce force1, Curve curve1, PushForce force2, Curve curve2, bool ignoreOtherForce = false, bool expireOnGround = false)
		{
			this.ignoreOtherForce = ignoreOtherForce;
			this.expireOnGround = expireOnGround;
			this.force1 = force1;
			this.curve1 = curve1;
			this.force2 = force2;
			this.curve2 = curve2;
		}

		// Token: 0x0400242A RID: 9258
		[SerializeField]
		internal bool ignoreOtherForce;

		// Token: 0x0400242B RID: 9259
		[SerializeField]
		internal bool expireOnGround;

		// Token: 0x0400242C RID: 9260
		[SerializeField]
		internal PushForce force1;

		// Token: 0x0400242D RID: 9261
		[SerializeField]
		internal Curve curve1;

		// Token: 0x0400242E RID: 9262
		[SerializeField]
		internal PushForce force2;

		// Token: 0x0400242F RID: 9263
		[SerializeField]
		internal Curve curve2;
	}
}
