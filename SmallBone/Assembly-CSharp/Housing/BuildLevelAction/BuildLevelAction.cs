using System;
using UnityEngine;

namespace Housing.BuildLevelAction
{
	// Token: 0x0200014C RID: 332
	[RequireComponent(typeof(BuildLevel))]
	public abstract class BuildLevelAction : MonoBehaviour
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x0001344A File Offset: 0x0001164A
		protected void Initialize()
		{
			this._buildLevel = base.GetComponent<BuildLevel>();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00013458 File Offset: 0x00011658
		private void Awake()
		{
			if (this._type == BuildLevelAction.Type.OnBuild)
			{
				this._buildLevel.onBuild += this.Run;
				return;
			}
			if (this._type == BuildLevelAction.Type.OnNew)
			{
				this._buildLevel.onNew += this.Run;
			}
		}

		// Token: 0x060006AA RID: 1706
		protected abstract void Run();

		// Token: 0x040004DB RID: 1243
		[GetComponent]
		[SerializeField]
		private BuildLevel _buildLevel;

		// Token: 0x040004DC RID: 1244
		[SerializeField]
		protected BuildLevelAction.Type _type;

		// Token: 0x0200014D RID: 333
		protected enum Type
		{
			// Token: 0x040004DE RID: 1246
			OnBuild,
			// Token: 0x040004DF RID: 1247
			OnNew
		}
	}
}
