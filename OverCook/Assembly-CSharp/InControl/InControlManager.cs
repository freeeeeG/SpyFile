using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x020002AB RID: 683
	public class InControlManager : SingletonMonoBehavior<InControlManager>
	{
		// Token: 0x06000CEE RID: 3310 RVA: 0x00042C78 File Offset: 0x00041078
		private void Awake()
		{
			if (!base.SetupSingleton())
			{
				return;
			}
			InputManager.InvertYAxis = this.invertYAxis;
			InputManager.EnableXInput = this.enableXInput;
			InputManager.XInputUpdateRate = (uint)Mathf.Max(this.xInputUpdateRate, 0);
			InputManager.XInputBufferSize = (uint)Mathf.Max(this.xInputBufferSize, 0);
			InputManager.EnableICade = this.enableICade;
			if (InputManager.SetupInternal())
			{
				if (this.logDebugInfo)
				{
					Debug.Log("InControl (version " + InputManager.Version + ")");
					Logger.OnLogMessage += this.LogMessage;
				}
				foreach (string text in this.customProfiles)
				{
					Type type = Type.GetType(text);
					if (type == null)
					{
						Debug.LogError("Cannot find class for custom profile: " + text);
					}
					else
					{
						InputDeviceProfile inputDeviceProfile = Activator.CreateInstance(type) as InputDeviceProfile;
						if (inputDeviceProfile != null)
						{
							InputManager.AttachDevice(new UnityInputDevice(inputDeviceProfile));
						}
					}
				}
			}
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00042DC4 File Offset: 0x000411C4
		private void OnDestroy()
		{
			if (SingletonMonoBehavior<InControlManager>.Instance == this)
			{
				InputManager.ResetInternal();
			}
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00042DEC File Offset: 0x000411EC
		private void Update()
		{
			if (!this.useFixedUpdate || Utility.IsZero(Time.timeScale))
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00042E0D File Offset: 0x0004120D
		private void FixedUpdate()
		{
			if (this.useFixedUpdate)
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00042E1F File Offset: 0x0004121F
		private void OnApplicationFocus(bool focusState)
		{
			InputManager.OnApplicationFocus(focusState);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00042E27 File Offset: 0x00041227
		private void OnApplicationPause(bool pauseState)
		{
			InputManager.OnApplicationPause(pauseState);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00042E2F File Offset: 0x0004122F
		private void OnApplicationQuit()
		{
			InputManager.OnApplicationQuit();
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00042E36 File Offset: 0x00041236
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (mode != LoadSceneMode.Additive)
			{
				InputManager.OnLevelWasLoaded();
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00042E44 File Offset: 0x00041244
		private void LogMessage(LogMessage logMessage)
		{
			LogMessageType type = logMessage.type;
			if (type != LogMessageType.Info)
			{
				if (type != LogMessageType.Warning)
				{
					if (type == LogMessageType.Error)
					{
						Debug.LogError(logMessage.text);
					}
				}
				else
				{
					Debug.LogWarning(logMessage.text);
				}
			}
			else
			{
				Debug.Log(logMessage.text);
			}
		}

		// Token: 0x04000A08 RID: 2568
		public bool logDebugInfo;

		// Token: 0x04000A09 RID: 2569
		public bool invertYAxis;

		// Token: 0x04000A0A RID: 2570
		public bool useFixedUpdate;

		// Token: 0x04000A0B RID: 2571
		public bool dontDestroyOnLoad;

		// Token: 0x04000A0C RID: 2572
		public bool enableXInput;

		// Token: 0x04000A0D RID: 2573
		public int xInputUpdateRate;

		// Token: 0x04000A0E RID: 2574
		public int xInputBufferSize;

		// Token: 0x04000A0F RID: 2575
		public bool enableICade;

		// Token: 0x04000A10 RID: 2576
		public List<string> customProfiles = new List<string>();
	}
}
