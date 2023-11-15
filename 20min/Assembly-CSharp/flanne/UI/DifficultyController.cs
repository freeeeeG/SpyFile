using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace flanne.UI
{
	// Token: 0x02000208 RID: 520
	public class DifficultyController : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0002B9E8 File Offset: 0x00029BE8
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x0002B9F0 File Offset: 0x00029BF0
		public int difficulty { get; private set; }

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002B9FC File Offset: 0x00029BFC
		public void OnMove(InputAction.CallbackContext context)
		{
			float x = context.ReadValue<Vector2>().x;
			if (x > 0.5f)
			{
				this.IncreaseDifficulty();
				return;
			}
			if (x < -0.5f)
			{
				this.DecreaseDifficulty();
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002BA33 File Offset: 0x00029C33
		public void OnSelect(BaseEventData eventData)
		{
			this._moveAction.performed += this.OnMove;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0002BA4C File Offset: 0x00029C4C
		public void OnDeselect(BaseEventData data)
		{
			this._moveAction.performed -= this.OnMove;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0002BA68 File Offset: 0x00029C68
		public void Init(int maxDiff)
		{
			this._moveAction = this.inputs.FindActionMap("UI", false).FindAction("Move", false);
			this._maxDiffictuly = maxDiff;
			int @int = PlayerPrefs.GetInt("LastSelectedDifficulty", -1);
			if (@int < 0)
			{
				this.SetDifficulty(maxDiff);
			}
			else
			{
				this.SetDifficulty(@int);
			}
			this.unlockTMP.text = string.Concat(new object[]
			{
				"Unlocked ",
				this._maxDiffictuly,
				"/",
				this.modList.mods.Length - 1
			});
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002BB0C File Offset: 0x00029D0C
		private void SetDifficulty(int diff)
		{
			diff = Mathf.Clamp(diff, 0, this._maxDiffictuly);
			this.difficulty = diff;
			this.labelTMP.text = LocalizationSystem.GetLocalizedValue(this.darknessLabel.key) + " " + this.difficulty;
			this.descriptionTMP.text = this.modList.mods[this.difficulty].description;
			Loadout.difficultyLevel = diff;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0002BB87 File Offset: 0x00029D87
		public void IncreaseDifficulty()
		{
			this.SetDifficulty(this.difficulty + 1);
			PlayerPrefs.SetInt("LastSelectedDifficulty", this.difficulty);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0002BBA7 File Offset: 0x00029DA7
		public void DecreaseDifficulty()
		{
			this.SetDifficulty(this.difficulty - 1);
			PlayerPrefs.SetInt("LastSelectedDifficulty", this.difficulty);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0002BBC7 File Offset: 0x00029DC7
		public void RefreshText()
		{
			this.SetDifficulty(this.difficulty);
		}

		// Token: 0x0400081E RID: 2078
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x0400081F RID: 2079
		[SerializeField]
		private TMP_Text labelTMP;

		// Token: 0x04000820 RID: 2080
		[SerializeField]
		private TMP_Text descriptionTMP;

		// Token: 0x04000821 RID: 2081
		[SerializeField]
		private TMP_Text unlockTMP;

		// Token: 0x04000822 RID: 2082
		[SerializeField]
		private DifficultyModList modList;

		// Token: 0x04000823 RID: 2083
		[SerializeField]
		private LocalizedString darknessLabel;

		// Token: 0x04000825 RID: 2085
		private int _maxDiffictuly;

		// Token: 0x04000826 RID: 2086
		private InputAction _moveAction;
	}
}
