using System;

namespace InControl
{
	// Token: 0x0200029D RID: 669
	public class KeyBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x0003F420 File Offset: 0x0003D820
		public void Reset()
		{
			this.detectFound.Clear();
			this.detectPhase = 0;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0003F434 File Offset: 0x0003D834
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeKeys)
			{
				return null;
			}
			if (this.detectFound.Count > 0 && !this.detectFound.IsPressed && this.detectPhase == 2)
			{
				KeyBindingSource result = new KeyBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			KeyCombo keyCombo = KeyCombo.Detect(listenOptions.IncludeModifiersAsFirstClassKeys);
			if (keyCombo.Count > 0)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = keyCombo;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x040009C0 RID: 2496
		private KeyCombo detectFound;

		// Token: 0x040009C1 RID: 2497
		private int detectPhase;
	}
}
