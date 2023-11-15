using System;
using UnityEngine;

// Token: 0x02000BEF RID: 3055
[ExecutionDependency(typeof(WorldMapFlipperBase))]
public class WorldMapRampOptimizer : MonoBehaviour, ITileFlipAnimatorProvider, ITileFlipStaticHandler
{
	// Token: 0x06003E58 RID: 15960 RVA: 0x0012A890 File Offset: 0x00128C90
	public Animator Begin(FlipDirection _direction)
	{
		this.m_complete = false;
		RuntimeAnimatorController runtimeAnimatorController = (_direction != FlipDirection.Unfold) ? this.m_foldController : this.m_unfoldController;
		this.m_animator = this.m_ramp.AddComponent<Animator>();
		if (this.m_animator == null)
		{
		}
		this.m_animator.runtimeAnimatorController = runtimeAnimatorController;
		this.m_animatorComs = this.m_ramp.AddComponent<AnimatorCommunications>();
		return this.m_animator;
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x0012A904 File Offset: 0x00128D04
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
		if (_direction == FlipDirection.Unfold)
		{
			this.m_top.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			if (this.m_bottom != null)
			{
				this.m_bottom.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		else
		{
			this.m_top.transform.localRotation = Quaternion.Euler(40f, 0f, 0f);
			if (this.m_bottom != null)
			{
				this.m_bottom.transform.localRotation = Quaternion.Euler(-40f, 0f, 0f);
			}
		}
		this.m_complete = true;
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x0012AA1B File Offset: 0x00128E1B
	public bool IsComplete()
	{
		return this.m_complete;
	}

	// Token: 0x06003E5B RID: 15963 RVA: 0x0012AA24 File Offset: 0x00128E24
	private void Awake()
	{
		if (this.m_debugBreak)
		{
		}
		WorldMapRampFlip componentInParent = base.gameObject.GetComponentInParent<WorldMapRampFlip>();
		if (componentInParent != null && !componentInParent.StartsFlipped && !componentInParent.ShouldBeUnfolded() && this.m_bottom != null)
		{
			this.m_bottom.SetActive(false);
		}
		if (!this.m_complete)
		{
			this.End(FlipDirection.Fold);
			this.m_complete = true;
		}
		if (this.m_startStatic)
		{
			this.SetAsStatic();
		}
	}

	// Token: 0x06003E5C RID: 15964 RVA: 0x0012AAB4 File Offset: 0x00128EB4
	public void SetAsStatic()
	{
		WorldMapTileFlip worldMapTileFlip = base.gameObject.RequestComponent<WorldMapTileFlip>();
		if (worldMapTileFlip != null)
		{
			this.m_top.SetActive(worldMapTileFlip.IsFlipped());
			if (this.m_bottom != null)
			{
				this.m_bottom.SetActive(!worldMapTileFlip.IsFlipped());
			}
		}
		if (this.m_bottom != null && !this.m_bottom.activeInHierarchy)
		{
			UnityEngine.Object.Destroy(this.m_bottom);
		}
	}

	// Token: 0x04003214 RID: 12820
	[SerializeField]
	[AssignChild("Ramp", Editorbility.NonEditable)]
	private GameObject m_ramp;

	// Token: 0x04003215 RID: 12821
	[SerializeField]
	[AssignChildRecursive("Top", Editorbility.NonEditable)]
	private GameObject m_top;

	// Token: 0x04003216 RID: 12822
	[SerializeField]
	[AssignChildRecursive("Bottom", Editorbility.NonEditable)]
	private GameObject m_bottom;

	// Token: 0x04003217 RID: 12823
	[SerializeField]
	private RuntimeAnimatorController m_unfoldController;

	// Token: 0x04003218 RID: 12824
	[SerializeField]
	private RuntimeAnimatorController m_foldController;

	// Token: 0x04003219 RID: 12825
	[SerializeField]
	private bool m_startStatic;

	// Token: 0x0400321A RID: 12826
	[SerializeField]
	private bool m_debugBreak;

	// Token: 0x0400321B RID: 12827
	private Animator m_animator;

	// Token: 0x0400321C RID: 12828
	private AnimatorCommunications m_animatorComs;

	// Token: 0x0400321D RID: 12829
	private bool m_complete;
}
