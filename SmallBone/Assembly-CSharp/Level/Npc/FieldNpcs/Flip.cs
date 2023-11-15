using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005E6 RID: 1510
	[ExecuteAlways]
	public class Flip : MonoBehaviour
	{
		// Token: 0x06001E30 RID: 7728 RVA: 0x0005BD75 File Offset: 0x00059F75
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			base.StartCoroutine(this.<Awake>g__CRun|1_0());
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0005BD95 File Offset: 0x00059F95
		[CompilerGenerated]
		private IEnumerator <Awake>g__CRun|1_0()
		{
			yield return null;
			FieldNpc componentInChildren = base.GetComponentInChildren<FieldNpc>();
			if (componentInChildren != null)
			{
				componentInChildren.Flip();
			}
			UnityEngine.Object.Destroy(this);
			yield break;
		}

		// Token: 0x0400197D RID: 6525
		[SerializeField]
		private Transform _body;
	}
}
