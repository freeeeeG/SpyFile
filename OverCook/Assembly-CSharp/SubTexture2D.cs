using System;
using UnityEngine;

// Token: 0x020001D9 RID: 473
[Serializable]
public class SubTexture2D
{
	// Token: 0x06000805 RID: 2053 RVA: 0x000315CE File Offset: 0x0002F9CE
	public SubTexture2D DeepClone()
	{
		return base.MemberwiseClone() as SubTexture2D;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x000315DC File Offset: 0x0002F9DC
	public static implicit operator bool(SubTexture2D _texture)
	{
		return _texture != null && _texture.m_atlasTexture != null && _texture.m_subRect.width >= 1f && _texture.m_subRect.height >= 1f;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0003162D File Offset: 0x0002FA2D
	public void Draw(Rect _rect, float _angle = 0f)
	{
		SubTexture2D.Draw(this, _rect, _angle);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00031638 File Offset: 0x0002FA38
	public void Draw(Vector2 _center, float _scale = 1f, float _angle = 0f)
	{
		Rect rect = new Rect(_center.x - _scale * 0.5f * this.m_subRect.width, _center.y - _scale * 0.5f * this.m_subRect.height, _scale * this.m_subRect.width, _scale * this.m_subRect.height);
		SubTexture2D.Draw(this, rect, _angle);
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x000316A4 File Offset: 0x0002FAA4
	public static void Draw(SubTexture2D _subTexture, Rect _rect, float _angle = 0f)
	{
		Matrix4x4 matrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(57.29578f * _angle, _rect.center);
		GUI.BeginGroup(_rect);
		Texture2D atlasTexture = _subTexture.m_atlasTexture;
		Rect subRect = _subTexture.m_subRect;
		float width = _rect.width * (float)atlasTexture.width / subRect.width;
		float num = _rect.width * subRect.x / subRect.width;
		float height = _rect.height * (float)atlasTexture.height / subRect.height;
		float num2 = _rect.height * subRect.y / subRect.height;
		GUI.DrawTexture(new Rect(-num, -num2, width, height), atlasTexture);
		GUI.EndGroup();
		GUI.matrix = matrix;
	}

	// Token: 0x04000664 RID: 1636
	public Texture2D m_atlasTexture;

	// Token: 0x04000665 RID: 1637
	public Rect m_subRect;
}
