using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000B9F RID: 2975
public class NewGameSettingSeed : NewGameSettingWidget
{
	// Token: 0x06005CCA RID: 23754 RVA: 0x0021FEEC File Offset: 0x0021E0EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.RandomizeButton.onClick += this.GetNewRandomSeed;
	}

	// Token: 0x06005CCB RID: 23755 RVA: 0x0021FF4E File Offset: 0x0021E14E
	public void Initialize(SeedSettingConfig config)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.GetNewRandomSeed();
	}

	// Token: 0x06005CCC RID: 23756 RVA: 0x0021FF80 File Offset: 0x0021E180
	public override void Refresh()
	{
		string currentQualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId(this.config);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		this.allowChange = (currentClusterLayout.fixedCoordinate == -1);
		this.Input.interactable = this.allowChange;
		this.RandomizeButton.isInteractable = this.allowChange;
		if (this.allowChange)
		{
			this.InputToolTip.enabled = false;
			this.RandomizeButtonToolTip.enabled = false;
		}
		else
		{
			this.InputToolTip.enabled = true;
			this.RandomizeButtonToolTip.enabled = true;
			this.InputToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
			this.RandomizeButtonToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
		}
		this.Input.text = currentQualitySettingLevelId;
	}

	// Token: 0x06005CCD RID: 23757 RVA: 0x0022004A File Offset: 0x0021E24A
	private char ValidateInput(string text, int charIndex, char addedChar)
	{
		if ('0' > addedChar || addedChar > '9')
		{
			return '\0';
		}
		return addedChar;
	}

	// Token: 0x06005CCE RID: 23758 RVA: 0x0022005C File Offset: 0x0021E25C
	private void OnEndEdit(string text)
	{
		int seed;
		try
		{
			seed = Convert.ToInt32(text);
		}
		catch
		{
			seed = 0;
		}
		this.SetSeed(seed);
	}

	// Token: 0x06005CCF RID: 23759 RVA: 0x00220090 File Offset: 0x0021E290
	public void SetSeed(int seed)
	{
		seed = Mathf.Min(seed, int.MaxValue);
		CustomGameSettings.Instance.SetQualitySetting(this.config, seed.ToString());
		this.Refresh();
	}

	// Token: 0x06005CD0 RID: 23760 RVA: 0x002200BC File Offset: 0x0021E2BC
	private void OnValueChanged(string text)
	{
		int num = 0;
		try
		{
			num = Convert.ToInt32(text);
		}
		catch
		{
			if (text.Length > 0)
			{
				this.Input.text = text.Substring(0, text.Length - 1);
			}
			else
			{
				this.Input.text = "";
			}
		}
		if (num > 2147483647)
		{
			this.Input.text = text.Substring(0, text.Length - 1);
		}
	}

	// Token: 0x06005CD1 RID: 23761 RVA: 0x00220140 File Offset: 0x0021E340
	private void GetNewRandomSeed()
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		this.SetSeed(seed);
	}

	// Token: 0x06005CD2 RID: 23762 RVA: 0x00220160 File Offset: 0x0021E360
	protected override bool IsEnabled()
	{
		return this.allowChange;
	}

	// Token: 0x04003E61 RID: 15969
	[SerializeField]
	private LocText Label;

	// Token: 0x04003E62 RID: 15970
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04003E63 RID: 15971
	[SerializeField]
	private KInputTextField Input;

	// Token: 0x04003E64 RID: 15972
	[SerializeField]
	private KButton RandomizeButton;

	// Token: 0x04003E65 RID: 15973
	[SerializeField]
	private ToolTip InputToolTip;

	// Token: 0x04003E66 RID: 15974
	[SerializeField]
	private ToolTip RandomizeButtonToolTip;

	// Token: 0x04003E67 RID: 15975
	private const int MAX_VALID_SEED = 2147483647;

	// Token: 0x04003E68 RID: 15976
	private SeedSettingConfig config;

	// Token: 0x04003E69 RID: 15977
	private bool allowChange = true;
}
