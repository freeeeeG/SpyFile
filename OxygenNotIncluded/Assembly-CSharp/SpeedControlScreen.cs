using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C6B RID: 3179
public class SpeedControlScreen : KScreen
{
	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06006531 RID: 25905 RVA: 0x00259AFD File Offset: 0x00257CFD
	// (set) Token: 0x06006532 RID: 25906 RVA: 0x00259B04 File Offset: 0x00257D04
	public static SpeedControlScreen Instance { get; private set; }

	// Token: 0x06006533 RID: 25907 RVA: 0x00259B0C File Offset: 0x00257D0C
	public static void DestroyInstance()
	{
		SpeedControlScreen.Instance = null;
	}

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06006534 RID: 25908 RVA: 0x00259B14 File Offset: 0x00257D14
	public bool IsPaused
	{
		get
		{
			return this.pauseCount > 0;
		}
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x00259B20 File Offset: 0x00257D20
	protected override void OnPrefabInit()
	{
		SpeedControlScreen.Instance = this;
		this.pauseButton = this.pauseButtonWidget.GetComponent<KToggle>();
		this.slowButton = this.speedButtonWidget_slow.GetComponent<KToggle>();
		this.mediumButton = this.speedButtonWidget_medium.GetComponent<KToggle>();
		this.fastButton = this.speedButtonWidget_fast.GetComponent<KToggle>();
		KToggle[] array = new KToggle[]
		{
			this.pauseButton,
			this.slowButton,
			this.mediumButton,
			this.fastButton
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].soundPlayer.Enabled = false;
		}
		this.slowButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(1f);
			this.SetSpeed(0);
		};
		this.mediumButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(2f);
			this.SetSpeed(1);
		};
		this.fastButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(3f);
			this.SetSpeed(2);
		};
		this.pauseButton.onClick += delegate()
		{
			this.TogglePause(true);
		};
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.playButtonWidget.GetComponent<KButton>().onClick += delegate()
		{
			this.TogglePause(true);
		};
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x00259CC1 File Offset: 0x00257EC1
	protected override void OnSpawn()
	{
		if (SaveGame.Instance != null)
		{
			this.speed = SaveGame.Instance.GetSpeed();
			this.SetSpeed(this.speed);
		}
		base.OnSpawn();
		this.OnChanged();
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x00259CF8 File Offset: 0x00257EF8
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x06006538 RID: 25912 RVA: 0x00259D16 File Offset: 0x00257F16
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x00259D20 File Offset: 0x00257F20
	public void SetSpeed(int Speed)
	{
		this.speed = Speed % 3;
		switch (this.speed)
		{
		case 0:
			this.slowButton.Select();
			this.slowButton.isOn = true;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = false;
			break;
		case 1:
			this.mediumButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = true;
			this.fastButton.isOn = false;
			break;
		case 2:
			this.fastButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = true;
			break;
		}
		this.OnSpeedChange();
	}

	// Token: 0x0600653A RID: 25914 RVA: 0x00259DEB File Offset: 0x00257FEB
	public void ToggleRidiculousSpeed()
	{
		if (this.ultraSpeed == 3f)
		{
			this.ultraSpeed = 10f;
		}
		else
		{
			this.ultraSpeed = 3f;
		}
		this.speed = 2;
		this.OnChanged();
	}

	// Token: 0x0600653B RID: 25915 RVA: 0x00259E1F File Offset: 0x0025801F
	public void TogglePause(bool playsound = true)
	{
		if (this.IsPaused)
		{
			this.Unpause(playsound);
			return;
		}
		this.Pause(playsound, false);
	}

	// Token: 0x0600653C RID: 25916 RVA: 0x00259E3C File Offset: 0x0025803C
	public void ResetToolTip()
	{
		this.speedButtonWidget_slow.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_medium.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_fast.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		if (this.pauseButton.isOn)
		{
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			return;
		}
		this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
	}

	// Token: 0x0600653D RID: 25917 RVA: 0x00259F6C File Offset: 0x0025816C
	public void Pause(bool playSound = true, bool isCrashed = false)
	{
		this.pauseCount++;
		if (this.pauseCount == 1)
		{
			if (playSound)
			{
				if (isCrashed)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Crash_Screen", false));
				}
				else
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Pause", false));
				}
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(0f);
				}
			}
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().SpeedPausedMigrated);
			MusicManager.instance.SetDynamicMusicPaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = true;
			this.OnPause();
		}
	}

	// Token: 0x0600653E RID: 25918 RVA: 0x0025A040 File Offset: 0x00258240
	public void Unpause(bool playSound = true)
	{
		this.pauseCount = Mathf.Max(0, this.pauseCount - 1);
		if (this.pauseCount == 0)
		{
			if (playSound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Unpause", false));
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(1f);
				}
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SpeedPausedMigrated, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicUnpaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = false;
			this.SetSpeed(this.speed);
			this.OnPlay();
		}
	}

	// Token: 0x0600653F RID: 25919 RVA: 0x0025A110 File Offset: 0x00258310
	private void OnPause()
	{
		this.OnChanged();
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x0025A118 File Offset: 0x00258318
	private void OnPlay()
	{
		this.OnChanged();
	}

	// Token: 0x06006541 RID: 25921 RVA: 0x0025A120 File Offset: 0x00258320
	public void OnSpeedChange()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.OnChanged();
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x0025A130 File Offset: 0x00258330
	private void OnChanged()
	{
		if (this.IsPaused)
		{
			Time.timeScale = 0f;
			return;
		}
		if (this.speed == 0)
		{
			Time.timeScale = this.normalSpeed;
			return;
		}
		if (this.speed == 1)
		{
			Time.timeScale = this.fastSpeed;
			return;
		}
		if (this.speed == 2)
		{
			Time.timeScale = this.ultraSpeed;
		}
	}

	// Token: 0x06006543 RID: 25923 RVA: 0x0025A190 File Offset: 0x00258390
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.TogglePause))
		{
			this.TogglePause(true);
			return;
		}
		if (e.TryConsume(global::Action.CycleSpeed))
		{
			this.PlaySpeedChangeSound((float)((this.speed + 1) % 3 + 1));
			this.SetSpeed(this.speed + 1);
			this.OnSpeedChange();
			return;
		}
		if (e.TryConsume(global::Action.SpeedUp))
		{
			this.speed++;
			this.speed = Math.Min(this.speed, 2);
			this.SetSpeed(this.speed);
			return;
		}
		if (e.TryConsume(global::Action.SlowDown))
		{
			this.speed--;
			this.speed = Math.Max(this.speed, 0);
			this.SetSpeed(this.speed);
		}
	}

	// Token: 0x06006544 RID: 25924 RVA: 0x0025A250 File Offset: 0x00258450
	private void PlaySpeedChangeSound(float speed)
	{
		string sound = GlobalAssets.GetSound("Speed_Change", false);
		if (sound != null)
		{
			EventInstance instance = SoundEvent.BeginOneShot(sound, Vector3.zero, 1f, false);
			instance.setParameterByName("Speed", speed, false);
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x06006545 RID: 25925 RVA: 0x0025A298 File Offset: 0x00258498
	public void DebugStepFrame()
	{
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Stepping one frame {0} ({1})", GameClock.Instance.GetTime(), GameClock.Instance.GetTime() / 600f)
		});
		this.stepTime = Time.time;
		this.Unpause(false);
		base.StartCoroutine(this.DebugStepFrameDelay());
	}

	// Token: 0x06006546 RID: 25926 RVA: 0x0025A300 File Offset: 0x00258500
	private IEnumerator DebugStepFrameDelay()
	{
		yield return null;
		DebugUtil.LogArgs(new object[]
		{
			"Stepped one frame",
			Time.time - this.stepTime,
			"seconds"
		});
		this.Pause(false, false);
		yield break;
	}

	// Token: 0x04004571 RID: 17777
	public GameObject playButtonWidget;

	// Token: 0x04004572 RID: 17778
	public GameObject pauseButtonWidget;

	// Token: 0x04004573 RID: 17779
	public Image playIcon;

	// Token: 0x04004574 RID: 17780
	public Image pauseIcon;

	// Token: 0x04004575 RID: 17781
	[SerializeField]
	private TextStyleSetting TooltipTextStyle;

	// Token: 0x04004576 RID: 17782
	public GameObject speedButtonWidget_slow;

	// Token: 0x04004577 RID: 17783
	public GameObject speedButtonWidget_medium;

	// Token: 0x04004578 RID: 17784
	public GameObject speedButtonWidget_fast;

	// Token: 0x04004579 RID: 17785
	public GameObject mainMenuWidget;

	// Token: 0x0400457A RID: 17786
	public float normalSpeed;

	// Token: 0x0400457B RID: 17787
	public float fastSpeed;

	// Token: 0x0400457C RID: 17788
	public float ultraSpeed;

	// Token: 0x0400457D RID: 17789
	private KToggle pauseButton;

	// Token: 0x0400457E RID: 17790
	private KToggle slowButton;

	// Token: 0x0400457F RID: 17791
	private KToggle mediumButton;

	// Token: 0x04004580 RID: 17792
	private KToggle fastButton;

	// Token: 0x04004581 RID: 17793
	private int speed;

	// Token: 0x04004582 RID: 17794
	private int pauseCount;

	// Token: 0x04004584 RID: 17796
	private float stepTime;
}
