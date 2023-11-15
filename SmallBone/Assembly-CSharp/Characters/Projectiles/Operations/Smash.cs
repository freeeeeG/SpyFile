using System;
using Characters.Movements;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000795 RID: 1941
	public class Smash : CharacterHitOperation
	{
		// Token: 0x060027B8 RID: 10168 RVA: 0x000774A6 File Offset: 0x000756A6
		private void OnEnd(Push push, Character from, Character to, Push.SmashEndType endType, RaycastHit2D? raycastHit, Movement.CollisionDirection direction)
		{
			if (endType == Push.SmashEndType.Collide)
			{
				base.StartCoroutine(this._onCollide.CRun(from, to));
			}
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000774C1 File Offset: 0x000756C1
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			character.movement.push.ApplySmash(projectile.owner, this._pushInfo, new Push.OnSmashEndDelegate(this.OnEnd));
		}

		// Token: 0x040021D2 RID: 8658
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(true, false);

		// Token: 0x040021D3 RID: 8659
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _onCollide;
	}
}
