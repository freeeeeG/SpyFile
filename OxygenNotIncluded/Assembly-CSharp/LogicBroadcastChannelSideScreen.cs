using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C2B RID: 3115
public class LogicBroadcastChannelSideScreen : SideScreenContent
{
	// Token: 0x0600628F RID: 25231 RVA: 0x0024605A File Offset: 0x0024425A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicBroadcastReceiver>() != null;
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x00246068 File Offset: 0x00244268
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicBroadcastReceiver>();
		this.Build();
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x00246084 File Offset: 0x00244284
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<LogicBroadcaster, GameObject> keyValuePair in this.broadcasterRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.broadcasterRows.Clear();
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x00246100 File Offset: 0x00244300
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.HEADER);
		this.ClearRows();
		foreach (object obj in Components.LogicBroadcasters)
		{
			LogicBroadcaster logicBroadcaster = (LogicBroadcaster)obj;
			if (!logicBroadcaster.IsNullOrDestroyed())
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = logicBroadcaster.gameObject.GetProperName();
				global::Debug.Assert(!this.broadcasterRows.ContainsKey(logicBroadcaster), "Adding two of the same broadcaster to LogicBroadcastChannelSideScreen UI: " + logicBroadcaster.gameObject.GetProperName());
				this.broadcasterRows.Add(logicBroadcaster, gameObject);
				gameObject.SetActive(true);
			}
		}
		this.noChannelRow.SetActive(Components.LogicBroadcasters.Count == 0);
		this.Refresh();
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x002461FC File Offset: 0x002443FC
	private void Refresh()
	{
		using (Dictionary<LogicBroadcaster, GameObject>.Enumerator enumerator = this.broadcasterRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<LogicBroadcaster, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.gameObject.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("DistanceLabel").SetText(LogicBroadcastReceiver.CheckRange(this.sensor.gameObject, kvp.Key.gameObject) ? UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.IN_RANGE : UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.OUT_OF_RANGE);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key.gameObject, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key.gameObject, "ui", false).second;
				WorldContainer myWorld = kvp.Key.GetMyWorld();
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").sprite = (myWorld.IsModuleInterior ? Assets.GetSprite("icon_category_rocketry") : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).first);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").color = (myWorld.IsModuleInterior ? Color.white : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).second);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetChannel(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState((this.sensor.GetChannel() == kvp.Key) ? 1 : 0);
			}
		}
	}

	// Token: 0x0400432E RID: 17198
	private LogicBroadcastReceiver sensor;

	// Token: 0x0400432F RID: 17199
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04004330 RID: 17200
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04004331 RID: 17201
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004332 RID: 17202
	[SerializeField]
	private GameObject noChannelRow;

	// Token: 0x04004333 RID: 17203
	private Dictionary<LogicBroadcaster, GameObject> broadcasterRows = new Dictionary<LogicBroadcaster, GameObject>();

	// Token: 0x04004334 RID: 17204
	private GameObject emptySpaceRow;
}
