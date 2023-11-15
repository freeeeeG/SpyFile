using System;
using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B48 RID: 2888
public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x0600590B RID: 22795 RVA: 0x002099FD File Offset: 0x00207BFD
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600590C RID: 22796 RVA: 0x00209A05 File Offset: 0x00207C05
	public void ConfigureSetup()
	{
	}

	// Token: 0x0600590D RID: 22797 RVA: 0x00209A07 File Offset: 0x00207C07
	public void ConfigureWith(PermitResource permit)
	{
		this.sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
		this.editorOnlyErrorMessageParent.gameObject.SetActive(false);
	}

	// Token: 0x0600590E RID: 22798 RVA: 0x00209A2A File Offset: 0x00207C2A
	public KleiPermitDioramaVis_Fallback WithError(string error)
	{
		this.error = error;
		global::Debug.Log("[KleiInventoryScreen Error] Had to use fallback vis. " + error);
		return this;
	}

	// Token: 0x04003C39 RID: 15417
	[SerializeField]
	private Image sprite;

	// Token: 0x04003C3A RID: 15418
	[SerializeField]
	private RectTransform editorOnlyErrorMessageParent;

	// Token: 0x04003C3B RID: 15419
	[SerializeField]
	private TextMeshProUGUI editorOnlyErrorMessageText;

	// Token: 0x04003C3C RID: 15420
	private Option<string> error;
}
