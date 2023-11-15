using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007CB RID: 1995
	public abstract class Movement : MonoBehaviour
	{
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x0007B1B3 File Offset: 0x000793B3
		// (set) Token: 0x06002874 RID: 10356 RVA: 0x0007B1BB File Offset: 0x000793BB
		public IProjectile projectile { get; private set; }

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x0007B1C4 File Offset: 0x000793C4
		// (set) Token: 0x06002876 RID: 10358 RVA: 0x0007B1CC File Offset: 0x000793CC
		public float direction { get; set; }

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x0007B1D5 File Offset: 0x000793D5
		// (set) Token: 0x06002878 RID: 10360 RVA: 0x0007B1DD File Offset: 0x000793DD
		public Vector2 directionVector { get; set; }

		// Token: 0x06002879 RID: 10361 RVA: 0x0007B1E8 File Offset: 0x000793E8
		public virtual void Initialize(IProjectile projectile, float direction)
		{
			this.projectile = projectile;
			this.direction = direction;
			float f = direction * 0.017453292f;
			this.directionVector = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
		}

		// Token: 0x0600287A RID: 10362
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public abstract ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime);

		// Token: 0x040022F1 RID: 8945
		public static readonly Type[] types = new Type[]
		{
			typeof(Simple),
			typeof(Ease2),
			typeof(Trajectory),
			typeof(TrajectoryToPoint),
			typeof(Homing),
			typeof(Missile),
			typeof(Ground),
			typeof(Spiral),
			typeof(ReadyAndFire)
		};

		// Token: 0x020007CC RID: 1996
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600287D RID: 10365 RVA: 0x0007B2B2 File Offset: 0x000794B2
			public SubcomponentAttribute() : base(true, Movement.types)
			{
			}
		}
	}
}
