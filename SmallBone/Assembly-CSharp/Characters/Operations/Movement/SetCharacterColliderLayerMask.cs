using System;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E67 RID: 3687
	public class SetCharacterColliderLayerMask : CharacterOperation
	{
		// Token: 0x0600492E RID: 18734 RVA: 0x000D5784 File Offset: 0x000D3984
		public override void Run(Character owner)
		{
			this.character = owner;
			switch (this._targetCollider)
			{
			case SetCharacterColliderLayerMask.TargetCollider.Terrain:
				this.character.movement.controller.terrainMask = this.layerMask;
				return;
			case SetCharacterColliderLayerMask.TargetCollider.Trigger:
				this.character.movement.controller.triggerMask = this.layerMask;
				return;
			case SetCharacterColliderLayerMask.TargetCollider.OneWayPlatformMask:
				this.character.movement.controller.oneWayPlatformMask = this.layerMask;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x00002191 File Offset: 0x00000391
		public override void Stop()
		{
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x000D5808 File Offset: 0x000D3A08
		public void SetLayerMask(SetCharacterColliderLayerMask.ApplyLayerMask _layerMask)
		{
			switch (_layerMask)
			{
			case SetCharacterColliderLayerMask.ApplyLayerMask.Default:
				this.layerMask = 0;
				return;
			case SetCharacterColliderLayerMask.ApplyLayerMask.Foothold:
				this.layerMask = Layers.footholdMask;
				return;
			case SetCharacterColliderLayerMask.ApplyLayerMask.Ground:
				this.layerMask = Layers.groundMask;
				return;
			case SetCharacterColliderLayerMask.ApplyLayerMask.TerrainMask:
				this.layerMask = Layers.terrainMask;
				return;
			case SetCharacterColliderLayerMask.ApplyLayerMask.TerrainMaskForProjectile:
				this.layerMask = Layers.terrainMaskForProjectile;
				return;
			default:
				return;
			}
		}

		// Token: 0x0400384E RID: 14414
		[SerializeField]
		private SetCharacterColliderLayerMask.TargetCollider _targetCollider;

		// Token: 0x0400384F RID: 14415
		[SerializeField]
		private LayerMask layerMask;

		// Token: 0x04003850 RID: 14416
		private Character character;

		// Token: 0x02000E68 RID: 3688
		public enum TargetCollider
		{
			// Token: 0x04003852 RID: 14418
			Terrain,
			// Token: 0x04003853 RID: 14419
			Trigger,
			// Token: 0x04003854 RID: 14420
			OneWayPlatformMask
		}

		// Token: 0x02000E69 RID: 3689
		public enum ApplyLayerMask
		{
			// Token: 0x04003856 RID: 14422
			Default,
			// Token: 0x04003857 RID: 14423
			Foothold,
			// Token: 0x04003858 RID: 14424
			Ground,
			// Token: 0x04003859 RID: 14425
			TerrainMask,
			// Token: 0x0400385A RID: 14426
			TerrainMaskForProjectile
		}
	}
}
