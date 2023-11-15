using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B2 RID: 1970
	public class HordeLockableCosmeticDecisions : MonoBehaviour
	{
		// Token: 0x060025CB RID: 9675 RVA: 0x000B299E File Offset: 0x000B0D9E
		private void Awake()
		{
			this.m_lockAnimationId = Animator.StringToHash(this.m_lockAnimationString);
			this.m_unlockAnimationId = Animator.StringToHash(this.m_unlockAnimationString);
		}

		// Token: 0x04001D8A RID: 7562
		[Header("User Interface")]
		[SerializeField]
		public Sprite m_hoverIcon;

		// Token: 0x04001D8B RID: 7563
		[SerializeField]
		public GameObject m_hoverIconPrefab;

		// Token: 0x04001D8C RID: 7564
		[SerializeField]
		public Transform m_hoverIconTarget;

		// Token: 0x04001D8D RID: 7565
		[SerializeField]
		public Vector2 m_offset;

		// Token: 0x04001D8E RID: 7566
		[Header("Animation")]
		[SerializeField]
		public Animator m_animator;

		// Token: 0x04001D8F RID: 7567
		[SerializeField]
		private string m_lockAnimationString = string.Empty;

		// Token: 0x04001D90 RID: 7568
		[SerializeField]
		private string m_unlockAnimationString = string.Empty;

		// Token: 0x04001D91 RID: 7569
		[HideInInspector]
		[NonSerialized]
		public int m_lockAnimationId;

		// Token: 0x04001D92 RID: 7570
		[HideInInspector]
		[NonSerialized]
		public int m_unlockAnimationId;

		// Token: 0x04001D93 RID: 7571
		[Header("Audio")]
		[SerializeField]
		public GameOneShotAudioTag m_onLockAudioTag = GameOneShotAudioTag.COUNT;

		// Token: 0x04001D94 RID: 7572
		[SerializeField]
		public GameOneShotAudioTag m_onUnlockAudioTag = GameOneShotAudioTag.COUNT;
	}
}
