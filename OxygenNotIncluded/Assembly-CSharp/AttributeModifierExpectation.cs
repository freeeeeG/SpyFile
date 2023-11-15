using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
public class AttributeModifierExpectation : Expectation
{
	// Token: 0x060036B9 RID: 14009 RVA: 0x00127580 File Offset: 0x00125780
	public AttributeModifierExpectation(string id, string name, string description, AttributeModifier modifier, Sprite icon) : base(id, name, description, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Add(modifier);
	}, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier);
	})
	{
		this.modifier = modifier;
		this.icon = icon;
	}

	// Token: 0x0400219F RID: 8607
	public AttributeModifier modifier;

	// Token: 0x040021A0 RID: 8608
	public Sprite icon;
}
