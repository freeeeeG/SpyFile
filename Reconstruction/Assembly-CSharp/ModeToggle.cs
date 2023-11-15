using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200029E RID: 670
public class ModeToggle : MonoBehaviour
{
	// Token: 0x06001071 RID: 4209 RVA: 0x0002D4F6 File Offset: 0x0002B6F6
	private void Awake()
	{
		this.m_Toggle = base.GetComponent<Toggle>();
		this.m_Anim = base.GetComponent<Animator>();
		this.m_Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChange));
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0002D52C File Offset: 0x0002B72C
	private void OnEnable()
	{
		this.OnValueChange(this.m_Toggle.isOn);
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x0002D53F File Offset: 0x0002B73F
	public void OnValueChange(bool value)
	{
		this.m_Anim.SetBool("Select", !value);
	}

	// Token: 0x040008CB RID: 2251
	private Animator m_Anim;

	// Token: 0x040008CC RID: 2252
	private Toggle m_Toggle;
}
