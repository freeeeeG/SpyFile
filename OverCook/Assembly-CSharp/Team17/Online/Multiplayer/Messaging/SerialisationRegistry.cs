using System;
using System.Collections.Generic;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008D1 RID: 2257
	public class SerialisationRegistry<T>
	{
		// Token: 0x06002BCE RID: 11214 RVA: 0x000CC7BD File Offset: 0x000CABBD
		public static void Initialise(IEqualityComparer<T> comparer)
		{
			SerialisationRegistry<T>.s_MessageTypes = new Dictionary<T, Serialisable>(comparer);
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000CC7CA File Offset: 0x000CABCA
		public static void RegisterMessageType(T type, Serialisable message)
		{
			SerialisationRegistry<T>.s_MessageTypes[type] = message;
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000CC7D8 File Offset: 0x000CABD8
		public static bool Deserialise(out Serialisable message, T type, BitStreamReader reader)
		{
			if (SerialisationRegistry<T>.s_MessageTypes.ContainsKey(type))
			{
				message = SerialisationRegistry<T>.s_MessageTypes[type];
				return message.Deserialise(reader);
			}
			message = null;
			return false;
		}

		// Token: 0x04002330 RID: 9008
		public static Dictionary<T, Serialisable> s_MessageTypes;
	}
}
