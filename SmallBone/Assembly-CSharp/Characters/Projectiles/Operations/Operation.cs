using System;
using Characters.Projectiles.Operations.Customs;
using Characters.Projectiles.Operations.Decorator;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200078A RID: 1930
	public abstract class Operation : HitOperation
	{
		// Token: 0x06002797 RID: 10135 RVA: 0x00076F60 File Offset: 0x00075160
		static Operation()
		{
			int length = typeof(Operation).Namespace.Length;
			Operation.names = new string[Operation.types.Length];
			for (int i = 0; i < Operation.names.Length; i++)
			{
				Type type = Operation.types[i];
				if (type == null)
				{
					string text = Operation.names[i - 1];
					int num = text.LastIndexOf('/');
					if (num == -1)
					{
						Operation.names[i] = string.Empty;
					}
					else
					{
						Operation.names[i] = text.Substring(0, num + 1);
					}
				}
				else
				{
					Operation.names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
				}
			}
		}

		// Token: 0x06002798 RID: 10136
		public abstract void Run(IProjectile projectile);

		// Token: 0x06002799 RID: 10137 RVA: 0x0007715A File Offset: 0x0007535A
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			this.Run(projectile);
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x0007715A File Offset: 0x0007535A
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			this.Run(projectile);
		}

		// Token: 0x040021BA RID: 8634
		public new static readonly Type[] types = new Type[]
		{
			typeof(RunCharacterOperation),
			null,
			typeof(InstantAttack),
			typeof(FireProjectile),
			typeof(SummonOperationRunner),
			typeof(SummonOperationRunnersOnGround),
			null,
			typeof(CameraShake),
			typeof(ScreenFlash),
			typeof(SpawnEffect),
			typeof(PlaySound),
			null,
			typeof(MoveOwnerToProjectile),
			typeof(SpawnRandomEnemy),
			typeof(SpawnBDRandomCharacter),
			typeof(ClearBounceHistory),
			typeof(DropSkulHead),
			typeof(Chance),
			typeof(Characters.Projectiles.Operations.Decorator.Random),
			typeof(WeightedRandom),
			typeof(Repeater),
			typeof(Repeater2),
			typeof(Repeater3),
			typeof(ByProjectileSpeed),
			typeof(SummonClusterGrenades)
		};

		// Token: 0x040021BB RID: 8635
		public static readonly string[] names;

		// Token: 0x0200078B RID: 1931
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600279C RID: 10140 RVA: 0x00077163 File Offset: 0x00075363
			public SubcomponentAttribute() : base(true, Operation.types, Operation.names)
			{
			}
		}

		// Token: 0x0200078C RID: 1932
		[Serializable]
		internal new class Subcomponents : SubcomponentArray<Operation>
		{
			// Token: 0x0600279D RID: 10141 RVA: 0x00077178 File Offset: 0x00075378
			public void Run(IProjectile projectile)
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Run(projectile);
				}
			}
		}
	}
}
