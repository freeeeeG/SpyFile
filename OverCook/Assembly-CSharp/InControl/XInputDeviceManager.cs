using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x0200034B RID: 843
	public class XInputDeviceManager : InputDeviceManager
	{
		// Token: 0x0600102D RID: 4141 RVA: 0x0005D95C File Offset: 0x0005BD5C
		public XInputDeviceManager()
		{
			if (InputManager.XInputUpdateRate == 0U)
			{
				this.timeStep = Mathf.FloorToInt(Time.fixedDeltaTime * 1000f);
			}
			else
			{
				this.timeStep = Mathf.FloorToInt(1f / InputManager.XInputUpdateRate * 1000f);
			}
			this.bufferSize = (int)Math.Max(InputManager.XInputBufferSize, 1U);
			for (int i = 0; i < 4; i++)
			{
				this.gamePadState[i] = new RingBuffer<GamePadState>(this.bufferSize);
			}
			this.StartWorker();
			for (int j = 0; j < 4; j++)
			{
				this.devices.Add(new XInputDevice(j, this));
			}
			this.Update(0UL, 0f);
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0005DA37 File Offset: 0x0005BE37
		private void StartWorker()
		{
			if (this.thread == null)
			{
				this.thread = new Thread(new ThreadStart(this.Worker));
				this.thread.IsBackground = true;
				this.thread.Start();
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0005DA72 File Offset: 0x0005BE72
		private void StopWorker()
		{
			if (this.thread != null)
			{
				this.thread.Abort();
				this.thread.Join();
				this.thread = null;
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0005DA9C File Offset: 0x0005BE9C
		private void Worker()
		{
			for (;;)
			{
				for (int i = 0; i < 4; i++)
				{
					this.gamePadState[i].Enqueue(GamePad.GetState((PlayerIndex)i));
				}
				Thread.Sleep(this.timeStep);
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0005DADD File Offset: 0x0005BEDD
		internal GamePadState GetState(int deviceIndex)
		{
			return this.gamePadState[deviceIndex].Dequeue();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0005DAEC File Offset: 0x0005BEEC
		public override void Update(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 4; i++)
			{
				XInputDevice xinputDevice = this.devices[i] as XInputDevice;
				if (!xinputDevice.IsConnected)
				{
					xinputDevice.GetState();
				}
				if (xinputDevice.IsConnected != this.deviceConnected[i])
				{
					if (xinputDevice.IsConnected)
					{
						InputManager.AttachDevice(xinputDevice);
					}
					else
					{
						InputManager.DetachDevice(xinputDevice);
					}
					this.deviceConnected[i] = xinputDevice.IsConnected;
				}
			}
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0005DB6B File Offset: 0x0005BF6B
		public override void Destroy()
		{
			this.StopWorker();
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0005DB74 File Offset: 0x0005BF74
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			try
			{
				GamePad.GetState(PlayerIndex.One);
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + ".dll could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0005DBDC File Offset: 0x0005BFDC
		internal static void Enable()
		{
			List<string> list = new List<string>();
			if (XInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.HideDevicesWithProfile(typeof(Xbox360WinProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWinProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWin10Profile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF310ModeXWinProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF510ModeXWinProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF710ModeXWinProfile));
				InputManager.AddDeviceManager<XInputDeviceManager>();
			}
			else
			{
				foreach (string text in list)
				{
					Logger.LogError(text);
				}
			}
		}

		// Token: 0x04000C27 RID: 3111
		private bool[] deviceConnected = new bool[4];

		// Token: 0x04000C28 RID: 3112
		private const int maxDevices = 4;

		// Token: 0x04000C29 RID: 3113
		private RingBuffer<GamePadState>[] gamePadState = new RingBuffer<GamePadState>[4];

		// Token: 0x04000C2A RID: 3114
		private Thread thread;

		// Token: 0x04000C2B RID: 3115
		private int timeStep;

		// Token: 0x04000C2C RID: 3116
		private int bufferSize;
	}
}
