using System;
using UnityEngine;

// Token: 0x02000B35 RID: 2869
public class PrefabDefinedUIPosition
{
	// Token: 0x060058AF RID: 22703 RVA: 0x00207FC6 File Offset: 0x002061C6
	public void SetOn(GameObject gameObject)
	{
		if (this.position.HasValue)
		{
			gameObject.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = gameObject.rectTransform().anchoredPosition;
	}

	// Token: 0x060058B0 RID: 22704 RVA: 0x00208002 File Offset: 0x00206202
	public void SetOn(Component component)
	{
		if (this.position.HasValue)
		{
			component.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = component.rectTransform().anchoredPosition;
	}

	// Token: 0x04003C07 RID: 15367
	private Option<Vector2> position;
}
