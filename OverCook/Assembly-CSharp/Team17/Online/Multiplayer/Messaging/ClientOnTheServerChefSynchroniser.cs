using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x0200090A RID: 2314
	public class ClientOnTheServerChefSynchroniser : ClientWorldObjectSynchroniser
	{
		// Token: 0x06002D3F RID: 11583 RVA: 0x000D6075 File Offset: 0x000D4475
		public override void Awake()
		{
			base.Awake();
			this.m_idProvider = base.GetComponent<PlayerIDProvider>();
			this.m_bHasEverReceived = true;
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000D6090 File Offset: 0x000D4490
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000D6098 File Offset: 0x000D4498
		public override void StartSynchronising(Component synchronisedObject)
		{
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000D609A File Offset: 0x000D449A
		public override EntityType GetEntityType()
		{
			return EntityType.Chef;
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000D609D File Offset: 0x000D449D
		public override void ApplyServerEvent(Serialisable serialisable)
		{
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000D609F File Offset: 0x000D449F
		public override void ApplyServerUpdate(Serialisable serialisable)
		{
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000D60A1 File Offset: 0x000D44A1
		public override void Pause()
		{
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000D60A3 File Offset: 0x000D44A3
		public override void Resume()
		{
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000D60A5 File Offset: 0x000D44A5
		public override void OnResumeDataReceived(Serialisable _data)
		{
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000D60A7 File Offset: 0x000D44A7
		public override bool IsReadyToResume()
		{
			return true;
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000D60AC File Offset: 0x000D44AC
		public override void UpdateSynchronising()
		{
			base.UpdateSynchronising();
			if (!this.m_idProvider.IsLocallyControlled())
			{
				List<ServerPhysicsObjectSynchroniser.SerialisationEntryTransformPair> allSynchroniserSerialisationEntryTransformPairs = ServerPhysicsObjectSynchroniser.GetAllSynchroniserSerialisationEntryTransformPairs();
				Vector3 position = base.transform.position;
				for (int i = 0; i < allSynchroniserSerialisationEntryTransformPairs.Count; i++)
				{
					ServerPhysicsObjectSynchroniser.SerialisationEntryTransformPair serialisationEntryTransformPair = allSynchroniserSerialisationEntryTransformPairs[i];
					if ((serialisationEntryTransformPair.m_Transform.position - position).sqrMagnitude < 4f)
					{
						serialisationEntryTransformPair.m_Entry.SetRequiresUrgentUpdate(true);
					}
				}
			}
		}

		// Token: 0x04002442 RID: 9282
		public const float kFastFoodRadius = 2f;

		// Token: 0x04002443 RID: 9283
		private PlayerIDProvider m_idProvider;
	}
}
