using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class Skill_Player0_Shileds_Single : MonoBehaviour
{
	// Token: 0x0600031E RID: 798 RVA: 0x0001326C File Offset: 0x0001146C
	private void Awake()
	{
		this.anm = base.gameObject.GetComponent<Animator>();
		this.parent = base.transform.root.GetComponentInChildren<Skill_Player0_Shileds>();
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00013295 File Offset: 0x00011495
	public void AnmCallBack_Destroy()
	{
		Object.Destroy(this.parent.gameObject);
	}

	// Token: 0x040002CF RID: 719
	public Animator anm;

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	private Skill_Player0_Shileds parent;
}
