using System;

namespace InControl
{
	// Token: 0x0200029A RID: 666
	public class DeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000C48 RID: 3144 RVA: 0x0003F139 File Offset: 0x0003D539
		public void Reset()
		{
			this.detectFound = InputControlType.None;
			this.detectPhase = 0;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0003F14C File Offset: 0x0003D54C
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeControllers || device.IsUnknown)
			{
				return null;
			}
			if (this.detectFound != InputControlType.None && !this.IsPressed(this.detectFound, device) && this.detectPhase == 2)
			{
				DeviceBindingSource result = new DeviceBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			InputControlType inputControlType = this.ListenForControl(listenOptions, device);
			if (inputControlType != InputControlType.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = inputControlType;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003F1EF File Offset: 0x0003D5EF
		private bool IsPressed(InputControl control)
		{
			return Utility.AbsoluteIsOverThreshold(control.Value, 0.5f);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0003F201 File Offset: 0x0003D601
		private bool IsPressed(InputControlType control, InputDevice device)
		{
			return this.IsPressed(device.GetControl(control));
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0003F210 File Offset: 0x0003D610
		private InputControlType ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsKnown)
			{
				int num = device.Controls.Length;
				for (int i = 0; i < num; i++)
				{
					InputControl inputControl = device.Controls[i];
					if (inputControl != null && this.IsPressed(inputControl) && (listenOptions.IncludeNonStandardControls || inputControl.IsStandard))
					{
						InputControlType target = inputControl.Target;
						if (target != InputControlType.Command || !listenOptions.IncludeNonStandardControls)
						{
							return target;
						}
					}
				}
			}
			return InputControlType.None;
		}

		// Token: 0x0400094A RID: 2378
		private InputControlType detectFound;

		// Token: 0x0400094B RID: 2379
		private int detectPhase;
	}
}
