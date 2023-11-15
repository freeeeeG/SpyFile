using System;
using flanne.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace flanne.CharacterPassives
{
	// Token: 0x02000260 RID: 608
	public abstract class SkillPassive : MonoBehaviour
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x00030080 File Offset: 0x0002E280
		private void PerformSkillCallback(InputAction.CallbackContext context)
		{
			if (this._timer <= 0f && !PauseController.isPaused)
			{
				this._timer += this.cooldown;
				this.PerformSkill();
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000300CC File Offset: 0x0002E2CC
		private void Start()
		{
			this._skillAction = this.inputs.FindActionMap("PlayerMap", false).FindAction("Skill", false);
			this._skillAction.performed += this.PerformSkillCallback;
			this.Init();
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00030118 File Offset: 0x0002E318
		private void OnDestroy()
		{
			this._skillAction.performed -= this.PerformSkillCallback;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00030131 File Offset: 0x0002E331
		private void Update()
		{
			if (this._timer > 0f)
			{
				this._timer -= Time.deltaTime;
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00002F51 File Offset: 0x00001151
		protected virtual void Init()
		{
		}

		// Token: 0x06000D2E RID: 3374
		protected abstract void PerformSkill();

		// Token: 0x0400098B RID: 2443
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x0400098C RID: 2444
		[SerializeField]
		private float cooldown;

		// Token: 0x0400098D RID: 2445
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x0400098E RID: 2446
		private InputAction _skillAction;

		// Token: 0x0400098F RID: 2447
		private float _timer;
	}
}
