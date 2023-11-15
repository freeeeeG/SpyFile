﻿using System;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
public class KleiItemDropScreen_PermitVis_DupeEquipment : KMonoBehaviour
{
	// Token: 0x060058BB RID: 22715 RVA: 0x00208140 File Offset: 0x00206340
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.dupeKAnim.GetComponent<UIDupeRandomizer>().Randomize();
		KAnimFile anim = Assets.GetAnim(info.BuildOverride);
		this.dupeKAnim.AddAnimOverrides(anim, 0f);
		KAnimHashedString kanimHashedString = new KAnimHashedString("snapto_neck");
		KAnim.Build.Symbol symbol = anim.GetData().build.GetSymbol(kanimHashedString);
		if (symbol != null)
		{
			this.dupeKAnim.GetComponent<SymbolOverrideController>().AddSymbolOverride(kanimHashedString, symbol, 6);
			this.dupeKAnim.SetSymbolVisiblity(kanimHashedString, true);
		}
		else
		{
			this.dupeKAnim.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(kanimHashedString, 6);
			this.dupeKAnim.SetSymbolVisiblity(kanimHashedString, false);
		}
		this.dupeKAnim.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.dupeKAnim.Queue("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.dupeKAnim.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.dupeKAnim.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.dupeKAnim.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x04003C0E RID: 15374
	[SerializeField]
	private KBatchedAnimController droppedItemKAnim;

	// Token: 0x04003C0F RID: 15375
	[SerializeField]
	private KBatchedAnimController dupeKAnim;
}
