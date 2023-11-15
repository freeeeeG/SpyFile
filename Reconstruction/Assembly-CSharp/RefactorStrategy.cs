using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200022A RID: 554
public class RefactorStrategy : StrategyBase
{
	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06000D8C RID: 3468 RVA: 0x000230B1 File Offset: 0x000212B1
	// (set) Token: 0x06000D8D RID: 3469 RVA: 0x000230B9 File Offset: 0x000212B9
	public List<Composition> Compositions
	{
		get
		{
			return this.compositions;
		}
		set
		{
			this.compositions = value;
		}
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x000230C2 File Offset: 0x000212C2
	public RefactorStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality)
	{
		this.Compositions = initCompositions;
		this.GetTurretSkills();
		if (initCompositions != null)
		{
			this.SortCompositions();
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x000230F8 File Offset: 0x000212F8
	public void SortCompositions()
	{
		for (int i = 0; i < this.compositions.Count - 1; i++)
		{
			for (int j = 0; j < this.compositions.Count - 1 - i; j++)
			{
				if (this.compositions[j].elementRequirement > this.compositions[j + 1].elementRequirement)
				{
					int num = this.compositions[j + 1].elementRequirement;
					this.compositions[j + 1].elementRequirement = this.compositions[j].elementRequirement;
					this.compositions[j].elementRequirement = num;
				}
				else if (this.compositions[j].elementRequirement == this.compositions[j + 1].elementRequirement && this.compositions[j].qualityRequeirement > this.compositions[j + 1].qualityRequeirement)
				{
					int num = this.compositions[j + 1].qualityRequeirement;
					this.compositions[j + 1].qualityRequeirement = this.compositions[j].qualityRequeirement;
					this.compositions[j].qualityRequeirement = num;
				}
			}
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00023250 File Offset: 0x00021450
	public void CheckElement()
	{
		List<IGameBehavior> list = Singleton<GameManager>.Instance.elementTurrets.behaviors.ToList<IGameBehavior>();
		int num = GameRes.PerfectElementCount;
		for (int i = 0; i < this.compositions.Count; i++)
		{
			this.compositions[i].obtained = false;
			this.compositions[i].isPerfect = false;
			for (int j = 0; j < list.Count; j++)
			{
				ElementTurret elementTurret = list[j] as ElementTurret;
				StrategyBase strategy = elementTurret.Strategy;
				if (this.compositions[i].elementRequirement == (int)strategy.Attribute.element && this.compositions[i].qualityRequeirement == strategy.Quality)
				{
					this.compositions[i].obtained = true;
					this.compositions[i].turret = elementTurret;
					list.Remove(list[j]);
					break;
				}
			}
			if (num > 0 && !this.compositions[i].obtained)
			{
				this.compositions[i].obtained = true;
				this.compositions[i].isPerfect = true;
				num--;
			}
		}
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00023394 File Offset: 0x00021594
	public void RefactorTurret()
	{
		this.unspawnElements.Clear();
		int perfectElementCount = GameRes.PerfectElementCount;
		foreach (Composition composition in this.Compositions)
		{
			if (!composition.isPerfect)
			{
				ContentStruct item;
				composition.turret.SaveContent(out item);
				this.unspawnElements.Add(item);
				Singleton<ObjectPool>.Instance.UnSpawn(composition.turret.m_GameTile);
			}
			else
			{
				GameRes.PerfectElementCount--;
			}
		}
		ConstructHelper.GetRefactorTurretByStrategy(this);
		this.usedPerfect = perfectElementCount - GameRes.PerfectElementCount;
		LevelManager instance = Singleton<LevelManager>.Instance;
		int lifeTotalRefactor = instance.LifeTotalRefactor;
		instance.LifeTotalRefactor = lifeTotalRefactor + 1;
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x00023460 File Offset: 0x00021660
	public override void UndoStrategy()
	{
		foreach (ContentStruct contentStruct in this.unspawnElements)
		{
			GameTile elementTurret = ConstructHelper.GetElementTurret(contentStruct);
			elementTurret.SetRotation(contentStruct.Direction);
			elementTurret.transform.position = (Vector3Int)contentStruct.Pos;
			elementTurret.TileLanded();
			Physics2D.SyncTransforms();
		}
		GameRes.PerfectElementCount += this.usedPerfect;
		if (BluePrintGrid.RefactorGrid != null)
		{
			Singleton<GameManager>.Instance.AddBluePrint(this);
		}
		Singleton<GameManager>.Instance.TransitionToState(StateName.BuildingState);
		Singleton<GameManager>.Instance.CheckAllBlueprints();
		BluePrintGrid.RefactorGrid = null;
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x00023528 File Offset: 0x00021728
	public void PreviewElements(bool value)
	{
		foreach (Composition composition in this.Compositions)
		{
			if (!composition.isPerfect)
			{
				composition.turret.m_GameTile.Highlight(value);
			}
		}
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x00023590 File Offset: 0x00021790
	public bool CheckBuildable()
	{
		bool flag = true;
		for (int i = 0; i < this.compositions.Count; i++)
		{
			flag = (flag && this.compositions[i].obtained);
		}
		return flag;
	}

	// Token: 0x04000682 RID: 1666
	private List<Composition> compositions = new List<Composition>();

	// Token: 0x04000683 RID: 1667
	private int usedPerfect;

	// Token: 0x04000684 RID: 1668
	private List<ContentStruct> unspawnElements = new List<ContentStruct>();
}
