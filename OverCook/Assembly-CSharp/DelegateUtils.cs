using System;
using System.Collections.Generic;

// Token: 0x02000239 RID: 569
public static class DelegateUtils
{
	// Token: 0x06000987 RID: 2439 RVA: 0x00036190 File Offset: 0x00034590
	public static bool CallForResult<T>(this List<Generic<T>> _delegates, T _result)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i]();
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x000361DC File Offset: 0x000345DC
	public static bool CallForResult<T, P1>(this List<Generic<T, P1>> _delegates, T _result, P1 _param1)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i](_param1);
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0003622C File Offset: 0x0003462C
	public static bool CallForResult<T, P1, P2>(this List<Generic<T, P1, P2>> _delegates, T _result, P1 _param1, P2 _param2)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i](_param1, _param2);
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0003627C File Offset: 0x0003467C
	public static bool CallForResult<T, P1, P2, P3>(this List<Generic<T, P1, P2, P3>> _delegates, T _result, P1 _param1, P2 _param2, P3 _param3)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i](_param1, _param2, _param3);
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x000362CC File Offset: 0x000346CC
	public static bool CallForResult<T, P1, P2, P3, P4>(this List<Generic<T, P1, P2, P3, P4>> _delegates, T _result, P1 _param1, P2 _param2, P3 _param3, P4 _param4)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i](_param1, _param2, _param3, _param4);
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00036320 File Offset: 0x00034720
	public static bool CallForResult<T, P1, P2, P3, P4, P5>(this List<Generic<T, P1, P2, P3, P4, P5>> _delegates, T _result, P1 _param1, P2 _param2, P3 _param3, P4 _param4, P5 _param5)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i](_param1, _param2, _param3, _param4, _param5);
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00036374 File Offset: 0x00034774
	public static bool CallForResult<T, P1, P2, P3, P4, P5, P6>(this List<Generic<T, P1, P2, P3, P4, P5, P6>> _delegates, T _result, P1 _param1, P2 _param2, P3 _param3, P4 _param4, P5 _param5, P6 _param6)
	{
		for (int i = 0; i < _delegates.Count; i++)
		{
			T t = _delegates[i](_param1, _param2, _param3, _param4, _param5, _param6);
			if (t.Equals(_result))
			{
				return true;
			}
		}
		return false;
	}
}
