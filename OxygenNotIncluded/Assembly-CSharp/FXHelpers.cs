﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public static class FXHelpers
{
	// Token: 0x0600220A RID: 8714 RVA: 0x000BACF0 File Offset: 0x000B8EF0
	public static KBatchedAnimController CreateEffect(string anim_file_name, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateId), position, layer, null, 0).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_name);
		component.name = anim_file_name;
		if (parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		KAnimFile anim = Assets.GetAnim(anim_file_name);
		if (anim == null)
		{
			global::Debug.LogWarning("Missing effect anim: " + anim_file_name);
		}
		else
		{
			component.AnimFiles = new KAnimFile[]
			{
				anim
			};
		}
		if (!set_inactive)
		{
			component.gameObject.SetActive(true);
		}
		return component;
	}

	// Token: 0x0600220B RID: 8715 RVA: 0x000BADB0 File Offset: 0x000B8FB0
	public static KBatchedAnimController CreateEffect(string[] anim_file_names, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateId), position, layer, null, 0).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
		component.name = anim_file_names[0];
		if (parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		component.AnimFiles = (from e in (from name in anim_file_names
		select new ValueTuple<string, KAnimFile>(name, Assets.GetAnim(name))).Where(delegate([TupleElementNames(new string[]
		{
			"name",
			"anim"
		})] ValueTuple<string, KAnimFile> e)
		{
			if (e.Item2 == null)
			{
				global::Debug.LogWarning("Missing effect anim: " + e.Item1);
				return false;
			}
			return true;
		})
		select e.Item2).ToArray<KAnimFile>();
		if (!set_inactive)
		{
			component.gameObject.SetActive(true);
		}
		return component;
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000BAEB4 File Offset: 0x000B90B4
	public static KBatchedAnimController CreateEffectOverride(string[] anim_file_names, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateOverrideId), position, layer, null, 0).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
		component.name = anim_file_names[0];
		if (parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		component.AnimFiles = (from e in (from name in anim_file_names
		select new ValueTuple<string, KAnimFile>(name, Assets.GetAnim(name))).Where(delegate([TupleElementNames(new string[]
		{
			"name",
			"anim"
		})] ValueTuple<string, KAnimFile> e)
		{
			if (e.Item2 == null)
			{
				global::Debug.LogWarning("Missing effect anim: " + e.Item1);
				return false;
			}
			return true;
		})
		select e.Item2).ToArray<KAnimFile>();
		if (!set_inactive)
		{
			component.gameObject.SetActive(true);
		}
		return component;
	}
}
