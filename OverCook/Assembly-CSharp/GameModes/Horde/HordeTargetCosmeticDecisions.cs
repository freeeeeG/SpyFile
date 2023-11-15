using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B5 RID: 1973
	[RequireComponent(typeof(HordeTarget))]
	public class HordeTargetCosmeticDecisions : MonoBehaviour
	{
		// Token: 0x060025DB RID: 9691 RVA: 0x000B320C File Offset: 0x000B160C
		public void Awake()
		{
			this.m_healthAnimationId = Animator.StringToHash(this.m_healthAnimationString);
			this.m_enemyAnimationId = Animator.StringToHash(this.m_enemyAnimationString);
			this.m_hitAnimationId = Animator.StringToHash(this.m_hitAnimationString);
			this.m_repairHitAnimationId = Animator.StringToHash(this.m_repairHitAnimationString);
		}

		// Token: 0x04001DA4 RID: 7588
		[Header("User Interface")]
		[AssignResource("HoverProgressUI", Editorbility.Editable)]
		[SerializeField]
		public GameObject m_healthUIPrefab;

		// Token: 0x04001DA5 RID: 7589
		[SerializeField]
		public Transform m_healthUITransform;

		// Token: 0x04001DA6 RID: 7590
		[AssignResource("horde_order_ui", Editorbility.Editable)]
		[SerializeField]
		public GameObject m_orderUIPrefab;

		// Token: 0x04001DA7 RID: 7591
		[SerializeField]
		public Transform m_orderUITransform;

		// Token: 0x04001DA8 RID: 7592
		[SerializeField]
		public HordeOrderUIController.Align m_orderUIAnchor;

		// Token: 0x04001DA9 RID: 7593
		[Header("Animation")]
		[SerializeField]
		public Animator m_animator;

		// Token: 0x04001DAA RID: 7594
		[SerializeField]
		private string m_healthAnimationString = string.Empty;

		// Token: 0x04001DAB RID: 7595
		[HideInInspector]
		[NonSerialized]
		public int m_healthAnimationId;

		// Token: 0x04001DAC RID: 7596
		[SerializeField]
		private string m_enemyAnimationString = string.Empty;

		// Token: 0x04001DAD RID: 7597
		[HideInInspector]
		[NonSerialized]
		public int m_enemyAnimationId;

		// Token: 0x04001DAE RID: 7598
		[SerializeField]
		private string m_hitAnimationString = string.Empty;

		// Token: 0x04001DAF RID: 7599
		[HideInInspector]
		[NonSerialized]
		public int m_hitAnimationId;

		// Token: 0x04001DB0 RID: 7600
		[SerializeField]
		private string m_repairHitAnimationString = string.Empty;

		// Token: 0x04001DB1 RID: 7601
		[HideInInspector]
		[NonSerialized]
		public int m_repairHitAnimationId;

		// Token: 0x04001DB2 RID: 7602
		[SerializeField]
		public AnimationCurve m_orderAppearAnimationCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1f);

		// Token: 0x04001DB3 RID: 7603
		[SerializeField]
		public AnimationCurve m_orderHealthWarningAnimationCurve = AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1f);

		// Token: 0x04001DB4 RID: 7604
		[Header("Effects")]
		[SerializeField]
		public GameObject m_hitParticlePrefab;

		// Token: 0x04001DB5 RID: 7605
		[SerializeField]
		public GameObject m_breakParticlePrefab;

		// Token: 0x04001DB6 RID: 7606
		[SerializeField]
		public GameObject m_destroyParticlePrefab;
	}
}
