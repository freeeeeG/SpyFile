using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000844 RID: 2116
[Serializable]
public class LocString
{
	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06003D9B RID: 15771 RVA: 0x00156A70 File Offset: 0x00154C70
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06003D9C RID: 15772 RVA: 0x00156A78 File Offset: 0x00154C78
	public StringKey key
	{
		get
		{
			return this._key;
		}
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x00156A80 File Offset: 0x00154C80
	public LocString(string text)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x00156A9B File Offset: 0x00154C9B
	public LocString(string text, string keystring)
	{
		this._text = text;
		this._key = new StringKey(keystring);
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x00156AB6 File Offset: 0x00154CB6
	public LocString(string text, bool isLocalized)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x00156AD1 File Offset: 0x00154CD1
	public static implicit operator LocString(string text)
	{
		return new LocString(text);
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x00156AD9 File Offset: 0x00154CD9
	public static implicit operator string(LocString loc_string)
	{
		return loc_string.text;
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x00156AE1 File Offset: 0x00154CE1
	public override string ToString()
	{
		return Strings.Get(this.key).String;
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x00156AF3 File Offset: 0x00154CF3
	public void SetKey(string key_name)
	{
		this._key = new StringKey(key_name);
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x00156B01 File Offset: 0x00154D01
	public void SetKey(StringKey key)
	{
		this._key = key;
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x00156B0A File Offset: 0x00154D0A
	public string Replace(string search, string replacement)
	{
		return this.ToString().Replace(search, replacement);
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x00156B1C File Offset: 0x00154D1C
	public static void CreateLocStringKeys(Type type, string parent_path = "STRINGS.")
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		string text = parent_path;
		if (text == null)
		{
			text = "";
		}
		text = text + type.Name + ".";
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + parent_path);
				}
				else
				{
					string text2 = text + fieldInfo.Name;
					LocString locString = (LocString)fieldInfo.GetValue(null);
					locString.SetKey(text2);
					string text3 = locString.text;
					Strings.Add(new string[]
					{
						text2,
						text3
					});
					fieldInfo.SetValue(null, locString);
				}
			}
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < nestedTypes.Length; i++)
		{
			LocString.CreateLocStringKeys(nestedTypes[i], text);
		}
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00156C08 File Offset: 0x00154E08
	public static string[] GetStrings(Type type)
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < fields.Length; i++)
		{
			LocString locString = (LocString)fields[i].GetValue(null);
			list.Add(locString.text);
		}
		return list.ToArray();
	}

	// Token: 0x04002823 RID: 10275
	[SerializeField]
	private string _text;

	// Token: 0x04002824 RID: 10276
	[SerializeField]
	private StringKey _key;

	// Token: 0x04002825 RID: 10277
	public const BindingFlags data_member_fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
}
