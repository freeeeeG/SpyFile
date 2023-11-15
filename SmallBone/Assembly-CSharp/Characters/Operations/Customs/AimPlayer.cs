using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FCE RID: 4046
	public class AimPlayer : CharacterOperation
	{
		// Token: 0x06004E51 RID: 20049 RVA: 0x000EA744 File Offset: 0x000E8944
		public override void Run(Character owner)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			float y;
			if (this._platform)
			{
				y = player.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
			}
			else
			{
				y = player.transform.position.y + player.collider.bounds.extents.y;
			}
			Vector3 vector = new Vector3(player.transform.position.x, y) - this._centerAxis.transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._centerAxis.rotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x04003E5A RID: 15962
		[SerializeField]
		private Transform _centerAxis;

		// Token: 0x04003E5B RID: 15963
		[SerializeField]
		private bool _platform;
	}
}
