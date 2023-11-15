using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

namespace GameModes.Horde
{
	// Token: 0x020007E2 RID: 2018
	public struct ServerHordeTargetComparer : IComparer<ServerHordeTarget>
	{
		// Token: 0x060026DD RID: 9949 RVA: 0x000B88C8 File Offset: 0x000B6CC8
		public int Compare(ServerHordeTarget x, ServerHordeTarget y)
		{
			return EntitySerialisationRegistry.GetId(x.gameObject).CompareTo(EntitySerialisationRegistry.GetId(y.gameObject));
		}
	}
}
