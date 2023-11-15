using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BDE RID: 3038
[ExecutionDependency(typeof(GridAutoParenting))]
[DisallowMultipleComponent]
public abstract class WorldMapFlipperBase : MonoBehaviour, ITileFlipStartup
{
	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06003DFD RID: 15869 RVA: 0x00127D13 File Offset: 0x00126113
	public bool StartsFlipped
	{
		get
		{
			return this.m_startFlipped;
		}
	}

	// Token: 0x06003DFE RID: 15870 RVA: 0x00127D1C File Offset: 0x0012611C
	public virtual void StartUp()
	{
		ITileFlipStartup[] array = base.gameObject.RequestInterfacesRecursive<ITileFlipStartup>();
		array = array.AllRemoved_Predicate(new Predicate<ITileFlipStartup>(this.Equals));
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			WorldMapFlipperBase.depthTest++;
			array[i].StartUp();
			WorldMapFlipperBase.depthTest--;
		}
	}

	// Token: 0x06003DFF RID: 15871 RVA: 0x00127D80 File Offset: 0x00126180
	private void Setup()
	{
		if (this.m_animator == null && this.m_iTileAnimator == null)
		{
			this.m_iTileAnimator = base.gameObject.RequestInterface<ITileFlipAnimatorProvider>();
			if (this.m_iTileAnimator == null)
			{
				this.m_iTileAnimator = base.gameObject.RequestInterfaceInImmediateChildren<ITileFlipAnimatorProvider>();
				if (this.m_iTileAnimator == null)
				{
					this.m_animator = base.gameObject.RequireComponentRecursive<Animator>();
				}
			}
		}
		if (this.m_colliders == null)
		{
			this.m_colliders = base.gameObject.RequestComponentsInImmediateChildren<Collider>();
		}
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x00127E0E File Offset: 0x0012620E
	protected virtual void Awake()
	{
		this.Setup();
		this.SetCollidable(this.m_startCollidable);
		if (this.m_startFlipped)
		{
			this.StartInstantUnfold();
		}
	}

	// Token: 0x06003E01 RID: 15873 RVA: 0x00127E33 File Offset: 0x00126233
	private void Start()
	{
		this.m_waitForStartDelay = new WaitForSeconds(this.m_startFlipDelay);
		this.m_waitForChildDelay = new WaitForSeconds(this.m_childFlipDelay);
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x00127E58 File Offset: 0x00126258
	private void SetCollidable(bool _collidable)
	{
		for (int i = 0; i < this.m_colliders.Length; i++)
		{
			this.m_colliders[i].enabled = _collidable;
		}
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x00127E8C File Offset: 0x0012628C
	public bool IsFlipped()
	{
		return !Application.isPlaying || this.m_shouldBeFlipped;
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x00127EA0 File Offset: 0x001262A0
	public bool IsFinishedFlipping()
	{
		bool flag;
		if (this.m_iTileAnimator != null)
		{
			flag = this.m_iTileAnimator.IsComplete();
		}
		else
		{
			int id = (!this.m_shouldBeFlipped) ? WorldMapFlipperBase.m_iPreFlipIdle : WorldMapFlipperBase.m_iPostFlipIdle;
			flag = this.m_animator.GetBool(id);
		}
		return !this.m_isFlipping && flag;
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x00127F02 File Offset: 0x00126302
	public virtual void StartUnfoldFlow()
	{
		base.StartCoroutine(this.UnfoldFlow());
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00127F14 File Offset: 0x00126314
	private IEnumerator UnfoldFlow()
	{
		this.m_shouldBeFlipped = true;
		this.m_isFlipping = true;
		this.SetCollidable(true);
		WorldMapFlipperBase[] iFlippers = base.gameObject.RequestComponentsRecursive<WorldMapFlipperBase>();
		iFlippers = iFlippers.AllRemoved_Predicate(new Predicate<WorldMapFlipperBase>(this.Equals));
		for (int i = 0; i < iFlippers.Length; i++)
		{
			while (!iFlippers[i].IsFinishedFlipping())
			{
				yield return null;
			}
		}
		if (this.m_iTileAnimator != null)
		{
			this.m_animator = this.m_iTileAnimator.Begin(FlipDirection.Unfold);
		}
		yield return this.m_waitForStartDelay;
		this.m_animator.SetBool(WorldMapFlipperBase.m_iFlipped, true);
		yield return this.m_waitForChildDelay;
		for (int j = 0; j < iFlippers.Length; j++)
		{
			iFlippers[j].StartUnfoldFlow();
			yield return this.m_waitForChildDelay;
		}
		for (int k = 0; k < iFlippers.Length; k++)
		{
			while (!iFlippers[k].IsFinishedFlipping())
			{
				yield return null;
			}
		}
		while (!this.m_animator.GetBool(WorldMapFlipperBase.m_iPostFlipIdle))
		{
			yield return null;
		}
		if (this.m_iTileAnimator != null)
		{
			this.m_iTileAnimator.End(FlipDirection.Unfold);
			this.m_animator = null;
		}
		this.m_isFlipping = false;
		yield break;
	}

	// Token: 0x06003E07 RID: 15879 RVA: 0x00127F30 File Offset: 0x00126330
	public List<ClientWorldMapInfoPopup.InfoPopupShowRequest> GetPopups()
	{
		WorldMapFlipperBase[] array = base.gameObject.RequestComponentsRecursive<WorldMapFlipperBase>();
		List<ClientWorldMapInfoPopup.InfoPopupShowRequest> list = new List<ClientWorldMapInfoPopup.InfoPopupShowRequest>();
		for (int i = 0; i < array.Length; i++)
		{
			WorldMapInfoPopup infoPopup = array[i].m_infoPopup;
			if (infoPopup != null)
			{
				ClientWorldMapInfoPopup popup = infoPopup.gameObject.RequireComponent<ClientWorldMapInfoPopup>();
				ClientWorldMapInfoPopup.InfoPopupShowRequest item = new ClientWorldMapInfoPopup.InfoPopupShowRequest(array[i].transform, popup);
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x06003E08 RID: 15880 RVA: 0x00127FA0 File Offset: 0x001263A0
	public virtual void StartInstantUnfold()
	{
		this.Setup();
		this.m_shouldBeFlipped = true;
		this.SetCollidable(true);
		if (this.m_iTileAnimator == null)
		{
			this.m_animator.SetBool(WorldMapFlipperBase.m_iSnapping, true);
			this.m_animator.SetBool(WorldMapFlipperBase.m_iFlipped, true);
		}
		WorldMapFlipperBase[] array = base.gameObject.RequestComponentsInImmediateChildren<WorldMapFlipperBase>();
		array = array.AllRemoved_Predicate(new Predicate<WorldMapFlipperBase>(this.Equals));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StartInstantUnfold();
		}
		if (this.m_iTileAnimator != null)
		{
			this.m_iTileAnimator.End(FlipDirection.Unfold);
			this.m_animator = null;
		}
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x00128048 File Offset: 0x00126448
	public virtual void StartFoldFlow()
	{
		base.StartCoroutine(this.FoldFlow());
	}

	// Token: 0x06003E0A RID: 15882 RVA: 0x00128058 File Offset: 0x00126458
	private IEnumerator FoldFlow()
	{
		this.m_shouldBeFlipped = false;
		this.m_isFlipping = true;
		this.SetCollidable(false);
		WorldMapFlipperBase[] iFlippers = base.gameObject.RequestComponentsInImmediateChildren<WorldMapFlipperBase>();
		iFlippers = iFlippers.AllRemoved_Predicate(new Predicate<WorldMapFlipperBase>(this.Equals));
		for (int i = 0; i < iFlippers.Length; i++)
		{
			if (iFlippers[i].IsFlipped())
			{
				iFlippers[i].StartFoldFlow();
				yield return new WaitForSeconds(this.m_childFlipDelay);
			}
		}
		if (this.m_iTileAnimator != null)
		{
			this.m_animator = this.m_iTileAnimator.Begin(FlipDirection.Fold);
		}
		this.m_animator.SetBool(WorldMapFlipperBase.m_iFlipped, true);
		while (!this.m_animator.GetBool(WorldMapFlipperBase.m_iPostFlipIdle))
		{
			yield return null;
		}
		if (this.m_iTileAnimator != null)
		{
			this.m_iTileAnimator.End(FlipDirection.Fold);
			this.m_animator = null;
		}
		this.m_isFlipping = false;
		yield break;
	}

	// Token: 0x040031C7 RID: 12743
	[SerializeField]
	protected bool m_startFlipped;

	// Token: 0x040031C8 RID: 12744
	[SerializeField]
	protected bool m_startCollidable;

	// Token: 0x040031C9 RID: 12745
	[SerializeField]
	protected float m_startFlipDelay;

	// Token: 0x040031CA RID: 12746
	[SerializeField]
	private float m_childFlipDelay = 0.5f;

	// Token: 0x040031CB RID: 12747
	[SerializeField]
	private bool m_foldChildrenOnFlip;

	// Token: 0x040031CC RID: 12748
	[SerializeField]
	private bool m_debugBreak;

	// Token: 0x040031CD RID: 12749
	private bool m_shouldBeFlipped;

	// Token: 0x040031CE RID: 12750
	private bool m_isFlipping;

	// Token: 0x040031CF RID: 12751
	private Animator m_animator;

	// Token: 0x040031D0 RID: 12752
	private Collider[] m_colliders;

	// Token: 0x040031D1 RID: 12753
	private ITileFlipAnimatorProvider m_iTileAnimator;

	// Token: 0x040031D2 RID: 12754
	[SerializeField]
	public WorldMapInfoPopup m_infoPopup;

	// Token: 0x040031D3 RID: 12755
	public static readonly int m_iFlipped = Animator.StringToHash("Flipped");

	// Token: 0x040031D4 RID: 12756
	public static readonly int m_iSnapping = Animator.StringToHash("Snapping");

	// Token: 0x040031D5 RID: 12757
	public static readonly int m_iPostFlipIdle = Animator.StringToHash("InPostFlipIdle");

	// Token: 0x040031D6 RID: 12758
	public static readonly int m_iPreFlipIdle = Animator.StringToHash("InPreFlipIdle");

	// Token: 0x040031D7 RID: 12759
	private WaitForSeconds m_waitForStartDelay;

	// Token: 0x040031D8 RID: 12760
	private WaitForSeconds m_waitForChildDelay;

	// Token: 0x040031D9 RID: 12761
	private static int depthTest = 0;
}
