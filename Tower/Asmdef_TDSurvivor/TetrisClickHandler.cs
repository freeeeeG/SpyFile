using System;
using Lean.Touch;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class TetrisClickHandler : MonoBehaviour
{
	// Token: 0x060006B1 RID: 1713 RVA: 0x0001875A File Offset: 0x0001695A
	private void OnMouseDown()
	{
		if (LeanTouch.PointOverGui(Input.mousePosition))
		{
			return;
		}
		if (this.ref_Tetris == null)
		{
			Debug.LogError("沒有Tetris物件連結");
			return;
		}
		this.ref_Tetris.OnChildMouseDown();
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00018792 File Offset: 0x00016992
	private void OnMouseUp()
	{
		if (this.ref_Tetris == null)
		{
			Debug.LogError("沒有Tetris物件連結");
			return;
		}
		this.ref_Tetris.OnChildMouseUp();
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x000187B8 File Offset: 0x000169B8
	private void OnMouseEnter()
	{
		this.ref_Tetris.OnChildMouseEnter();
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x000187C5 File Offset: 0x000169C5
	private void OnMouseExit()
	{
		this.ref_Tetris.OnChildMouseExit();
	}

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	private Obj_TetrisBlock ref_Tetris;
}
