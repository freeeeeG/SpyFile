using System;

namespace InControl
{
	// Token: 0x020002A8 RID: 680
	public class UnknownDeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000CDD RID: 3293 RVA: 0x00042690 File Offset: 0x00040A90
		public void Reset()
		{
			this.detectFound = UnknownDeviceControl.None;
			this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease;
			this.TakeSnapshotOnUnknownDevices();
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000426AC File Offset: 0x00040AAC
		private void TakeSnapshotOnUnknownDevices()
		{
			int count = InputManager.Devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.Devices[i];
				if (inputDevice.IsUnknown)
				{
					UnknownUnityInputDevice unknownUnityInputDevice = inputDevice as UnknownUnityInputDevice;
					if (unknownUnityInputDevice != null)
					{
						unknownUnityInputDevice.TakeSnapshot();
					}
				}
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00042700 File Offset: 0x00040B00
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeUnknownControllers || device.IsKnown)
			{
				return null;
			}
			if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease && this.detectFound && !this.IsPressed(this.detectFound, device))
			{
				UnknownDeviceBindingSource result = new UnknownDeviceBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			UnknownDeviceControl control = this.ListenForControl(listenOptions, device);
			if (control)
			{
				if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress)
				{
					this.detectFound = control;
					this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease;
				}
			}
			else if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease)
			{
				this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress;
			}
			return null;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000427B0 File Offset: 0x00040BB0
		private bool IsPressed(UnknownDeviceControl control, InputDevice device)
		{
			float value = control.GetValue(device);
			return Utility.AbsoluteIsOverThreshold(value, 0.5f);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000427D4 File Offset: 0x00040BD4
		private UnknownDeviceControl ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsUnknown)
			{
				UnknownUnityInputDevice unknownUnityInputDevice = device as UnknownUnityInputDevice;
				if (unknownUnityInputDevice != null)
				{
					UnknownDeviceControl firstPressedButton = unknownUnityInputDevice.GetFirstPressedButton();
					if (firstPressedButton)
					{
						return firstPressedButton;
					}
					UnknownDeviceControl firstPressedAnalog = unknownUnityInputDevice.GetFirstPressedAnalog();
					if (firstPressedAnalog)
					{
						return firstPressedAnalog;
					}
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x040009FF RID: 2559
		private UnknownDeviceControl detectFound;

		// Token: 0x04000A00 RID: 2560
		private UnknownDeviceBindingSourceListener.DetectPhase detectPhase;

		// Token: 0x020002A9 RID: 681
		private enum DetectPhase
		{
			// Token: 0x04000A02 RID: 2562
			WaitForInitialRelease,
			// Token: 0x04000A03 RID: 2563
			WaitForControlPress,
			// Token: 0x04000A04 RID: 2564
			WaitForControlRelease
		}
	}
}
