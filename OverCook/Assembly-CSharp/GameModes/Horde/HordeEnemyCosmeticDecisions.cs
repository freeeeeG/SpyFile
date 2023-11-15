using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007AF RID: 1967
	[RequireComponent(typeof(HordeEnemy))]
	public class HordeEnemyCosmeticDecisions : MonoBehaviour, ITriggerReceiver
	{
		// Token: 0x060025C1 RID: 9665 RVA: 0x000B26A0 File Offset: 0x000B0AA0
		private void Awake()
		{
			this.m_idleAnimationTriggerId = Animator.StringToHash(this.m_idleAnimationTrigger);
			this.m_moveAnimationTriggerId = Animator.StringToHash(this.m_moveAnimationTrigger);
			this.m_spawnAnimationTriggerId = Animator.StringToHash(this.m_spawnAnimationTrigger);
			this.m_attackAnimationTriggerId = Animator.StringToHash(this.m_attackAnimationTrigger);
			this.m_channelAnimationTriggerId = Animator.StringToHash(this.m_channelAnimationTrigger);
			this.m_despawnAnimationTriggerId = Animator.StringToHash(this.m_despawnAnimationTrigger);
			this.m_attackAnimationImpactTriggerId = Animator.StringToHash(this.m_attackAnimationImpactTrigger);
			this.m_spawnAnimationEndTriggerId = Animator.StringToHash(this.m_spawnAnimationEndTrigger);
			this.m_attackAnimationEndTriggerId = Animator.StringToHash(this.m_attackAnimationEndTrigger);
			this.m_despawnAnimationEndTriggerId = Animator.StringToHash(this.m_despawnAnimationEndTrigger);
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000B2757 File Offset: 0x000B0B57
		public void OnTrigger(string trigger)
		{
			this.OnTriggerCallback(trigger);
		}

		// Token: 0x04001D67 RID: 7527
		[Header("Animation")]
		[SerializeField]
		public Animator m_animator;

		// Token: 0x04001D68 RID: 7528
		[SerializeField]
		private string m_idleAnimationTrigger = string.Empty;

		// Token: 0x04001D69 RID: 7529
		[SerializeField]
		private string m_moveAnimationTrigger = string.Empty;

		// Token: 0x04001D6A RID: 7530
		[SerializeField]
		private string m_spawnAnimationTrigger = string.Empty;

		// Token: 0x04001D6B RID: 7531
		[SerializeField]
		private string m_attackAnimationTrigger = string.Empty;

		// Token: 0x04001D6C RID: 7532
		[SerializeField]
		private string m_channelAnimationTrigger = string.Empty;

		// Token: 0x04001D6D RID: 7533
		[SerializeField]
		private string m_despawnAnimationTrigger = string.Empty;

		// Token: 0x04001D6E RID: 7534
		[SerializeField]
		private string m_attackAnimationImpactTrigger = string.Empty;

		// Token: 0x04001D6F RID: 7535
		[SerializeField]
		private string m_spawnAnimationEndTrigger = string.Empty;

		// Token: 0x04001D70 RID: 7536
		[SerializeField]
		private string m_attackAnimationEndTrigger = string.Empty;

		// Token: 0x04001D71 RID: 7537
		[SerializeField]
		private string m_despawnAnimationEndTrigger = string.Empty;

		// Token: 0x04001D72 RID: 7538
		[HideInInspector]
		[NonSerialized]
		public int m_idleAnimationTriggerId;

		// Token: 0x04001D73 RID: 7539
		[HideInInspector]
		[NonSerialized]
		public int m_moveAnimationTriggerId;

		// Token: 0x04001D74 RID: 7540
		[HideInInspector]
		[NonSerialized]
		public int m_spawnAnimationTriggerId;

		// Token: 0x04001D75 RID: 7541
		[HideInInspector]
		[NonSerialized]
		public int m_attackAnimationTriggerId;

		// Token: 0x04001D76 RID: 7542
		[HideInInspector]
		[NonSerialized]
		public int m_channelAnimationTriggerId;

		// Token: 0x04001D77 RID: 7543
		[HideInInspector]
		[NonSerialized]
		public int m_despawnAnimationTriggerId;

		// Token: 0x04001D78 RID: 7544
		[HideInInspector]
		[NonSerialized]
		public int m_attackAnimationImpactTriggerId;

		// Token: 0x04001D79 RID: 7545
		[HideInInspector]
		[NonSerialized]
		public int m_spawnAnimationEndTriggerId;

		// Token: 0x04001D7A RID: 7546
		[HideInInspector]
		[NonSerialized]
		public int m_attackAnimationEndTriggerId;

		// Token: 0x04001D7B RID: 7547
		[HideInInspector]
		[NonSerialized]
		public int m_channelAnimationEndTriggerId;

		// Token: 0x04001D7C RID: 7548
		[HideInInspector]
		[NonSerialized]
		public int m_despawnAnimationEndTriggerId;

		// Token: 0x04001D7D RID: 7549
		[Header("Audio")]
		[SerializeField]
		public GameOneShotAudioTag m_onSpawnAudioTag = GameOneShotAudioTag.COUNT;

		// Token: 0x04001D7E RID: 7550
		[SerializeField]
		public GameOneShotAudioTag m_onAttackAudioTag = GameOneShotAudioTag.COUNT;

		// Token: 0x04001D7F RID: 7551
		[SerializeField]
		public GameOneShotAudioTag m_onChannelAudioTag = GameOneShotAudioTag.COUNT;

		// Token: 0x04001D80 RID: 7552
		[SerializeField]
		public GameOneShotAudioTag m_onDespawnAudioTag = GameOneShotAudioTag.COUNT;

		// Token: 0x04001D81 RID: 7553
		[Header("Effects")]
		[SerializeField]
		public GameObject m_spawnEffectsPrefab;

		// Token: 0x04001D82 RID: 7554
		[SerializeField]
		public GameObject m_despawnEffectsPrefab;

		// Token: 0x04001D83 RID: 7555
		public GenericVoid<string> OnTriggerCallback;
	}
}
