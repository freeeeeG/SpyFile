using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
	// Token: 0x060058B2 RID: 22706 RVA: 0x00208048 File Offset: 0x00206248
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.ResetState();
		this.equipmentVis.gameObject.SetActive(false);
		this.fallbackVis.gameObject.SetActive(false);
		if (info.UseEquipmentVis)
		{
			this.equipmentVis.gameObject.SetActive(true);
			this.equipmentVis.ConfigureWith(info);
			return;
		}
		this.fallbackVis.gameObject.SetActive(true);
		this.fallbackVis.ConfigureWith(info);
	}

	// Token: 0x060058B3 RID: 22707 RVA: 0x002080C0 File Offset: 0x002062C0
	public Promise AnimateIn()
	{
		return Updater.RunRoutine(this, this.AnimateInRoutine());
	}

	// Token: 0x060058B4 RID: 22708 RVA: 0x002080CE File Offset: 0x002062CE
	public Promise AnimateOut()
	{
		return Updater.RunRoutine(this, this.AnimateOutRoutine());
	}

	// Token: 0x060058B5 RID: 22709 RVA: 0x002080DC File Offset: 0x002062DC
	private IEnumerator AnimateInRoutine()
	{
		this.root.gameObject.SetActive(true);
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack);
		yield break;
	}

	// Token: 0x060058B6 RID: 22710 RVA: 0x002080EB File Offset: 0x002062EB
	private IEnumerator AnimateOutRoutine()
	{
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.zero, 0.25f, null);
		this.root.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x060058B7 RID: 22711 RVA: 0x002080FA File Offset: 0x002062FA
	public void ResetState()
	{
		this.root.transform.localScale = Vector3.zero;
	}

	// Token: 0x04003C0B RID: 15371
	[SerializeField]
	private RectTransform root;

	// Token: 0x04003C0C RID: 15372
	[Header("Different Permit Visualizers")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis_Fallback fallbackVis;

	// Token: 0x04003C0D RID: 15373
	[SerializeField]
	private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;
}
