using System;
using UI;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class CursorManager : MonoBehaviour
{
	// Token: 0x06000301 RID: 769 RVA: 0x0000B98C File Offset: 0x00009B8C
	private void Update()
	{
		if (!Input.mousePresent)
		{
			Cursor.visible = false;
			return;
		}
		if (Mathf.Abs(Input.GetAxis("mouse x")) > 0f || Mathf.Abs(Input.GetAxis("mouse y")) > 0f || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			this._remainTimeToHide = 3f;
		}
		this._remainTimeToHide -= Time.unscaledDeltaTime;
		Dialogue current = Dialogue.GetCurrent();
		Cursor.visible = (this._remainTimeToHide > 0f || (current != null && !(current is NpcConversationBody)));
	}

	// Token: 0x04000279 RID: 633
	private const float _timeToHide = 3f;

	// Token: 0x0400027A RID: 634
	private float _remainTimeToHide;
}
