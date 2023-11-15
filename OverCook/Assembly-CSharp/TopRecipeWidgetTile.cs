using System;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
public class TopRecipeWidgetTile : RecipeWidgetTile
{
	// Token: 0x0600399A RID: 14746 RVA: 0x001113FA File Offset: 0x0010F7FA
	public void SetProgress(float _value)
	{
		this.m_value = _value;
		if (this.m_progressBar != null)
		{
			this.m_progressBar.SetValue(this.m_value);
		}
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x00111425 File Offset: 0x0010F825
	public float GetProgress()
	{
		return this.m_value;
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x00111430 File Offset: 0x0010F830
	protected override void OnCreateSubObjects(GameObject _container)
	{
		base.OnCreateSubObjects(_container);
		TopRecipeWidgetTile.TopDisplayConfiguration topDisplayConfiguration = this.m_displayConfig as TopRecipeWidgetTile.TopDisplayConfiguration;
		if (topDisplayConfiguration.m_hasProgressBar)
		{
			RectTransform rectTransform = base.CreateRect(_container, "ProgressBar");
			UIUtils.SetupFillParentAreaRect(rectTransform);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.SetSiblingIndex(1);
			rectTransform.anchorMax = topDisplayConfiguration.m_progressBarAnchorMax;
			rectTransform.anchorMin = topDisplayConfiguration.m_progressBarAnchorMin;
			rectTransform.offsetMin = topDisplayConfiguration.m_progressBarOffsetMin;
			rectTransform.offsetMax = topDisplayConfiguration.m_progressBarOffsetMax;
			this.m_progressBar = rectTransform.gameObject.AddComponent<ProgressBarUI>();
			this.m_progressBar.SetSprites(topDisplayConfiguration.m_progressUIBackground, topDisplayConfiguration.m_progressUIFill);
			this.m_progressBar.SetValue(this.m_value);
			this.m_progressBar.SetNotchs(new Sprite[]
			{
				topDisplayConfiguration.lowTipSprite,
				topDisplayConfiguration.highTipSprite
			}, topDisplayConfiguration.m_NotchOffsets, topDisplayConfiguration.notchColor);
			if (Application.isPlaying)
			{
				this.m_progressBar.RefreshSubElements();
			}
		}
	}

	// Token: 0x04002E49 RID: 11849
	[SerializeField]
	private ProgressBarUI m_progressBar;

	// Token: 0x04002E4A RID: 11850
	private float m_value = 1f;

	// Token: 0x02000B1E RID: 2846
	[Serializable]
	public class TopDisplayConfiguration : RecipeWidgetTile.DisplayConfiguration
	{
		// Token: 0x04002E4B RID: 11851
		[Header("ProgressBar")]
		public bool m_hasProgressBar = true;

		// Token: 0x04002E4C RID: 11852
		public Vector2 m_progressBarAnchorMin = Vector2.zero;

		// Token: 0x04002E4D RID: 11853
		public Vector2 m_progressBarAnchorMax = Vector2.one;

		// Token: 0x04002E4E RID: 11854
		public Vector2 m_progressBarOffsetMin = Vector2.zero;

		// Token: 0x04002E4F RID: 11855
		public Vector2 m_progressBarOffsetMax = Vector2.zero;

		// Token: 0x04002E50 RID: 11856
		public BaseProgressBarUI.ColourChangingImageConfig m_progressUIBackground;

		// Token: 0x04002E51 RID: 11857
		public BaseProgressBarUI.ColourChangingImageConfig m_progressUIFill;

		// Token: 0x04002E52 RID: 11858
		public Sprite lowTipSprite;

		// Token: 0x04002E53 RID: 11859
		public Sprite highTipSprite;

		// Token: 0x04002E54 RID: 11860
		public Color notchColor;

		// Token: 0x04002E55 RID: 11861
		public float[] m_NotchOffsets;
	}
}
