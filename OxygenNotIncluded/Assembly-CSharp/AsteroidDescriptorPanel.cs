using System;
using System.Collections.Generic;
using Database;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AA9 RID: 2729
[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
	// Token: 0x06005342 RID: 21314 RVA: 0x001DD546 File Offset: 0x001DB746
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x06005343 RID: 21315 RVA: 0x001DD556 File Offset: 0x001DB756
	public void EnableClusterDetails(bool setActive)
	{
		this.clusterNameLabel.gameObject.SetActive(setActive);
		this.clusterDifficultyLabel.gameObject.SetActive(setActive);
	}

	// Token: 0x06005344 RID: 21316 RVA: 0x001DD57A File Offset: 0x001DB77A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005345 RID: 21317 RVA: 0x001DD584 File Offset: 0x001DB784
	public void SetClusterDetailLabels(ColonyDestinationAsteroidBeltData cluster)
	{
		StringEntry stringEntry;
		Strings.TryGet(cluster.properName, out stringEntry);
		this.clusterNameLabel.SetText((stringEntry == null) ? "" : string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, stringEntry.String));
		int index = Mathf.Clamp(cluster.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		string text = string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third);
		text = text.Trim(new char[]
		{
			'\n'
		});
		this.clusterDifficultyLabel.SetText(text);
	}

	// Token: 0x06005346 RID: 21318 RVA: 0x001DD628 File Offset: 0x001DB828
	public void SetParameterDescriptors(IList<AsteroidDescriptor> descriptors)
	{
		for (int i = 0; i < this.parameterWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.parameterWidgets[i]);
		}
		this.parameterWidgets.Clear();
		for (int j = 0; j < descriptors.Count; j++)
		{
			GameObject gameObject = global::Util.KInstantiateUI(this.prefabParameterWidget, base.gameObject, true);
			gameObject.GetComponent<LocText>().SetText(descriptors[j].text);
			ToolTip component = gameObject.GetComponent<ToolTip>();
			if (!string.IsNullOrEmpty(descriptors[j].tooltip))
			{
				component.SetSimpleTooltip(descriptors[j].tooltip);
			}
			this.parameterWidgets.Add(gameObject);
		}
	}

	// Token: 0x06005347 RID: 21319 RVA: 0x001DD6DC File Offset: 0x001DB8DC
	private void ClearTraitDescriptors()
	{
		for (int i = 0; i < this.traitWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.traitWidgets[i]);
		}
		this.traitWidgets.Clear();
		for (int j = 0; j < this.traitCategoryWidgets.Count; j++)
		{
			UnityEngine.Object.Destroy(this.traitCategoryWidgets[j]);
		}
		this.traitCategoryWidgets.Clear();
	}

	// Token: 0x06005348 RID: 21320 RVA: 0x001DD750 File Offset: 0x001DB950
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, List<string> stories, bool includeDescriptions = true)
	{
		foreach (string id in stories)
		{
			WorldTrait storyTrait = Db.Get().Stories.Get(id).StoryTrait;
			string tooltip = DlcManager.IsPureVanilla() ? Strings.Get(storyTrait.description + "_SHORT") : Strings.Get(storyTrait.description);
			descriptors.Add(new AsteroidDescriptor(Strings.Get(storyTrait.name).String, tooltip, Color.white, null, storyTrait.icon));
		}
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
		if (stories.Count != 0)
		{
			this.storyTraitHeader.rectTransform().SetSiblingIndex(this.storyTraitHeader.rectTransform().parent.childCount - stories.Count - 1);
			this.storyTraitHeader.SetActive(true);
			return;
		}
		this.storyTraitHeader.SetActive(false);
	}

	// Token: 0x06005349 RID: 21321 RVA: 0x001DD868 File Offset: 0x001DBA68
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, bool includeDescriptions = true)
	{
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
	}

	// Token: 0x0600534A RID: 21322 RVA: 0x001DD880 File Offset: 0x001DBA80
	public void SetTraitDescriptors(List<IList<AsteroidDescriptor>> descriptorSets, bool includeDescriptions = true, List<global::Tuple<string, Sprite>> headerData = null)
	{
		this.ClearTraitDescriptors();
		for (int i = 0; i < descriptorSets.Count; i++)
		{
			IList<AsteroidDescriptor> list = descriptorSets[i];
			GameObject gameObject = base.gameObject;
			if (descriptorSets.Count > 1)
			{
				global::Debug.Assert(headerData != null, "Asteroid Header data is null - traits wont have their world as contex in the selection UI");
				GameObject gameObject2 = global::Util.KInstantiate(this.prefabTraitCategoryWidget, base.gameObject, null);
				HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
				gameObject2.transform.localScale = Vector3.one;
				StringEntry stringEntry;
				Strings.TryGet(headerData[i].first, out stringEntry);
				component.GetReference<LocText>("NameLabel").SetText(stringEntry.String);
				component.GetReference<Image>("Icon").sprite = headerData[i].second;
				gameObject2.SetActive(true);
				gameObject = component.GetReference<RectTransform>("Contents").gameObject;
				this.traitCategoryWidgets.Add(gameObject2);
			}
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject3 = global::Util.KInstantiate(this.prefabTraitWidget, gameObject, null);
				HierarchyReferences component2 = gameObject3.GetComponent<HierarchyReferences>();
				gameObject3.SetActive(true);
				component2.GetReference<LocText>("NameLabel").SetText("<b>" + list[j].text + "</b>");
				Image reference = component2.GetReference<Image>("Icon");
				reference.color = list[j].associatedColor;
				if (list[j].associatedIcon != null)
				{
					Sprite sprite = Assets.GetSprite(list[j].associatedIcon);
					if (sprite != null)
					{
						reference.sprite = sprite;
					}
				}
				if (gameObject3.GetComponent<ToolTip>() != null)
				{
					gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(list[j].tooltip);
				}
				LocText reference2 = component2.GetReference<LocText>("DescLabel");
				if (includeDescriptions && !string.IsNullOrEmpty(list[j].tooltip))
				{
					reference2.SetText(list[j].tooltip);
				}
				else
				{
					reference2.gameObject.SetActive(false);
				}
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject3.SetActive(true);
				this.traitWidgets.Add(gameObject3);
			}
		}
	}

	// Token: 0x0600534B RID: 21323 RVA: 0x001DDACC File Offset: 0x001DBCCC
	public void EnableClusterLocationLabels(bool enable)
	{
		this.startingAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.nearbyAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.distantAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
	}

	// Token: 0x0600534C RID: 21324 RVA: 0x001DDB2C File Offset: 0x001DBD2C
	public void RefreshAsteroidLines(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> storyTraits)
	{
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			if (!keyValuePair.Value.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
		}
		this.asteroidLines.Clear();
		this.SpawnAsteroidLine(cluster.GetStartWorld, this.startingAsteroidRowContainer, cluster);
		for (int i = 0; i < cluster.worlds.Count; i++)
		{
			ProcGen.World world = cluster.worlds[i];
			WorldPlacement worldPlacement = null;
			for (int j = 0; j < cluster.Layout.worldPlacements.Count; j++)
			{
				if (cluster.Layout.worldPlacements[j].world == world.filePath)
				{
					worldPlacement = cluster.Layout.worldPlacements[j];
					break;
				}
			}
			this.SpawnAsteroidLine(world, (worldPlacement.locationType == WorldPlacement.LocationType.InnerCluster) ? this.nearbyAsteroidRowContainer : this.distantAsteroidRowContainer, cluster);
		}
		using (Dictionary<ProcGen.World, GameObject>.Enumerator enumerator = this.asteroidLines.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ProcGen.World, GameObject> line = enumerator.Current;
				MultiToggle component = line.Value.GetComponent<MultiToggle>();
				component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
				{
					this.SelectAsteroidInCluster(line.Key, cluster, selectedAsteroidDetailsPanel);
				}));
			}
		}
		this.SelectWholeClusterDetails(cluster, selectedAsteroidDetailsPanel, storyTraits);
	}

	// Token: 0x0600534D RID: 21325 RVA: 0x001DDD34 File Offset: 0x001DBF34
	private void SelectAsteroidInCluster(ProcGen.World asteroid, ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(true);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(0);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == asteroid) ? 1 : 0);
			if (keyValuePair.Key == asteroid)
			{
				this.SetSelectedAsteroid(keyValuePair.Key, selectedAsteroidDetailsPanel, cluster.GenerateTraitDescriptors(keyValuePair.Key, true));
			}
		}
	}

	// Token: 0x0600534E RID: 21326 RVA: 0x001DDDE4 File Offset: 0x001DBFE4
	public void SelectWholeClusterDetails(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> stories)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(false);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
		}
		this.SetSelectedCluster(cluster, selectedAsteroidDetailsPanel, stories);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(1);
	}

	// Token: 0x0600534F RID: 21327 RVA: 0x001DDE68 File Offset: 0x001DC068
	private void SpawnAsteroidLine(ProcGen.World asteroid, GameObject parentContainer, ColonyDestinationAsteroidBeltData cluster)
	{
		if (this.asteroidLines.ContainsKey(asteroid))
		{
			return;
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.prefabAsteroidLine, parentContainer.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("Label");
		RectTransform reference3 = component.GetReference<RectTransform>("TraitsRow");
		LocText reference4 = component.GetReference<LocText>("TraitLabel");
		ToolTip component2 = gameObject.GetComponent<ToolTip>();
		Sprite uisprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		reference.sprite = uisprite;
		StringEntry stringEntry;
		Strings.TryGet(asteroid.name, out stringEntry);
		reference2.SetText(stringEntry.String);
		List<WorldTrait> worldTraits = cluster.GetWorldTraits(asteroid);
		reference4.gameObject.SetActive(worldTraits.Count == 0);
		reference4.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS);
		RectTransform reference5 = component.GetReference<RectTransform>("TraitIconPrefab");
		foreach (WorldTrait worldTrait in worldTraits)
		{
			Image component3 = global::Util.KInstantiateUI(reference5.gameObject, reference3.gameObject, true).GetComponent<Image>();
			Sprite sprite = Assets.GetSprite(worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1));
			if (sprite != null)
			{
				component3.sprite = sprite;
			}
			component3.color = global::Util.ColorFromHex(worldTrait.colorHex);
		}
		string text = "";
		if (worldTraits.Count > 0)
		{
			for (int i = 0; i < worldTraits.Count; i++)
			{
				StringEntry stringEntry2;
				Strings.TryGet(worldTraits[i].name, out stringEntry2);
				StringEntry stringEntry3;
				Strings.TryGet(worldTraits[i].description, out stringEntry3);
				text = string.Concat(new string[]
				{
					text,
					"<color=#",
					worldTraits[i].colorHex,
					">",
					stringEntry2.String,
					"</color>\n",
					stringEntry3.String
				});
				if (i != worldTraits.Count - 1)
				{
					text += "\n\n";
				}
			}
		}
		else
		{
			text = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		component2.SetSimpleTooltip(text);
		this.asteroidLines.Add(asteroid, gameObject);
	}

	// Token: 0x06005350 RID: 21328 RVA: 0x001DE0D0 File Offset: 0x001DC2D0
	private void SetSelectedAsteroid(ProcGen.World asteroid, AsteroidDescriptorPanel detailPanel, List<AsteroidDescriptor> traitDescriptors)
	{
		detailPanel.SetTraitDescriptors(traitDescriptors, true);
		detailPanel.selectedAsteroidIcon.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(true);
		StringEntry stringEntry;
		Strings.TryGet(asteroid.name, out stringEntry);
		detailPanel.selectedAsteroidLabel.SetText(stringEntry.String);
		StringEntry stringEntry2;
		Strings.TryGet(asteroid.description, out stringEntry2);
		detailPanel.selectedAsteroidDescription.SetText(stringEntry2.String);
	}

	// Token: 0x06005351 RID: 21329 RVA: 0x001DE14C File Offset: 0x001DC34C
	private void SetSelectedCluster(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel detailPanel, List<string> stories)
	{
		List<IList<AsteroidDescriptor>> list = new List<IList<AsteroidDescriptor>>();
		List<global::Tuple<string, Sprite>> list2 = new List<global::Tuple<string, Sprite>>();
		List<AsteroidDescriptor> list3 = cluster.GenerateTraitDescriptors(cluster.GetStartWorld, false);
		if (list3.Count != 0)
		{
			list2.Add(new global::Tuple<string, Sprite>(cluster.GetStartWorld.name, ColonyDestinationAsteroidBeltData.GetUISprite(cluster.GetStartWorld.asteroidIcon)));
			list.Add(list3);
		}
		foreach (ProcGen.World world in cluster.worlds)
		{
			List<AsteroidDescriptor> list4 = cluster.GenerateTraitDescriptors(world, false);
			if (list4.Count != 0)
			{
				list2.Add(new global::Tuple<string, Sprite>(world.name, ColonyDestinationAsteroidBeltData.GetUISprite(world.asteroidIcon)));
				list.Add(list4);
			}
		}
		list2.Add(new global::Tuple<string, Sprite>("STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER", Assets.GetSprite("codexIconStoryTraits")));
		List<AsteroidDescriptor> list5 = new List<AsteroidDescriptor>();
		foreach (string id in stories)
		{
			Story story = Db.Get().Stories.Get(id);
			string icon = story.StoryTrait.icon;
			AsteroidDescriptor item = new AsteroidDescriptor(Strings.Get(story.StoryTrait.name).String, Strings.Get(story.StoryTrait.description).String, Color.white, null, icon);
			list5.Add(item);
		}
		list.Add(list5);
		detailPanel.SetTraitDescriptors(list, false, list2);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(false);
		StringEntry stringEntry;
		Strings.TryGet(cluster.properName, out stringEntry);
		detailPanel.selectedAsteroidLabel.SetText(stringEntry.String);
		detailPanel.selectedAsteroidDescription.SetText("");
	}

	// Token: 0x04003775 RID: 14197
	[Header("Destination Details")]
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x04003776 RID: 14198
	[SerializeField]
	private GameObject prefabTraitWidget;

	// Token: 0x04003777 RID: 14199
	[SerializeField]
	private GameObject prefabTraitCategoryWidget;

	// Token: 0x04003778 RID: 14200
	[SerializeField]
	private GameObject prefabParameterWidget;

	// Token: 0x04003779 RID: 14201
	[SerializeField]
	private GameObject startingAsteroidRowContainer;

	// Token: 0x0400377A RID: 14202
	[SerializeField]
	private GameObject nearbyAsteroidRowContainer;

	// Token: 0x0400377B RID: 14203
	[SerializeField]
	private GameObject distantAsteroidRowContainer;

	// Token: 0x0400377C RID: 14204
	[SerializeField]
	private LocText clusterNameLabel;

	// Token: 0x0400377D RID: 14205
	[SerializeField]
	private LocText clusterDifficultyLabel;

	// Token: 0x0400377E RID: 14206
	[SerializeField]
	public LocText headerLabel;

	// Token: 0x0400377F RID: 14207
	[SerializeField]
	public MultiToggle clusterDetailsButton;

	// Token: 0x04003780 RID: 14208
	[SerializeField]
	public GameObject storyTraitHeader;

	// Token: 0x04003781 RID: 14209
	private List<GameObject> labels = new List<GameObject>();

	// Token: 0x04003782 RID: 14210
	[Header("Selected Asteroid Details")]
	[SerializeField]
	private GameObject SpacedOutContentContainer;

	// Token: 0x04003783 RID: 14211
	public Image selectedAsteroidIcon;

	// Token: 0x04003784 RID: 14212
	public LocText selectedAsteroidLabel;

	// Token: 0x04003785 RID: 14213
	public LocText selectedAsteroidDescription;

	// Token: 0x04003786 RID: 14214
	[SerializeField]
	private GameObject prefabAsteroidLine;

	// Token: 0x04003787 RID: 14215
	private Dictionary<ProcGen.World, GameObject> asteroidLines = new Dictionary<ProcGen.World, GameObject>();

	// Token: 0x04003788 RID: 14216
	private List<GameObject> traitWidgets = new List<GameObject>();

	// Token: 0x04003789 RID: 14217
	private List<GameObject> traitCategoryWidgets = new List<GameObject>();

	// Token: 0x0400378A RID: 14218
	private List<GameObject> parameterWidgets = new List<GameObject>();
}
