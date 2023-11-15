using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions.FairyTaleSummon
{
	// Token: 0x020008D3 RID: 2259
	public sealed class OberonBombOrb : MonoBehaviour
	{
		// Token: 0x0600301D RID: 12317 RVA: 0x0009060A File Offset: 0x0008E80A
		private void Awake()
		{
			this._activateInfo.Initialize();
			this._deactivateInfo.Initialize();
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x00090622 File Offset: 0x0008E822
		public void Activate(Character owner)
		{
			this.Show();
			this.MoveRandom();
			this._activateInfo.gameObject.SetActive(true);
			this._activateInfo.Run(owner);
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x0009064D File Offset: 0x0008E84D
		public void Deactivate(Character owner)
		{
			this.Hide();
			this._deactivateInfo.gameObject.SetActive(true);
			this._deactivateInfo.Run(owner);
			this.Restore();
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x00090678 File Offset: 0x0008E878
		private void MoveRandom()
		{
			this._originPoistion = base.transform.position;
			base.transform.Translate(UnityEngine.Random.insideUnitSphere * this._noise);
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x000906AB File Offset: 0x0008E8AB
		private void Restore()
		{
			base.transform.position = this._originPoistion;
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x000906C3 File Offset: 0x0008E8C3
		private void Show()
		{
			this._body.SetActive(true);
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x000906D1 File Offset: 0x0008E8D1
		private void Hide()
		{
			this._body.SetActive(false);
		}

		// Token: 0x040027DB RID: 10203
		[SerializeField]
		private GameObject _body;

		// Token: 0x040027DC RID: 10204
		[SerializeField]
		private float _noise;

		// Token: 0x040027DD RID: 10205
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _activateInfo;

		// Token: 0x040027DE RID: 10206
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _deactivateInfo;

		// Token: 0x040027DF RID: 10207
		private Vector2 _originPoistion;
	}
}
