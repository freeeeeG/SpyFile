using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200008A RID: 138
	public abstract class VersionedMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver, IVersionedMonoBehaviourInternal
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x000280FF File Offset: 0x000262FF
		protected virtual void Awake()
		{
			if (Application.isPlaying)
			{
				this.version = this.OnUpgradeSerializedData(int.MaxValue, true);
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002811A File Offset: 0x0002631A
		protected virtual void Reset()
		{
			this.version = this.OnUpgradeSerializedData(int.MaxValue, true);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002812E File Offset: 0x0002632E
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00028130 File Offset: 0x00026330
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			int num = this.OnUpgradeSerializedData(this.version, false);
			if (num >= 0)
			{
				this.version = num;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00028156 File Offset: 0x00026356
		protected virtual int OnUpgradeSerializedData(int version, bool unityThread)
		{
			return 1;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002815C File Offset: 0x0002635C
		void IVersionedMonoBehaviourInternal.UpgradeFromUnityThread()
		{
			int num = this.OnUpgradeSerializedData(this.version, true);
			if (num < 0)
			{
				throw new Exception();
			}
			this.version = num;
		}

		// Token: 0x040003E4 RID: 996
		[SerializeField]
		[HideInInspector]
		private int version;
	}
}
