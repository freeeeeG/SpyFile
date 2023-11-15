using System;
using Characters.Controllers;

namespace CutScenes
{
	// Token: 0x020001AC RID: 428
	public class PlayerInputBlock : State
	{
		// Token: 0x0600091F RID: 2335 RVA: 0x0001A364 File Offset: 0x00018564
		public override void Attach()
		{
			PlayerInput.blocked.Attach(State.key);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001A375 File Offset: 0x00018575
		public override void Detach()
		{
			PlayerInput.blocked.Detach(State.key);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDisable()
		{
			this.Detach();
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDestroy()
		{
			this.Detach();
		}
	}
}
