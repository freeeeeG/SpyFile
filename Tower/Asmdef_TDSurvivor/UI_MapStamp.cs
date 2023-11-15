using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AE RID: 174
public class UI_MapStamp : MonoBehaviour
{
	// Token: 0x060003AC RID: 940 RVA: 0x0000E8F8 File Offset: 0x0000CAF8
	public void Stamp()
	{
		foreach (Image image in this.list_MapIcons)
		{
			image.enabled = true;
			Collider component = image.GetComponent<Collider>();
			component.enabled = false;
			RaycastHit raycastHit;
			if (Physics.Raycast(image.transform.position - Vector3.forward * 10f, Vector3.forward, out raycastHit))
			{
				image.enabled = false;
				Debug.DrawLine(image.transform.position - Vector3.forward * 10f, image.transform.position + Vector3.forward * 50f, Color.red, 60f);
			}
			else
			{
				component.enabled = true;
				Debug.DrawLine(image.transform.position - Vector3.forward * 10f, image.transform.position + Vector3.forward * 50f, Color.green, 60f);
			}
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0000EA44 File Offset: 0x0000CC44
	private void FetchReferences()
	{
		this.list_MapIcons = new List<Image>();
		this.list_Colliders = new List<Collider>();
		foreach (Image image in base.GetComponentsInChildren<Image>())
		{
			this.list_MapIcons.Add(image);
			this.list_Colliders.Add(image.GetComponent<Collider>());
		}
	}

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private List<Image> list_MapIcons;

	// Token: 0x040003CA RID: 970
	private List<Collider> list_Colliders;
}
