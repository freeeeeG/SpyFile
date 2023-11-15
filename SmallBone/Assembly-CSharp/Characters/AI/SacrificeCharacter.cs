using System;
using Characters.AI.Behaviours;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001110 RID: 4368
	public sealed class SacrificeCharacter : MonoBehaviour
	{
		// Token: 0x060054F8 RID: 21752 RVA: 0x000FE027 File Offset: 0x000FC227
		private void Awake()
		{
			this._character = this._aiController.character;
			this._onForceSacrifice.Initialize();
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x000FE048 File Offset: 0x000FC248
		public void Run(bool force = false)
		{
			if (this.AlreadyCheck())
			{
				return;
			}
			if (this._aiController != null && this._aiController.gameObject.activeSelf)
			{
				if (force)
				{
					this._aiController.character.CancelAction();
					base.StartCoroutine(this._onForceSacrifice.CRun(this._character));
				}
				base.StartCoroutine(this._sacrifice.CRun(this._aiController));
			}
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x000FE0C1 File Offset: 0x000FC2C1
		public bool CanSacrifice()
		{
			return this._character.health.percent < this._triggerHealthPercent;
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x000FE0DB File Offset: 0x000FC2DB
		private bool AlreadyCheck()
		{
			return this._sacrifice.result == Characters.AI.Behaviours.Behaviour.Result.Doing;
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x000FE0EB File Offset: 0x000FC2EB
		private void Update()
		{
			if (this._character.health.dead)
			{
				return;
			}
			if (this.AlreadyCheck())
			{
				return;
			}
			if (!this.CanSacrifice())
			{
				return;
			}
			this.Run(false);
		}

		// Token: 0x04004422 RID: 17442
		[SerializeField]
		private AIController _aiController;

		// Token: 0x04004423 RID: 17443
		[SerializeField]
		private Sacrifice _sacrifice;

		// Token: 0x04004424 RID: 17444
		[SerializeField]
		[Range(0f, 1f)]
		private double _triggerHealthPercent;

		// Token: 0x04004425 RID: 17445
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onForceSacrifice;

		// Token: 0x04004426 RID: 17446
		private Character _character;
	}
}
