﻿using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000DDF RID: 3551
	public class HeatRash : Sickness
	{
		// Token: 0x06006D59 RID: 27993 RVA: 0x002B165C File Offset: 0x002AF85C
		public HeatRash() : base("HeatSickness", Sickness.SicknessType.Ailment, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 180f, null)
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier("Learning", -5f, DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true),
				new AttributeModifier("Machinery", -5f, DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true),
				new AttributeModifier("Construction", -5f, DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true),
				new AttributeModifier("Cooking", -5f, DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_hot_kanim",
				"anim_loco_run_hot_kanim",
				"anim_loco_walk_hot_kanim"
			}, Db.Get().Expressions.SickFierySkin));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Hot, 15f));
		}

		// Token: 0x040051FF RID: 20991
		public const string ID = "HeatSickness";
	}
}
