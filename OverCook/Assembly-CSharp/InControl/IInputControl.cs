using System;

namespace InControl
{
	// Token: 0x020002AC RID: 684
	public interface IInputControl
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000CF7 RID: 3319
		bool HasChanged { get; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000CF8 RID: 3320
		bool IsPressed { get; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000CF9 RID: 3321
		bool WasPressed { get; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000CFA RID: 3322
		bool WasReleased { get; }

		// Token: 0x06000CFB RID: 3323
		void ClearInputState();
	}
}
