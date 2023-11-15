using System;
using TUNING;
using UnityEngine;

// Token: 0x02000680 RID: 1664
[AddComponentMenu("KMonoBehaviour/scripts/ResearchModule")]
public class ResearchModule : KMonoBehaviour
{
	// Token: 0x06002C70 RID: 11376 RVA: 0x000EC690 File Offset: 0x000EA890
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<ResearchModule>(-1277991738, ResearchModule.OnLaunchDelegate);
		base.Subscribe<ResearchModule>(-887025858, ResearchModule.OnLandDelegate);
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000EC6E5 File Offset: 0x000EA8E5
	public void OnLaunch(object data)
	{
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000EC6E8 File Offset: 0x000EA8E8
	public void OnLand(object data)
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			SpaceDestination.ResearchOpportunity researchOpportunity = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>())).TryCompleteResearchOpportunity();
			if (researchOpportunity != null)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.SetActive(true);
				gameObject.GetComponent<PrimaryElement>().Mass = (float)researchOpportunity.dataValue;
				if (!string.IsNullOrEmpty(researchOpportunity.discoveredRareItem))
				{
					GameObject prefab = Assets.GetPrefab(researchOpportunity.discoveredRareItem);
					if (prefab == null)
					{
						KCrashReporter.Assert(false, "Missing prefab: " + researchOpportunity.discoveredRareItem);
					}
					else
					{
						GameUtil.KInstantiate(prefab, base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
					}
				}
			}
		}
		GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<PrimaryElement>().Mass = (float)ROCKETRY.DESTINATION_RESEARCH.EVERGREEN;
	}

	// Token: 0x04001A2F RID: 6703
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04001A30 RID: 6704
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLandDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLand(data);
	});
}
