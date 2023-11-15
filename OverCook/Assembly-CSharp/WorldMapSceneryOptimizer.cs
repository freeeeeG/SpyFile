using System;
using UnityEngine;

// Token: 0x02000BF0 RID: 3056
public class WorldMapSceneryOptimizer : MonoBehaviour, ITileFlipAnimatorProvider
{
	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06003E5E RID: 15966 RVA: 0x0012AB43 File Offset: 0x00128F43
	public GameObject Mesh
	{
		get
		{
			return this.m_mesh;
		}
	}

	// Token: 0x06003E5F RID: 15967 RVA: 0x0012AB4B File Offset: 0x00128F4B
	private void Awake()
	{
		if (!this.m_complete)
		{
			if (this.m_mesh != null)
			{
				this.m_mesh.SetActive(false);
			}
			this.m_complete = true;
		}
	}

	// Token: 0x06003E60 RID: 15968 RVA: 0x0012AB7C File Offset: 0x00128F7C
	public Animator Begin(FlipDirection _direction)
	{
		this.m_animator = base.gameObject.AddComponent<Animator>();
		this.m_animator.runtimeAnimatorController = this.m_controller;
		this.m_animatorComs = base.gameObject.AddComponent<AnimatorCommunications>();
		return this.m_animator;
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x0012ABB8 File Offset: 0x00128FB8
	public void End(FlipDirection _direction)
	{
		if (this.m_animatorComs != null)
		{
			UnityEngine.Object.Destroy(this.m_animatorComs);
			this.m_animatorComs = null;
		}
		if (this.m_animator != null)
		{
			UnityEngine.Object.Destroy(this.m_animator);
			this.m_animator = null;
		}
		if (this.m_mesh != null)
		{
			this.m_mesh.SetActive(true);
		}
		this.m_complete = true;
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0012AC2F File Offset: 0x0012902F
	public bool IsComplete()
	{
		return this.m_complete;
	}

	// Token: 0x0400321E RID: 12830
	[SerializeField]
	private RuntimeAnimatorController m_controller;

	// Token: 0x0400321F RID: 12831
	private Animator m_animator;

	// Token: 0x04003220 RID: 12832
	private AnimatorCommunications m_animatorComs;

	// Token: 0x04003221 RID: 12833
	private bool m_complete;

	// Token: 0x04003222 RID: 12834
	[SerializeField]
	[AssignChild("Mesh", Editorbility.Editable)]
	private GameObject m_mesh;
}
