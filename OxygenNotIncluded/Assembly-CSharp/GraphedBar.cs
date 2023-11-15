using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B0D RID: 2829
[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
	// Token: 0x0600574B RID: 22347 RVA: 0x001FEB8F File Offset: 0x001FCD8F
	public void SetFormat(GraphedBarFormatting format)
	{
		this.format = format;
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x001FEB98 File Offset: 0x001FCD98
	public void SetValues(int[] values, float x_position)
	{
		this.ClearValues();
		base.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0f);
		base.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
		base.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.format.width);
		for (int i = 0; i < values.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredHeight = (float)values[i];
			component.minWidth = (float)this.format.width;
			gameObject.GetComponent<Image>().color = this.format.colors[i % this.format.colors.Length];
			this.segments.Add(gameObject);
		}
	}

	// Token: 0x0600574D RID: 22349 RVA: 0x001FEC78 File Offset: 0x001FCE78
	public void ClearValues()
	{
		foreach (GameObject obj in this.segments)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		this.segments.Clear();
	}

	// Token: 0x04003AF1 RID: 15089
	public GameObject segments_container;

	// Token: 0x04003AF2 RID: 15090
	public GameObject prefab_segment;

	// Token: 0x04003AF3 RID: 15091
	private List<GameObject> segments = new List<GameObject>();

	// Token: 0x04003AF4 RID: 15092
	private GraphedBarFormatting format;
}
