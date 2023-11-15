using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AA6 RID: 2726
[RequireComponent(typeof(T17Button))]
public class CarouselButton : MonoBehaviour
{
	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x060035EA RID: 13802 RVA: 0x000FC117 File Offset: 0x000FA517
	public Selectable Button
	{
		get
		{
			if (this.m_button == null)
			{
				this.m_button = base.gameObject.RequireComponent<T17Button>();
			}
			return this.m_button;
		}
	}

	// Token: 0x060035EB RID: 13803 RVA: 0x000FC141 File Offset: 0x000FA541
	protected virtual void Start()
	{
		this.Initialise();
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x000FC149 File Offset: 0x000FA549
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x000FC14C File Offset: 0x000FA54C
	protected virtual void Initialise()
	{
		if (this.m_button == null)
		{
			this.m_button = base.gameObject.RequireComponent<T17Button>();
		}
		if (Application.isEditor)
		{
			ColorBlock colors = this.m_button.colors;
			colors.disabledColor = colors.normalColor;
			this.m_button.colors = colors;
		}
		T17Button button = this.m_button;
		button.OnButtonSelect = (T17Button.T17ButtonDelegate)Delegate.Combine(button.OnButtonSelect, new T17Button.T17ButtonDelegate(this.OnSelected));
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x000FC1D2 File Offset: 0x000FA5D2
	protected void OnSelected(T17Button _button)
	{
		this.m_rootMenu.OnButtonSelected(this);
	}

	// Token: 0x04002B6D RID: 11117
	private T17Button m_button;

	// Token: 0x04002B6E RID: 11118
	[HideInInspector]
	public CarouselRootMenu m_rootMenu;
}
