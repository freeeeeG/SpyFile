using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000207 RID: 519
	public class ChestUIController : MonoBehaviour
	{
		// Token: 0x06000BB0 RID: 2992 RVA: 0x0002B8AD File Offset: 0x00029AAD
		private void OnTakeClick()
		{
			EventHandler takeClickEvent = this.TakeClickEvent;
			if (takeClickEvent == null)
			{
				return;
			}
			takeClickEvent(this, null);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002B8C1 File Offset: 0x00029AC1
		private void OnLeaveClick()
		{
			EventHandler leaveClickEvent = this.LeaveClickEvent;
			if (leaveClickEvent == null)
			{
				return;
			}
			leaveClickEvent(this, null);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002B8D5 File Offset: 0x00029AD5
		public void SetToPowerup(Powerup powerup)
		{
			this.powerupWidget.SetProperties(new PowerupProperties(powerup));
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002B8E8 File Offset: 0x00029AE8
		public void Show()
		{
			this.chestRenderer.enabled = true;
			this.chestAnimator.Play("A_ChestAnimation", -1, 0f);
			base.StartCoroutine(this.WaitToChestOpen());
			this.takeButton.onClick.AddListener(new UnityAction(this.OnTakeClick));
			this.leaveButton.onClick.AddListener(new UnityAction(this.OnLeaveClick));
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002B95C File Offset: 0x00029B5C
		public void Hide()
		{
			this.chestRenderer.enabled = false;
			this.powerupIconPanel.Hide();
			this.powerupDescriptionPanel.Hide();
			this.takeButtonPanel.Hide();
			this.leaveButtonPanel.Hide();
			this.takeButton.onClick.RemoveListener(new UnityAction(this.OnTakeClick));
			this.leaveButton.onClick.RemoveListener(new UnityAction(this.OnLeaveClick));
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002B9D9 File Offset: 0x00029BD9
		private IEnumerator WaitToChestOpen()
		{
			SoundEffectSO soundEffectSO = this.chestLeadupSFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			yield return new WaitForSecondsRealtime(this.chestOpenTiming);
			SoundEffectSO soundEffectSO2 = this.chestOpenSFX;
			if (soundEffectSO2 != null)
			{
				soundEffectSO2.Play(null);
			}
			this.coinParticles.Play();
			this.powerupIconPanel.Show();
			this.powerupIconPanel.transform.localPosition = Vector3.zero;
			LeanTween.moveLocalY(this.powerupIconPanel.gameObject, 50f, 0.5f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutBack);
			this.screenFlash.Flash(3);
			yield return new WaitForSecondsRealtime(0.1f);
			this.powerupDescriptionPanel.Show();
			yield return new WaitForSecondsRealtime(1f);
			this.takeButtonPanel.Show();
			this.leaveButtonPanel.Show();
			yield break;
		}

		// Token: 0x0400080E RID: 2062
		public EventHandler TakeClickEvent;

		// Token: 0x0400080F RID: 2063
		public EventHandler LeaveClickEvent;

		// Token: 0x04000810 RID: 2064
		[SerializeField]
		private ParticleSystem coinParticles;

		// Token: 0x04000811 RID: 2065
		[SerializeField]
		private SpriteRenderer chestRenderer;

		// Token: 0x04000812 RID: 2066
		[SerializeField]
		private Animator chestAnimator;

		// Token: 0x04000813 RID: 2067
		[SerializeField]
		private PowerupWidget powerupWidget;

		// Token: 0x04000814 RID: 2068
		[SerializeField]
		private Panel powerupIconPanel;

		// Token: 0x04000815 RID: 2069
		[SerializeField]
		private Panel powerupDescriptionPanel;

		// Token: 0x04000816 RID: 2070
		[SerializeField]
		private Panel takeButtonPanel;

		// Token: 0x04000817 RID: 2071
		[SerializeField]
		private Button takeButton;

		// Token: 0x04000818 RID: 2072
		[SerializeField]
		private Panel leaveButtonPanel;

		// Token: 0x04000819 RID: 2073
		[SerializeField]
		private Button leaveButton;

		// Token: 0x0400081A RID: 2074
		[SerializeField]
		private ScreenFlash screenFlash;

		// Token: 0x0400081B RID: 2075
		[SerializeField]
		private SoundEffectSO chestLeadupSFX;

		// Token: 0x0400081C RID: 2076
		[SerializeField]
		private SoundEffectSO chestOpenSFX;

		// Token: 0x0400081D RID: 2077
		[SerializeField]
		private float chestOpenTiming;
	}
}
