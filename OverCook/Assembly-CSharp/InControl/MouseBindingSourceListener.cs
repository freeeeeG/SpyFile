using System;

namespace InControl
{
	// Token: 0x020002A2 RID: 674
	public class MouseBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000C88 RID: 3208 RVA: 0x00040C8B File Offset: 0x0003F08B
		public void Reset()
		{
			this.detectFound = Mouse.None;
			this.detectPhase = 0;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00040C9C File Offset: 0x0003F09C
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeMouseButtons)
			{
				return null;
			}
			if (this.detectFound != Mouse.None && !MouseBindingSource.ButtonIsPressed(this.detectFound) && this.detectPhase == 2)
			{
				MouseBindingSource result = new MouseBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			Mouse mouse = this.ListenForControl();
			if (mouse != Mouse.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = mouse;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00040D30 File Offset: 0x0003F130
		private Mouse ListenForControl()
		{
			for (Mouse mouse = Mouse.None; mouse <= Mouse.Button9; mouse++)
			{
				if (MouseBindingSource.ButtonIsPressed(mouse))
				{
					return mouse;
				}
			}
			return Mouse.None;
		}

		// Token: 0x040009E0 RID: 2528
		private Mouse detectFound;

		// Token: 0x040009E1 RID: 2529
		private int detectPhase;
	}
}
