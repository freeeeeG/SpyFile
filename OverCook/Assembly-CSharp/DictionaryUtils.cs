using System;
using System.Collections.Generic;

// Token: 0x0200021D RID: 541
public static class DictionaryUtils
{
	// Token: 0x0600091B RID: 2331 RVA: 0x00035E0C File Offset: 0x0003420C
	public static Dictionary<K, U> ConvertValues<K, U, T>(this Dictionary<K, T> _input, Converter<T, U> _converter)
	{
		Dictionary<K, U> dictionary = new Dictionary<K, U>();
		foreach (KeyValuePair<K, T> keyValuePair in _input)
		{
			dictionary.Add(keyValuePair.Key, _converter(keyValuePair.Value));
		}
		return dictionary;
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00035E80 File Offset: 0x00034280
	public static void RemoveAll<K, T>(this Dictionary<K, T> _input, Generic<bool, KeyValuePair<K, T>> _shouldRemove)
	{
		List<K> list = new List<K>();
		foreach (KeyValuePair<K, T> param in _input)
		{
			if (_shouldRemove(param))
			{
				list.Add(param.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			_input.Remove(list[i]);
		}
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00035F14 File Offset: 0x00034314
	public static V CreationGet<K, V>(this Dictionary<K, V> _input, K _key) where V : new()
	{
		if (!_input.ContainsKey(_key))
		{
			_input.Add(_key, Activator.CreateInstance<V>());
		}
		return _input[_key];
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00035F35 File Offset: 0x00034335
	public static V CreationGet<K, V>(this Dictionary<K, V> _input, K _key, V _default = default(V))
	{
		if (!_input.ContainsKey(_key))
		{
			_input.Add(_key, _default);
		}
		return _input[_key];
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00035F52 File Offset: 0x00034352
	public static V SafeGet<K, V>(this Dictionary<K, V> _input, K _key, V _default = default(V))
	{
		if (_input.ContainsKey(_key))
		{
			return _input[_key];
		}
		return _default;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00035F69 File Offset: 0x00034369
	public static void SafeAdd<K, V>(this Dictionary<K, V> _input, K _key, V _value)
	{
		if (_input.ContainsKey(_key))
		{
			_input[_key] = _value;
		}
		else
		{
			_input.Add(_key, _value);
		}
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00035F8C File Offset: 0x0003438C
	public static void SafeRemove<K, V>(this Dictionary<K, V> _input, K _key)
	{
		if (_input.ContainsKey(_key))
		{
			_input.Remove(_key);
		}
	}
}
