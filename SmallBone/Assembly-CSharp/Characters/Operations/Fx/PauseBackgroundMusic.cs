using System;
using Singletons;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F13 RID: 3859
	public sealed class PauseBackgroundMusic : Operation
	{
		// Token: 0x06004B5B RID: 19291 RVA: 0x000DDDFB File Offset: 0x000DBFFB
		public override void Run()
		{
			this._resumed = false;
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x000DDE0E File Offset: 0x000DC00E
		public override void Stop()
		{
			if (this._resumed)
			{
				return;
			}
			this._resumed = true;
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(PersistentSingleton<SoundManager>.Instance.backgroundClip, 1f, true, true, false);
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x000CD13F File Offset: 0x000CB33F
		private void OnDisable()
		{
			this.Stop();
		}

		// Token: 0x04003A99 RID: 15001
		private bool _resumed;
	}
}
