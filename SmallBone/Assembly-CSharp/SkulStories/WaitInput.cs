using System;
using System.Collections;
using UserInput;

namespace SkulStories
{
	// Token: 0x0200012D RID: 301
	public class WaitInput : Sequence
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x00011123 File Offset: 0x0000F323
		public override IEnumerator CRun()
		{
			this._narration.skippable = false;
			if (!this._narration.skipped)
			{
				yield return this.CWaitInput();
			}
			this._narration.skipped = false;
			yield break;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00011132 File Offset: 0x0000F332
		private IEnumerator CWaitInput()
		{
			do
			{
				yield return null;
				this._narration.skippable = false;
			}
			while (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Jump.WasPressed && !KeyMapper.Map.Submit.WasPressed);
			this._narration.textVisible = false;
			yield break;
		}
	}
}
