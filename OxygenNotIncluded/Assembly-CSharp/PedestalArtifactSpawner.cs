using System;
using KSerialization;
using UnityEngine;

// Token: 0x020008D7 RID: 2263
public class PedestalArtifactSpawner : KMonoBehaviour
{
	// Token: 0x06004175 RID: 16757 RVA: 0x0016E6B0 File Offset: 0x0016C8B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject gameObject in this.storage.items)
		{
			if (ArtifactSelector.Instance.GetArtifactType(gameObject.name) == ArtifactType.Terrestrial)
			{
				gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
			}
		}
		if (this.artifactSpawned)
		{
			return;
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Terrestrial)), base.transform.position);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
		this.storage.Store(gameObject2, false, false, true, false);
		this.receptacle.ForceDeposit(gameObject2);
		this.artifactSpawned = true;
	}

	// Token: 0x04002A99 RID: 10905
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002A9A RID: 10906
	[MyCmpReq]
	private SingleEntityReceptacle receptacle;

	// Token: 0x04002A9B RID: 10907
	[Serialize]
	private bool artifactSpawned;
}
