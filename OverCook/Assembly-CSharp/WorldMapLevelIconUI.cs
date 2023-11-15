using System;
using UnityEngine;

// Token: 0x02000B5A RID: 2906
[RequireComponent(typeof(Animator))]
public class WorldMapLevelIconUI : HoverIconUIController
{
	// Token: 0x06003B06 RID: 15110 RVA: 0x00119015 File Offset: 0x00117415
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.gameObject.RequireComponent<Animator>();
	}

	// Token: 0x06003B07 RID: 15111 RVA: 0x0011902E File Offset: 0x0011742E
	public bool IsReady()
	{
		return this.m_animator.IsActive();
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x0011903C File Offset: 0x0011743C
	public void SetTitle(string _title)
	{
		for (int i = 0; i < this.m_titles.Length; i++)
		{
			this.m_titles[i].SetLocalisedTextCatchAll(_title);
		}
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x00119070 File Offset: 0x00117470
	public void SetAvatarProximity(bool _inProximity)
	{
		this.m_animator.SetBool(WorldMapLevelIconUI.m_iPlayerClose, _inProximity);
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x00119083 File Offset: 0x00117483
	public void ActivateAttentionPopup()
	{
		this.m_attentionPopup.SetActive(true);
	}

	// Token: 0x04003007 RID: 12295
	[SerializeField]
	private T17Text[] m_titles;

	// Token: 0x04003008 RID: 12296
	[SerializeField]
	private GameObject m_attentionPopup;

	// Token: 0x04003009 RID: 12297
	protected Animator m_animator;

	// Token: 0x0400300A RID: 12298
	private static readonly int m_iPlayerClose = Animator.StringToHash("PlayerClose");
}
