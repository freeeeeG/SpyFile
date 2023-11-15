using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007AD RID: 1965
	public class ServerHordeEnemyCosmeticDecisions : ServerSynchroniserBase
	{
		// Token: 0x060025B7 RID: 9655 RVA: 0x000B222B File Offset: 0x000B062B
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_cosmeticDecisions = (HordeEnemyCosmeticDecisions)synchronisedObject;
		}

		// Token: 0x04001D61 RID: 7521
		private HordeEnemyCosmeticDecisions m_cosmeticDecisions;
	}
}
