using System;

namespace GameModes
{
	// Token: 0x020006AF RID: 1711
	public interface IServerMode
	{
		// Token: 0x06002064 RID: 8292
		void Setup(ServerContext context, SessionConfig config, ref ServerSetupData setupData);

		// Token: 0x06002065 RID: 8293
		void Begin();

		// Token: 0x06002066 RID: 8294
		void Update();

		// Token: 0x06002067 RID: 8295
		void End();
	}
}
