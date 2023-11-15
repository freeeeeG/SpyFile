using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000773 RID: 1907
	public abstract class CharacterHitOperation : MonoBehaviour
	{
		// Token: 0x0600275E RID: 10078
		public abstract void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character);

		// Token: 0x0400217B RID: 8571
		public static readonly Type[] types = new Type[]
		{
			typeof(AddMarkStack),
			typeof(AttachAbility),
			typeof(AttachAbilityToOwner),
			typeof(AttachCurseOfLight),
			typeof(Attack),
			typeof(Knockback),
			typeof(KnockbackTo),
			typeof(GrabTo),
			typeof(ShaderEffect),
			typeof(ApplyStatus),
			typeof(SummonOperationRunner),
			typeof(Smash),
			typeof(StuckToCharacter),
			typeof(Heal),
			typeof(InstantAttack),
			typeof(PlaySound),
			typeof(Despawn),
			typeof(MoveOwnerToProjectile)
		};

		// Token: 0x02000774 RID: 1908
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06002761 RID: 10081 RVA: 0x00076348 File Offset: 0x00074548
			public SubcomponentAttribute() : base(true, CharacterHitOperation.types)
			{
			}
		}

		// Token: 0x02000775 RID: 1909
		[Serializable]
		internal class Subcomponents : SubcomponentArray<CharacterHitOperation>
		{
		}
	}
}
