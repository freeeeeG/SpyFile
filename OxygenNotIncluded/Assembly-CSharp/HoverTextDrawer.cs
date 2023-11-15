using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A65 RID: 2661
public class HoverTextDrawer
{
	// Token: 0x06005071 RID: 20593 RVA: 0x001C8200 File Offset: 0x001C6400
	public HoverTextDrawer(HoverTextDrawer.Skin skin, RectTransform parent)
	{
		this.shadowBars = new HoverTextDrawer.Pool<Image>(skin.shadowBarWidget.gameObject, parent);
		this.selectBorders = new HoverTextDrawer.Pool<Image>(skin.selectBorderWidget.gameObject, parent);
		this.textWidgets = new HoverTextDrawer.Pool<LocText>(skin.textWidget.gameObject, parent);
		this.iconWidgets = new HoverTextDrawer.Pool<Image>(skin.iconWidget.gameObject, parent);
		this.skin = skin;
	}

	// Token: 0x06005072 RID: 20594 RVA: 0x001C8276 File Offset: 0x001C6476
	public void SetEnabled(bool enabled)
	{
		this.shadowBars.SetEnabled(enabled);
		this.textWidgets.SetEnabled(enabled);
		this.iconWidgets.SetEnabled(enabled);
		this.selectBorders.SetEnabled(enabled);
	}

	// Token: 0x06005073 RID: 20595 RVA: 0x001C82A8 File Offset: 0x001C64A8
	public void BeginDrawing(Vector2 root_pos)
	{
		this.rootPos = root_pos + this.skin.baseOffset;
		if (this.skin.enableDebugOffset)
		{
			this.rootPos += this.skin.debugOffset;
		}
		this.currentPos = this.rootPos;
		this.textWidgets.BeginDrawing();
		this.iconWidgets.BeginDrawing();
		this.shadowBars.BeginDrawing();
		this.selectBorders.BeginDrawing();
		this.firstShadowBar = true;
		this.minLineHeight = 0;
	}

	// Token: 0x06005074 RID: 20596 RVA: 0x001C833B File Offset: 0x001C653B
	public void EndDrawing()
	{
		this.shadowBars.EndDrawing();
		this.iconWidgets.EndDrawing();
		this.textWidgets.EndDrawing();
		this.selectBorders.EndDrawing();
	}

	// Token: 0x06005075 RID: 20597 RVA: 0x001C836C File Offset: 0x001C656C
	public void DrawText(string text, TextStyleSetting style, Color color, bool override_color = true)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		LocText widget = this.textWidgets.Draw(this.currentPos).widget;
		Color color2 = Color.white;
		if (widget.textStyleSetting != style)
		{
			widget.textStyleSetting = style;
			widget.ApplySettings();
		}
		if (style != null)
		{
			color2 = style.textColor;
		}
		if (override_color)
		{
			color2 = color;
		}
		widget.color = color2;
		if (widget.text != text)
		{
			widget.text = text;
			widget.KForceUpdateDirty();
		}
		this.currentPos.x = this.currentPos.x + widget.renderedWidth;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
		this.minLineHeight = (int)Mathf.Max((float)this.minLineHeight, widget.renderedHeight);
	}

	// Token: 0x06005076 RID: 20598 RVA: 0x001C8441 File Offset: 0x001C6641
	public void DrawText(string text, TextStyleSetting style)
	{
		this.DrawText(text, style, Color.white, false);
	}

	// Token: 0x06005077 RID: 20599 RVA: 0x001C8451 File Offset: 0x001C6651
	public void AddIndent(int width = 36)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.x = this.currentPos.x + (float)width;
	}

	// Token: 0x06005078 RID: 20600 RVA: 0x001C8474 File Offset: 0x001C6674
	public void NewLine(int min_height = 26)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.y = this.currentPos.y - (float)Math.Max(min_height, this.minLineHeight);
		this.currentPos.x = this.rootPos.x;
		this.minLineHeight = 0;
	}

	// Token: 0x06005079 RID: 20601 RVA: 0x001C84C8 File Offset: 0x001C66C8
	public void DrawIcon(Sprite icon, int min_width = 18)
	{
		this.DrawIcon(icon, Color.white, min_width, 2);
	}

	// Token: 0x0600507A RID: 20602 RVA: 0x001C84D8 File Offset: 0x001C66D8
	public void DrawIcon(Sprite icon, Color color, int image_size = 18, int horizontal_spacing = 2)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.AddIndent(horizontal_spacing);
		HoverTextDrawer.Pool<Image>.Entry entry = this.iconWidgets.Draw(this.currentPos + this.skin.shadowImageOffset);
		entry.widget.sprite = icon;
		entry.widget.color = this.skin.shadowImageColor;
		entry.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		HoverTextDrawer.Pool<Image>.Entry entry2 = this.iconWidgets.Draw(this.currentPos);
		entry2.widget.sprite = icon;
		entry2.widget.color = color;
		entry2.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		this.AddIndent(horizontal_spacing);
		this.currentPos.x = this.currentPos.x + (float)image_size;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
	}

	// Token: 0x0600507B RID: 20603 RVA: 0x001C85C4 File Offset: 0x001C67C4
	public void BeginShadowBar(bool selected = false)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		if (this.firstShadowBar)
		{
			this.firstShadowBar = false;
		}
		else
		{
			this.NewLine(22);
		}
		this.isShadowBarSelected = selected;
		this.shadowStartPos = this.currentPos;
		this.maxShadowX = this.rootPos.x;
	}

	// Token: 0x0600507C RID: 20604 RVA: 0x001C861C File Offset: 0x001C681C
	public void EndShadowBar()
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.NewLine(22);
		HoverTextDrawer.Pool<Image>.Entry entry = this.shadowBars.Draw(this.currentPos);
		entry.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x, this.skin.shadowBarBorder.y);
		entry.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f);
		if (this.isShadowBarSelected)
		{
			HoverTextDrawer.Pool<Image>.Entry entry2 = this.selectBorders.Draw(this.currentPos);
			entry2.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x - this.skin.selectBorder.x, this.skin.shadowBarBorder.y + this.skin.selectBorder.y);
			entry2.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f + this.skin.selectBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f + this.skin.selectBorder.y * 2f);
		}
	}

	// Token: 0x0600507D RID: 20605 RVA: 0x001C8800 File Offset: 0x001C6A00
	public void Cleanup()
	{
		this.shadowBars.Cleanup();
		this.textWidgets.Cleanup();
		this.iconWidgets.Cleanup();
	}

	// Token: 0x040034B0 RID: 13488
	public HoverTextDrawer.Skin skin;

	// Token: 0x040034B1 RID: 13489
	private Vector2 currentPos;

	// Token: 0x040034B2 RID: 13490
	private Vector2 rootPos;

	// Token: 0x040034B3 RID: 13491
	private Vector2 shadowStartPos;

	// Token: 0x040034B4 RID: 13492
	private float maxShadowX;

	// Token: 0x040034B5 RID: 13493
	private bool firstShadowBar;

	// Token: 0x040034B6 RID: 13494
	private bool isShadowBarSelected;

	// Token: 0x040034B7 RID: 13495
	private int minLineHeight;

	// Token: 0x040034B8 RID: 13496
	private HoverTextDrawer.Pool<LocText> textWidgets;

	// Token: 0x040034B9 RID: 13497
	private HoverTextDrawer.Pool<Image> iconWidgets;

	// Token: 0x040034BA RID: 13498
	private HoverTextDrawer.Pool<Image> shadowBars;

	// Token: 0x040034BB RID: 13499
	private HoverTextDrawer.Pool<Image> selectBorders;

	// Token: 0x020018F8 RID: 6392
	[Serializable]
	public class Skin
	{
		// Token: 0x04007392 RID: 29586
		public Vector2 baseOffset;

		// Token: 0x04007393 RID: 29587
		public LocText textWidget;

		// Token: 0x04007394 RID: 29588
		public Image iconWidget;

		// Token: 0x04007395 RID: 29589
		public Vector2 shadowImageOffset;

		// Token: 0x04007396 RID: 29590
		public Color shadowImageColor;

		// Token: 0x04007397 RID: 29591
		public Image shadowBarWidget;

		// Token: 0x04007398 RID: 29592
		public Image selectBorderWidget;

		// Token: 0x04007399 RID: 29593
		public Vector2 shadowBarBorder;

		// Token: 0x0400739A RID: 29594
		public Vector2 selectBorder;

		// Token: 0x0400739B RID: 29595
		public bool drawWidgets;

		// Token: 0x0400739C RID: 29596
		public bool enableProfiling;

		// Token: 0x0400739D RID: 29597
		public bool enableDebugOffset;

		// Token: 0x0400739E RID: 29598
		public bool drawInProgressHoverText;

		// Token: 0x0400739F RID: 29599
		public Vector2 debugOffset;
	}

	// Token: 0x020018F9 RID: 6393
	private class Pool<WidgetType> where WidgetType : MonoBehaviour
	{
		// Token: 0x0600935B RID: 37723 RVA: 0x0032F184 File Offset: 0x0032D384
		public Pool(GameObject prefab, RectTransform master_root)
		{
			this.prefab = prefab;
			GameObject gameObject = new GameObject(typeof(WidgetType).Name);
			this.root = gameObject.AddComponent<RectTransform>();
			this.root.SetParent(master_root);
			this.root.anchoredPosition = Vector2.zero;
			this.root.anchorMin = Vector2.zero;
			this.root.anchorMax = Vector2.one;
			this.root.sizeDelta = Vector2.zero;
			gameObject.AddComponent<CanvasGroup>();
		}

		// Token: 0x0600935C RID: 37724 RVA: 0x0032F220 File Offset: 0x0032D420
		public HoverTextDrawer.Pool<WidgetType>.Entry Draw(Vector2 pos)
		{
			HoverTextDrawer.Pool<WidgetType>.Entry entry;
			if (this.drawnWidgets < this.entries.Count)
			{
				entry = this.entries[this.drawnWidgets];
				if (!entry.widget.gameObject.activeSelf)
				{
					entry.widget.gameObject.SetActive(true);
				}
			}
			else
			{
				GameObject gameObject = Util.KInstantiateUI(this.prefab, this.root.gameObject, false);
				gameObject.SetActive(true);
				entry.widget = gameObject.GetComponent<WidgetType>();
				entry.rect = gameObject.GetComponent<RectTransform>();
				this.entries.Add(entry);
			}
			entry.rect.anchoredPosition = new Vector2(pos.x, pos.y);
			this.drawnWidgets++;
			return entry;
		}

		// Token: 0x0600935D RID: 37725 RVA: 0x0032F2F1 File Offset: 0x0032D4F1
		public void BeginDrawing()
		{
			this.drawnWidgets = 0;
		}

		// Token: 0x0600935E RID: 37726 RVA: 0x0032F2FC File Offset: 0x0032D4FC
		public void EndDrawing()
		{
			for (int i = this.drawnWidgets; i < this.entries.Count; i++)
			{
				if (this.entries[i].widget.gameObject.activeSelf)
				{
					this.entries[i].widget.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600935F RID: 37727 RVA: 0x0032F367 File Offset: 0x0032D567
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.root.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				return;
			}
			this.root.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}

		// Token: 0x06009360 RID: 37728 RVA: 0x0032F3A4 File Offset: 0x0032D5A4
		public void Cleanup()
		{
			foreach (HoverTextDrawer.Pool<WidgetType>.Entry entry in this.entries)
			{
				UnityEngine.Object.Destroy(entry.widget.gameObject);
			}
			this.entries.Clear();
		}

		// Token: 0x040073A0 RID: 29600
		private GameObject prefab;

		// Token: 0x040073A1 RID: 29601
		private RectTransform root;

		// Token: 0x040073A2 RID: 29602
		private List<HoverTextDrawer.Pool<WidgetType>.Entry> entries = new List<HoverTextDrawer.Pool<WidgetType>.Entry>();

		// Token: 0x040073A3 RID: 29603
		private int drawnWidgets;

		// Token: 0x0200220F RID: 8719
		public struct Entry
		{
			// Token: 0x0400987F RID: 39039
			public WidgetType widget;

			// Token: 0x04009880 RID: 39040
			public RectTransform rect;
		}
	}
}
