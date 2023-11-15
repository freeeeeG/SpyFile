using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEC RID: 2796
[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
	// Token: 0x06005621 RID: 22049 RVA: 0x001F5A67 File Offset: 0x001F3C67
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x06005622 RID: 22050 RVA: 0x001F5A78 File Offset: 0x001F3C78
	public void SetDescriptors(IList<Descriptor> descriptors)
	{
		int i;
		for (i = 0; i < descriptors.Count; i++)
		{
			GameObject gameObject;
			if (i >= this.labels.Count)
			{
				gameObject = Util.KInstantiate((this.customLabelPrefab != null) ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, base.gameObject, null);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.labels.Add(gameObject);
			}
			else
			{
				gameObject = this.labels[i];
			}
			gameObject.GetComponent<LocText>().text = descriptors[i].IndentedText();
			gameObject.GetComponent<ToolTip>().toolTip = descriptors[i].tooltipText;
			gameObject.SetActive(true);
		}
		while (i < this.labels.Count)
		{
			this.labels[i].SetActive(false);
			i++;
		}
	}

	// Token: 0x040039E3 RID: 14819
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x040039E4 RID: 14820
	private List<GameObject> labels = new List<GameObject>();
}
