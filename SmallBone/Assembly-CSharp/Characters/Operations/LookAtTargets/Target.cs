using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F05 RID: 3845
	public abstract class Target : MonoBehaviour
	{
		// Token: 0x06004B30 RID: 19248
		public abstract Character.LookingDirection GetDirectionFrom(Character character);

		// Token: 0x04003A5F RID: 14943
		public static readonly Type[] types = new Type[]
		{
			typeof(AITarget),
			typeof(BTTarget),
			typeof(BDTarget),
			typeof(ClosestSideOnPlatform),
			typeof(ClosestSideFromPlayer),
			typeof(FlipInDistanceFromPlatform),
			typeof(Chance),
			typeof(Inverter),
			typeof(Player),
			typeof(PlatformPoint),
			typeof(TargetObject),
			typeof(TurnAround)
		};

		// Token: 0x02000F06 RID: 3846
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06004B33 RID: 19251 RVA: 0x000DD624 File Offset: 0x000DB824
			public SubcomponentAttribute() : base(true, Target.types)
			{
			}
		}
	}
}
