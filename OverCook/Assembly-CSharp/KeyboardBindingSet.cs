using System;
using System.Collections.Generic;
using InControl;

// Token: 0x020001F3 RID: 499
public class KeyboardBindingSet
{
	// Token: 0x06000847 RID: 2119 RVA: 0x0003216C File Offset: 0x0003056C
	public void CopyFrom(KeyboardBindingSet original)
	{
		this.CopyMap<ControlPadInput.Button>(original.m_ButtonBindings, this.m_ButtonBindings);
		this.CopyMap<ControlPadInput.Value>(original.m_PositiveValueBindings, this.m_PositiveValueBindings);
		this.CopyMap<ControlPadInput.Value>(original.m_NegativeValueBindings, this.m_NegativeValueBindings);
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x000321A4 File Offset: 0x000305A4
	public void Save(GlobalSave saveData, string prefix)
	{
		this.SaveMap<ControlPadInput.Button>(prefix + "_BB", this.m_ButtonBindings, saveData);
		this.SaveMap<ControlPadInput.Value>(prefix + "_PV", this.m_PositiveValueBindings, saveData);
		this.SaveMap<ControlPadInput.Value>(prefix + "_NV", this.m_NegativeValueBindings, saveData);
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x000321FC File Offset: 0x000305FC
	public bool Load(GlobalSave saveData, string prefix)
	{
		bool flag = true;
		flag &= this.LoadMap<ControlPadInput.Button>(prefix + "_BB", this.m_ButtonBindings, saveData);
		flag &= this.LoadMap<ControlPadInput.Value>(prefix + "_PV", this.m_PositiveValueBindings, saveData);
		return flag & this.LoadMap<ControlPadInput.Value>(prefix + "_NV", this.m_NegativeValueBindings, saveData);
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00032260 File Offset: 0x00030660
	public void CopyMap<T>(Dictionary<T, List<Key>> from, Dictionary<T, List<Key>> to)
	{
		to.Clear();
		foreach (KeyValuePair<T, List<Key>> keyValuePair in from)
		{
			List<Key> list = new List<Key>();
			foreach (Key item in keyValuePair.Value)
			{
				list.Add(item);
			}
			to.Add(keyValuePair.Key, list);
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00032318 File Offset: 0x00030718
	public void SaveMap<T>(string name, Dictionary<T, List<Key>> map, GlobalSave saveData)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (KeyValuePair<T, List<Key>> keyValuePair in map)
		{
			string text = string.Empty;
			foreach (Key key in keyValuePair.Value)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				text += key.ToString();
			}
			Dictionary<string, string> dictionary2 = dictionary;
			T key2 = keyValuePair.Key;
			dictionary2.Add(key2.ToString(), text);
		}
		saveData.Set(name, dictionary);
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0003240C File Offset: 0x0003080C
	public bool LoadMap<T>(string name, Dictionary<T, List<Key>> map, GlobalSave saveData)
	{
		map.Clear();
		Dictionary<string, string> dictionary;
		if (saveData.Get(name, out dictionary, null))
		{
			try
			{
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					List<Key> value = new List<Key>();
					if (!string.IsNullOrEmpty(keyValuePair.Value))
					{
						List<string> list = new List<string>(keyValuePair.Value.Split(new char[]
						{
							','
						}));
						value = list.ConvertAll<Key>((string x) => (Key)Enum.Parse(typeof(Key), x));
					}
					map.Add((T)((object)Enum.Parse(typeof(T), keyValuePair.Key)), value);
				}
				return true;
			}
			catch
			{
				return false;
			}
			return false;
		}
		return false;
	}

	// Token: 0x04000703 RID: 1795
	public Dictionary<ControlPadInput.Button, List<Key>> m_ButtonBindings = new Dictionary<ControlPadInput.Button, List<Key>>();

	// Token: 0x04000704 RID: 1796
	public Dictionary<ControlPadInput.Value, List<Key>> m_PositiveValueBindings = new Dictionary<ControlPadInput.Value, List<Key>>();

	// Token: 0x04000705 RID: 1797
	public Dictionary<ControlPadInput.Value, List<Key>> m_NegativeValueBindings = new Dictionary<ControlPadInput.Value, List<Key>>();
}
