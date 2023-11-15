using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes.Horde
{
	// Token: 0x020007E3 RID: 2019
	public struct ClientHordeTargetComparer : IComparer<ClientHordeTarget>
	{
		// Token: 0x060026DE RID: 9950 RVA: 0x000B88F4 File Offset: 0x000B6CF4
		public int Compare(ClientHordeTarget x, ClientHordeTarget y)
		{
			return EntitySerialisationRegistry.GetId(x.gameObject).CompareTo(EntitySerialisationRegistry.GetId(y.gameObject));
		}
	}
}
