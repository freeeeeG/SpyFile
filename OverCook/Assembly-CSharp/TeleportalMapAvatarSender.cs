using System;
using UnityEngine;
using UnityEngine.PostProcessing;

// Token: 0x02000BD2 RID: 3026
[RequireComponent(typeof(Teleportal))]
public class TeleportalMapAvatarSender : BaseTeleportalSender
{
	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x001278D3 File Offset: 0x00125CD3
	public TeleportalMapAvatarSender.TransitionTypes TransitionType
	{
		get
		{
			return this.m_transitionType;
		}
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x001278DB File Offset: 0x00125CDB
	private void Awake()
	{
		if (this.m_useMotionBlur && this.m_postProcessingBehaviour != null)
		{
			this.m_postProcessingBehaviour.profile.motionBlur.enabled = false;
		}
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x0012790F File Offset: 0x00125D0F
	public void RegisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x00127928 File Offset: 0x00125D28
	public void DeregisterAnimationFinishedCallback(GenericVoid<string> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<string>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x00127941 File Offset: 0x00125D41
	public void OnAnimationFinished(string _animName)
	{
		this.m_animFinishedCallback(_animName);
	}

	// Token: 0x040031A4 RID: 12708
	[SerializeField]
	private TeleportalMapAvatarSender.TransitionTypes m_transitionType = TeleportalMapAvatarSender.TransitionTypes.Lerp;

	// Token: 0x040031A5 RID: 12709
	[SerializeField]
	[HideInInspectorTest("m_transitionType", TeleportalMapAvatarSender.TransitionTypes.Lerp)]
	public TeleportalMapAvatarSender.LerpConfig m_lerpConfig = new TeleportalMapAvatarSender.LerpConfig();

	// Token: 0x040031A6 RID: 12710
	[SerializeField]
	public bool m_useMotionBlur = true;

	// Token: 0x040031A7 RID: 12711
	[SerializeField]
	[HideInInspectorTest("m_useMotionBlur", true)]
	public PostProcessingBehaviour m_postProcessingBehaviour;

	// Token: 0x040031A8 RID: 12712
	private GenericVoid<string> m_animFinishedCallback = delegate(string _animName)
	{
	};

	// Token: 0x040031A9 RID: 12713
	public ManualAnimation m_SenderAnimation;

	// Token: 0x02000BD3 RID: 3027
	[Serializable]
	public enum TransitionTypes
	{
		// Token: 0x040031AC RID: 12716
		None,
		// Token: 0x040031AD RID: 12717
		ScreenTransition,
		// Token: 0x040031AE RID: 12718
		Lerp,
		// Token: 0x040031AF RID: 12719
		Instant
	}

	// Token: 0x02000BD4 RID: 3028
	[Serializable]
	public class LerpConfig
	{
		// Token: 0x040031B0 RID: 12720
		[SerializeField]
		public float GradientLimit = 20f;

		// Token: 0x040031B1 RID: 12721
		[SerializeField]
		public float TimeToMax = 0.5f;
	}
}
