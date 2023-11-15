using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003AE RID: 942
	public class LoadingScreen : MonoBehaviour
	{
		// Token: 0x06001167 RID: 4455 RVA: 0x0003378C File Offset: 0x0003198C
		private void Awake()
		{
			this._loadingAnimatorOverrider = new AnimatorOverrideController(this._loadingAnimator.runtimeAnimatorController);
			this._loadingAnimator.runtimeAnimatorController = this._loadingAnimatorOverrider;
			this._loadingAnimator_AlphaAddtion1.runtimeAnimatorController = this._loadingAnimatorOverrider;
			this._loadingAnimator_AlphaAddtion2.runtimeAnimatorController = this._loadingAnimatorOverrider;
			this.spriteRenderer[0] = this._loadingAnimation.gameObject.AddComponent<SpriteRenderer>();
			this.spriteRenderer[1] = this._loadingAnimation_AlphaAddtion1.gameObject.AddComponent<SpriteRenderer>();
			this.spriteRenderer[2] = this._loadingAnimation_AlphaAddtion2.gameObject.AddComponent<SpriteRenderer>();
			this._canvasGroup.alpha = 0f;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0003383A File Offset: 0x00031A3A
		public IEnumerator CShow(LoadingScreen.LoadingScreenData loadingScreenData)
		{
			float t = 0f;
			this._background.sprite = loadingScreenData.background;
			if (loadingScreenData.walkClip == null)
			{
				this._loadingAnimatorOverrider["Walk"] = this._defaultWalkClip;
			}
			else
			{
				this._loadingAnimatorOverrider["Walk"] = loadingScreenData.walkClip;
			}
			this._loadingAnimator.Update(0f);
			this._loadingAnimator_AlphaAddtion1.Update(0f);
			this._loadingAnimator_AlphaAddtion2.Update(0f);
			this._loadingAnimation.SetNativeSize();
			this._loadingAnimation_AlphaAddtion1.SetNativeSize();
			this._loadingAnimation_AlphaAddtion2.SetNativeSize();
			this._name.text = loadingScreenData.stageName;
			this._description.text = loadingScreenData.description;
			if (loadingScreenData.displayTime)
			{
				this._currentTimeContainer.SetActive(true);
				this._bestTimeContainer.SetActive(true);
				this._currentTime.text = loadingScreenData.currentTime;
				this._bestTime.text = loadingScreenData.bestTime;
				this._bestTime.color = (loadingScreenData.bestTimeUpdated ? LoadingScreen._updatedBestTimeColor : LoadingScreen._bestTimeColor);
			}
			else
			{
				this._currentTimeContainer.SetActive(false);
				this._bestTimeContainer.SetActive(false);
			}
			this._canvasGroup.alpha = 0f;
			yield return null;
			while (t < 1f)
			{
				this._canvasGroup.alpha = t;
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			this._canvasGroup.alpha = 1f;
			yield break;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00033850 File Offset: 0x00031A50
		private void Update()
		{
			if (this._canvasGroup.alpha != 0f)
			{
				this._loadingAnimation.sprite = this.spriteRenderer[0].sprite;
				this._loadingAnimation.SetNativeSize();
				this._loadingAnimation_AlphaAddtion1.sprite = this.spriteRenderer[1].sprite;
				this._loadingAnimation_AlphaAddtion1.SetNativeSize();
				this._loadingAnimation_AlphaAddtion2.sprite = this.spriteRenderer[2].sprite;
				this._loadingAnimation_AlphaAddtion2.SetNativeSize();
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000338D8 File Offset: 0x00031AD8
		public IEnumerator CHide()
		{
			float t = 0f;
			this._canvasGroup.alpha = 1f;
			yield return null;
			while (t < 1f)
			{
				this._canvasGroup.alpha = 1f - t;
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			this._loadingAnimatorOverrider["Walk"] = null;
			this._canvasGroup.alpha = 0f;
			yield break;
		}

		// Token: 0x04000E52 RID: 3666
		public const float minimumDisplayingTime = 3f;

		// Token: 0x04000E53 RID: 3667
		public const string walkAnimationName = "Walk";

		// Token: 0x04000E54 RID: 3668
		private static readonly Color _bestTimeColor = new Color(0.635f, 0.509f, 0.407f);

		// Token: 0x04000E55 RID: 3669
		private static readonly Color _updatedBestTimeColor = new Color(1f, 0.823f, 0f);

		// Token: 0x04000E56 RID: 3670
		[SerializeField]
		private CanvasGroup _canvasGroup;

		// Token: 0x04000E57 RID: 3671
		[SerializeField]
		[Space]
		private Image _background;

		// Token: 0x04000E58 RID: 3672
		[SerializeField]
		[Header("Loading Animation")]
		private Image _loadingAnimation;

		// Token: 0x04000E59 RID: 3673
		[SerializeField]
		private Image _loadingAnimation_AlphaAddtion1;

		// Token: 0x04000E5A RID: 3674
		[SerializeField]
		private Image _loadingAnimation_AlphaAddtion2;

		// Token: 0x04000E5B RID: 3675
		[Header("Loading Animator")]
		[SerializeField]
		private Animator _loadingAnimator;

		// Token: 0x04000E5C RID: 3676
		[SerializeField]
		private Animator _loadingAnimator_AlphaAddtion1;

		// Token: 0x04000E5D RID: 3677
		[SerializeField]
		private Animator _loadingAnimator_AlphaAddtion2;

		// Token: 0x04000E5E RID: 3678
		[Space]
		[SerializeField]
		private AnimationClip _defaultWalkClip;

		// Token: 0x04000E5F RID: 3679
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x04000E60 RID: 3680
		[SerializeField]
		private TMP_Text _description;

		// Token: 0x04000E61 RID: 3681
		[SerializeField]
		[Space]
		private GameObject _currentTimeContainer;

		// Token: 0x04000E62 RID: 3682
		[SerializeField]
		private TMP_Text _currentTime;

		// Token: 0x04000E63 RID: 3683
		[Space]
		[SerializeField]
		private GameObject _bestTimeContainer;

		// Token: 0x04000E64 RID: 3684
		[SerializeField]
		private TMP_Text _bestTime;

		// Token: 0x04000E65 RID: 3685
		[Space]
		[SerializeField]
		private GameObject _difficultyContainer;

		// Token: 0x04000E66 RID: 3686
		[SerializeField]
		private TMP_Text _difficulty;

		// Token: 0x04000E67 RID: 3687
		private AnimatorOverrideController _loadingAnimatorOverrider;

		// Token: 0x04000E68 RID: 3688
		private SpriteRenderer[] spriteRenderer = new SpriteRenderer[3];

		// Token: 0x020003AF RID: 943
		public struct LoadingScreenData
		{
			// Token: 0x0600116D RID: 4461 RVA: 0x0003392F File Offset: 0x00031B2F
			public LoadingScreenData(Sprite background, AnimationClip walkClip, string stageName, string description, string currentTime, string bestTime, bool bestTimeUpdated)
			{
				this.background = background;
				this.walkClip = walkClip;
				this.stageName = stageName;
				this.description = description;
				this.displayTime = true;
				this.currentTime = currentTime;
				this.bestTime = bestTime;
				this.bestTimeUpdated = bestTimeUpdated;
			}

			// Token: 0x0600116E RID: 4462 RVA: 0x00033970 File Offset: 0x00031B70
			public LoadingScreenData(Sprite background, AnimationClip walkClip, string stageName, string description)
			{
				this.background = background;
				this.walkClip = walkClip;
				this.stageName = stageName;
				this.description = description;
				this.displayTime = false;
				this.currentTime = string.Empty;
				this.bestTime = string.Empty;
				this.bestTimeUpdated = false;
			}

			// Token: 0x04000E69 RID: 3689
			public readonly Sprite background;

			// Token: 0x04000E6A RID: 3690
			public readonly AnimationClip walkClip;

			// Token: 0x04000E6B RID: 3691
			public readonly string stageName;

			// Token: 0x04000E6C RID: 3692
			public readonly string description;

			// Token: 0x04000E6D RID: 3693
			public readonly bool displayTime;

			// Token: 0x04000E6E RID: 3694
			public readonly string currentTime;

			// Token: 0x04000E6F RID: 3695
			public readonly string bestTime;

			// Token: 0x04000E70 RID: 3696
			public readonly bool bestTimeUpdated;
		}
	}
}
