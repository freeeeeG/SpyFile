using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200021C RID: 540
public static class ArrayUtils
{
	// Token: 0x060008F2 RID: 2290 RVA: 0x00035474 File Offset: 0x00033874
	public static bool IsEmpty<T>(this T[] _array)
	{
		return _array.Length == 0;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0003547C File Offset: 0x0003387C
	public static void SetValues<T>(this T[] _array, T _value, int _start, int _end)
	{
		for (int i = _start; i < _end; i++)
		{
			_array[i] = _value;
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x000354A4 File Offset: 0x000338A4
	public static T[] Union<T>(this T[] _a, T[] _b)
	{
		T[] array = new T[_a.Length + _b.Length];
		_a.CopyTo(array, 0);
		_b.CopyTo(array, _a.Length);
		return array;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x000354D4 File Offset: 0x000338D4
	public static T[] Compliment<T>(this T[] _a, T[] _b)
	{
		Predicate<T> match = (T _v) => !_b.Contains(_v);
		return _a.FindAll(match);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00035502 File Offset: 0x00033902
	public static void ExpandingAssign<T>(ref T[] _array, int _index, T _value)
	{
		if (_index >= _array.Length)
		{
			Array.Resize<T>(ref _array, _index + 1);
		}
		_array[_index] = _value;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x00035520 File Offset: 0x00033920
	public static T TryAtIndex<T>(this T[] _array, int _index)
	{
		return _array.TryAtIndex(_index, default(T));
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0003553D File Offset: 0x0003393D
	public static T TryAtIndex<T>(this T[] _array, int _index, T _default)
	{
		if (_index >= _array.Length || _index < 0)
		{
			return _default;
		}
		return _array[_index];
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00035558 File Offset: 0x00033958
	public static void SafeSet<T>(this T[] _array, int _index, T _value)
	{
		if (_index >= 0 && _index < _array.Length)
		{
			_array[_index] = _value;
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00035572 File Offset: 0x00033972
	public static U[] ConvertAll<T, U>(this T[] _array, Converter<T, U> _converter)
	{
		return Array.ConvertAll<T, U>(_array, _converter);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0003557C File Offset: 0x0003397C
	public static U[] ConvertAll<T, U>(this T[] _array, Generic<U, int, T> _converter)
	{
		U[] array = new U[_array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _converter(i, _array[i]);
		}
		return array;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x000355BC File Offset: 0x000339BC
	public static U[] ConvertRange<T, U>(this T[] _array, Generic<U, int, T> _converter, int _start, int _length)
	{
		U[] array = new U[_length];
		for (int i = 0; i < _length; i++)
		{
			int num = i + _start;
			array[i] = _converter(num, _array[num]);
		}
		return array;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x000355FC File Offset: 0x000339FC
	public static T[] Intersection<T>(this T[] _a, T[] _b) where T : IComparable, IConvertible
	{
		T[] array = new T[0];
		bool[] array2 = new bool[_b.Length];
		for (int i = 0; i < _a.Length; i++)
		{
			for (int j = 0; j < _b.Length; j++)
			{
				if (!array2[j] && _b[j].Equals(_a[i]))
				{
					array2[j] = true;
					Array.Resize<T>(ref array, array.Length + 1);
					array[array.Length - 1] = _a[i];
					break;
				}
			}
		}
		return array;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00035698 File Offset: 0x00033A98
	public static bool Contains<T>(this T[] _array, T _value)
	{
		return _array.FindIndex_Predicate((T x) => _value.Equals(x)) != -1;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x000356CA File Offset: 0x00033ACA
	public static bool Contains<T>(this T[] _array, Predicate<T> _matchFunction)
	{
		return _array.FindIndex_Predicate(_matchFunction) != -1;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000356D9 File Offset: 0x00033AD9
	public static bool Contains<T>(this T[] _array, Generic<bool, int, T> _matchFunction)
	{
		return _array.FindIndex_Generic(_matchFunction) != -1;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x000356E8 File Offset: 0x00033AE8
	public static bool Contains<T>(this T[] _array, T[] _subArray)
	{
		ArrayUtils.<Contains>c__AnonStorey2<T> <Contains>c__AnonStorey = new ArrayUtils.<Contains>c__AnonStorey2<T>();
		<Contains>c__AnonStorey._subArray = _subArray;
		<Contains>c__AnonStorey.used = new bool[_array.Length];
		int i;
		for (i = 0; i < <Contains>c__AnonStorey._subArray.Length; i++)
		{
			int num = _array.FindIndex_Generic((int j, T x) => !<Contains>c__AnonStorey.used[j] && <Contains>c__AnonStorey._subArray[i].Equals(x));
			if (num == -1)
			{
				return false;
			}
			<Contains>c__AnonStorey.used[num] = true;
		}
		return true;
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00035770 File Offset: 0x00033B70
	public static int FindIndex<T>(this T[] _array, T _value) where T : IComparable
	{
		for (int i = 0; i < _array.Length; i++)
		{
			if (_value.Equals(_array[i]))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x000357B2 File Offset: 0x00033BB2
	public static int FindIndex_Predicate<T>(this T[] _array, Predicate<T> _matchFunction)
	{
		return Array.FindIndex<T>(_array, _matchFunction);
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000357BC File Offset: 0x00033BBC
	public static int FindIndex_Generic<T>(this T[] _array, Generic<bool, int, T> _matchFunction)
	{
		for (int i = 0; i < _array.Length; i++)
		{
			if (_matchFunction(i, _array[i]))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x000357F4 File Offset: 0x00033BF4
	public static void PushBack<T>(ref T[] _array, T _value)
	{
		int num = _array.Length;
		Array.Resize<T>(ref _array, num + 1);
		_array[num] = _value;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00035818 File Offset: 0x00033C18
	public static void Insert<T>(ref T[] _array, int _index, T _value)
	{
		int num = _array.Length;
		Array.Resize<T>(ref _array, 1 + num);
		for (int i = num; i > _index; i--)
		{
			_array[i] = _array[i - 1];
		}
		_array[_index] = _value;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00035860 File Offset: 0x00033C60
	public static void RemoveAllDuplicates<T>(ref T[] _array)
	{
		for (int i = _array.Length - 1; i >= 0; i--)
		{
			T e = _array[i];
			if (_array.FindAll((T x) => x.Equals(e)).Length > 1)
			{
				ArrayUtils.RemoveAt<T>(ref _array, i);
			}
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x000358BC File Offset: 0x00033CBC
	public static void RemoveAt<T>(ref T[] _array, int _index)
	{
		int num = _array.Length;
		for (int i = _index + 1; i < num; i++)
		{
			_array[i - 1] = _array[i];
		}
		Array.Resize<T>(ref _array, num - 1);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00035900 File Offset: 0x00033D00
	public static KeyValuePair<int, T> FindLowestScoring<T>(this T[] _array, Generic<float, T> _scoreFunction)
	{
		float num;
		return _array.FindLowestScoring(_scoreFunction, out num);
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00035918 File Offset: 0x00033D18
	public static KeyValuePair<int, T> FindLowestScoring<T>(this T[] _array, Generic<float, T> _scoreFunction, out float o_score)
	{
		o_score = float.MaxValue;
		int num = -1;
		for (int i = 0; i < _array.Length; i++)
		{
			float num2 = _scoreFunction(_array[i]);
			if (num2 < o_score)
			{
				o_score = num2;
				num = i;
			}
		}
		if (num == -1)
		{
			return new KeyValuePair<int, T>(num, default(T));
		}
		return new KeyValuePair<int, T>(num, _array[num]);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00035984 File Offset: 0x00033D84
	public static KeyValuePair<int, T> FindHighestScoring<T>(this T[] _array, Generic<float, T> _scoreFunction)
	{
		float num;
		return _array.FindHighestScoring(_scoreFunction, out num);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003599C File Offset: 0x00033D9C
	public static KeyValuePair<int, T> FindHighestScoring<T>(this T[] _array, Generic<float, T> _scoreFunction, out float o_score)
	{
		o_score = float.MinValue;
		int num = -1;
		for (int i = 0; i < _array.Length; i++)
		{
			float num2 = _scoreFunction(_array[i]);
			if (num2 > o_score)
			{
				o_score = num2;
				num = i;
			}
		}
		if (num == -1)
		{
			return new KeyValuePair<int, T>(num, default(T));
		}
		return new KeyValuePair<int, T>(num, _array[num]);
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00035A08 File Offset: 0x00033E08
	public static T GetRandomElement<T>(this T[] _items)
	{
		if (_items.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, _items.Length);
			return _items[num];
		}
		return default(T);
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00035A3C File Offset: 0x00033E3C
	public static KeyValuePair<int, T> GetWeightedRandomElement<T>(this T[] _items, Generic<float, int, T> _weight)
	{
		float num = 0f;
		for (int i = 0; i < _items.Length; i++)
		{
			float num2 = _weight(i, _items[i]);
			num += num2;
		}
		float num3 = UnityEngine.Random.Range(0f, num);
		float num4 = 0f;
		for (int j = 0; j < _items.Length; j++)
		{
			num4 += _weight(j, _items[j]);
			if (num3 <= num4)
			{
				return new KeyValuePair<int, T>(j, _items[j]);
			}
		}
		return new KeyValuePair<int, T>(-1, default(T));
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00035ADE File Offset: 0x00033EDE
	public static KeyValuePair<int, T> GetWeightedRandomElement<T>(this T[] _items) where T : IWeight
	{
		return _items.GetWeightedRandomElement((int i, T t) => t.Weight);
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00035AF4 File Offset: 0x00033EF4
	public static T[] FindAll<T>(this T[] _array, Predicate<T> _match)
	{
		T[] result = new T[0];
		for (int i = 0; i < _array.Length; i++)
		{
			if (_match(_array[i]))
			{
				ArrayUtils.PushBack<T>(ref result, _array[i]);
			}
		}
		return result;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00035B40 File Offset: 0x00033F40
	public static T[] AllRemoved_Generic<T>(this T[] _array, Generic<bool, int, T> _match)
	{
		List<T> list = new List<T>(_array);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (_match(i, list[i]))
			{
				list.RemoveAt(i);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00035B90 File Offset: 0x00033F90
	public static T[] AllRemoved_Predicate<T>(this T[] _array, Predicate<T> _match)
	{
		List<T> list = new List<T>(_array);
		list.RemoveAll(_match);
		return list.ToArray();
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00035BB4 File Offset: 0x00033FB4
	public static string Concatenated<T>(this T[] _array, Generic<string, int, T> _converter)
	{
		string text = string.Empty;
		for (int i = 0; i < _array.Length; i++)
		{
			text += _converter(i, _array[i]);
		}
		return text;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00035BF4 File Offset: 0x00033FF4
	public static Vector3 Mean<T>(this T[] _array, Generic<Vector3, T> _converter)
	{
		Vector3 vector = Vector3.zero;
		if (_array.Length > 0)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				vector += _converter(_array[i]);
			}
			return vector / (float)_array.Length;
		}
		return vector;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00035C44 File Offset: 0x00034044
	public static U Collapse<T, U>(this T[] _array, Generic<U, T, U> _converter)
	{
		U u = default(U);
		for (int i = 0; i < _array.Length; i++)
		{
			u = _converter(_array[i], u);
		}
		return u;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00035C80 File Offset: 0x00034080
	public static T[] Shuffled<T>(this T[] _array)
	{
		T[] array = new T[_array.Length];
		_array.CopyTo(array, 0);
		array.ShuffleContents<T>();
		return array;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00035CA8 File Offset: 0x000340A8
	public static void ShuffleContents<T>(this T[] _array)
	{
		for (int i = 0; i < _array.Length; i++)
		{
			int num = UnityEngine.Random.Range(0, _array.Length);
			if (num != i)
			{
				T t = _array[i];
				_array[i] = _array[num];
				_array[num] = t;
			}
		}
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00035CF8 File Offset: 0x000340F8
	public static string Stringify<T>(this T[] _array)
	{
		string str = "{ ";
		str = str + _array.Collapse(new Generic<string, T, string>(ArrayUtils.AssembleString<T>)) + " }";
		return str + " }";
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00035D34 File Offset: 0x00034134
	public static string AssembleString<T>(T _v, string _s)
	{
		return _s + ", " + _v.ToString();
	}
}
