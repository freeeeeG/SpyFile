using System;
using TMPro;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class TileSelect : MonoBehaviour
{
	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x0001AF48 File Offset: 0x00019148
	// (set) Token: 0x060009EE RID: 2542 RVA: 0x0001AF50 File Offset: 0x00019150
	public TileShape Shape { get; set; }

	// Token: 0x060009EF RID: 2543 RVA: 0x0001AF5C File Offset: 0x0001915C
	public void InitializeDisplay(int displayID, TileShape shape)
	{
		this.Shape = shape;
		shape.SetUIDisplay(displayID, this.renderTexture);
		StrategyBase strategy = this.Shape.m_ElementTurret.Strategy;
		if (strategy.Quality > 1)
		{
			this.m_LevelDownSelect.SetStrategy(strategy);
			this.m_LevelDownSelect.gameObject.SetActive(true);
		}
		else
		{
			this.m_LevelDownSelect.gameObject.SetActive(false);
		}
		this.m_InfoBtn.SetStrategy(strategy);
		this.m_ElementPreview.SetStrategy(strategy);
		this.CheckElementNum();
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0001AFE8 File Offset: 0x000191E8
	private void CheckElementNum()
	{
		int num = 0;
		foreach (IGameBehavior gameBehavior in Singleton<GameManager>.Instance.elementTurrets.behaviors)
		{
			ElementTurret elementTurret = gameBehavior as ElementTurret;
			if (elementTurret.Strategy.Attribute.element == this.Shape.m_ElementTurret.Strategy.Attribute.element && elementTurret.Strategy.Quality == this.Shape.m_ElementTurret.Strategy.Quality)
			{
				num++;
			}
		}
		this.hasNumTxt.text = GameMultiLang.GetTraduction("HAS") + ":" + num.ToString();
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0001B0BC File Offset: 0x000192BC
	public void OnShapeClick(bool levelDown = false)
	{
		Singleton<GameManager>.Instance.PreviewComposition(false, ElementType.DUST, 1);
		Singleton<GameManager>.Instance.SelectShape(this.Shape, levelDown);
		Singleton<TipsManager>.Instance.HideTips();
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.SelectShape);
		if (BluePrintGrid.SelectingGrid != null)
		{
			BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
		}
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0001B114 File Offset: 0x00019314
	public void ClearShape()
	{
		if (this.Shape == null)
		{
			return;
		}
		this.Shape.ReclaimTiles();
		this.Shape = null;
	}

	// Token: 0x0400052D RID: 1325
	[SerializeField]
	private RenderTexture renderTexture;

	// Token: 0x0400052F RID: 1327
	[SerializeField]
	private LevelDownSelect m_LevelDownSelect;

	// Token: 0x04000530 RID: 1328
	[SerializeField]
	private ElementSelectPreview m_ElementPreview;

	// Token: 0x04000531 RID: 1329
	[SerializeField]
	private ElementInfoBtn m_InfoBtn;

	// Token: 0x04000532 RID: 1330
	[SerializeField]
	private TextMeshProUGUI hasNumTxt;
}
