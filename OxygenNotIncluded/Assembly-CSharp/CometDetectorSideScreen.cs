using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C08 RID: 3080
public class CometDetectorSideScreen : SideScreenContent
{
	// Token: 0x06006187 RID: 24967 RVA: 0x0023FD3C File Offset: 0x0023DF3C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x06006188 RID: 24968 RVA: 0x0023FD50 File Offset: 0x0023DF50
	private void RefreshOptions()
	{
		if (this.clusterDetector != null)
		{
			int num = 0;
			this.SetClusterRow(num++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite("meteors"), ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower, null);
			this.SetClusterRow(num++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.DUPEMADE, Assets.GetSprite("dupe_made_ballistics"), ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject, null);
			foreach (object obj in Components.Clustercrafts)
			{
				Clustercraft clustercraft = (Clustercraft)obj;
				this.SetClusterRow(num++, clustercraft.Name, Assets.GetSprite("rocket_landing"), ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket, clustercraft);
			}
			for (int i = num; i < this.rowContainer.childCount; i++)
			{
				this.rowContainer.GetChild(i).gameObject.SetActive(false);
			}
			return;
		}
		int num2 = 0;
		this.SetRow(num2++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite("meteors"), null);
		foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
		{
			this.SetRow(num2++, spacecraft.GetRocketName(), Assets.GetSprite("rocket_landing"), spacecraft.launchConditions);
		}
		for (int j = num2; j < this.rowContainer.childCount; j++)
		{
			this.rowContainer.GetChild(j).gameObject.SetActive(false);
		}
	}

	// Token: 0x06006189 RID: 24969 RVA: 0x0023FF18 File Offset: 0x0023E118
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600618A RID: 24970 RVA: 0x0023FF59 File Offset: 0x0023E159
	public override void SetTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			this.clusterDetector = target.GetSMI<ClusterCometDetector.Instance>();
		}
		else
		{
			this.detector = target.GetSMI<CometDetector.Instance>();
		}
		this.RefreshOptions();
	}

	// Token: 0x0600618B RID: 24971 RVA: 0x0023FF84 File Offset: 0x0023E184
	private void SetClusterRow(int idx, string name, Sprite icon, ClusterCometDetector.Instance.ClusterCometDetectorState state, Clustercraft rocketTarget = null)
	{
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").text = name;
		component.GetReference<Image>("icon").sprite = icon;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.clusterDetector.GetDetectorState() == state && this.clusterDetector.GetClustercraftTarget() == rocketTarget) ? 1 : 0);
		ClusterCometDetector.Instance.ClusterCometDetectorState _state = state;
		Clustercraft _rocketTarget = rocketTarget;
		component2.onClick = delegate()
		{
			this.clusterDetector.SetDetectorState(_state);
			this.clusterDetector.SetClustercraftTarget(_rocketTarget);
			this.RefreshOptions();
		};
	}

	// Token: 0x0600618C RID: 24972 RVA: 0x00240054 File Offset: 0x0023E254
	private void SetRow(int idx, string name, Sprite icon, LaunchConditionManager target)
	{
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").text = name;
		component.GetReference<Image>("icon").sprite = icon;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.detector.GetTargetCraft() == target) ? 1 : 0);
		LaunchConditionManager _target = target;
		component2.onClick = delegate()
		{
			this.detector.SetTargetCraft(_target);
			this.RefreshOptions();
		};
	}

	// Token: 0x0600618D RID: 24973 RVA: 0x0024010C File Offset: 0x0023E30C
	public override bool IsValidForTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			return target.GetSMI<ClusterCometDetector.Instance>() != null;
		}
		return target.GetSMI<CometDetector.Instance>() != null;
	}

	// Token: 0x04004267 RID: 16999
	private CometDetector.Instance detector;

	// Token: 0x04004268 RID: 17000
	private ClusterCometDetector.Instance clusterDetector;

	// Token: 0x04004269 RID: 17001
	public GameObject rowPrefab;

	// Token: 0x0400426A RID: 17002
	public RectTransform rowContainer;

	// Token: 0x0400426B RID: 17003
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();
}
