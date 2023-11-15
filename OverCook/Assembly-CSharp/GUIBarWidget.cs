using System;
using UnityEngine;

// Token: 0x020009A2 RID: 2466
public class GUIBarWidget
{
	// Token: 0x0600304A RID: 12362 RVA: 0x000E3168 File Offset: 0x000E1568
	public GUIBarWidget(GUIBarWidgetConfig _config)
	{
		this.m_config = _config;
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, Color.white);
		texture2D.wrapMode = TextureWrapMode.Repeat;
		texture2D.Apply();
		this.m_blockColourStyle = new GUIStyle();
		this.m_blockColourStyle.normal.background = texture2D;
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000E31CB File Offset: 0x000E15CB
	public virtual void SetValue(float _value)
	{
		this.m_value = Mathf.Clamp01(_value);
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000E31D9 File Offset: 0x000E15D9
	public float GetValue()
	{
		return this.m_value;
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x000E31E1 File Offset: 0x000E15E1
	public void SetCroppedWidth(float _cropWidth)
	{
		this.m_croppedSize = _cropWidth / this.m_config.m_width;
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x000E31F8 File Offset: 0x000E15F8
	private Rect GetGUIRect(Camera _camera)
	{
		Transform transform = this.m_config.m_transform;
		if (transform != null)
		{
			Vector3 vector = _camera.WorldToViewportPoint(transform.position);
			vector.y = 1f - vector.y;
			float width = this.m_config.m_width;
			float height = this.m_config.m_height;
			Vector2 offset = this.m_config.m_offset;
			float x = _camera.pixelRect.width * vector.x - 0.5f * width;
			float y = _camera.pixelRect.height * vector.y - 0.5f * height;
			Vector2 vector2 = new Vector2(x, y) + offset;
			return new Rect(vector2.x, vector2.y, width, height);
		}
		float width2 = this.m_config.m_width;
		float height2 = this.m_config.m_height;
		Vector2 offset2 = this.m_config.m_offset;
		return new Rect(offset2.x, offset2.y, width2, height2);
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x000E330C File Offset: 0x000E170C
	public void Draw(Vector2? _offset = null, float _widthScale = 1f, float _heightScale = 1f)
	{
		Vector2 vector = (_offset == null) ? Vector2.zero : _offset.Value;
		foreach (Camera camera in Camera.allCameras)
		{
			Rect position = new Rect(camera.pixelRect.x, (float)Screen.height - (camera.pixelRect.height + camera.pixelRect.y), camera.pixelRect.width, camera.pixelRect.height);
			GUI.BeginGroup(position);
			Rect guirect = this.GetGUIRect(camera);
			guirect.x = _widthScale * guirect.x + vector.x;
			guirect.y = _heightScale * guirect.y + vector.y;
			guirect.width *= _widthScale;
			guirect.height *= _heightScale;
			float num = guirect.width * this.m_croppedSize;
			float border = this.m_config.m_border;
			Rect position2 = new Rect(guirect.x - border, guirect.y - border, num + 2f * border, guirect.height + 2f * border);
			GUI.BeginGroup(position2);
			Texture2D image = null;
			Color color = GUI.color;
			GUI.color = this.m_config.m_borderColor;
			GUI.Box(new Rect(0f, 0f, position2.width, position2.height), image, this.m_blockColourStyle);
			Rect position3 = new Rect(border, border, num, guirect.height);
			GUI.BeginGroup(position3);
			GUI.color = this.m_config.m_emptyColor;
			GUI.Box(new Rect(guirect.width * this.m_value, 0f, position3.width, position3.height), image, this.m_blockColourStyle);
			GUI.color = this.m_config.m_fillColor;
			GUI.Box(new Rect(0f, 0f, guirect.width * this.m_value, position3.height), image, this.m_blockColourStyle);
			GUI.EndGroup();
			GUI.color = color;
			GUI.EndGroup();
			GUI.EndGroup();
		}
	}

	// Token: 0x040026CD RID: 9933
	public GUIBarWidgetConfig m_config;

	// Token: 0x040026CE RID: 9934
	private float m_value;

	// Token: 0x040026CF RID: 9935
	private GUIStyle m_blockColourStyle;

	// Token: 0x040026D0 RID: 9936
	private float m_croppedSize = 1f;
}
