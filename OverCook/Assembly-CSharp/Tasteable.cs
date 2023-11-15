using System;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
[RequireComponent(typeof(IngredientContentGUI))]
public class Tasteable : MonoBehaviour
{
	// Token: 0x06001BEF RID: 7151 RVA: 0x0008872F File Offset: 0x00086B2F
	public void Taste()
	{
		this.m_timer = this.m_tasteUITime;
		this.m_gui.enabled = true;
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x00088749 File Offset: 0x00086B49
	private void Awake()
	{
		this.m_gui = base.gameObject.RequireComponent<IngredientContentGUI>();
		this.m_gui.enabled = false;
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x00088768 File Offset: 0x00086B68
	private void Update()
	{
		if (this.m_timer > 0f)
		{
			this.m_timer -= TimeManager.GetDeltaTime(base.gameObject);
			this.m_gui.enabled = (this.m_timer > 0f);
		}
		else
		{
			this.m_gui.enabled = false;
		}
	}

	// Token: 0x040015E8 RID: 5608
	[SerializeField]
	private float m_tasteUITime = 5f;

	// Token: 0x040015E9 RID: 5609
	private IngredientContentGUI m_gui;

	// Token: 0x040015EA RID: 5610
	private float m_timer;
}
