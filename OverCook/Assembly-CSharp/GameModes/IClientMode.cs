using System;

namespace GameModes
{
	// Token: 0x020006B0 RID: 1712
	public interface IClientMode
	{
		// Token: 0x06002068 RID: 8296
		void Setup(ClientContext context, SessionConfig config, ref ClientSetupData setupData);

		// Token: 0x06002069 RID: 8297
		void Begin();

		// Token: 0x0600206A RID: 8298
		void Update();

		// Token: 0x0600206B RID: 8299
		void End();
	}
}
