using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Upgrades
{
	// Token: 0x020003EA RID: 1002
	public sealed class Level : MonoBehaviour
	{
		// Token: 0x060012BB RID: 4795 RVA: 0x00038048 File Offset: 0x00036248
		public void Set(int level, int maxLevel, bool risky, bool flick = false)
		{
			this.DeactivateAll();
			Sprite frame = risky ? this._risky : this._normal;
			for (int i = 0; i < maxLevel; i++)
			{
				this._gems[i].Set(i, level, frame);
			}
			if (!this._flick)
			{
				return;
			}
			if (level == maxLevel)
			{
				return;
			}
			if (flick)
			{
				this._gems[level].Flick();
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000380A8 File Offset: 0x000362A8
		public void LevelUp(int level)
		{
			base.StartCoroutine(this._gems[level - 1].CLevelUp());
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000380C0 File Offset: 0x000362C0
		private void OnDisable()
		{
			for (int i = 0; i < this._gems.Length; i++)
			{
				this._gems[i].HideEffect();
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x000380F0 File Offset: 0x000362F0
		private void DeactivateAll()
		{
			for (int i = 0; i < this._gems.Length; i++)
			{
				this._gems[i].Hide();
			}
		}

		// Token: 0x04000FB9 RID: 4025
		[SerializeField]
		private Level.Gem[] _gems;

		// Token: 0x04000FBA RID: 4026
		[SerializeField]
		private Sprite _normal;

		// Token: 0x04000FBB RID: 4027
		[SerializeField]
		private Sprite _risky;

		// Token: 0x04000FBC RID: 4028
		[SerializeField]
		private bool _flick;

		// Token: 0x04000FBD RID: 4029
		[SerializeField]
		private float _flickTime = 0.35f;

		// Token: 0x020003EB RID: 1003
		[Serializable]
		private class Gem
		{
			// Token: 0x060012C0 RID: 4800 RVA: 0x00038130 File Offset: 0x00036330
			public void Set(int index, int level, Sprite frame)
			{
				this._frame.gameObject.SetActive(true);
				this._frame.sprite = frame;
				this._image.gameObject.SetActive(index < level);
				this._animator.Play(Level.Gem.empty);
			}

			// Token: 0x060012C1 RID: 4801 RVA: 0x0003817E File Offset: 0x0003637E
			public void Hide()
			{
				this._frame.gameObject.SetActive(false);
				this._image.gameObject.SetActive(false);
			}

			// Token: 0x060012C2 RID: 4802 RVA: 0x000381A2 File Offset: 0x000363A2
			public void HideEffect()
			{
				this._effect.gameObject.SetActive(false);
			}

			// Token: 0x060012C3 RID: 4803 RVA: 0x000381B5 File Offset: 0x000363B5
			public void Flick()
			{
				this._image.gameObject.SetActive(true);
				this._animator.Play(Level.Gem.flicker);
			}

			// Token: 0x060012C4 RID: 4804 RVA: 0x000381D8 File Offset: 0x000363D8
			public IEnumerator CLevelUp()
			{
				this._animator.Play(Level.Gem.empty);
				this._effect.enabled = true;
				this._effect.Play(0, 0, 0f);
				this._effect.enabled = false;
				this._effect.gameObject.SetActive(true);
				float deltaTime;
				for (float remainTime = 0.32f; remainTime > 0f; remainTime -= deltaTime)
				{
					yield return null;
					deltaTime = Chronometer.global.deltaTime;
					this._effect.Update(deltaTime);
				}
				this._effect.gameObject.SetActive(false);
				yield break;
			}

			// Token: 0x04000FBE RID: 4030
			[SerializeField]
			private Image _frame;

			// Token: 0x04000FBF RID: 4031
			[SerializeField]
			private Image _image;

			// Token: 0x04000FC0 RID: 4032
			[SerializeField]
			private Animator _animator;

			// Token: 0x04000FC1 RID: 4033
			[SerializeField]
			private Animator _effect;

			// Token: 0x04000FC2 RID: 4034
			private const float _length = 0.32f;

			// Token: 0x04000FC3 RID: 4035
			private static int empty = Animator.StringToHash("Empty");

			// Token: 0x04000FC4 RID: 4036
			private static int flicker = Animator.StringToHash("Flicker");
		}
	}
}
