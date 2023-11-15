using System;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x0200015E RID: 350
	public sealed class DarktechReward : MonoBehaviour
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x000140AC File Offset: 0x000122AC
		private void Start()
		{
			Singleton<DarktechManager>.Instance.UnlockDarktech(this._type);
		}

		// Token: 0x0400051C RID: 1308
		[SerializeField]
		private DarktechData.Type _type;
	}
}
