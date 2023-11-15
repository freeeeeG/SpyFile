using System;
using Characters;
using Characters.Movements;
using UnityEditor;
using UnityEngine;

namespace FX.SmashAttackVisualEffect
{
	// Token: 0x02000277 RID: 631
	public abstract class SmashAttackVisualEffect : VisualEffect
	{
		// Token: 0x06000C54 RID: 3156
		public abstract void Spawn(Character owner, Push push, RaycastHit2D raycastHit, Movement.CollisionDirection direction, Damage damage, ITarget target);

		// Token: 0x04000A60 RID: 2656
		public static readonly Type[] types = new Type[]
		{
			typeof(SpawnOnHitPoint)
		};

		// Token: 0x02000278 RID: 632
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000C57 RID: 3159 RVA: 0x00021DF7 File Offset: 0x0001FFF7
			public SubcomponentAttribute() : base(true, SmashAttackVisualEffect.types)
			{
			}
		}

		// Token: 0x02000279 RID: 633
		[Serializable]
		public class Subcomponents : SubcomponentArray<SmashAttackVisualEffect>
		{
			// Token: 0x06000C58 RID: 3160 RVA: 0x00021E08 File Offset: 0x00020008
			public void Spawn(Character owner, Push push, RaycastHit2D raycastHit, Movement.CollisionDirection direction, Damage damage, ITarget target)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(owner, push, raycastHit, direction, damage, target);
				}
			}
		}
	}
}
