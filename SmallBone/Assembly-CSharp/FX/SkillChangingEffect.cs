using System;
using System.Collections;
using UnityEngine;

namespace FX
{
	// Token: 0x02000259 RID: 601
	public class SkillChangingEffect : MonoBehaviour
	{
		// Token: 0x06000BC5 RID: 3013 RVA: 0x000206B0 File Offset: 0x0001E8B0
		private void Awake()
		{
			this._animationLength = this._animator.GetCurrentAnimatorStateInfo(0).length;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000206D8 File Offset: 0x0001E8D8
		public void Play(Sprite[] oldSkills, Sprite[] newSkills)
		{
			if (oldSkills.Length != 0)
			{
				this._oldSkill1.enabled = true;
				this._oldSkill1.sprite = oldSkills[0];
			}
			else
			{
				this._oldSkill1.enabled = false;
			}
			if (oldSkills.Length > 1)
			{
				this._oldSkill2.enabled = true;
				this._oldSkill2.sprite = oldSkills[1];
			}
			else
			{
				this._oldSkill2.enabled = false;
			}
			if (newSkills.Length != 0)
			{
				this._newSkill1.enabled = true;
				this._newSkill1.sprite = newSkills[0];
			}
			else
			{
				this._newSkill1.enabled = false;
			}
			if (newSkills.Length > 1)
			{
				this._newSkill2.enabled = true;
				this._newSkill2.sprite = newSkills[1];
			}
			else
			{
				this._newSkill2.enabled = false;
			}
			this.PlayAnimation();
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000207A0 File Offset: 0x0001E9A0
		private void PlayAnimation()
		{
			base.gameObject.SetActive(true);
			this._animator.enabled = true;
			this._animator.Play(0, 0, 0f);
			this._animator.enabled = false;
			this._remainTime = this._animationLength;
			base.StopAllCoroutines();
			base.StartCoroutine(this.CPlay());
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00020802 File Offset: 0x0001EA02
		private IEnumerator CPlay()
		{
			this._animator.Update(0f);
			while (this._remainTime > 0f)
			{
				yield return null;
				float deltaTime = Chronometer.global.deltaTime;
				this._animator.Update(deltaTime);
				this._remainTime -= deltaTime;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040009CB RID: 2507
		[SerializeField]
		private Animator _animator;

		// Token: 0x040009CC RID: 2508
		[SerializeField]
		private SpriteRenderer _oldSkill1;

		// Token: 0x040009CD RID: 2509
		[SerializeField]
		private SpriteRenderer _oldSkill2;

		// Token: 0x040009CE RID: 2510
		[SerializeField]
		private SpriteRenderer _newSkill1;

		// Token: 0x040009CF RID: 2511
		[SerializeField]
		private SpriteRenderer _newSkill2;

		// Token: 0x040009D0 RID: 2512
		private float _animationLength;

		// Token: 0x040009D1 RID: 2513
		private float _remainTime;
	}
}
