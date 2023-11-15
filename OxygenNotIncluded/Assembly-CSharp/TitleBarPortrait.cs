using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C7C RID: 3196
[AddComponentMenu("KMonoBehaviour/scripts/TitleBarPortrait")]
public class TitleBarPortrait : KMonoBehaviour
{
	// Token: 0x060065EB RID: 26091 RVA: 0x002608E9 File Offset: 0x0025EAE9
	public void SetSaturation(bool saturated)
	{
		this.ImageObject.GetComponent<Image>().material = (saturated ? this.DefaultMaterial : this.DesatMaterial);
	}

	// Token: 0x060065EC RID: 26092 RVA: 0x0026090C File Offset: 0x0025EB0C
	public void SetPortrait(GameObject selectedTarget)
	{
		MinionIdentity component = selectedTarget.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.SetPortrait(component);
			return;
		}
		Building component2 = selectedTarget.GetComponent<Building>();
		if (component2 != null)
		{
			this.SetPortrait(component2.Def.GetUISprite("ui", false));
			return;
		}
		MeshRenderer componentInChildren = selectedTarget.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren)
		{
			this.SetPortrait(Sprite.Create((Texture2D)componentInChildren.material.mainTexture, new Rect(0f, 0f, (float)componentInChildren.material.mainTexture.width, (float)componentInChildren.material.mainTexture.height), new Vector2(0.5f, 0.5f)));
		}
	}

	// Token: 0x060065ED RID: 26093 RVA: 0x002609C4 File Offset: 0x0025EBC4
	public void SetPortrait(Sprite image)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(true);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
		if (image == null)
		{
			this.ClearPortrait();
			return;
		}
		this.ImageObject.GetComponent<Image>().sprite = image;
	}

	// Token: 0x060065EE RID: 26094 RVA: 0x00260A58 File Offset: 0x0025EC58
	private void SetPortrait(MinionIdentity identity)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		CrewPortrait component = base.GetComponent<CrewPortrait>();
		if (component != null)
		{
			component.SetIdentityObject(identity, true);
			return;
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(true);
			CrewPortrait.SetPortraitData(identity, this.AnimControllerObject.GetComponent<KBatchedAnimController>(), true);
		}
	}

	// Token: 0x060065EF RID: 26095 RVA: 0x00260AF4 File Offset: 0x0025ECF4
	public void ClearPortrait()
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(false);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
	}

	// Token: 0x04004631 RID: 17969
	public GameObject FaceObject;

	// Token: 0x04004632 RID: 17970
	public GameObject ImageObject;

	// Token: 0x04004633 RID: 17971
	public GameObject PortraitShadow;

	// Token: 0x04004634 RID: 17972
	public GameObject AnimControllerObject;

	// Token: 0x04004635 RID: 17973
	public Material DefaultMaterial;

	// Token: 0x04004636 RID: 17974
	public Material DesatMaterial;
}
