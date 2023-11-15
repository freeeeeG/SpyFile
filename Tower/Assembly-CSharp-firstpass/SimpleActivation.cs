using System;

namespace Highlighters
{
	// Token: 0x02000008 RID: 8
	public class SimpleActivation : TriggerWrapper
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000026C3 File Offset: 0x000008C3
		private void Start()
		{
			this.DisableHighlighter();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026CB File Offset: 0x000008CB
		protected override void TriggeringStarted()
		{
			this.EnableHighlighter();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026D3 File Offset: 0x000008D3
		protected override void TriggeringEnded()
		{
			this.DisableHighlighter();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026DB File Offset: 0x000008DB
		private void EnableHighlighter()
		{
			if (!this.highlighter.enabled)
			{
				this.highlighter.enabled = true;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026F6 File Offset: 0x000008F6
		private void DisableHighlighter()
		{
			this.highlighter.enabled = false;
		}
	}
}
