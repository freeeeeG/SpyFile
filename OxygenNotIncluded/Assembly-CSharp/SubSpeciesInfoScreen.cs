using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C74 RID: 3188
public class SubSpeciesInfoScreen : KModalScreen
{
	// Token: 0x060065B5 RID: 26037 RVA: 0x0025EF34 File Offset: 0x0025D134
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060065B6 RID: 26038 RVA: 0x0025EF37 File Offset: 0x0025D137
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060065B7 RID: 26039 RVA: 0x0025EF40 File Offset: 0x0025D140
	private void ClearMutations()
	{
		for (int i = this.mutationLineItems.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.mutationLineItems[i]);
		}
		this.mutationLineItems.Clear();
	}

	// Token: 0x060065B8 RID: 26040 RVA: 0x0025EF81 File Offset: 0x0025D181
	public void DisplayDiscovery(Tag speciesID, Tag subSpeciesID, GeneticAnalysisStation station)
	{
		this.SetSubspecies(speciesID, subSpeciesID);
		this.targetStation = station;
	}

	// Token: 0x060065B9 RID: 26041 RVA: 0x0025EF94 File Offset: 0x0025D194
	private void SetSubspecies(Tag speciesID, Tag subSpeciesID)
	{
		this.ClearMutations();
		ref PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(speciesID, subSpeciesID);
		this.plantIcon.sprite = Def.GetUISprite(Assets.GetPrefab(speciesID), "ui", false).first;
		foreach (string id in subSpecies.mutationIDs)
		{
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			GameObject gameObject = Util.KInstantiateUI(this.mutationsItemPrefab, this.mutationsList.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("nameLabel").text = plantMutation.Name;
			component.GetReference<LocText>("descriptionLabel").text = plantMutation.description;
			this.mutationLineItems.Add(gameObject);
		}
	}

	// Token: 0x04004602 RID: 17922
	[SerializeField]
	private KButton renameButton;

	// Token: 0x04004603 RID: 17923
	[SerializeField]
	private KButton saveButton;

	// Token: 0x04004604 RID: 17924
	[SerializeField]
	private KButton discardButton;

	// Token: 0x04004605 RID: 17925
	[SerializeField]
	private RectTransform mutationsList;

	// Token: 0x04004606 RID: 17926
	[SerializeField]
	private Image plantIcon;

	// Token: 0x04004607 RID: 17927
	[SerializeField]
	private GameObject mutationsItemPrefab;

	// Token: 0x04004608 RID: 17928
	private List<GameObject> mutationLineItems = new List<GameObject>();

	// Token: 0x04004609 RID: 17929
	private GeneticAnalysisStation targetStation;
}
