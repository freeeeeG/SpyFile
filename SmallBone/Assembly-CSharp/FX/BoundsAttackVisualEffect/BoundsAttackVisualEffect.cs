using System;
using Characters;
using UnityEditor;
using UnityEngine;

namespace FX.BoundsAttackVisualEffect
{
	// Token: 0x0200028E RID: 654
	public abstract class BoundsAttackVisualEffect : VisualEffect
	{
		// Token: 0x06000CBA RID: 3258
		public abstract void Spawn(Character owner, Bounds bounds, in Damage damage, ITarget target);

		// Token: 0x04000AE7 RID: 2791
		public static readonly Type[] types = new Type[]
		{
			typeof(RandomWithinIntersect)
		};

		// Token: 0x0200028F RID: 655
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000CBD RID: 3261 RVA: 0x00029778 File Offset: 0x00027978
			public SubcomponentAttribute() : base(true, BoundsAttackVisualEffect.types)
			{
			}
		}

		// Token: 0x02000290 RID: 656
		[Serializable]
		public class Subcomponents : SubcomponentArray<BoundsAttackVisualEffect>
		{
			// Token: 0x06000CBE RID: 3262 RVA: 0x00029788 File Offset: 0x00027988
			public void Spawn(Character owner, Bounds bounds, in Damage damage, ITarget target)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Spawn(owner, bounds, damage, target);
				}
			}
		}
	}
}
