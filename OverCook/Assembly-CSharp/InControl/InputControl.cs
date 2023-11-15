using System;

namespace InControl
{
	// Token: 0x020002AD RID: 685
	public class InputControl : InputControlBase
	{
		// Token: 0x06000CFC RID: 3324 RVA: 0x00042EA5 File Offset: 0x000412A5
		private InputControl()
		{
			this.Handle = "None";
			this.Target = InputControlType.None;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00042EBF File Offset: 0x000412BF
		public InputControl(string handle, InputControlType target)
		{
			this.Handle = handle;
			this.Target = target;
			this.IsButton = Utility.TargetIsButton(target);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00042EF0 File Offset: 0x000412F0
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x00042EF8 File Offset: 0x000412F8
		public string Handle { get; protected set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00042F01 File Offset: 0x00041301
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00042F09 File Offset: 0x00041309
		public InputControlType Target { get; protected set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00042F12 File Offset: 0x00041312
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00042F1A File Offset: 0x0004131A
		public bool IsButton { get; protected set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00042F23 File Offset: 0x00041323
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x00042F2B File Offset: 0x0004132B
		public bool IsAnalog { get; protected set; }

		// Token: 0x06000D06 RID: 3334 RVA: 0x00042F34 File Offset: 0x00041334
		internal void SetZeroTick()
		{
			this.zeroTick = base.UpdateTick;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00042F42 File Offset: 0x00041342
		internal bool IsOnZeroTick
		{
			get
			{
				return base.UpdateTick == this.zeroTick;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00042F52 File Offset: 0x00041352
		public bool IsStandard
		{
			get
			{
				return Utility.TargetIsStandard(this.Target);
			}
		}

		// Token: 0x04000A11 RID: 2577
		public static readonly InputControl Null = new InputControl();

		// Token: 0x04000A16 RID: 2582
		private ulong zeroTick;
	}
}
