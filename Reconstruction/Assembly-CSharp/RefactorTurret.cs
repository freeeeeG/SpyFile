using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class RefactorTurret : TurretContent
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06000D6D RID: 3437 RVA: 0x00022C28 File Offset: 0x00020E28
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.RefactorTurret;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00022C2B File Offset: 0x00020E2B
	public override bool CanEquip
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x00022C2E File Offset: 0x00020E2E
	public override void OnSwitch()
	{
		base.OnSwitch();
		Singleton<GameManager>.Instance.refactorTurrets.Remove(this);
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00022C46 File Offset: 0x00020E46
	public override void ContentLanded()
	{
		Singleton<GameManager>.Instance.refactorTurrets.Add(this);
		base.m_GameTile.tag = StaticData.OnlyRefactorTag;
		base.ContentLanded();
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x00022C70 File Offset: 0x00020E70
	public override void SaveContent(out ContentStruct contentStruct)
	{
		base.SaveContent(out contentStruct);
		contentStruct = this.m_ContentStruct;
		this.m_ContentStruct.ContentName = this.Strategy.Attribute.Name;
		this.m_ContentStruct.Quality = this.Strategy.Quality;
		this.m_ContentStruct.ExtraSlot = this.Strategy.PrivateExtraSlot;
		this.m_ContentStruct.SkillList = new Dictionary<string, List<int>>();
		this.m_ContentStruct.ElementsList = new Dictionary<string, List<int>>();
		this.m_ContentStruct.IsException = new Dictionary<string, bool>();
		for (int i = 1; i < this.Strategy.TurretSkills.Count; i++)
		{
			ElementSkill elementSkill = this.Strategy.TurretSkills[i] as ElementSkill;
			this.m_ContentStruct.SkillList.Add(i.ToString(), elementSkill.InitElements);
			this.m_ContentStruct.ElementsList.Add(i.ToString(), elementSkill.Elements);
			this.m_ContentStruct.IsException.Add(i.ToString(), elementSkill.IsException);
		}
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x00022D90 File Offset: 0x00020F90
	protected override void ContentLandedCheck(Collider2D col)
	{
		this.ShowLandedEffect();
		if (col != null)
		{
			GameTile component = col.GetComponent<GameTile>();
			if (component.Content.CanEquip)
			{
				ConcreteContent concreteContent = component.Content as ConcreteContent;
				for (int i = 1; i < this.Strategy.TurretSkills.Count; i++)
				{
					ElementSkill elementSkill = (ElementSkill)this.Strategy.TurretSkills[i];
					concreteContent.Strategy.AddElementSkill(elementSkill);
					elementSkill.OnEquip();
				}
				((BasicTile)concreteContent.m_GameTile).EquipTurret(concreteContent.Strategy.TurretSkills.Count - 1);
				BoardSystem.PreviewEquipTile = null;
				Singleton<ObjectPool>.Instance.UnSpawn(base.m_GameTile);
				TechnologySystem.OnRefactor(concreteContent.Strategy);
				return;
			}
			Singleton<ObjectPool>.Instance.UnSpawn(component);
		}
		if (!base.IsSwitching)
		{
			GameRes.TotalLandedRefactor++;
			TechnologySystem.OnRefactor(this.Strategy);
		}
		((BasicTile)base.m_GameTile).EquipTurret(this.Strategy.TurretSkills.Count - 1);
		base.IsSwitching = false;
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x00022EB0 File Offset: 0x000210B0
	public void ShowLandedEffect()
	{
		Singleton<ObjectPool>.Instance.Spawn(Singleton<StaticData>.Instance.LandedEffect).transform.position = base.transform.position + Vector3.up * 0.2f;
		Singleton<Sound>.Instance.PlayEffect("Sound_Landed");
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x00022F09 File Offset: 0x00021109
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		Singleton<GameManager>.Instance.refactorTurrets.Remove(this);
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00022F21 File Offset: 0x00021121
	protected override void UndoUnSwitching()
	{
		base.UndoUnSwitching();
	}
}
