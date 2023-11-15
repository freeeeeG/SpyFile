using System;
using UnityEngine;

// Token: 0x02000A35 RID: 2613
public abstract class Option<T> : INameListOption, IOption where T : struct, IConvertible, IComparable
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x060033A5 RID: 13221
	public abstract string Label { get; }

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060033A6 RID: 13222
	public abstract OptionsData.Categories Category { get; }

	// Token: 0x060033A7 RID: 13223 RVA: 0x000F2F78 File Offset: 0x000F1378
	public string[] GetNames()
	{
		T[] array = Enum.GetValues(typeof(T)) as T[];
		return array.ConvertAll((T x) => "OptionValue." + x.ToString());
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x000F2FC0 File Offset: 0x000F13C0
	public void SetOption(int _value)
	{
		T[] array = Enum.GetValues(typeof(T)) as T[];
		this.SetState(array[Mathf.Clamp(_value, 0, array.Length - 1)]);
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x000F2FFC File Offset: 0x000F13FC
	public int GetOption()
	{
		T[] array = Enum.GetValues(typeof(T)) as T[];
		T state = this.GetState();
		return array.FindIndex_Predicate((T x) => x.Equals(state));
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x000F3042 File Offset: 0x000F1442
	protected virtual string ToString(T _value)
	{
		return _value.ToString();
	}

	// Token: 0x060033AB RID: 13227
	protected abstract T GetState();

	// Token: 0x060033AC RID: 13228
	protected abstract void SetState(T _state);

	// Token: 0x060033AD RID: 13229
	public abstract void Commit();
}
