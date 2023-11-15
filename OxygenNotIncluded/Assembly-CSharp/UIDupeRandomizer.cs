using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000C81 RID: 3201
public class UIDupeRandomizer : MonoBehaviour
{
	// Token: 0x0600660E RID: 26126 RVA: 0x0026136C File Offset: 0x0025F56C
	protected virtual void Start()
	{
		this.slots = Db.Get().AccessorySlots;
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.anims[i].curBody = null;
			this.GetNewBody(i);
		}
	}

	// Token: 0x0600660F RID: 26127 RVA: 0x002613B8 File Offset: 0x0025F5B8
	protected void GetNewBody(int minion_idx)
	{
		Personality random = Db.Get().Personalities.GetRandom(true, false);
		foreach (KBatchedAnimController dupe in this.anims[minion_idx].minions)
		{
			this.Apply(dupe, random);
		}
	}

	// Token: 0x06006610 RID: 26128 RVA: 0x0026142C File Offset: 0x0025F62C
	private void Apply(KBatchedAnimController dupe, Personality personality)
	{
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
		SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Hair.Lookup(bodyData.hair));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Eyes.Lookup(bodyData.eyes));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HeadShape.Lookup(bodyData.headShape));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Mouth.Lookup(bodyData.mouth));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Body.Lookup(bodyData.body));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Arm.Lookup(bodyData.arms));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLower.Lookup(bodyData.armslower));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Belt.Lookup(bodyData.belt));
		if (this.applySuit && UnityEngine.Random.value < 0.15f)
		{
			component.AddBuildOverride(Assets.GetAnim("body_oxygen_kanim").GetData(), 6);
			dupe.SetSymbolVisiblity("snapto_neck", true);
			dupe.SetSymbolVisiblity("belt", false);
		}
		else
		{
			dupe.SetSymbolVisiblity("snapto_neck", false);
		}
		if (this.applyHat && UnityEngine.Random.value < 0.5f)
		{
			List<string> list = new List<string>();
			foreach (Skill skill in Db.Get().Skills.resources)
			{
				list.Add(skill.hat);
			}
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			UIDupeRandomizer.AddAccessory(dupe, this.slots.Hat.Lookup(id));
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		}
		else
		{
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
		}
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, false);
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, false);
	}

	// Token: 0x06006611 RID: 26129 RVA: 0x0026172C File Offset: 0x0025F92C
	public static KAnimHashedString AddAccessory(KBatchedAnimController minion, Accessory accessory)
	{
		if (accessory != null)
		{
			SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, minion.name + " is missing symbol override controller");
			component.TryRemoveSymbolOverride(accessory.slot.targetSymbolId, 0);
			component.AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, 0);
			minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, true);
			return accessory.slot.targetSymbolId;
		}
		return HashedString.Invalid;
	}

	// Token: 0x06006612 RID: 26130 RVA: 0x002617BC File Offset: 0x0025F9BC
	public KAnimHashedString AddRandomAccessory(KBatchedAnimController minion, List<Accessory> choices)
	{
		Accessory accessory = choices[UnityEngine.Random.Range(1, choices.Count)];
		return UIDupeRandomizer.AddAccessory(minion, accessory);
	}

	// Token: 0x06006613 RID: 26131 RVA: 0x002617E4 File Offset: 0x0025F9E4
	public void Randomize()
	{
		if (this.slots == null)
		{
			return;
		}
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.GetNewBody(i);
		}
	}

	// Token: 0x06006614 RID: 26132 RVA: 0x00261814 File Offset: 0x0025FA14
	protected virtual void Update()
	{
	}

	// Token: 0x0400464F RID: 17999
	[Tooltip("Enable this to allow for a chance for skill hats to appear")]
	public bool applyHat = true;

	// Token: 0x04004650 RID: 18000
	[Tooltip("Enable this to allow for a chance for suit helmets to appear (ie. atmosuit and leadsuit)")]
	public bool applySuit = true;

	// Token: 0x04004651 RID: 18001
	public UIDupeRandomizer.AnimChoice[] anims;

	// Token: 0x04004652 RID: 18002
	private AccessorySlots slots;

	// Token: 0x02001BB2 RID: 7090
	[Serializable]
	public struct AnimChoice
	{
		// Token: 0x04007D98 RID: 32152
		public string anim_name;

		// Token: 0x04007D99 RID: 32153
		public List<KBatchedAnimController> minions;

		// Token: 0x04007D9A RID: 32154
		public float minSecondsBetweenAction;

		// Token: 0x04007D9B RID: 32155
		public float maxSecondsBetweenAction;

		// Token: 0x04007D9C RID: 32156
		public float lastWaitTime;

		// Token: 0x04007D9D RID: 32157
		public KAnimFile curBody;
	}
}
