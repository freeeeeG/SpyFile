using System;
using UnityEngine;

// Token: 0x02000A66 RID: 2662
public class HoverTextScreen : KScreen
{
	// Token: 0x0600507E RID: 20606 RVA: 0x001C8823 File Offset: 0x001C6A23
	public static void DestroyInstance()
	{
		HoverTextScreen.Instance = null;
	}

	// Token: 0x0600507F RID: 20607 RVA: 0x001C882B File Offset: 0x001C6A2B
	protected override void OnActivate()
	{
		base.OnActivate();
		HoverTextScreen.Instance = this;
		this.drawer = new HoverTextDrawer(this.skin.skin, base.GetComponent<RectTransform>());
	}

	// Token: 0x06005080 RID: 20608 RVA: 0x001C8858 File Offset: 0x001C6A58
	public HoverTextDrawer BeginDrawing()
	{
		Vector2 zero = Vector2.zero;
		Vector2 screenPoint = KInputManager.GetMousePos();
		RectTransform rectTransform = base.transform.parent as RectTransform;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, base.transform.parent.GetComponent<Canvas>().worldCamera, out zero);
		zero.x += rectTransform.sizeDelta.x / 2f;
		zero.y -= rectTransform.sizeDelta.y / 2f;
		this.drawer.BeginDrawing(zero);
		return this.drawer;
	}

	// Token: 0x06005081 RID: 20609 RVA: 0x001C88F0 File Offset: 0x001C6AF0
	private void Update()
	{
		bool enabled = PlayerController.Instance.ActiveTool.ShowHoverUI();
		this.drawer.SetEnabled(enabled);
	}

	// Token: 0x06005082 RID: 20610 RVA: 0x001C891C File Offset: 0x001C6B1C
	public Sprite GetSprite(string byName)
	{
		foreach (Sprite sprite in this.HoverIcons)
		{
			if (sprite != null && sprite.name == byName)
			{
				return sprite;
			}
		}
		global::Debug.LogWarning("No icon named " + byName + " was found on HoverTextScreen.prefab");
		return null;
	}

	// Token: 0x06005083 RID: 20611 RVA: 0x001C8971 File Offset: 0x001C6B71
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.drawer.Cleanup();
	}

	// Token: 0x040034BC RID: 13500
	[SerializeField]
	private HoverTextSkin skin;

	// Token: 0x040034BD RID: 13501
	public Sprite[] HoverIcons;

	// Token: 0x040034BE RID: 13502
	public HoverTextDrawer drawer;

	// Token: 0x040034BF RID: 13503
	public static HoverTextScreen Instance;
}
