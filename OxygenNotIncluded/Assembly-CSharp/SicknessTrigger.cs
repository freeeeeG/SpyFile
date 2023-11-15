﻿using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using UnityEngine;

// Token: 0x0200096D RID: 2413
[AddComponentMenu("KMonoBehaviour/scripts/SicknessTrigger")]
public class SicknessTrigger : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060046C3 RID: 18115 RVA: 0x0018F790 File Offset: 0x0018D990
	public void AddTrigger(GameHashes src_event, string[] sickness_ids, SicknessTrigger.SourceCallback source_callback)
	{
		this.triggers.Add(new SicknessTrigger.TriggerInfo
		{
			srcEvent = src_event,
			sickness_ids = sickness_ids,
			sourceCallback = source_callback
		});
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x0018F7CC File Offset: 0x0018D9CC
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			SicknessTrigger.TriggerInfo trigger = this.triggers[i];
			base.Subscribe((int)trigger.srcEvent, delegate(object data)
			{
				this.OnSicknessTrigger((GameObject)data, trigger);
			});
		}
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x0018F82C File Offset: 0x0018DA2C
	private void OnSicknessTrigger(GameObject target, SicknessTrigger.TriggerInfo trigger)
	{
		int num = UnityEngine.Random.Range(0, trigger.sickness_ids.Length);
		string text = trigger.sickness_ids[num];
		Sickness sickness = null;
		Database.Sicknesses sicknesses = Db.Get().Sicknesses;
		for (int i = 0; i < sicknesses.Count; i++)
		{
			if (sicknesses[i].Id == text)
			{
				sickness = sicknesses[i];
				break;
			}
		}
		if (sickness != null)
		{
			string infection_source_info = trigger.sourceCallback(base.gameObject, target);
			SicknessExposureInfo exposure_info = new SicknessExposureInfo(sickness.Id, infection_source_info);
			target.GetComponent<MinionModifiers>().sicknesses.Infect(exposure_info);
			return;
		}
		DebugUtil.DevLogErrorFormat(base.gameObject, "Couldn't find sickness with id [{0}]", new object[]
		{
			text
		});
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x0018F8E8 File Offset: 0x0018DAE8
	public List<Descriptor> EffectDescriptors(GameObject go)
	{
		Dictionary<GameHashes, HashSet<string>> dictionary = new Dictionary<GameHashes, HashSet<string>>();
		foreach (SicknessTrigger.TriggerInfo triggerInfo in this.triggers)
		{
			HashSet<string> hashSet = null;
			if (!dictionary.TryGetValue(triggerInfo.srcEvent, out hashSet))
			{
				hashSet = new HashSet<string>();
				dictionary[triggerInfo.srcEvent] = hashSet;
			}
			foreach (string item in triggerInfo.sickness_ids)
			{
				hashSet.Add(item);
			}
		}
		List<Descriptor> list = new List<Descriptor>();
		List<string> list2 = new List<string>();
		string properName = base.GetComponent<KSelectable>().GetProperName();
		foreach (KeyValuePair<GameHashes, HashSet<string>> keyValuePair in dictionary)
		{
			HashSet<string> value = keyValuePair.Value;
			list2.Clear();
			foreach (string id in value)
			{
				Sickness sickness = Db.Get().Sicknesses.TryGet(id);
				list2.Add(sickness.Name);
			}
			string newValue = string.Join(", ", list2.ToArray());
			string text = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS." + Enum.GetName(typeof(GameHashes), keyValuePair.Key).ToUpper()).String;
			string text2 = Strings.Get("STRINGS.DUPLICANTS.DISEASES.TRIGGERS.TOOLTIPS." + Enum.GetName(typeof(GameHashes), keyValuePair.Key).ToUpper()).String;
			text = text.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
			text2 = text2.Replace("{ItemName}", properName).Replace("{Diseases}", newValue);
			list.Add(new Descriptor(text, text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x060046C7 RID: 18119 RVA: 0x0018FB38 File Offset: 0x0018DD38
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.EffectDescriptors(go);
	}

	// Token: 0x04002EEB RID: 12011
	public List<SicknessTrigger.TriggerInfo> triggers = new List<SicknessTrigger.TriggerInfo>();

	// Token: 0x020017CD RID: 6093
	// (Invoke) Token: 0x06008F81 RID: 36737
	public delegate string SourceCallback(GameObject source, GameObject target);

	// Token: 0x020017CE RID: 6094
	[Serializable]
	public struct TriggerInfo
	{
		// Token: 0x04006FF0 RID: 28656
		[HashedEnum]
		public GameHashes srcEvent;

		// Token: 0x04006FF1 RID: 28657
		public string[] sickness_ids;

		// Token: 0x04006FF2 RID: 28658
		public SicknessTrigger.SourceCallback sourceCallback;
	}
}
