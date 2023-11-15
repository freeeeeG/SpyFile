using System;
using Data;
using InControl;
using Scenes;
using UnityEngine;
using UserInput;

// Token: 0x02000080 RID: 128
public class ControllerManager : MonoBehaviour
{
	// Token: 0x06000253 RID: 595 RVA: 0x00009B39 File Offset: 0x00007D39
	private void Start()
	{
		KeyMapper.Bind(GameData.Settings.keyBindings);
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00009B45 File Offset: 0x00007D45
	private void OnEnable()
	{
		InputManager.OnActiveDeviceChanged += this.OnActiveDeviceChanged;
		InputManager.OnDeviceAttached += this.OnDeviceAttached;
		InputManager.OnDeviceDetached += this.OnDeviceDetached;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00009B7A File Offset: 0x00007D7A
	private void OnDisable()
	{
		InputManager.OnActiveDeviceChanged -= this.OnActiveDeviceChanged;
		InputManager.OnDeviceAttached -= this.OnDeviceAttached;
		InputManager.OnDeviceDetached -= this.OnDeviceDetached;
	}

	// Token: 0x06000256 RID: 598 RVA: 0x00009BB0 File Offset: 0x00007DB0
	private void Update()
	{
		this.vibration.Update();
		float intensity = this.vibration.value * Chronometer.global.timeScale * 10f * GameData.Settings.vibrationIntensity;
		InputManager.ActiveDevice.Vibrate(intensity);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00009BF6 File Offset: 0x00007DF6
	private void OnActiveDeviceChanged(InputDevice inputDevice)
	{
		this.HideControllerDisconnedtedPopup();
		this.Stop();
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00009C04 File Offset: 0x00007E04
	public void Stop()
	{
		this.vibration.Clear();
		InputManager.ActiveDevice.StopVibration();
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00009C1B File Offset: 0x00007E1B
	public void ShowControllerDisconnedtedPopup()
	{
		if (this._disconnedtedPopup.activeSelf)
		{
			return;
		}
		Chronometer.global.AttachTimeScale(this._disconnedtedPopup, 0f);
		this._disconnedtedPopup.SetActive(true);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00009C4C File Offset: 0x00007E4C
	public void HideControllerDisconnedtedPopup()
	{
		if (!this._disconnedtedPopup.activeSelf)
		{
			return;
		}
		Chronometer.global.DetachTimeScale(this._disconnedtedPopup);
		this._disconnedtedPopup.SetActive(false);
		if (Scene<GameBase>.instance == null)
		{
			return;
		}
		Scene<GameBase>.instance.uiManager.ShowPausePopup();
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00009CA1 File Offset: 0x00007EA1
	private void OnDeviceAttached(InputDevice inputDevice)
	{
		this.HideControllerDisconnedtedPopup();
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00009CA9 File Offset: 0x00007EA9
	private void OnDeviceDetached(InputDevice inputDevice)
	{
		if (InputManager.ActiveDevice != null && InputManager.ActiveDevice != inputDevice)
		{
			return;
		}
		this.ShowControllerDisconnedtedPopup();
	}

	// Token: 0x04000207 RID: 519
	public readonly MaxOnlyTimedFloats vibration = new MaxOnlyTimedFloats();

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private GameObject _disconnedtedPopup;
}
