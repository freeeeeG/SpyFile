using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000901 RID: 2305
	public class SnapLerp : EmptyLerp
	{
		// Token: 0x06002CF9 RID: 11513 RVA: 0x000D4480 File Offset: 0x000D2880
		public override void ReceiveServerUpdate(Vector3 localPosition, Quaternion localRotation)
		{
			this.ReceiveData(localPosition, localRotation);
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000D448A File Offset: 0x000D288A
		public override void ReceiveServerEvent(Vector3 localPosition, Quaternion localRotation)
		{
			this.ReceiveData(localPosition, localRotation);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000D4494 File Offset: 0x000D2894
		private void ReceiveData(Vector3 localPosition, Quaternion localRotation)
		{
			if (this.m_bSetNextData)
			{
				this.m_bSetNextData = false;
				base.transform.localPosition = localPosition;
				base.transform.localRotation = localRotation;
			}
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000D44C0 File Offset: 0x000D28C0
		public override void Reparented()
		{
			this.m_bSetNextData = true;
		}

		// Token: 0x04002422 RID: 9250
		private bool m_bSetNextData;
	}
}
