﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x020007B7 RID: 1975
public static class Expectations
{
	// Token: 0x060036BA RID: 14010 RVA: 0x001275D0 File Offset: 0x001257D0
	private static AttributeModifier QOLModifier(int level)
	{
		return new AttributeModifier(Db.Get().Attributes.QualityOfLifeExpectation.Id, (float)level, DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x001275FA File Offset: 0x001257FA
	private static AttributeModifierExpectation QOLExpectation(int level, string name, string description)
	{
		return new AttributeModifierExpectation("QOL_" + level.ToString(), name, description, Expectations.QOLModifier(level), Assets.GetSprite("icon_category_morale"));
	}

	// Token: 0x040021A1 RID: 8609
	public static List<Expectation[]> ExpectationsByTier = new List<Expectation[]>
	{
		new Expectation[]
		{
			Expectations.QOLExpectation(1, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER0.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER0.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(2, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER1.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER1.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(4, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER2.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER2.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(8, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER3.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER3.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(12, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER4.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER4.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(16, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER5.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER5.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(20, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER6.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER6.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(25, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER7.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER7.DESCRIPTION)
		},
		new Expectation[]
		{
			Expectations.QOLExpectation(30, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER8.NAME, UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER8.DESCRIPTION)
		}
	};
}
