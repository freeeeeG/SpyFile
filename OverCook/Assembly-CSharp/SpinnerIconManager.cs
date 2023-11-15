using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class SpinnerIconManager : Manager
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06003944 RID: 14660 RVA: 0x0010FD62 File Offset: 0x0010E162
	public static SpinnerIconManager Instance
	{
		get
		{
			return SpinnerIconManager.m_instance;
		}
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x0010FD6C File Offset: 0x0010E16C
	private void Awake()
	{
		if (SpinnerIconManager.m_instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SpinnerIconManager.m_instance = this;
		for (int i = 0; i < this.m_spinnerIcons.Count; i++)
		{
			this.m_spinnerIcons[i].Initialise();
		}
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x0010FDC4 File Offset: 0x0010E1C4
	public Suppressor Show(SpinnerIconManager.SpinnerIconType _type, UnityEngine.Object _arbitrator, bool _resetTimer = true)
	{
		SpinnerIconManager.SpinnerIcon spinnerIcon = this.FindEntry(_type);
		if (_resetTimer)
		{
			spinnerIcon.ResetStartTime();
		}
		return spinnerIcon.m_arbitrators.AddSuppressor(_arbitrator);
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x0010FDF4 File Offset: 0x0010E1F4
	protected SpinnerIconManager.SpinnerIcon FindEntry(SpinnerIconManager.SpinnerIconType _type)
	{
		for (int i = 0; i < this.m_spinnerIcons.Count; i++)
		{
			SpinnerIconManager.SpinnerIcon spinnerIcon = this.m_spinnerIcons[i];
			if (spinnerIcon.m_type == _type)
			{
				return spinnerIcon;
			}
		}
		return null;
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x0010FE3C File Offset: 0x0010E23C
	private void Update()
	{
		for (int i = 0; i < this.m_spinnerIcons.Count; i++)
		{
			SpinnerIconManager.SpinnerIcon spinnerIcon = this.m_spinnerIcons[i];
			spinnerIcon.Update(this.m_activeMask);
			this.m_activeMask[(int)spinnerIcon.m_type] = spinnerIcon.IsVisible(this.m_activeMask);
		}
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x0010FE98 File Offset: 0x0010E298
	private void OnDestroy()
	{
		if (SpinnerIconManager.m_instance == this)
		{
			SpinnerIconManager.m_instance = null;
		}
	}

	// Token: 0x04002E0E RID: 11790
	private static SpinnerIconManager m_instance;

	// Token: 0x04002E0F RID: 11791
	private bool[] m_activeMask = new bool[6];

	// Token: 0x04002E10 RID: 11792
	public List<SpinnerIconManager.SpinnerIcon> m_spinnerIcons;

	// Token: 0x02000B10 RID: 2832
	public enum SpinnerIconType
	{
		// Token: 0x04002E12 RID: 11794
		Save,
		// Token: 0x04002E13 RID: 11795
		Load,
		// Token: 0x04002E14 RID: 11796
		Error,
		// Token: 0x04002E15 RID: 11797
		Warning,
		// Token: 0x04002E16 RID: 11798
		SpinnerDialog,
		// Token: 0x04002E17 RID: 11799
		SavingDisabled,
		// Token: 0x04002E18 RID: 11800
		COUNT
	}

	// Token: 0x02000B11 RID: 2833
	[Serializable]
	public class SpinnerIcon
	{
		// Token: 0x0600394B RID: 14667 RVA: 0x0010FEC3 File Offset: 0x0010E2C3
		public void Initialise()
		{
			this.m_startTime = Time.time - this.m_minTime;
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x0010FED8 File Offset: 0x0010E2D8
		public bool Update(bool[] _activeMask)
		{
			this.m_arbitrators.UpdateSuppressors();
			if (this.m_animator != null)
			{
				this.m_animator.speed = 1f;
			}
			if (this.IsVisible(_activeMask))
			{
				this.m_icon.gameObject.SetActive(true);
				return true;
			}
			this.m_icon.gameObject.SetActive(false);
			return false;
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x0010FF42 File Offset: 0x0010E342
		public bool IsVisible(bool[] _activeMask)
		{
			return (this.m_arbitrators.IsSuppressed() || Time.time - this.m_startTime < this.m_minTime) && this.CanShow(_activeMask);
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x0010FF75 File Offset: 0x0010E375
		public void ResetStartTime()
		{
			this.m_startTime = Time.time;
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x0010FF84 File Offset: 0x0010E384
		private bool CanShow(bool[] _activeMask)
		{
			for (int i = 0; i < _activeMask.Length; i++)
			{
				if (_activeMask[i])
				{
					if (MaskUtils.HasFlag<SpinnerIconManager.SpinnerIconType>(this.m_mask, (SpinnerIconManager.SpinnerIconType)i))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04002E19 RID: 11801
		public SpinnerIconManager.SpinnerIconType m_type;

		// Token: 0x04002E1A RID: 11802
		public CanvasRenderer m_icon;

		// Token: 0x04002E1B RID: 11803
		public Animator m_animator;

		// Token: 0x04002E1C RID: 11804
		public float m_minTime;

		// Token: 0x04002E1D RID: 11805
		[Mask(typeof(SpinnerIconManager.SpinnerIconType))]
		public int m_mask;

		// Token: 0x04002E1E RID: 11806
		[HideInInspector]
		public SuppressionController m_arbitrators = new SuppressionController();

		// Token: 0x04002E1F RID: 11807
		private float m_startTime;
	}
}
