﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02000B0A RID: 2826
[AddComponentMenu("KMonoBehaviour/scripts/GraphBase")]
public class GraphBase : KMonoBehaviour
{
	// Token: 0x0600573D RID: 22333 RVA: 0x001FE4E4 File Offset: 0x001FC6E4
	public Vector2 GetRelativePosition(Vector2 absolute_point)
	{
		Vector2 zero = Vector2.zero;
		float num = Mathf.Max(1f, this.axis_x.max_value - this.axis_x.min_value);
		float num2 = absolute_point.x - this.axis_x.min_value;
		zero.x = num2 / num;
		float num3 = Mathf.Max(1f, this.axis_y.max_value - this.axis_y.min_value);
		float num4 = absolute_point.y - this.axis_y.min_value;
		zero.y = num4 / num3;
		return zero;
	}

	// Token: 0x0600573E RID: 22334 RVA: 0x001FE578 File Offset: 0x001FC778
	public Vector2 GetRelativeSize(Vector2 absolute_size)
	{
		return this.GetRelativePosition(absolute_size);
	}

	// Token: 0x0600573F RID: 22335 RVA: 0x001FE581 File Offset: 0x001FC781
	public void ClearGuides()
	{
		this.ClearVerticalGuides();
		this.ClearHorizontalGuides();
	}

	// Token: 0x06005740 RID: 22336 RVA: 0x001FE590 File Offset: 0x001FC790
	public void ClearHorizontalGuides()
	{
		foreach (GameObject gameObject in this.horizontalGuides)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
		this.horizontalGuides.Clear();
	}

	// Token: 0x06005741 RID: 22337 RVA: 0x001FE5F8 File Offset: 0x001FC7F8
	public void ClearVerticalGuides()
	{
		foreach (GameObject gameObject in this.verticalGuides)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
		this.verticalGuides.Clear();
	}

	// Token: 0x06005742 RID: 22338 RVA: 0x001FE660 File Offset: 0x001FC860
	public void RefreshGuides()
	{
		this.ClearGuides();
		this.RefreshHorizontalGuides();
		this.RefreshVerticalGuides();
	}

	// Token: 0x06005743 RID: 22339 RVA: 0x001FE674 File Offset: 0x001FC874
	public void RefreshHorizontalGuides()
	{
		if (this.prefab_guide_x != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_guide_x, this.guides_x, true);
			gameObject.name = "guides_horizontal";
			Vector2[] array = new Vector2[2 * (int)(this.axis_y.range / this.axis_y.guide_frequency)];
			for (int i = 0; i < array.Length; i += 2)
			{
				Vector2 absolute_point = new Vector2(this.axis_x.min_value, (float)i * (this.axis_y.guide_frequency / 2f));
				array[i] = this.GetRelativePosition(absolute_point);
				Vector2 absolute_point2 = new Vector2(this.axis_x.max_value, (float)i * (this.axis_y.guide_frequency / 2f));
				array[i + 1] = this.GetRelativePosition(absolute_point2);
				if (this.prefab_guide_horizontal_label != null)
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.prefab_guide_horizontal_label, gameObject, true);
					gameObject2.GetComponent<LocText>().alignment = TextAlignmentOptions.MidlineLeft;
					gameObject2.GetComponent<LocText>().text = ((int)this.axis_y.guide_frequency * (i / 2)).ToString();
					gameObject2.rectTransform().SetLocalPosition(new Vector2(8f, (float)i * (base.gameObject.rectTransform().rect.height / (float)array.Length)) - base.gameObject.rectTransform().rect.size / 2f);
				}
			}
			gameObject.GetComponent<UILineRenderer>().Points = array;
			this.horizontalGuides.Add(gameObject);
		}
	}

	// Token: 0x06005744 RID: 22340 RVA: 0x001FE81C File Offset: 0x001FCA1C
	public void RefreshVerticalGuides()
	{
		if (this.prefab_guide_y != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_guide_y, this.guides_y, true);
			gameObject.name = "guides_vertical";
			Vector2[] array = new Vector2[2 * (int)(this.axis_x.range / this.axis_x.guide_frequency)];
			for (int i = 0; i < array.Length; i += 2)
			{
				Vector2 absolute_point = new Vector2((float)i * (this.axis_x.guide_frequency / 2f), this.axis_y.min_value);
				array[i] = this.GetRelativePosition(absolute_point);
				Vector2 absolute_point2 = new Vector2((float)i * (this.axis_x.guide_frequency / 2f), this.axis_y.max_value);
				array[i + 1] = this.GetRelativePosition(absolute_point2);
				if (this.prefab_guide_vertical_label != null)
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.prefab_guide_vertical_label, gameObject, true);
					gameObject2.GetComponent<LocText>().alignment = TextAlignmentOptions.Bottom;
					gameObject2.GetComponent<LocText>().text = ((int)this.axis_x.guide_frequency * (i / 2)).ToString();
					gameObject2.rectTransform().SetLocalPosition(new Vector2((float)i * (base.gameObject.rectTransform().rect.width / (float)array.Length), 4f) - base.gameObject.rectTransform().rect.size / 2f);
				}
			}
			gameObject.GetComponent<UILineRenderer>().Points = array;
			this.verticalGuides.Add(gameObject);
		}
	}

	// Token: 0x04003ADA RID: 15066
	[Header("Axis")]
	public GraphAxis axis_x;

	// Token: 0x04003ADB RID: 15067
	public GraphAxis axis_y;

	// Token: 0x04003ADC RID: 15068
	[Header("References")]
	public GameObject prefab_guide_x;

	// Token: 0x04003ADD RID: 15069
	public GameObject prefab_guide_y;

	// Token: 0x04003ADE RID: 15070
	public GameObject prefab_guide_horizontal_label;

	// Token: 0x04003ADF RID: 15071
	public GameObject prefab_guide_vertical_label;

	// Token: 0x04003AE0 RID: 15072
	public GameObject guides_x;

	// Token: 0x04003AE1 RID: 15073
	public GameObject guides_y;

	// Token: 0x04003AE2 RID: 15074
	public LocText label_title;

	// Token: 0x04003AE3 RID: 15075
	public LocText label_x;

	// Token: 0x04003AE4 RID: 15076
	public LocText label_y;

	// Token: 0x04003AE5 RID: 15077
	public string graphName;

	// Token: 0x04003AE6 RID: 15078
	protected List<GameObject> horizontalGuides = new List<GameObject>();

	// Token: 0x04003AE7 RID: 15079
	protected List<GameObject> verticalGuides = new List<GameObject>();

	// Token: 0x04003AE8 RID: 15080
	private const int points_per_guide_line = 2;
}
