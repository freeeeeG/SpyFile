﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C2C RID: 3116
public class LureSideScreen : SideScreenContent
{
	// Token: 0x06006295 RID: 25237 RVA: 0x00246493 File Offset: 0x00244693
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CreatureLure>() != null;
	}

	// Token: 0x06006296 RID: 25238 RVA: 0x002464A4 File Offset: 0x002446A4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target_lure = target.GetComponent<CreatureLure>();
		using (List<Tag>.Enumerator enumerator = this.target_lure.baitTypes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag bait = enumerator.Current;
				Tag bait3 = bait;
				if (!this.toggles_by_tag.ContainsKey(bait))
				{
					GameObject gameObject = Util.KInstantiateUI(this.prefab_toggle, this.toggle_container, true);
					Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage");
					gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").text = ElementLoader.GetElement(bait).name;
					reference.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(bait).substance.anim, "ui", false, "");
					MultiToggle component = gameObject.GetComponent<MultiToggle>();
					this.toggles_by_tag.Add(bait3, component);
				}
				this.toggles_by_tag[bait].onClick = delegate()
				{
					Tag bait2 = bait;
					this.SelectToggle(bait2);
				};
			}
		}
		this.RefreshToggles();
	}

	// Token: 0x06006297 RID: 25239 RVA: 0x002465F0 File Offset: 0x002447F0
	public void SelectToggle(Tag tag)
	{
		if (this.target_lure.activeBaitSetting != tag)
		{
			this.target_lure.ChangeBaitSetting(tag);
		}
		else
		{
			this.target_lure.ChangeBaitSetting(Tag.Invalid);
		}
		this.RefreshToggles();
	}

	// Token: 0x06006298 RID: 25240 RVA: 0x0024662C File Offset: 0x0024482C
	private void RefreshToggles()
	{
		foreach (KeyValuePair<Tag, MultiToggle> keyValuePair in this.toggles_by_tag)
		{
			if (this.target_lure.activeBaitSetting == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
			keyValuePair.Value.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.UISIDESCREENS.LURE.ATTRACTS, ElementLoader.GetElement(keyValuePair.Key).name, this.baitAttractionStrings[keyValuePair.Key]));
		}
	}

	// Token: 0x04004335 RID: 17205
	protected CreatureLure target_lure;

	// Token: 0x04004336 RID: 17206
	public GameObject prefab_toggle;

	// Token: 0x04004337 RID: 17207
	public GameObject toggle_container;

	// Token: 0x04004338 RID: 17208
	public Dictionary<Tag, MultiToggle> toggles_by_tag = new Dictionary<Tag, MultiToggle>();

	// Token: 0x04004339 RID: 17209
	private Dictionary<Tag, string> baitAttractionStrings = new Dictionary<Tag, string>
	{
		{
			GameTags.SlimeMold,
			CREATURES.SPECIES.PUFT.NAME
		},
		{
			GameTags.Phosphorite,
			CREATURES.SPECIES.LIGHTBUG.NAME
		}
	};
}
