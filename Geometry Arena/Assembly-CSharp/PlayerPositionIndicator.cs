using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class PlayerPositionIndicator : MonoBehaviour
{
	// Token: 0x06000128 RID: 296 RVA: 0x000089FC File Offset: 0x00006BFC
	private void Update()
	{
		this.UpdateIndicators();
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00008A04 File Offset: 0x00006C04
	private void UpdateIndicators()
	{
		if (Player.inst == null)
		{
			return;
		}
		Vector2 vector = Player.inst.transform.position;
		float x = vector.x;
		float y = vector.y;
		this.transPosX.position = new Vector2(0f, y);
		this.transPoxY.position = new Vector2(x, 0f);
		this.transSquare.position = new Vector2(x, y);
		float num = Mathf.Max(1f, Player.inst.unit.lastScale);
		this.sprSquare.size = new Vector2(num, num);
	}

	// Token: 0x0400016C RID: 364
	[SerializeField]
	private Transform transPosX;

	// Token: 0x0400016D RID: 365
	[SerializeField]
	private Transform transPoxY;

	// Token: 0x0400016E RID: 366
	[SerializeField]
	private Transform transSquare;

	// Token: 0x0400016F RID: 367
	[SerializeField]
	private SpriteRenderer sprSquare;
}
