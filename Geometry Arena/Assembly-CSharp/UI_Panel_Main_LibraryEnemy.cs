using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C5 RID: 197
public class UI_Panel_Main_LibraryEnemy : UI_Panel_Main_IconList
{
	// Token: 0x060006C7 RID: 1735 RVA: 0x00026244 File Offset: 0x00024444
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.InitIcons(null);
		this.UpdateLanguage();
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00026260 File Offset: 0x00024460
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.EnemyLibrary enemyLibrary = inst.enemyLibrary;
		this.textMultiTip.text = enemyLibrary.multiTip;
		this.textManualTip.text = enemyLibrary.manualTip.ReplaceLineBreak();
		for (int i = 0; i < 3; i++)
		{
			this.textShapes[i].text = inst.shapes[i].shapeName;
		}
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x000262C8 File Offset: 0x000244C8
	private void DestroyAllEnemies()
	{
		foreach (GameObject gameObject in this.listIcons)
		{
			Object.Destroy(gameObject.gameObject);
		}
		this.listIcons = new List<GameObject>();
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x00026328 File Offset: 0x00024528
	public override void InitIcons(Transform transformParent = null)
	{
		transformParent = this.transParent;
		UI_ToolTip.inst.TryClose();
		this.DestroyAllEnemies();
		int num = 0;
		for (int i = 0; i < this.IconNum(); i++)
		{
			if (this.IfAvailable(i))
			{
				int num2 = num % this.columnNum;
				int num3 = num / this.columnNum;
				Vector2 b = new Vector2(this.distX * (float)num2, this.distY * (float)num3);
				Vector2 v = this.startPos + b;
				GameObject gameObject = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Enemys[i]);
				this.listIcons.Add(gameObject.gameObject);
				gameObject.transform.SetParent((transformParent == null) ? base.transform : transformParent);
				gameObject.transform.localPosition = v;
				this.InitSingleIcon(gameObject, i);
				num++;
			}
		}
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0002640F File Offset: 0x0002460F
	protected override int IconNum()
	{
		return 36;
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool IfAvailable(int ID)
	{
		return true;
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00026414 File Offset: 0x00024614
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		Enemy component = obj.GetComponent<Enemy>();
		BasicUnit component2 = obj.GetComponent<BasicUnit>();
		int num = (int)DataBase.Inst.Data_EnemyModels[ID].rank;
		if ((ID + 1) % 12 == 0)
		{
			num = 3;
		}
		component2.DyeSprsWithColor(UI_Setting.Inst.rankColors[num]);
		Object.Destroy(component);
		Object.Destroy(component2);
		obj.AddComponent<BoxCollider2D>().size = Vector2.one * this.colliderEdge;
		obj.transform.localScale = Vector2.one * this.enemySize;
		obj.AddComponent<UI_Panel_EnemyLibrary_EnemyIcon>().enemyID = ID;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x000264B2 File Offset: 0x000246B2
	private void OnDisable()
	{
		this.DestroyAllEnemies();
	}

	// Token: 0x04000597 RID: 1431
	[SerializeField]
	private Text textMultiTip;

	// Token: 0x04000598 RID: 1432
	[SerializeField]
	private Text textManualTip;

	// Token: 0x04000599 RID: 1433
	[SerializeField]
	private float colliderEdge = 2f;

	// Token: 0x0400059A RID: 1434
	[SerializeField]
	private float enemySize = 5f;

	// Token: 0x0400059B RID: 1435
	[SerializeField]
	private Text[] textShapes;

	// Token: 0x0400059C RID: 1436
	[SerializeField]
	private Transform transParent;
}
