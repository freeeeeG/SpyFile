using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED6 RID: 3798
	public abstract class Policy : MonoBehaviour
	{
		// Token: 0x06004A94 RID: 19092
		public abstract Vector2 GetPosition();

		// Token: 0x06004A95 RID: 19093
		public abstract Vector2 GetPosition(Character owner);

		// Token: 0x02000ED7 RID: 3799
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06004A97 RID: 19095 RVA: 0x000D9A92 File Offset: 0x000D7C92
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Policy.SubcomponentAttribute.types)
			{
			}

			// Token: 0x040039B3 RID: 14771
			public new static readonly Type[] types = new Type[]
			{
				typeof(ToBTTarget),
				typeof(ToBDTarget),
				typeof(ToBDTargetOpposition),
				typeof(ToBDKeepDistance),
				typeof(ToKeepDistance),
				typeof(ToBDTransform),
				typeof(ToBDTargetPlatformPoint),
				typeof(ToBounds),
				typeof(ToClosestTarget),
				typeof(ToCharacterBased),
				typeof(ToLookingSide),
				typeof(ToObject),
				typeof(ToOppositionPlatform),
				typeof(ToPlatformPoint),
				typeof(ToPlayer),
				typeof(ToPlayerBased),
				typeof(ToRandomPoint),
				typeof(ToRayPoint),
				typeof(ToTargetOpposition),
				typeof(ToSavedPosition),
				typeof(ToRandomEnemyBased),
				typeof(ToRandomTarget),
				typeof(ToColliderBased),
				typeof(ToCirclePoint),
				typeof(ToOwner),
				typeof(ToLinear),
				typeof(ToCenterPoints)
			};
		}
	}
}
