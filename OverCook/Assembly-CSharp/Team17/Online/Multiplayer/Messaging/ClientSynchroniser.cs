using System;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000923 RID: 2339
	public interface ClientSynchroniser : Synchroniser
	{
		// Token: 0x06002DF7 RID: 11767
		bool IsValidServerUpdateSequenceNumber(uint uSequence);

		// Token: 0x06002DF8 RID: 11768
		void SetLastServerUpdateSequenceNumber(uint uSequence);

		// Token: 0x06002DF9 RID: 11769
		bool IsValidLastUpdateTimeStamp(float timeStamp, float diff);

		// Token: 0x06002DFA RID: 11770
		void SetLastUpdateTimeStamp(float timeStamp);

		// Token: 0x06002DFB RID: 11771
		void ApplyServerUpdate(Serialisable serialisable);

		// Token: 0x06002DFC RID: 11772
		void ApplyServerEvent(Serialisable serialisable);
	}
}
