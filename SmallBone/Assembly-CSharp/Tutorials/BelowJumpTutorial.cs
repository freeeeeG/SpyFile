using System;
using System.Collections;

namespace Tutorials
{
	// Token: 0x020000D4 RID: 212
	public class BelowJumpTutorial : Tutorial
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x0000DDB9 File Offset: 0x0000BFB9
		public override void Activate()
		{
			this._state = Tutorial.State.Progress;
			base.StartCoroutine(this.Process());
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000DDCF File Offset: 0x0000BFCF
		public override void Deactivate()
		{
			base.state = Tutorial.State.Done;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		protected override IEnumerator Process()
		{
			yield return this.Converse(0f);
			yield break;
		}
	}
}
