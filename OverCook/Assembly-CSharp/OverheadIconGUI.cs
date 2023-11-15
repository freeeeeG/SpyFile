using System;
using UnityEngine;

// Token: 0x020009AE RID: 2478
[AddComponentMenu("Scripts/Game/GUI/OverheadIconGUI")]
public class OverheadIconGUI : MonoBehaviour
{
	// Token: 0x06003081 RID: 12417 RVA: 0x000E410E File Offset: 0x000E250E
	public void SetSubTexture(SubTexture2D _texture)
	{
		this.m_subTexture = _texture;
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x000E4117 File Offset: 0x000E2517
	public void SetBackground(SubTexture2D _texture)
	{
		this.m_backgroundTexture = _texture;
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x000E4120 File Offset: 0x000E2520
	public void SetScreenOffset(Vector2 _offset)
	{
		this.m_screenSpaceOffset = _offset;
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x000E4129 File Offset: 0x000E2529
	public void SetWorldOffset(Vector3 _offset)
	{
		this.m_worldOffset = _offset;
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x000E4132 File Offset: 0x000E2532
	public void SetIconScale(float _scale)
	{
		this.m_iconScale = _scale;
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x000E413B File Offset: 0x000E253B
	public void SetLifeTime(float _lifetime, bool _destroyOnLifeOver = true)
	{
		this.m_dead = false;
		this.m_lifetime = _lifetime;
		this.m_destroyOnLifeOver = _destroyOnLifeOver;
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x000E4154 File Offset: 0x000E2554
	private void Update()
	{
		if (this.m_dead)
		{
			return;
		}
		this.m_alphaTimer += TimeManager.GetDeltaTime(base.gameObject);
		float num = Mathf.Clamp01(this.m_alphaTimer / this.m_fadeInOutTime);
		if (this.m_lifetime > 0f)
		{
			this.m_lifetime -= TimeManager.GetDeltaTime(base.gameObject);
			num = Mathf.Min(num, Mathf.Clamp01(this.m_lifetime / this.m_fadeInOutTime));
			if (this.m_lifetime < 0f)
			{
				if (this.m_destroyOnLifeOver)
				{
					UnityEngine.Object.Destroy(this);
				}
				this.m_dead = true;
				this.m_alphaTimer = 0f;
			}
		}
		this.m_alphaProp = 0.5f * (1f - Mathf.Cos(num * 3.1415927f));
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x000E422C File Offset: 0x000E262C
	private void OnGUI()
	{
		if (this.m_dead || this.m_subTexture == null || this.m_subTexture.m_atlasTexture == null)
		{
			return;
		}
		foreach (Camera camera in Camera.allCameras)
		{
			Rect position = new Rect(camera.pixelRect.x, (float)Screen.height - (camera.pixelRect.height + camera.pixelRect.y), camera.pixelRect.width, camera.pixelRect.height);
			GUI.BeginGroup(position);
			Vector3 a = camera.WorldToViewportPoint(base.transform.position + base.transform.rotation * this.m_worldOffset);
			a.y = 1f - a.y;
			a += VectorUtils.FromXY(this.m_screenSpaceOffset, 0f);
			Vector2 vCenter = new Vector2(camera.pixelRect.width * a.x, camera.pixelRect.height * a.y);
			GUI.color = new Color(1f, 1f, 1f, this.m_alphaProp);
			if (this.m_backgroundTexture)
			{
				OverheadIconGUI.DrawScaledSubTexture(this.m_backgroundTexture, vCenter, this.m_iconScale);
			}
			if (this.m_subTexture)
			{
				OverheadIconGUI.DrawScaledSubTexture(this.m_subTexture, vCenter, this.m_subToBackRatio * this.m_iconScale);
			}
			GUI.EndGroup();
		}
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x000E43E8 File Offset: 0x000E27E8
	private static void DrawScaledSubTexture(SubTexture2D _texture, Vector2 _vCenter, float _nScale)
	{
		Rect subRect = _texture.m_subRect;
		Vector2 vector = _vCenter - _nScale * 0.5f * new Vector2(subRect.width, subRect.height);
		Rect rect = new Rect(vector.x, vector.y, _nScale * subRect.width, _nScale * subRect.height);
		_texture.Draw(rect, 0f);
	}

	// Token: 0x040026EC RID: 9964
	[SerializeField]
	private SubTexture2D m_backgroundTexture;

	// Token: 0x040026ED RID: 9965
	[SerializeField]
	private SubTexture2D m_subTexture;

	// Token: 0x040026EE RID: 9966
	[SerializeField]
	private Vector3 m_worldOffset;

	// Token: 0x040026EF RID: 9967
	[SerializeField]
	private Vector2 m_screenSpaceOffset;

	// Token: 0x040026F0 RID: 9968
	[SerializeField]
	private float m_fadeInOutTime = 1f;

	// Token: 0x040026F1 RID: 9969
	[SerializeField]
	private float m_subToBackRatio = 1f;

	// Token: 0x040026F2 RID: 9970
	[SerializeField]
	private float m_iconScale = 1f;

	// Token: 0x040026F3 RID: 9971
	[SerializeField]
	private float m_lifetime;

	// Token: 0x040026F4 RID: 9972
	[SerializeField]
	private bool m_destroyOnLifeOver;

	// Token: 0x040026F5 RID: 9973
	private float m_alphaTimer;

	// Token: 0x040026F6 RID: 9974
	private float m_alphaProp;

	// Token: 0x040026F7 RID: 9975
	private bool m_dead;
}
