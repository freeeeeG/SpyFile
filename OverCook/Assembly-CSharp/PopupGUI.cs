using System;
using UnityEngine;

// Token: 0x020009AF RID: 2479
public class PopupGUI : MonoBehaviour
{
	// Token: 0x0600308B RID: 12427 RVA: 0x000E44A9 File Offset: 0x000E28A9
	private void Awake()
	{
		this.m_mainCamera = Camera.main;
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x000E44B8 File Offset: 0x000E28B8
	private void Update()
	{
		if (this.m_fadeInTime > 0f)
		{
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			this.m_alphaProp = Mathf.Min(this.m_alphaProp + deltaTime / this.m_fadeInTime, 1f);
		}
		else
		{
			this.m_alphaProp = 1f;
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x000E4510 File Offset: 0x000E2910
	private void OnGUI()
	{
		GUI.color = new Color(1f, 1f, 1f, this.m_alphaProp);
		Rect rect = this.m_textureRect.Added(0.5f * (float)this.m_mainCamera.pixelWidth, 0.5f * (float)this.m_mainCamera.pixelHeight);
		Rect position = this.m_messageRect.Added(0.5f * (float)this.m_mainCamera.pixelWidth, 0.5f * (float)this.m_mainCamera.pixelHeight);
		this.m_texture.Draw(rect, 0f);
		GUIStyle guistyle = new GUIStyle("label");
		guistyle.font = this.m_font;
		guistyle.alignment = TextAnchor.MiddleCenter;
		guistyle.clipping = TextClipping.Overflow;
		guistyle.fontSize = (int)position.height / this.m_message.Split(new char[]
		{
			'\n'
		}).Length;
		GUI.Label(position, this.m_message, guistyle);
	}

	// Token: 0x040026F8 RID: 9976
	[SerializeField]
	private Rect m_textureRect = new Rect(0f, 0f, 100f, 100f);

	// Token: 0x040026F9 RID: 9977
	[SerializeField]
	private SubTexture2D m_texture;

	// Token: 0x040026FA RID: 9978
	[SerializeField]
	[Multiline]
	private string m_message;

	// Token: 0x040026FB RID: 9979
	[SerializeField]
	private Rect m_messageRect = new Rect(0f, 0f, 100f, 20f);

	// Token: 0x040026FC RID: 9980
	[SerializeField]
	private float m_fadeInTime;

	// Token: 0x040026FD RID: 9981
	[SerializeField]
	private Font m_font;

	// Token: 0x040026FE RID: 9982
	private Camera m_mainCamera;

	// Token: 0x040026FF RID: 9983
	private float m_alphaProp;
}
