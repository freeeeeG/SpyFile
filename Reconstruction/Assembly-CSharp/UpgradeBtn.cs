using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200029F RID: 671
public class UpgradeBtn : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06001075 RID: 4213 RVA: 0x0002D55D File Offset: 0x0002B75D
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.turretTips.showingTurret = false;
		this.turretTips.UpdateLevelUpInfo();
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0002D576 File Offset: 0x0002B776
	public void OnPointerExit(PointerEventData eventData)
	{
		this.turretTips.showingTurret = true;
	}

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private TurretTips turretTips;
}
