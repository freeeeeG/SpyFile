using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1E RID: 3102
public class GeneticAnalysisStationSideScreen : SideScreenContent
{
	// Token: 0x06006226 RID: 25126 RVA: 0x00243CF0 File Offset: 0x00241EF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Refresh();
	}

	// Token: 0x06006227 RID: 25127 RVA: 0x00243CFE File Offset: 0x00241EFE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<GeneticAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x06006228 RID: 25128 RVA: 0x00243D09 File Offset: 0x00241F09
	public override void SetTarget(GameObject target)
	{
		this.target = target.GetSMI<GeneticAnalysisStation.StatesInstance>();
		target.GetComponent<GeneticAnalysisStationWorkable>();
		this.Refresh();
	}

	// Token: 0x06006229 RID: 25129 RVA: 0x00243D24 File Offset: 0x00241F24
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.DrawPickerMenu();
	}

	// Token: 0x0600622A RID: 25130 RVA: 0x00243D38 File Offset: 0x00241F38
	private void DrawPickerMenu()
	{
		Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> dictionary = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();
		foreach (Tag tag in PlantSubSpeciesCatalog.Instance.GetAllDiscoveredSpecies())
		{
			dictionary[tag] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>(PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(tag));
		}
		int num = 0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in dictionary)
		{
			if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(keyValuePair.Key).Count > 1)
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
				if (!(prefab == null))
				{
					SeedProducer component = prefab.GetComponent<SeedProducer>();
					if (!(component == null))
					{
						Tag tag2 = component.seedInfo.seedId.ToTag();
						if (DiscoveredResources.Instance.IsDiscovered(tag2))
						{
							HierarchyReferences hierarchyReferences;
							if (num < this.rows.Count)
							{
								hierarchyReferences = this.rows[num];
							}
							else
							{
								hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.rowPrefab.gameObject, this.rowContainer, false);
								this.rows.Add(hierarchyReferences);
							}
							this.ConfigureButton(hierarchyReferences, keyValuePair.Key);
							this.rows[num].gameObject.SetActive(true);
							num++;
						}
					}
				}
			}
		}
		for (int i = num; i < this.rows.Count; i++)
		{
			this.rows[i].gameObject.SetActive(false);
		}
		if (num == 0)
		{
			this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.NONE_DISCOVERED;
			this.contents.gameObject.SetActive(false);
			return;
		}
		this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SELECT_SEEDS;
		this.contents.gameObject.SetActive(true);
	}

	// Token: 0x0600622B RID: 25131 RVA: 0x00243F44 File Offset: 0x00242144
	private void ConfigureButton(HierarchyReferences button, Tag speciesID)
	{
		TMP_Text reference = button.GetReference<LocText>("Label");
		Image reference2 = button.GetReference<Image>("Icon");
		LocText reference3 = button.GetReference<LocText>("ProgressLabel");
		button.GetReference<ToolTip>("ToolTip");
		Tag seedID = this.GetSeedIDFromPlantID(speciesID);
		bool isForbidden = this.target.GetSeedForbidden(seedID);
		reference.text = seedID.ProperName();
		reference2.sprite = Def.GetUISprite(seedID, "ui", false).first;
		if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(speciesID).Count > 0)
		{
			reference3.text = (isForbidden ? UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_FORBIDDEN : UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_ALLOWED);
		}
		else
		{
			reference3.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_NO_MUTANTS;
		}
		KToggle component = button.GetComponent<KToggle>();
		component.isOn = !isForbidden;
		component.ClearOnClick();
		component.onClick += delegate()
		{
			this.target.SetSeedForbidden(seedID, !isForbidden);
			this.Refresh();
		};
	}

	// Token: 0x0600622C RID: 25132 RVA: 0x00244052 File Offset: 0x00242252
	private Tag GetSeedIDFromPlantID(Tag speciesID)
	{
		return Assets.GetPrefab(speciesID).GetComponent<SeedProducer>().seedInfo.seedId;
	}

	// Token: 0x040042E6 RID: 17126
	[SerializeField]
	private LocText message;

	// Token: 0x040042E7 RID: 17127
	[SerializeField]
	private GameObject contents;

	// Token: 0x040042E8 RID: 17128
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x040042E9 RID: 17129
	[SerializeField]
	private HierarchyReferences rowPrefab;

	// Token: 0x040042EA RID: 17130
	private List<HierarchyReferences> rows = new List<HierarchyReferences>();

	// Token: 0x040042EB RID: 17131
	private GeneticAnalysisStation.StatesInstance target;

	// Token: 0x040042EC RID: 17132
	private Dictionary<Tag, bool> expandedSeeds = new Dictionary<Tag, bool>();
}
