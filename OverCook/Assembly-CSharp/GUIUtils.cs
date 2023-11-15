using System;
using UnityEngine;

// Token: 0x020009A5 RID: 2469
internal class GUIUtils
{
	// Token: 0x06003055 RID: 12373 RVA: 0x000E367C File Offset: 0x000E1A7C
	public static GUIRect ConvertToScreenSpace(GUIRect _guiRect, Camera _camera, Vector3 _worldPoint)
	{
		Vector3 input = _camera.WorldToViewportPoint(_worldPoint);
		input.y = 1f - input.y;
		Vector2 offset = GUIUtils.NormalToGUI(_camera, input.XY(), _guiRect.m_anchor, _guiRect.m_coordSystem);
		return new GUIRect(_guiRect.m_rect.Added(offset), _guiRect.m_anchor, _guiRect.m_coordSystem);
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x000E36DB File Offset: 0x000E1ADB
	public static Rect ToPixels(GUIRect _guiRect, Camera _camera)
	{
		return _guiRect.GetInPixels(_camera);
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x000E36E4 File Offset: 0x000E1AE4
	public static Rect ToPixels(GUIRect _guiRect, Camera _camera, Vector3 _worldPoint)
	{
		GUIRect guirect = GUIUtils.ConvertToScreenSpace(_guiRect, _camera, _worldPoint);
		return guirect.GetInPixels(_camera);
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x000E3704 File Offset: 0x000E1B04
	public static Vector2 GUIToPixels(Camera _camera, Vector2 _pos, GUIAnchor _anchor, GUICoordSystem _coords)
	{
		Vector2 result = GUIUtils.GUIToNormal(_camera, _pos, _anchor, _coords);
		result.x *= (float)_camera.pixelWidth;
		result.y *= (float)_camera.pixelHeight;
		return result;
	}

	// Token: 0x06003059 RID: 12377 RVA: 0x000E3748 File Offset: 0x000E1B48
	public static Vector2 GUIToNormal(Camera _camera, Vector2 _pos, GUIAnchor _anchor, GUICoordSystem _coords)
	{
		_pos.x = GUIUtils.XGUIToNormal(_camera, _pos.x, _coords);
		_pos.y = GUIUtils.YGUIToNormal(_camera, _pos.y, _coords);
		switch (_anchor)
		{
		case GUIAnchor.TopLeft:
			return _pos;
		case GUIAnchor.TopRight:
			_pos.x = 1f - _pos.x;
			return _pos;
		case GUIAnchor.BottomLeft:
			_pos.y = 1f - _pos.y;
			return _pos;
		case GUIAnchor.BottomRight:
			_pos.x = 1f - _pos.x;
			_pos.y = 1f - _pos.y;
			return _pos;
		default:
			return Vector2.zero;
		}
	}

	// Token: 0x0600305A RID: 12378 RVA: 0x000E37F8 File Offset: 0x000E1BF8
	public static Vector2 NormalToGUI(Camera _camera, Vector2 _pos, GUIAnchor _anchor, GUICoordSystem _coords)
	{
		switch (_anchor)
		{
		case GUIAnchor.TopRight:
			_pos.x = 1f - _pos.x;
			break;
		case GUIAnchor.BottomLeft:
			_pos.y = 1f - _pos.y;
			break;
		case GUIAnchor.BottomRight:
			_pos.x = 1f - _pos.x;
			_pos.y = 1f - _pos.y;
			break;
		}
		_pos.x = GUIUtils.NormalToXGUI(_camera, _pos.x, _coords);
		_pos.y = GUIUtils.NormalToYGUI(_camera, _pos.y, _coords);
		return _pos;
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x000E38AF File Offset: 0x000E1CAF
	public static float XGUIToNormal(Camera _camera, float _length, GUICoordSystem _coords)
	{
		if (_coords == GUICoordSystem.X_0ToAspect)
		{
			return _length / _camera.aspect;
		}
		return _length;
	}

	// Token: 0x0600305C RID: 12380 RVA: 0x000E38C1 File Offset: 0x000E1CC1
	public static float NormalToXGUI(Camera _camera, float _length, GUICoordSystem _coords)
	{
		if (_coords == GUICoordSystem.X_0ToAspect)
		{
			return _length * _camera.aspect;
		}
		return _length;
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x000E38D3 File Offset: 0x000E1CD3
	public static float YGUIToNormal(Camera _camera, float _length, GUICoordSystem _coords)
	{
		if (_coords == GUICoordSystem.Y_0ToInverseAspect)
		{
			return _length * _camera.aspect;
		}
		return _length;
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x000E38E6 File Offset: 0x000E1CE6
	public static float NormalToYGUI(Camera _camera, float _length, GUICoordSystem _coords)
	{
		if (_coords == GUICoordSystem.Y_0ToInverseAspect)
		{
			return _length / _camera.aspect;
		}
		return _length;
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x000E38F9 File Offset: 0x000E1CF9
	public static float LengthXGUIToPixels(Camera _camera, float _length, GUICoordSystem _coords)
	{
		return GUIUtils.XGUIToNormal(_camera, _length, _coords) * (float)_camera.pixelWidth;
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000E390B File Offset: 0x000E1D0B
	public static float LengthYGUIToPixels(Camera _camera, float _length, GUICoordSystem _coords)
	{
		return GUIUtils.YGUIToNormal(_camera, _length, _coords) * (float)_camera.pixelHeight;
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000E3920 File Offset: 0x000E1D20
	public static GUIStyle GetTextStyle(Rect _size, TextAnchor _alignment, Font _font, int _maxLetterCount)
	{
		string text = string.Empty;
		for (int i = 0; i < _maxLetterCount; i++)
		{
			text += ((i % 2 != 0) ? "y" : "t");
		}
		return GUIUtils.GetTextStyle(_size, _alignment, _font, text);
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000E396C File Offset: 0x000E1D6C
	public static GUIStyle GetTextStyle(Rect _size, TextAnchor _alignment, Font _font, string _message)
	{
		GUIStyle guistyle = new GUIStyle("label");
		guistyle.font = _font;
		guistyle.alignment = _alignment;
		guistyle.fontSize = (int)_size.height;
		Vector2 vector = guistyle.CalcSize(new GUIContent(_message));
		float num = Mathf.Min(_size.width / vector.x, _size.height / vector.y);
		guistyle.fontSize = (int)(num * (float)guistyle.fontSize);
		return guistyle;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000E39E8 File Offset: 0x000E1DE8
	public static void ShadowedLabel(Rect r, string s, GUIStyle style)
	{
		Color color = GUI.color;
		GUI.color = Color.black;
		GUI.Label(r, s, style);
		r.x -= 1f;
		r.y -= 1f;
		GUI.color = Color.white;
		GUI.Label(r, s, style);
		GUI.color = color;
	}
}
