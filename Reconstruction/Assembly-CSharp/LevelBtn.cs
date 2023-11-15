using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000236 RID: 566
public class LevelBtn : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000EA3 RID: 3747 RVA: 0x00025CFB File Offset: 0x00023EFB
	private void Start()
	{
		this.infoPanel.Initialize();
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x00025D08 File Offset: 0x00023F08
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.infoPanel.Show();
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x00025D15 File Offset: 0x00023F15
	public void OnPointerExit(PointerEventData eventData)
	{
		this.infoPanel.Hide();
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x00025D24 File Offset: 0x00023F24
	public void LevelBtnClick()
	{
		if (GameRes.SystemLevel < Singleton<StaticData>.Instance.SystemMaxLevel && Singleton<GameManager>.Instance.ConsumeMoney(GameRes.SystemUpgradeCost))
		{
			GameRes.SystemLevel++;
			if (GameRes.SystemLevel == 2 || GameRes.SystemLevel == 4 || GameRes.SystemLevel == 6)
			{
				GameRes.ShopCapacity++;
			}
			this.LevelUpPartical.Play();
			Singleton<Sound>.Instance.PlayUISound("LevelUp");
			this.infoPanel.SetInfo();
			Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.SystemBtnClick);
		}
	}

	// Token: 0x04000708 RID: 1800
	[SerializeField]
	private LevelInfoPanel infoPanel;

	// Token: 0x04000709 RID: 1801
	[SerializeField]
	private ParticleSystem LevelUpPartical;
}
