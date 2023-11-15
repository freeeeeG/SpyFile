using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200017C RID: 380
public class UI_Obj_StageUpgradeItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00025904 File Offset: 0x00023B04
	public UI_StageUpgradeUnlockPopup.eUpgradeType UpgradeType
	{
		get
		{
			return this.upgradeType;
		}
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0002590C File Offset: 0x00023B0C
	public void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0002591F File Offset: 0x00023B1F
	public void PlaySelectedAnimation()
	{
		this.particle_SelectedEffect.Play();
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0002592C File Offset: 0x00023B2C
	public void ShakeEffect(float duration, float strengthMultiplier, float delay)
	{
		this.node_icon.DOShakePosition(duration, strengthMultiplier * 0.1f, 10, 90f, false, true, ShakeRandomnessMode.Harmonic).SetDelay(delay);
		this.node_icon.DOShakeRotation(duration, strengthMultiplier * 5f, 10, 90f, true, ShakeRandomnessMode.Harmonic).SetDelay(delay);
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00025980 File Offset: 0x00023B80
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundManager.PlaySound("UI", "StageUpgrade_ItemShow", -1f, -1f, -1f);
		this.ShakeEffect(0.5f, 1f, 0f);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x000259B6 File Offset: 0x00023BB6
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x040007C4 RID: 1988
	[SerializeField]
	private Animator animator;

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	private Transform node_icon;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	private ParticleSystem particle_SelectedEffect;

	// Token: 0x040007C7 RID: 1991
	[SerializeField]
	private UI_StageUpgradeUnlockPopup.eUpgradeType upgradeType;
}
