using System;
using Klei.AI;

// Token: 0x020008AC RID: 2220
public abstract class Need : KMonoBehaviour
{
	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x0600405E RID: 16478 RVA: 0x00168963 File Offset: 0x00166B63
	// (set) Token: 0x0600405F RID: 16479 RVA: 0x0016896B File Offset: 0x00166B6B
	public string Name { get; protected set; }

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06004060 RID: 16480 RVA: 0x00168974 File Offset: 0x00166B74
	// (set) Token: 0x06004061 RID: 16481 RVA: 0x0016897C File Offset: 0x00166B7C
	public string ExpectationTooltip { get; protected set; }

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06004062 RID: 16482 RVA: 0x00168985 File Offset: 0x00166B85
	// (set) Token: 0x06004063 RID: 16483 RVA: 0x0016898D File Offset: 0x00166B8D
	public string Tooltip { get; protected set; }

	// Token: 0x06004064 RID: 16484 RVA: 0x00168996 File Offset: 0x00166B96
	public Klei.AI.Attribute GetExpectationAttribute()
	{
		return this.expectationAttribute.Attribute;
	}

	// Token: 0x06004065 RID: 16485 RVA: 0x001689A3 File Offset: 0x00166BA3
	protected void SetModifier(Need.ModifierType modifier)
	{
		if (this.currentStressModifier != modifier)
		{
			if (this.currentStressModifier != null)
			{
				this.UnapplyModifier(this.currentStressModifier);
			}
			if (modifier != null)
			{
				this.ApplyModifier(modifier);
			}
			this.currentStressModifier = modifier;
		}
	}

	// Token: 0x06004066 RID: 16486 RVA: 0x001689D4 File Offset: 0x00166BD4
	private void ApplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Add(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().AddStatusItem(modifier.statusItem, null);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().AddThought(modifier.thought);
		}
	}

	// Token: 0x06004067 RID: 16487 RVA: 0x00168A30 File Offset: 0x00166C30
	private void UnapplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Remove(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(modifier.statusItem, false);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().RemoveThought(modifier.thought);
		}
	}

	// Token: 0x04002A03 RID: 10755
	protected AttributeInstance expectationAttribute;

	// Token: 0x04002A04 RID: 10756
	protected Need.ModifierType stressBonus;

	// Token: 0x04002A05 RID: 10757
	protected Need.ModifierType stressNeutral;

	// Token: 0x04002A06 RID: 10758
	protected Need.ModifierType stressPenalty;

	// Token: 0x04002A07 RID: 10759
	protected Need.ModifierType currentStressModifier;

	// Token: 0x020016EE RID: 5870
	protected class ModifierType
	{
		// Token: 0x04006D75 RID: 28021
		public AttributeModifier modifier;

		// Token: 0x04006D76 RID: 28022
		public StatusItem statusItem;

		// Token: 0x04006D77 RID: 28023
		public Thought thought;
	}
}
