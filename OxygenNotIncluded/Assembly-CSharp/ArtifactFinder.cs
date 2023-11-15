using System;
using System.Collections.Generic;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x0200058D RID: 1421
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactFinder")]
public class ArtifactFinder : KMonoBehaviour
{
	// Token: 0x0600225D RID: 8797 RVA: 0x000BCDB8 File Offset: 0x000BAFB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ArtifactFinder>(-887025858, ArtifactFinder.OnLandDelegate);
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000BCDD4 File Offset: 0x000BAFD4
	public ArtifactTier GetArtifactDropTier(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactDropRate artifactDropTable = destination.GetDestinationType().artifactDropTable;
		bool flag = minionID.traitIDs.Contains("Archaeologist");
		if (artifactDropTable != null)
		{
			float num = artifactDropTable.totalWeight;
			if (flag)
			{
				num -= artifactDropTable.GetTierWeight(DECOR.SPACEARTIFACT.TIER_NONE);
			}
			float num2 = UnityEngine.Random.value * num;
			foreach (global::Tuple<ArtifactTier, float> tuple in artifactDropTable.rates)
			{
				if (!flag || (flag && tuple.first != DECOR.SPACEARTIFACT.TIER_NONE))
				{
					num2 -= tuple.second;
				}
				if (num2 <= 0f)
				{
					return tuple.first;
				}
			}
		}
		return DECOR.SPACEARTIFACT.TIER0;
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000BCEA0 File Offset: 0x000BB0A0
	public List<string> GetArtifactsOfTier(ArtifactTier tier)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<ArtifactType, List<string>> keyValuePair in ArtifactConfig.artifactItems)
		{
			foreach (string text in keyValuePair.Value)
			{
				if (Assets.GetPrefab(text.ToTag()).GetComponent<SpaceArtifact>().GetArtifactTier() == tier)
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000BCF50 File Offset: 0x000BB150
	public string SearchForArtifact(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactTier artifactDropTier = this.GetArtifactDropTier(minionID, destination);
		if (artifactDropTier == DECOR.SPACEARTIFACT.TIER_NONE)
		{
			return null;
		}
		List<string> artifactsOfTier = this.GetArtifactsOfTier(artifactDropTier);
		return artifactsOfTier[UnityEngine.Random.Range(0, artifactsOfTier.Count)];
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000BCF8C File Offset: 0x000BB18C
	public void OnLand(object data)
	{
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>()));
		foreach (MinionStorage.Info info in this.minionStorage.GetStoredMinionInfo())
		{
			StoredMinionIdentity minionID = info.serializedMinion.Get<StoredMinionIdentity>();
			string text = this.SearchForArtifact(minionID, spacecraftDestination);
			if (text != null)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(text.ToTag()), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
			}
		}
	}

	// Token: 0x040013A1 RID: 5025
	public const string ID = "ArtifactFinder";

	// Token: 0x040013A2 RID: 5026
	[MyCmpReq]
	private MinionStorage minionStorage;

	// Token: 0x040013A3 RID: 5027
	private static readonly EventSystem.IntraObjectHandler<ArtifactFinder> OnLandDelegate = new EventSystem.IntraObjectHandler<ArtifactFinder>(delegate(ArtifactFinder component, object data)
	{
		component.OnLand(data);
	});
}
