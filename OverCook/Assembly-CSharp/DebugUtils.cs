using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020001AE RID: 430
public static class DebugUtils
{
	// Token: 0x06000735 RID: 1845 RVA: 0x0002EE35 File Offset: 0x0002D235
	[Conditional("DEBUG")]
	public static void Assert(bool condition)
	{
		if (UnityEngine.Debug.isDebugBuild && !condition)
		{
			UnityEngine.Debug.Log("Assert Failed");
			throw new UnityException();
		}
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0002EE57 File Offset: 0x0002D257
	[Conditional("DEBUG")]
	public static void Assert(bool condition, string msg)
	{
		if (UnityEngine.Debug.isDebugBuild && !condition)
		{
			UnityEngine.Debug.Log("Assert Failed: " + msg);
			throw new UnityException(msg);
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0002EE80 File Offset: 0x0002D280
	[Conditional("DEBUG")]
	public static void AssertPopup(bool condition, string msg)
	{
		if (UnityEngine.Debug.isDebugBuild && !condition)
		{
			UnityEngine.Debug.Log("Assert Failed: " + msg);
		}
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0002EEA2 File Offset: 0x0002D2A2
	[Conditional("DEBUG")]
	public static void Unreachable(string msg)
	{
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Unreachable: " + msg);
			throw new UnityException(msg);
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0002EEC5 File Offset: 0x0002D2C5
	[Conditional("DEBUG")]
	public static void Error(string msg)
	{
		if (UnityEngine.Debug.isDebugBuild)
		{
			throw new UnityException(msg);
		}
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0002EED8 File Offset: 0x0002D2D8
	public static void LogPrint(string _text, Color _color)
	{
		if (DebugUtils.debugDrawManager)
		{
			DebugUtils.debugDrawManager.AddLogText(_text, _color);
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002EEF5 File Offset: 0x0002D2F5
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector2 _pos)
	{
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x0002EEF7 File Offset: 0x0002D2F7
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector2 _pos, Color _color)
	{
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x0002EEF9 File Offset: 0x0002D2F9
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector2 _pos, Color _color, bool _centred)
	{
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0002EEFB File Offset: 0x0002D2FB
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector2 _pos, Color _color, bool _centred, float _lifeTime)
	{
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0002EF00 File Offset: 0x0002D300
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector2 _pos, Color _color, bool _centred, float _lifeTime, int _fontSize)
	{
		if (UnityEngine.Debug.isDebugBuild && DebugUtils.debugDrawManager)
		{
			DebugDrawManager.TextRequest2d textRequest;
			textRequest.m_contents = _text;
			textRequest.m_position = _pos;
			textRequest.m_color = _color;
			textRequest.m_centred = _centred;
			textRequest.m_lifeTime = _lifeTime;
			textRequest.m_fontSize = _fontSize;
			DebugUtils.debugDrawManager.AddText(textRequest);
		}
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0002EF63 File Offset: 0x0002D363
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector3 _pos)
	{
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0002EF65 File Offset: 0x0002D365
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector3 _pos, Color _color)
	{
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0002EF67 File Offset: 0x0002D367
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector3 _pos, Color _color, bool _centred)
	{
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0002EF69 File Offset: 0x0002D369
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector3 _pos, Color _color, bool _centred, float _lifeTime)
	{
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0002EF6C File Offset: 0x0002D36C
	[Conditional("DEBUG")]
	public static void Print(string _text, Vector3 _pos, Color _color, bool _centred, float _lifeTime, int _fontSize)
	{
		if (UnityEngine.Debug.isDebugBuild && DebugUtils.debugDrawManager)
		{
			DebugDrawManager.TextRequest3d textRequest;
			textRequest.m_contents = _text;
			textRequest.m_position = _pos;
			textRequest.m_color = _color;
			textRequest.m_centred = _centred;
			textRequest.m_lifeTime = _lifeTime;
			textRequest.m_fontSize = _fontSize;
			DebugUtils.debugDrawManager.AddText(textRequest);
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0002EFCF File Offset: 0x0002D3CF
	[Conditional("DEBUG")]
	public static void DrawLabelFlag(string _text, Vector3 _pos)
	{
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0002EFD4 File Offset: 0x0002D3D4
	[Conditional("DEBUG")]
	public static void DrawCross(Vector3 _pos, float _crossSize)
	{
		if (UnityEngine.Debug.isDebugBuild)
		{
			Camera main = Camera.main;
			if (main != null)
			{
				Vector3 right = main.transform.right;
				Vector3 up = main.transform.up;
				Vector3 b = right * _crossSize + up * _crossSize;
				Vector3 b2 = right * _crossSize - up * _crossSize;
				Color white = Color.white;
				UnityEngine.Debug.DrawLine(_pos + b, _pos - b, white);
				UnityEngine.Debug.DrawLine(_pos + b2, _pos - b2, white);
			}
		}
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0002F071 File Offset: 0x0002D471
	[Conditional("DEBUG")]
	public static void DrawLabelFlag(string _text, Vector3 _pos, Color _color)
	{
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0002F073 File Offset: 0x0002D473
	[Conditional("DEBUG")]
	public static void DrawLabelFlag(string _text, Vector3 _pos, Color _color, Vector3 _offset)
	{
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.DrawLine(_pos, _pos + _offset, _color);
		}
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0002F090 File Offset: 0x0002D490
	[Conditional("DEBUG")]
	public static void DrawShape(PrimitiveType _type, Vector3 _pos, Vector3 _scale, Color _colour, Quaternion _rotation, float _duration = 1f)
	{
		GameObject gameObject = GameObject.CreatePrimitive(_type);
		gameObject.name = "Debug_DrawShape";
		gameObject.transform.position = _pos;
		gameObject.transform.localScale = _scale;
		gameObject.transform.rotation = _rotation;
		Collider[] array = gameObject.RequestComponents<Collider>();
		for (int i = array.Length - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(array[i]);
		}
		Renderer renderer = gameObject.RequestComponent<Renderer>();
		if (renderer != null)
		{
			renderer.material.color = _colour;
		}
		UnityEngine.Object.Destroy(gameObject, _duration);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0002F120 File Offset: 0x0002D520
	[Conditional("DEBUG")]
	public static void Break(bool _condition)
	{
		if (_condition)
		{
		}
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0002F135 File Offset: 0x0002D535
	[Conditional("DEV_LOGGING")]
	public static void Log(object msg)
	{
		UnityEngine.Debug.Log(msg);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0002F13D File Offset: 0x0002D53D
	[Conditional("DEBUG")]
	public static void Warn(object msg)
	{
		UnityEngine.Debug.LogWarning(msg);
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0002F145 File Offset: 0x0002D545
	[Conditional("DEV_LOGGING")]
	public static void LogError(object msg)
	{
		UnityEngine.Debug.LogError(msg);
	}

	// Token: 0x04000604 RID: 1540
	public static DebugDrawManager debugDrawManager;
}
