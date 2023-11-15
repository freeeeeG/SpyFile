using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000A76 RID: 2678
[AddComponentMenu("KMonoBehaviour/scripts/OverlayScreen")]
public class OverlayScreen : KMonoBehaviour
{
	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x0600514A RID: 20810 RVA: 0x001CE782 File Offset: 0x001CC982
	public HashedString mode
	{
		get
		{
			return this.currentModeInfo.mode.ViewMode();
		}
	}

	// Token: 0x0600514B RID: 20811 RVA: 0x001CE794 File Offset: 0x001CC994
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(OverlayScreen.Instance == null);
		OverlayScreen.Instance = this;
		this.powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
	}

	// Token: 0x0600514C RID: 20812 RVA: 0x001CE7C1 File Offset: 0x001CC9C1
	protected override void OnLoadLevel()
	{
		this.harvestableNotificationPrefab = null;
		this.powerLabelParent = null;
		OverlayScreen.Instance = null;
		OverlayModes.Mode.Clear();
		this.modeInfos = null;
		this.currentModeInfo = default(OverlayScreen.ModeInfo);
		base.OnLoadLevel();
	}

	// Token: 0x0600514D RID: 20813 RVA: 0x001CE7F8 File Offset: 0x001CC9F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techViewSound = KFMOD.CreateInstance(this.techViewSoundPath);
		this.techViewSoundPlaying = false;
		Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
		this.RegisterModes();
		this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
	}

	// Token: 0x0600514E RID: 20814 RVA: 0x001CE850 File Offset: 0x001CCA50
	private void RegisterModes()
	{
		this.modeInfos.Clear();
		OverlayModes.None mode = new OverlayModes.None();
		this.RegisterMode(mode);
		this.RegisterMode(new OverlayModes.Oxygen());
		this.RegisterMode(new OverlayModes.Power(this.powerLabelParent, this.powerLabelPrefab, this.batUIPrefab, this.powerLabelOffset, this.batteryUIOffset, this.batteryUITransformerOffset, this.batteryUISmallTransformerOffset));
		this.RegisterMode(new OverlayModes.Temperature());
		this.RegisterMode(new OverlayModes.ThermalConductivity());
		this.RegisterMode(new OverlayModes.Light());
		this.RegisterMode(new OverlayModes.LiquidConduits());
		this.RegisterMode(new OverlayModes.GasConduits());
		this.RegisterMode(new OverlayModes.Decor());
		this.RegisterMode(new OverlayModes.Disease(this.powerLabelParent, this.diseaseOverlayPrefab));
		this.RegisterMode(new OverlayModes.Crop(this.powerLabelParent, this.harvestableNotificationPrefab));
		this.RegisterMode(new OverlayModes.Harvest());
		this.RegisterMode(new OverlayModes.Priorities());
		this.RegisterMode(new OverlayModes.HeatFlow());
		this.RegisterMode(new OverlayModes.Rooms());
		this.RegisterMode(new OverlayModes.Suit(this.powerLabelParent, this.suitOverlayPrefab));
		this.RegisterMode(new OverlayModes.Logic(this.logicModeUIPrefab));
		this.RegisterMode(new OverlayModes.SolidConveyor());
		this.RegisterMode(new OverlayModes.TileMode());
		this.RegisterMode(new OverlayModes.Radiation());
	}

	// Token: 0x0600514F RID: 20815 RVA: 0x001CE99C File Offset: 0x001CCB9C
	private void RegisterMode(OverlayModes.Mode mode)
	{
		this.modeInfos[mode.ViewMode()] = new OverlayScreen.ModeInfo
		{
			mode = mode
		};
	}

	// Token: 0x06005150 RID: 20816 RVA: 0x001CE9CB File Offset: 0x001CCBCB
	private void LateUpdate()
	{
		this.currentModeInfo.mode.Update();
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x001CE9E0 File Offset: 0x001CCBE0
	public void ToggleOverlay(HashedString newMode, bool allowSound = true)
	{
		bool flag = allowSound && !(this.currentModeInfo.mode.ViewMode() == newMode);
		if (newMode != OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		this.currentModeInfo.mode.Disable();
		if (newMode != this.currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		SimDebugView.Instance.SetMode(newMode);
		if (!this.modeInfos.TryGetValue(newMode, out this.currentModeInfo))
		{
			this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
		}
		this.currentModeInfo.mode.Enable();
		if (flag)
		{
			this.UpdateOverlaySounds();
		}
		if (OverlayModes.None.ID == this.currentModeInfo.mode.ViewMode())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterOnMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicOverlayInactive();
			this.techViewSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.techViewSoundPlaying = false;
		}
		else if (!this.techViewSoundPlaying)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
			MusicManager.instance.SetDynamicMusicOverlayActive();
			this.techViewSound.start();
			this.techViewSoundPlaying = true;
		}
		if (this.OnOverlayChanged != null)
		{
			this.OnOverlayChanged(this.currentModeInfo.mode.ViewMode());
		}
		this.ActivateLegend();
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x001CEB67 File Offset: 0x001CCD67
	private void ActivateLegend()
	{
		if (OverlayLegend.Instance == null)
		{
			return;
		}
		OverlayLegend.Instance.SetLegend(this.currentModeInfo.mode, false);
	}

	// Token: 0x06005153 RID: 20819 RVA: 0x001CEB8D File Offset: 0x001CCD8D
	public void Refresh()
	{
		this.LateUpdate();
	}

	// Token: 0x06005154 RID: 20820 RVA: 0x001CEB95 File Offset: 0x001CCD95
	public HashedString GetMode()
	{
		if (this.currentModeInfo.mode == null)
		{
			return OverlayModes.None.ID;
		}
		return this.currentModeInfo.mode.ViewMode();
	}

	// Token: 0x06005155 RID: 20821 RVA: 0x001CEBBC File Offset: 0x001CCDBC
	private void UpdateOverlaySounds()
	{
		string text = this.currentModeInfo.mode.GetSoundName();
		if (text != "")
		{
			text = GlobalAssets.GetSound(text, false);
			KMonoBehaviour.PlaySound(text);
		}
	}

	// Token: 0x04003543 RID: 13635
	public static HashSet<Tag> WireIDs = new HashSet<Tag>();

	// Token: 0x04003544 RID: 13636
	public static HashSet<Tag> GasVentIDs = new HashSet<Tag>();

	// Token: 0x04003545 RID: 13637
	public static HashSet<Tag> LiquidVentIDs = new HashSet<Tag>();

	// Token: 0x04003546 RID: 13638
	public static HashSet<Tag> HarvestableIDs = new HashSet<Tag>();

	// Token: 0x04003547 RID: 13639
	public static HashSet<Tag> DiseaseIDs = new HashSet<Tag>();

	// Token: 0x04003548 RID: 13640
	public static HashSet<Tag> SuitIDs = new HashSet<Tag>();

	// Token: 0x04003549 RID: 13641
	public static HashSet<Tag> SolidConveyorIDs = new HashSet<Tag>();

	// Token: 0x0400354A RID: 13642
	public static HashSet<Tag> RadiationIDs = new HashSet<Tag>();

	// Token: 0x0400354B RID: 13643
	[SerializeField]
	public EventReference techViewSoundPath;

	// Token: 0x0400354C RID: 13644
	private EventInstance techViewSound;

	// Token: 0x0400354D RID: 13645
	private bool techViewSoundPlaying;

	// Token: 0x0400354E RID: 13646
	public static OverlayScreen Instance;

	// Token: 0x0400354F RID: 13647
	[Header("Power")]
	[SerializeField]
	private Canvas powerLabelParent;

	// Token: 0x04003550 RID: 13648
	[SerializeField]
	private LocText powerLabelPrefab;

	// Token: 0x04003551 RID: 13649
	[SerializeField]
	private BatteryUI batUIPrefab;

	// Token: 0x04003552 RID: 13650
	[SerializeField]
	private Vector3 powerLabelOffset;

	// Token: 0x04003553 RID: 13651
	[SerializeField]
	private Vector3 batteryUIOffset;

	// Token: 0x04003554 RID: 13652
	[SerializeField]
	private Vector3 batteryUITransformerOffset;

	// Token: 0x04003555 RID: 13653
	[SerializeField]
	private Vector3 batteryUISmallTransformerOffset;

	// Token: 0x04003556 RID: 13654
	[SerializeField]
	private Color consumerColour;

	// Token: 0x04003557 RID: 13655
	[SerializeField]
	private Color generatorColour;

	// Token: 0x04003558 RID: 13656
	[SerializeField]
	private Color buildingDisabledColour = Color.gray;

	// Token: 0x04003559 RID: 13657
	[Header("Circuits")]
	[SerializeField]
	private Color32 circuitUnpoweredColour;

	// Token: 0x0400355A RID: 13658
	[SerializeField]
	private Color32 circuitSafeColour;

	// Token: 0x0400355B RID: 13659
	[SerializeField]
	private Color32 circuitStrainingColour;

	// Token: 0x0400355C RID: 13660
	[SerializeField]
	private Color32 circuitOverloadingColour;

	// Token: 0x0400355D RID: 13661
	[Header("Crops")]
	[SerializeField]
	private GameObject harvestableNotificationPrefab;

	// Token: 0x0400355E RID: 13662
	[Header("Disease")]
	[SerializeField]
	private GameObject diseaseOverlayPrefab;

	// Token: 0x0400355F RID: 13663
	[Header("Suit")]
	[SerializeField]
	private GameObject suitOverlayPrefab;

	// Token: 0x04003560 RID: 13664
	[Header("ToolTip")]
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x04003561 RID: 13665
	[SerializeField]
	private TextStyleSetting TooltipDescription;

	// Token: 0x04003562 RID: 13666
	[Header("Logic")]
	[SerializeField]
	private LogicModeUI logicModeUIPrefab;

	// Token: 0x04003563 RID: 13667
	public Action<HashedString> OnOverlayChanged;

	// Token: 0x04003564 RID: 13668
	private OverlayScreen.ModeInfo currentModeInfo;

	// Token: 0x04003565 RID: 13669
	private Dictionary<HashedString, OverlayScreen.ModeInfo> modeInfos = new Dictionary<HashedString, OverlayScreen.ModeInfo>();

	// Token: 0x02001931 RID: 6449
	private struct ModeInfo
	{
		// Token: 0x040074A2 RID: 29858
		public OverlayModes.Mode mode;
	}
}
