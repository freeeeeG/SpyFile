using System;
using Characters;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class AdventurerHealthBarUIController : MonoBehaviour
{
	// Token: 0x060002D3 RID: 723 RVA: 0x0000B214 File Offset: 0x00009414
	public void Initialize(Character character, AdventurerHealthBarUIController.AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerHealthBarUIController.AdventurerClass.Hero:
			this._hero.GetComponent<CharacterHealthBar>().Initialize(character);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Archer:
			this._archer.GetComponent<CharacterHealthBar>().Initialize(character);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Cleric:
			this._cleric.GetComponent<CharacterHealthBar>().Initialize(character);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Warrior:
			this._warrior.GetComponent<CharacterHealthBar>().Initialize(character);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Thief:
			this._thief.GetComponent<CharacterHealthBar>().Initialize(character);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Magician:
			this._magician.GetComponent<CharacterHealthBar>().Initialize(character);
			return;
		default:
			return;
		}
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0000B2AC File Offset: 0x000094AC
	public void ShowHealthBarOf(AdventurerHealthBarUIController.AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerHealthBarUIController.AdventurerClass.Hero:
			this._hero.gameObject.SetActive(true);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Archer:
			this._archer.gameObject.SetActive(true);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Cleric:
			this._cleric.gameObject.SetActive(true);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Warrior:
			this._warrior.gameObject.SetActive(true);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Thief:
			this._thief.gameObject.SetActive(true);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Magician:
			this._magician.gameObject.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000B344 File Offset: 0x00009544
	public void HideHealthBarOf(AdventurerHealthBarUIController.AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerHealthBarUIController.AdventurerClass.Hero:
			this._hero.gameObject.SetActive(false);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Archer:
			this._archer.gameObject.SetActive(false);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Cleric:
			this._cleric.gameObject.SetActive(false);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Warrior:
			this._warrior.gameObject.SetActive(false);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Thief:
			this._thief.gameObject.SetActive(false);
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Magician:
			this._magician.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0000B3DC File Offset: 0x000095DC
	public void HideHealthBarAll()
	{
		AdventurerHealthBar hero = this._hero;
		if (hero != null)
		{
			hero.gameObject.SetActive(false);
		}
		AdventurerHealthBar archer = this._archer;
		if (archer != null)
		{
			archer.gameObject.SetActive(false);
		}
		AdventurerHealthBar cleric = this._cleric;
		if (cleric != null)
		{
			cleric.gameObject.SetActive(false);
		}
		AdventurerHealthBar warrior = this._warrior;
		if (warrior != null)
		{
			warrior.gameObject.SetActive(false);
		}
		AdventurerHealthBar thief = this._thief;
		if (thief != null)
		{
			thief.gameObject.SetActive(false);
		}
		AdventurerHealthBar magician = this._magician;
		if (magician == null)
		{
			return;
		}
		magician.gameObject.SetActive(false);
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000B474 File Offset: 0x00009674
	public void ShowDeadUIOf(AdventurerHealthBarUIController.AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerHealthBarUIController.AdventurerClass.Hero:
			this._hero.ShowDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Archer:
			this._archer.ShowDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Cleric:
			this._cleric.ShowDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Warrior:
			this._warrior.ShowDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Thief:
			this._thief.ShowDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Magician:
			this._magician.ShowDeadPortrait();
			return;
		default:
			return;
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000B4E8 File Offset: 0x000096E8
	public void HideDeadUIOf(AdventurerHealthBarUIController.AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerHealthBarUIController.AdventurerClass.Hero:
			this._hero.HideDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Archer:
			this._archer.HideDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Cleric:
			this._cleric.HideDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Warrior:
			this._warrior.HideDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Thief:
			this._thief.HideDeadPortrait();
			return;
		case AdventurerHealthBarUIController.AdventurerClass.Magician:
			this._magician.HideDeadPortrait();
			return;
		default:
			return;
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000B55C File Offset: 0x0000975C
	public void HideDeadUIAll()
	{
		AdventurerHealthBar hero = this._hero;
		if (hero != null)
		{
			hero.HideDeadPortrait();
		}
		AdventurerHealthBar archer = this._archer;
		if (archer != null)
		{
			archer.HideDeadPortrait();
		}
		AdventurerHealthBar cleric = this._cleric;
		if (cleric != null)
		{
			cleric.HideDeadPortrait();
		}
		AdventurerHealthBar warrior = this._warrior;
		if (warrior != null)
		{
			warrior.HideDeadPortrait();
		}
		AdventurerHealthBar thief = this._thief;
		if (thief != null)
		{
			thief.HideDeadPortrait();
		}
		AdventurerHealthBar magician = this._magician;
		if (magician == null)
		{
			return;
		}
		magician.HideDeadPortrait();
	}

	// Token: 0x04000258 RID: 600
	[SerializeField]
	private AdventurerHealthBar _hero;

	// Token: 0x04000259 RID: 601
	[SerializeField]
	private AdventurerHealthBar _archer;

	// Token: 0x0400025A RID: 602
	[SerializeField]
	private AdventurerHealthBar _cleric;

	// Token: 0x0400025B RID: 603
	[SerializeField]
	private AdventurerHealthBar _warrior;

	// Token: 0x0400025C RID: 604
	[SerializeField]
	private AdventurerHealthBar _thief;

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private AdventurerHealthBar _magician;

	// Token: 0x02000096 RID: 150
	public enum AdventurerClass
	{
		// Token: 0x0400025F RID: 607
		Hero,
		// Token: 0x04000260 RID: 608
		Archer,
		// Token: 0x04000261 RID: 609
		Cleric,
		// Token: 0x04000262 RID: 610
		Warrior,
		// Token: 0x04000263 RID: 611
		Thief,
		// Token: 0x04000264 RID: 612
		Magician
	}
}
