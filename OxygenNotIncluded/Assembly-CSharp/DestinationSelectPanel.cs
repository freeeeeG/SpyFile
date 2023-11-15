using System;
using System.Collections.Generic;
using System.Linq;
using Klei.CustomSettings;
using ProcGen;
using UnityEngine;

// Token: 0x02000AEE RID: 2798
[AddComponentMenu("KMonoBehaviour/scripts/DestinationSelectPanel")]
public class DestinationSelectPanel : KMonoBehaviour
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x0600562A RID: 22058 RVA: 0x001F5D8C File Offset: 0x001F3F8C
	// (set) Token: 0x0600562B RID: 22059 RVA: 0x001F5D93 File Offset: 0x001F3F93
	public static int ChosenClusterCategorySetting
	{
		get
		{
			return DestinationSelectPanel.chosenClusterCategorySetting;
		}
		set
		{
			DestinationSelectPanel.chosenClusterCategorySetting = value;
		}
	}

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x0600562C RID: 22060 RVA: 0x001F5D9C File Offset: 0x001F3F9C
	// (remove) Token: 0x0600562D RID: 22061 RVA: 0x001F5DD4 File Offset: 0x001F3FD4
	public event Action<ColonyDestinationAsteroidBeltData> OnAsteroidClicked;

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x0600562E RID: 22062 RVA: 0x001F5E0C File Offset: 0x001F400C
	private float min
	{
		get
		{
			return this.asteroidContainer.rect.x + this.offset;
		}
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x0600562F RID: 22063 RVA: 0x001F5E34 File Offset: 0x001F4034
	private float max
	{
		get
		{
			return this.min + this.asteroidContainer.rect.width;
		}
	}

	// Token: 0x06005630 RID: 22064 RVA: 0x001F5E5C File Offset: 0x001F405C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dragTarget.onBeginDrag += this.BeginDrag;
		this.dragTarget.onDrag += this.Drag;
		this.dragTarget.onEndDrag += this.EndDrag;
		MultiToggle multiToggle = this.leftArrowButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ClickLeft));
		MultiToggle multiToggle2 = this.rightArrowButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.ClickRight));
	}

	// Token: 0x06005631 RID: 22065 RVA: 0x001F5F02 File Offset: 0x001F4102
	private void BeginDrag()
	{
		this.dragStartPos = KInputManager.GetMousePos();
		this.dragLastPos = this.dragStartPos;
		this.isDragging = true;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Start", false));
	}

	// Token: 0x06005632 RID: 22066 RVA: 0x001F5F38 File Offset: 0x001F4138
	private void Drag()
	{
		Vector2 vector = KInputManager.GetMousePos();
		float num = vector.x - this.dragLastPos.x;
		this.dragLastPos = vector;
		this.offset += num;
		int num2 = this.selectedIndex;
		this.selectedIndex = Mathf.RoundToInt(-this.offset / this.asteroidXSeparation);
		this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.clusterStartWorlds.Count - 1);
		if (num2 != this.selectedIndex)
		{
			this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
			KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll", false));
		}
	}

	// Token: 0x06005633 RID: 22067 RVA: 0x001F5FF5 File Offset: 0x001F41F5
	private void EndDrag()
	{
		this.Drag();
		this.isDragging = false;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Stop", false));
	}

	// Token: 0x06005634 RID: 22068 RVA: 0x001F6014 File Offset: 0x001F4214
	private void ClickLeft()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex - 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06005635 RID: 22069 RVA: 0x001F606C File Offset: 0x001F426C
	private void ClickRight()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex + 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06005636 RID: 22070 RVA: 0x001F60C1 File Offset: 0x001F42C1
	public void Init()
	{
		this.clusterKeys = new List<string>();
		this.clusterStartWorlds = new Dictionary<string, string>();
		this.UpdateDisplayedClusters();
	}

	// Token: 0x06005637 RID: 22071 RVA: 0x001F60E0 File Offset: 0x001F42E0
	private void Update()
	{
		if (!this.isDragging)
		{
			float num = this.offset + (float)this.selectedIndex * this.asteroidXSeparation;
			float num2 = 0f;
			if (num != 0f)
			{
				num2 = -num;
			}
			num2 = Mathf.Clamp(num2, -this.asteroidXSeparation * 2f, this.asteroidXSeparation * 2f);
			if (num2 != 0f)
			{
				float num3 = this.centeringSpeed * Time.unscaledDeltaTime;
				float num4 = num2 * this.centeringSpeed * Time.unscaledDeltaTime;
				if (num4 > 0f && num4 < num3)
				{
					num4 = Mathf.Min(num3, num2);
				}
				else if (num4 < 0f && num4 > -num3)
				{
					num4 = Mathf.Max(-num3, num2);
				}
				this.offset += num4;
			}
		}
		float x = this.asteroidContainer.rect.min.x;
		float x2 = this.asteroidContainer.rect.max.x;
		this.offset = Mathf.Clamp(this.offset, (float)(-(float)(this.clusterStartWorlds.Count - 1)) * this.asteroidXSeparation + x, x2);
		this.RePlaceAsteroids();
		for (int i = 0; i < this.moonContainer.transform.childCount; i++)
		{
			this.moonContainer.transform.GetChild(i).GetChild(0).SetLocalPosition(new Vector3(0f, 1.5f + 3f * Mathf.Sin(((float)i + Time.realtimeSinceStartup) * 1.25f), 0f));
		}
	}

	// Token: 0x06005638 RID: 22072 RVA: 0x001F627C File Offset: 0x001F447C
	public void UpdateDisplayedClusters()
	{
		this.clusterKeys.Clear();
		this.clusterStartWorlds.Clear();
		this.asteroidData.Clear();
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
		{
			if ((!DlcManager.FeatureClusterSpaceEnabled() || !(keyValuePair.Key == "clusters/SandstoneDefault")) && keyValuePair.Value.clusterCategory == DestinationSelectPanel.ChosenClusterCategorySetting)
			{
				this.clusterKeys.Add(keyValuePair.Key);
				ColonyDestinationAsteroidBeltData value = new ColonyDestinationAsteroidBeltData(keyValuePair.Value.GetStartWorld(), 0, keyValuePair.Key);
				this.asteroidData[keyValuePair.Key] = value;
				this.clusterStartWorlds.Add(keyValuePair.Key, keyValuePair.Value.GetStartWorld());
			}
		}
		this.clusterKeys.Sort((string a, string b) => SettingsCache.clusterLayouts.clusterCache[a].menuOrder.CompareTo(SettingsCache.clusterLayouts.clusterCache[b].menuOrder));
	}

	// Token: 0x06005639 RID: 22073 RVA: 0x001F63A8 File Offset: 0x001F45A8
	[ContextMenu("RePlaceAsteroids")]
	public void RePlaceAsteroids()
	{
		this.BeginAsteroidDrawing();
		for (int i = 0; i < this.clusterKeys.Count; i++)
		{
			float x = this.offset + (float)i * this.asteroidXSeparation;
			string text = this.clusterKeys[i];
			float iconScale = this.asteroidData[text].GetStartWorld.iconScale;
			this.GetAsteroid(text, (i == this.selectedIndex) ? (this.asteroidFocusScale * iconScale) : iconScale).transform.SetLocalPosition(new Vector3(x, (i == this.selectedIndex) ? (5f + 10f * Mathf.Sin(Time.realtimeSinceStartup * 1f)) : 0f, 0f));
		}
		this.EndAsteroidDrawing();
	}

	// Token: 0x0600563A RID: 22074 RVA: 0x001F646F File Offset: 0x001F466F
	private void BeginAsteroidDrawing()
	{
		this.numAsteroids = 0;
	}

	// Token: 0x0600563B RID: 22075 RVA: 0x001F6478 File Offset: 0x001F4678
	private void ShowMoons(ColonyDestinationAsteroidBeltData asteroid)
	{
		if (asteroid.worlds.Count > 0)
		{
			while (this.moonContainer.transform.childCount < asteroid.worlds.Count)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.moonPrefab, this.moonContainer.transform);
			}
			for (int i = 0; i < asteroid.worlds.Count; i++)
			{
				KBatchedAnimController componentInChildren = this.moonContainer.transform.GetChild(i).GetComponentInChildren<KBatchedAnimController>();
				int index = (-1 + i + asteroid.worlds.Count / 2) % asteroid.worlds.Count;
				ProcGen.World world = asteroid.worlds[index];
				KAnimFile anim = Assets.GetAnim(world.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : world.asteroidIcon);
				if (anim != null)
				{
					componentInChildren.SetVisiblity(true);
					componentInChildren.SwapAnims(new KAnimFile[]
					{
						anim
					});
					componentInChildren.initialMode = KAnim.PlayMode.Loop;
					componentInChildren.initialAnim = "idle_loop";
					componentInChildren.gameObject.SetActive(true);
					if (componentInChildren.HasAnimation(componentInChildren.initialAnim))
					{
						componentInChildren.Play(componentInChildren.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
					}
					componentInChildren.transform.parent.gameObject.SetActive(true);
				}
			}
			for (int j = asteroid.worlds.Count; j < this.moonContainer.transform.childCount; j++)
			{
				KBatchedAnimController componentInChildren2 = this.moonContainer.transform.GetChild(j).GetComponentInChildren<KBatchedAnimController>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.SetVisiblity(false);
				}
				this.moonContainer.transform.GetChild(j).gameObject.SetActive(false);
			}
			return;
		}
		KBatchedAnimController[] componentsInChildren = this.moonContainer.GetComponentsInChildren<KBatchedAnimController>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].SetVisiblity(false);
		}
	}

	// Token: 0x0600563C RID: 22076 RVA: 0x001F6674 File Offset: 0x001F4874
	private DestinationAsteroid2 GetAsteroid(string name, float scale)
	{
		DestinationAsteroid2 destinationAsteroid;
		if (this.numAsteroids < this.asteroids.Count)
		{
			destinationAsteroid = this.asteroids[this.numAsteroids];
		}
		else
		{
			destinationAsteroid = global::Util.KInstantiateUI<DestinationAsteroid2>(this.asteroidPrefab, this.asteroidContainer.gameObject, false);
			destinationAsteroid.OnClicked += this.OnAsteroidClicked;
			this.asteroids.Add(destinationAsteroid);
		}
		destinationAsteroid.SetAsteroid(this.asteroidData[name]);
		this.asteroidData[name].TargetScale = scale;
		this.asteroidData[name].Scale += (this.asteroidData[name].TargetScale - this.asteroidData[name].Scale) * this.focusScaleSpeed * Time.unscaledDeltaTime;
		destinationAsteroid.transform.localScale = Vector3.one * this.asteroidData[name].Scale;
		this.numAsteroids++;
		return destinationAsteroid;
	}

	// Token: 0x0600563D RID: 22077 RVA: 0x001F677C File Offset: 0x001F497C
	private void EndAsteroidDrawing()
	{
		for (int i = 0; i < this.asteroids.Count; i++)
		{
			this.asteroids[i].gameObject.SetActive(i < this.numAsteroids);
		}
	}

	// Token: 0x0600563E RID: 22078 RVA: 0x001F67BE File Offset: 0x001F49BE
	public ColonyDestinationAsteroidBeltData SelectCluster(string name, int seed)
	{
		this.selectedIndex = this.clusterKeys.IndexOf(name);
		this.asteroidData[name].ReInitialize(seed);
		return this.asteroidData[name];
	}

	// Token: 0x0600563F RID: 22079 RVA: 0x001F67F0 File Offset: 0x001F49F0
	public string GetDefaultAsteroid()
	{
		return this.clusterKeys.First<string>();
	}

	// Token: 0x06005640 RID: 22080 RVA: 0x001F6800 File Offset: 0x001F4A00
	public ColonyDestinationAsteroidBeltData SelectDefaultAsteroid(int seed)
	{
		this.selectedIndex = 0;
		string key = this.asteroidData.Keys.First<string>();
		this.asteroidData[key].ReInitialize(seed);
		return this.asteroidData[key];
	}

	// Token: 0x06005641 RID: 22081 RVA: 0x001F6844 File Offset: 0x001F4A44
	public void ScrollLeft()
	{
		int index = Mathf.Max(this.selectedIndex - 1, 0);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x06005642 RID: 22082 RVA: 0x001F6884 File Offset: 0x001F4A84
	public void ScrollRight()
	{
		int index = Mathf.Min(this.selectedIndex + 1, this.clusterStartWorlds.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x06005643 RID: 22083 RVA: 0x001F68D0 File Offset: 0x001F4AD0
	private void DebugCurrentSetting()
	{
		ColonyDestinationAsteroidBeltData colonyDestinationAsteroidBeltData = this.asteroidData[this.clusterKeys[this.selectedIndex]];
		string text = "{world}: {seed} [{traits}] {{settings}}";
		string startWorldName = colonyDestinationAsteroidBeltData.startWorldName;
		string newValue = colonyDestinationAsteroidBeltData.seed.ToString();
		text = text.Replace("{world}", startWorldName);
		text = text.Replace("{seed}", newValue);
		List<AsteroidDescriptor> traitDescriptors = colonyDestinationAsteroidBeltData.GetTraitDescriptors();
		string[] array = new string[traitDescriptors.Count];
		for (int i = 0; i < traitDescriptors.Count; i++)
		{
			array[i] = traitDescriptors[i].text;
		}
		string newValue2 = string.Join(", ", array);
		text = text.Replace("{traits}", newValue2);
		CustomGameSettings.CustomGameMode customGameMode = CustomGameSettings.Instance.customGameMode;
		if (customGameMode != CustomGameSettings.CustomGameMode.Survival)
		{
			if (customGameMode != CustomGameSettings.CustomGameMode.Nosweat)
			{
				if (customGameMode == CustomGameSettings.CustomGameMode.Custom)
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.QualitySettings)
					{
						if (keyValuePair.Value.coordinate_dimension >= 0L && keyValuePair.Value.coordinate_dimension_width >= 0L)
						{
							SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(keyValuePair.Key);
							if (currentQualitySetting.id != keyValuePair.Value.GetDefaultLevelId())
							{
								list.Add(string.Format("{0}={1}", keyValuePair.Value.label, currentQualitySetting.label));
							}
						}
					}
					text = text.Replace("{settings}", string.Join(", ", list.ToArray()));
				}
			}
			else
			{
				text = text.Replace("{settings}", "Nosweat");
			}
		}
		else
		{
			text = text.Replace("{settings}", "Survival");
		}
		global::Debug.Log(text);
	}

	// Token: 0x040039EA RID: 14826
	[SerializeField]
	private GameObject asteroidPrefab;

	// Token: 0x040039EB RID: 14827
	[SerializeField]
	private KButtonDrag dragTarget;

	// Token: 0x040039EC RID: 14828
	[SerializeField]
	private MultiToggle leftArrowButton;

	// Token: 0x040039ED RID: 14829
	[SerializeField]
	private MultiToggle rightArrowButton;

	// Token: 0x040039EE RID: 14830
	[SerializeField]
	private RectTransform asteroidContainer;

	// Token: 0x040039EF RID: 14831
	[SerializeField]
	private float asteroidFocusScale = 2f;

	// Token: 0x040039F0 RID: 14832
	[SerializeField]
	private float asteroidXSeparation = 240f;

	// Token: 0x040039F1 RID: 14833
	[SerializeField]
	private float focusScaleSpeed = 0.5f;

	// Token: 0x040039F2 RID: 14834
	[SerializeField]
	private float centeringSpeed = 0.5f;

	// Token: 0x040039F3 RID: 14835
	[SerializeField]
	private GameObject moonContainer;

	// Token: 0x040039F4 RID: 14836
	[SerializeField]
	private GameObject moonPrefab;

	// Token: 0x040039F5 RID: 14837
	private static int chosenClusterCategorySetting;

	// Token: 0x040039F7 RID: 14839
	private float offset;

	// Token: 0x040039F8 RID: 14840
	private int selectedIndex = -1;

	// Token: 0x040039F9 RID: 14841
	private List<DestinationAsteroid2> asteroids = new List<DestinationAsteroid2>();

	// Token: 0x040039FA RID: 14842
	private int numAsteroids;

	// Token: 0x040039FB RID: 14843
	private List<string> clusterKeys;

	// Token: 0x040039FC RID: 14844
	private Dictionary<string, string> clusterStartWorlds;

	// Token: 0x040039FD RID: 14845
	private Dictionary<string, ColonyDestinationAsteroidBeltData> asteroidData = new Dictionary<string, ColonyDestinationAsteroidBeltData>();

	// Token: 0x040039FE RID: 14846
	private Vector2 dragStartPos;

	// Token: 0x040039FF RID: 14847
	private Vector2 dragLastPos;

	// Token: 0x04003A00 RID: 14848
	private bool isDragging;

	// Token: 0x04003A01 RID: 14849
	private const string debugFmt = "{world}: {seed} [{traits}] {{settings}}";
}
