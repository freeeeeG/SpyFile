using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B3 RID: 1971
	public class ServerHordeTargetCosmeticDecisions : ServerSynchroniserBase
	{
		// Token: 0x060025CD RID: 9677 RVA: 0x000B29CA File Offset: 0x000B0DCA
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_cosmeticDecisions = (HordeTargetCosmeticDecisions)synchronisedObject;
		}

		// Token: 0x04001D95 RID: 7573
		private HordeTargetCosmeticDecisions m_cosmeticDecisions;
	}
}
