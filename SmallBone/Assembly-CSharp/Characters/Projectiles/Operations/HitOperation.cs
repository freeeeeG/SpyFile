using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000781 RID: 1921
	public abstract class HitOperation : CharacterHitOperation
	{
		// Token: 0x06002781 RID: 10113
		public abstract void Run(IProjectile projectile, RaycastHit2D raycastHit);

		// Token: 0x06002782 RID: 10114 RVA: 0x00076835 File Offset: 0x00074A35
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			this.Run(projectile, raycastHit);
		}

		// Token: 0x040021A4 RID: 8612
		public new static readonly Type[] types = new Type[]
		{
			typeof(Despawn),
			typeof(PlaySound),
			typeof(DropSkulHead),
			typeof(Stuck),
			typeof(SummonOperationRunner),
			typeof(InstantAttack),
			typeof(MoveOwnerToProjectile),
			typeof(SummonOperationRunner),
			typeof(SummonOperationRunnerOnHitPoint),
			typeof(SpreadOperationRunner),
			typeof(ActivateObject),
			typeof(Bounce),
			typeof(SpawnObject),
			typeof(CameraShake),
			typeof(DropParts),
			typeof(InHardmode)
		};

		// Token: 0x02000782 RID: 1922
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06002785 RID: 10117 RVA: 0x00076930 File Offset: 0x00074B30
			public SubcomponentAttribute() : base(true, HitOperation.types)
			{
			}
		}

		// Token: 0x02000783 RID: 1923
		[Serializable]
		internal new class Subcomponents : SubcomponentArray<HitOperation>
		{
		}
	}
}
