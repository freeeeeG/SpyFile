﻿using System;
using System.Collections.Generic;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02000DDA RID: 3546
	public class Allergies : Sickness
	{
		// Token: 0x06006D41 RID: 27969 RVA: 0x002B00A8 File Offset: 0x002AE2A8
		public Allergies() : base("Allergies", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 60f, null)
		{
			float value = 0.025f;
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_allergies_kanim"
			}, Db.Get().Expressions.Pollen));
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, value, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true),
				new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 10f, DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true)
			}));
		}

		// Token: 0x040051E4 RID: 20964
		public const string ID = "Allergies";

		// Token: 0x040051E5 RID: 20965
		public const float STRESS_PER_CYCLE = 15f;
	}
}
