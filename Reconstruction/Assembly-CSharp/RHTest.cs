using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CB RID: 459
public class RHTest : MonoBehaviour
{
	// Token: 0x06000BA8 RID: 2984 RVA: 0x0001E6DC File Offset: 0x0001C8DC
	private void Start()
	{
		this.compositeInputField.text = "F1";
		this.e1.text = "0";
		this.e2.text = "0";
		this.e3.text = "0";
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0001E729 File Offset: 0x0001C929
	public void MenuBtnClick()
	{
		if (Singleton<Game>.Instance.TestMode)
		{
			this.panel.SetActive(!this.panel.activeSelf);
		}
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x0001E750 File Offset: 0x0001C950
	public void GetMoneyClick()
	{
		Singleton<GameManager>.Instance.GainMoney(int.Parse(this.moneyInputField.text));
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0001E76C File Offset: 0x0001C96C
	public void GetCompositeClick()
	{
		ConstructHelper.GetRefactorTurretByNameAndElement(this.compositeInputField.text, int.Parse(this.e1.text), int.Parse(this.e2.text), int.Parse(this.e3.text));
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x0001E7BA File Offset: 0x0001C9BA
	public void GetElementClick()
	{
		ConstructHelper.GetElementTurretByQualityAndElement((ElementType)int.Parse(this.elementInputField.text), int.Parse(this.qualityInputField.text));
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0001E7E2 File Offset: 0x0001C9E2
	public void GetTrapClick()
	{
		ConstructHelper.GetTrapShapeByName(this.trapInputField.text);
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0001E7F5 File Offset: 0x0001C9F5
	public void GetLifeBtnClick()
	{
		GameRes.Life += int.Parse(this.lifeInputField.text);
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0001E812 File Offset: 0x0001CA12
	public void GetTechBtnClick()
	{
		GameRes.AbnormalRate = 1f;
		Singleton<GameManager>.Instance.ShowTechSelect(true, false);
	}

	// Token: 0x040005D3 RID: 1491
	[SerializeField]
	private GameObject panel;

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	private InputField moneyInputField;

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	private InputField lifeInputField;

	// Token: 0x040005D6 RID: 1494
	[SerializeField]
	private InputField compositeInputField;

	// Token: 0x040005D7 RID: 1495
	[SerializeField]
	private InputField e1;

	// Token: 0x040005D8 RID: 1496
	[SerializeField]
	private InputField e2;

	// Token: 0x040005D9 RID: 1497
	[SerializeField]
	private InputField e3;

	// Token: 0x040005DA RID: 1498
	[SerializeField]
	private InputField qualityInputField;

	// Token: 0x040005DB RID: 1499
	[SerializeField]
	private InputField elementInputField;

	// Token: 0x040005DC RID: 1500
	[SerializeField]
	private InputField trapInputField;
}
