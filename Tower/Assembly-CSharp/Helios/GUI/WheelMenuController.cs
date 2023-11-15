using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Helios.GUI
{
	// Token: 0x020000E8 RID: 232
	public class WheelMenuController : MonoBehaviour
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0000F241 File Offset: 0x0000D441
		private void Awake()
		{
			this.TurnButton.onClick.AddListener(new UnityAction(this.TurnWheel));
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000F25F File Offset: 0x0000D45F
		private IEnumerator ShowReward(int index)
		{
			yield return new WaitForSeconds(0.4f);
			this.TurnButton.interactable = true;
			this._animator.SetTrigger("Released");
			this._imgFocusLine.gameObject.SetActive(true);
			this._goRewardPopup.SetActive(true);
			this._imgRewardIcon.sprite = this._imgRewards[index].sprite;
			yield break;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000F275 File Offset: 0x0000D475
		private void Start()
		{
			this.spinning = false;
			this.anglePerItem = (float)(360 / this._imgRewards.Length);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000F294 File Offset: 0x0000D494
		private void TurnWheel()
		{
			if (!this.spinning)
			{
				this._animator.SetTrigger("Pressed");
				this._btnTapToClose.interactable = false;
				this._imgFocusLine.gameObject.SetActive(false);
				this.itemNumber = Random.Range(0, this._imgRewards.Length);
				float maxAngle = (float)(this._nbSpinTime * 360) + (float)this.itemNumber * this.anglePerItem;
				base.StartCoroutine(this.SpinTheWheel((float)this._nbAnimationTime, maxAngle));
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000F31C File Offset: 0x0000D51C
		private IEnumerator SpinTheWheel(float time, float maxAngle)
		{
			this.spinning = true;
			this.TurnButton.interactable = false;
			float timer = 0f;
			float startAngle = this.Circle.transform.eulerAngles.z;
			maxAngle -= startAngle;
			int animationCurveNumber = Random.Range(0, this.animationCurves.Count);
			Debug.Log("Animation Curve No. : " + animationCurveNumber.ToString());
			while (timer < time)
			{
				float num = maxAngle * this.animationCurves[animationCurveNumber].Evaluate(timer / time);
				this.Circle.transform.eulerAngles = new Vector3(0f, 0f, num + startAngle);
				timer += Time.deltaTime;
				yield return 0;
			}
			this.Circle.transform.eulerAngles = new Vector3(0f, 0f, maxAngle + startAngle);
			this.spinning = false;
			this._btnTapToClose.interactable = true;
			base.StartCoroutine(this.ShowReward(this.itemNumber));
			yield break;
		}

		// Token: 0x0400032F RID: 815
		private const int FULL_CIRCLE = 360;

		// Token: 0x04000330 RID: 816
		[Header("References")]
		[SerializeField]
		private Image[] _imgRewards;

		// Token: 0x04000331 RID: 817
		[SerializeField]
		private GameObject _goRewardPopup;

		// Token: 0x04000332 RID: 818
		[SerializeField]
		private Image _imgRewardIcon;

		// Token: 0x04000333 RID: 819
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000334 RID: 820
		[SerializeField]
		private Image _imgFocusLine;

		// Token: 0x04000335 RID: 821
		[SerializeField]
		private Button _btnTapToClose;

		// Token: 0x04000336 RID: 822
		[SerializeField]
		private Button TurnButton;

		// Token: 0x04000337 RID: 823
		[SerializeField]
		private GameObject Circle;

		// Token: 0x04000338 RID: 824
		[Header("Config params")]
		[SerializeField]
		private int _nbSpinTime = 5;

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private int _nbAnimationTime = 3;

		// Token: 0x0400033A RID: 826
		[SerializeField]
		private List<AnimationCurve> animationCurves;

		// Token: 0x0400033B RID: 827
		private bool spinning;

		// Token: 0x0400033C RID: 828
		private float anglePerItem;

		// Token: 0x0400033D RID: 829
		private int itemNumber;
	}
}
