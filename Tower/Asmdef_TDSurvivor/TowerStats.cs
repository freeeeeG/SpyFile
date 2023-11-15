using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000125 RID: 293
[Serializable]
public class TowerStats
{
	// Token: 0x0600077E RID: 1918 RVA: 0x0001C645 File Offset: 0x0001A845
	public TowerStats()
	{
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0001C650 File Offset: 0x0001A850
	public TowerStats(TowerStats copyFrom)
	{
		this.StatType = copyFrom.StatType;
		this.BaseValue = copyFrom.BaseValue;
		this.list_Modifiers = new List<StatModifier>();
		for (int i = 0; i < copyFrom.list_Modifiers.Count; i++)
		{
			this.list_Modifiers.Add(new StatModifier(copyFrom.list_Modifiers[i].ModifierType, copyFrom.list_Modifiers[i].Value));
		}
		this.LocKey_description = copyFrom.LocKey_description;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0001C6DC File Offset: 0x0001A8DC
	public void OverrideByMultiplier(float multiplier)
	{
		foreach (StatModifier statModifier in this.list_Modifiers)
		{
			if (statModifier.ModifierType == eModifierType.ADD)
			{
				statModifier.Value *= multiplier;
			}
			else if (statModifier.ModifierType == eModifierType.MULTIPLY)
			{
				statModifier.Value = (statModifier.Value - 1f) * multiplier + 1f;
			}
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0001C764 File Offset: 0x0001A964
	public virtual string GetFinalValueText_Combined()
	{
		return Mathf.CeilToInt(this.FinalValue).ToString();
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0001C784 File Offset: 0x0001A984
	public virtual string GetFinalValueText_Detailed()
	{
		string text = this.BaseValue.ToString() ?? "";
		for (int i = 0; i < this.list_Modifiers.Count; i++)
		{
			StatModifier statModifier = this.list_Modifiers[i];
			if (statModifier.ModifierType == eModifierType.ADD)
			{
				text += "+";
			}
			else if (statModifier.ModifierType == eModifierType.MULTIPLY)
			{
				text += "x";
			}
			text += string.Format("{0}", statModifier.Value);
		}
		return string.Format("<color=#33cc33>{0}</color> ({1})", Mathf.CeilToInt(this.FinalValue), text);
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001C834 File Offset: 0x0001AA34
	public float FinalValue
	{
		get
		{
			this.list_Modifiers = (from mod in this.list_Modifiers
			orderby mod.ModifierType
			select mod).ToList<StatModifier>();
			float num = this.BaseValue;
			if (this.list_Modifiers.Count > 0)
			{
				foreach (StatModifier statModifier in this.list_Modifiers)
				{
					num = statModifier.ApplyModifier(num);
				}
			}
			return num;
		}
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
	public void AddModifier(StatModifier modifier)
	{
		if (this.list_Modifiers == null)
		{
			this.list_Modifiers = new List<StatModifier>();
		}
		this.list_Modifiers.Add(modifier);
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0001C8F5 File Offset: 0x0001AAF5
	public void RemoveModifier(StatModifier modifier)
	{
		if (this.list_Modifiers == null)
		{
			this.list_Modifiers = new List<StatModifier>();
		}
		if (this.list_Modifiers.Contains(modifier))
		{
			this.list_Modifiers.Remove(modifier);
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0001C928 File Offset: 0x0001AB28
	public string GetSingleModifierValueLocString(bool isPercentage = false)
	{
		if (this.list_Modifiers == null)
		{
			this.list_Modifiers = new List<StatModifier>();
		}
		if (this.list_Modifiers.Count == 0)
		{
			return "";
		}
		float num = this.list_Modifiers[0].Value;
		if (this.list_Modifiers[0].ModifierType == eModifierType.MULTIPLY && isPercentage)
		{
			num *= 100f;
		}
		int num2 = Mathf.RoundToInt(num);
		eModifierType modifierType = this.list_Modifiers[0].ModifierType;
		if (modifierType == eModifierType.ADD)
		{
			return string.Format("<color=#33cc33>{0}</color>", num2);
		}
		if (modifierType != eModifierType.MULTIPLY)
		{
			return "";
		}
		return string.Format("<color=#33cc33>{0}</color>", num2);
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001C9D6 File Offset: 0x0001ABD6
	public bool IsModified
	{
		get
		{
			return this.list_Modifiers.Count != 0;
		}
	}

	// Token: 0x04000614 RID: 1556
	public eStatType StatType;

	// Token: 0x04000615 RID: 1557
	[FormerlySerializedAs("Value")]
	public float BaseValue;

	// Token: 0x04000616 RID: 1558
	public List<StatModifier> list_Modifiers;

	// Token: 0x04000617 RID: 1559
	public string LocKey_description;
}
