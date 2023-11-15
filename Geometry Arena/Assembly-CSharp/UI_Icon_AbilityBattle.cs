using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000081 RID: 129
public class UI_Icon_AbilityBattle : UI_Icon
{
	// Token: 0x06000493 RID: 1171 RVA: 0x0001C53D File Offset: 0x0001A73D
	public void Init(int i)
	{
		this.abilityId = i;
		this.UpdateNum();
		base.TextSetNormal();
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0001C554 File Offset: 0x0001A754
	public void UpdateNum()
	{
		Factor playerFactorTotal = Player.inst.unit.playerFactorTotal;
		LanguageText inst = LanguageText.Inst;
		this.textName.text = inst.factor[this.abilityId];
		this.textNum.text = playerFactorTotal.GetActualFactorInfo(this.abilityId);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001C5A6 File Offset: 0x0001A7A6
	private void Update()
	{
		if (this.flagMouseAbove)
		{
			UI_ToolTip.inst.ShowWithAbilityBattle(this.abilityId);
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0001C5C0 File Offset: 0x0001A7C0
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithAbilityBattle(this.abilityId);
		base.TextSetHighlight();
		this.flagMouseAbove = true;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0001C5DF File Offset: 0x0001A7DF
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
		this.flagMouseAbove = false;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x040003E6 RID: 998
	public int abilityId;

	// Token: 0x040003E7 RID: 999
	public Text textName;

	// Token: 0x040003E8 RID: 1000
	public Text textNum;

	// Token: 0x040003E9 RID: 1001
	private bool flagMouseAbove;
}
