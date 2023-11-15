using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class ShapeSelectUI : IUserInterface
{
	// Token: 0x060009E6 RID: 2534 RVA: 0x0001ADC7 File Offset: 0x00018FC7
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0001ADDC File Offset: 0x00018FDC
	public List<ShapeInfo> GetCurrent3ShapeInfos()
	{
		List<ShapeInfo> list = new List<ShapeInfo>();
		if (base.IsVisible())
		{
			foreach (TileSelect tileSelect in this.tileSelects)
			{
				if (tileSelect.Shape != null)
				{
					list.Add(tileSelect.Shape.m_ShapeInfo);
				}
			}
		}
		return list;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0001AE30 File Offset: 0x00019030
	public void ShowThreeShapes()
	{
		this.ClearAllSelections();
		for (int i = 0; i < this.tileSelects.Length; i++)
		{
			TileShape shape = (GameRes.PreSetShape[i] != null) ? ConstructHelper.GetTutorialShape(GameRes.PreSetShape[i], false) : ConstructHelper.GetRandomShape();
			this.tileSelects[i].InitializeDisplay(i, shape);
		}
		this.Show();
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x0001AE8C File Offset: 0x0001908C
	public void LoadSaveGame()
	{
		if (Singleton<LevelManager>.Instance.LastGameSave.SaveShapes.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.tileSelects.Length; i++)
		{
			TileShape tutorialShape = ConstructHelper.GetTutorialShape(Singleton<LevelManager>.Instance.LastGameSave.SaveShapes[i], false);
			this.tileSelects[i].InitializeDisplay(i, tutorialShape);
		}
		this.Show();
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0001AEF8 File Offset: 0x000190F8
	public void ClearAllSelections()
	{
		TileSelect[] array = this.tileSelects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ClearShape();
		}
		this.Hide();
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0001AF28 File Offset: 0x00019128
	public override void Show()
	{
		base.Show();
		this.m_Anim.SetTrigger("Show");
	}

	// Token: 0x0400052B RID: 1323
	private Animator m_Anim;

	// Token: 0x0400052C RID: 1324
	[SerializeField]
	private TileSelect[] tileSelects;
}
