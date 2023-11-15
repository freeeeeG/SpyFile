using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FF0 RID: 4080
	public class TeleportToSkulHead : CharacterOperation
	{
		// Token: 0x06004ECF RID: 20175 RVA: 0x000ECAA0 File Offset: 0x000EACA0
		public override void Run(Character owner)
		{
			Vector3 position = SkulHeadToTeleport.instance.transform.position;
			if (!owner.movement.controller.TeleportUponGround(position, 1.5f) && !owner.movement.controller.Teleport(position, 3f))
			{
				return;
			}
			this._skulHeadController.cooldown.time.remainTime = 0f;
			SkulHeadToTeleport.instance.Despawn();
		}

		// Token: 0x04003EED RID: 16109
		[SerializeField]
		private SkulHeadController _skulHeadController;
	}
}
