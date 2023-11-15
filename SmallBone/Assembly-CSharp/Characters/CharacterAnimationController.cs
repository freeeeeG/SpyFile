using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006B3 RID: 1715
	public class CharacterAnimationController : MonoBehaviour
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x0600227E RID: 8830 RVA: 0x00067928 File Offset: 0x00065B28
		// (remove) Token: 0x0600227F RID: 8831 RVA: 0x00067960 File Offset: 0x00065B60
		public event Action onExpire;

		// Token: 0x06002280 RID: 8832 RVA: 0x00067998 File Offset: 0x00065B98
		private void Update()
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				CharacterAnimation characterAnimation = this.animations[i];
				characterAnimation.speed = this._character.chronometer.animation.timeScale / Time.timeScale;
				characterAnimation.parameter.walk.Value = this.parameter.walk;
				characterAnimation.parameter.grounded.Value = this.parameter.grounded;
				characterAnimation.parameter.movementSpeed.Value = this.parameter.movementSpeed;
				characterAnimation.parameter.ySpeed.Value = this.parameter.ySpeed;
				characterAnimation.transform.localScale = (this.parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00067A8C File Offset: 0x00065C8C
		public void Initialize()
		{
			base.GetComponentsInChildren<CharacterAnimation>(true, this.animations);
			this.animations.ForEach(delegate(CharacterAnimation animation)
			{
				animation.Initialize();
			});
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00067AC8 File Offset: 0x00065CC8
		public void ForceUpdate()
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				CharacterAnimation characterAnimation = this.animations[i];
				characterAnimation.speed = this._character.chronometer.animation.timeScale / Time.timeScale;
				characterAnimation.parameter.walk.ForceSet(this.parameter.walk);
				characterAnimation.parameter.grounded.ForceSet(this.parameter.grounded);
				characterAnimation.parameter.movementSpeed.ForceSet(this.parameter.movementSpeed);
				characterAnimation.parameter.ySpeed.ForceSet(this.parameter.ySpeed);
				characterAnimation.transform.localScale = (this.parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00067BBC File Offset: 0x00065DBC
		public void UpdateScale()
		{
			for (int i = 0; i < this.animations.Count; i++)
			{
				this.animations[i].transform.localScale = (this.parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
			}
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00067C20 File Offset: 0x00065E20
		public void Play(CharacterAnimationController.AnimationInfo animationInfo, float speed)
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
			foreach (CharacterAnimation characterAnimation in this.animations)
			{
				AnimationClip clip;
				if (characterAnimation.isActiveAndEnabled && animationInfo.dictionary.TryGetValue(characterAnimation.key, out clip))
				{
					characterAnimation.Play(clip, speed);
				}
			}
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00067CAC File Offset: 0x00065EAC
		public void Play(CharacterAnimationController.AnimationInfo animationInfo, float length, float speed)
		{
			this.Play(animationInfo, speed);
			this._coroutine = base.StartCoroutine(this.ExpireInSeconds(length));
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00067CCC File Offset: 0x00065ECC
		public void Stun()
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
			foreach (CharacterAnimation characterAnimation in this.animations)
			{
				characterAnimation.Stun();
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00067D38 File Offset: 0x00065F38
		public void Unmove(CharacterAnimationController.AnimationInfo info)
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
			this.Play(info, 1f);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00067D61 File Offset: 0x00065F61
		private IEnumerator ExpireInSeconds(float seconds)
		{
			while (seconds >= 0f)
			{
				yield return null;
				seconds -= this._character.chronometer.animation.deltaTime;
			}
			this.StopAll();
			Action action = this.onExpire;
			if (action != null)
			{
				action();
			}
			yield break;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00067D78 File Offset: 0x00065F78
		public void Loop()
		{
			foreach (CharacterAnimation characterAnimation in this.animations)
			{
				characterAnimation.Play();
			}
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00067DC8 File Offset: 0x00065FC8
		public void StopAll()
		{
			foreach (CharacterAnimation characterAnimation in this.animations)
			{
				characterAnimation.Stop();
			}
		}

		// Token: 0x04001D53 RID: 7507
		public const string characterBodyKey = "CharacterBody";

		// Token: 0x04001D54 RID: 7508
		public const string polymorphKey = "Polymorph";

		// Token: 0x04001D56 RID: 7510
		public readonly CharacterAnimationController.Parameter parameter = new CharacterAnimationController.Parameter();

		// Token: 0x04001D57 RID: 7511
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001D58 RID: 7512
		public List<CharacterAnimation> animations = new List<CharacterAnimation>();

		// Token: 0x04001D59 RID: 7513
		private Coroutine _coroutine;

		// Token: 0x020006B4 RID: 1716
		public class KeyAttribute : PopupAttribute
		{
			// Token: 0x0600228C RID: 8844 RVA: 0x00067E36 File Offset: 0x00066036
			public KeyAttribute() : base(true, new object[]
			{
				"CharacterBody",
				"Polymorph"
			})
			{
			}
		}

		// Token: 0x020006B5 RID: 1717
		[Serializable]
		public class AnimationInfo : ReorderableArray<CharacterAnimationController.AnimationInfo.KeyClip>
		{
			// Token: 0x0600228D RID: 8845 RVA: 0x00067E55 File Offset: 0x00066055
			public AnimationInfo(params CharacterAnimationController.AnimationInfo.KeyClip[] keyClips)
			{
				this.values = keyClips;
			}

			// Token: 0x17000729 RID: 1833
			// (get) Token: 0x0600228E RID: 8846 RVA: 0x00067E64 File Offset: 0x00066064
			public Dictionary<string, AnimationClip> dictionary
			{
				get
				{
					Dictionary<string, AnimationClip> result;
					if ((result = this._dictionary) == null)
					{
						result = (this._dictionary = this.values.ToDictionary((CharacterAnimationController.AnimationInfo.KeyClip v) => v.key, (CharacterAnimationController.AnimationInfo.KeyClip v) => v.clip));
					}
					return result;
				}
			}

			// Token: 0x1700072A RID: 1834
			// (get) Token: 0x0600228F RID: 8847 RVA: 0x00067ECD File Offset: 0x000660CD
			public AnimationClip defaultClip
			{
				get
				{
					if (this.values.Length == 0)
					{
						return null;
					}
					return this.values[0].clip;
				}
			}

			// Token: 0x06002290 RID: 8848 RVA: 0x00067EE8 File Offset: 0x000660E8
			public void Dispose()
			{
				Dictionary<string, AnimationClip> dictionary = this._dictionary;
				if (dictionary != null)
				{
					dictionary.Clear();
				}
				CharacterAnimationController.AnimationInfo.KeyClip[] values = this.values;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].Dispose();
				}
			}

			// Token: 0x04001D5A RID: 7514
			private Dictionary<string, AnimationClip> _dictionary;

			// Token: 0x020006B6 RID: 1718
			[Serializable]
			public class KeyClip
			{
				// Token: 0x06002291 RID: 8849 RVA: 0x00067F23 File Offset: 0x00066123
				public KeyClip(string key, AnimationClip clip)
				{
					this._key = key;
					this._clip = clip;
				}

				// Token: 0x1700072B RID: 1835
				// (get) Token: 0x06002292 RID: 8850 RVA: 0x00067F44 File Offset: 0x00066144
				public string key
				{
					get
					{
						return this._key;
					}
				}

				// Token: 0x1700072C RID: 1836
				// (get) Token: 0x06002293 RID: 8851 RVA: 0x00067F4C File Offset: 0x0006614C
				public AnimationClip clip
				{
					get
					{
						return this._clip;
					}
				}

				// Token: 0x06002294 RID: 8852 RVA: 0x00067F54 File Offset: 0x00066154
				public void Dispose()
				{
					this._key = null;
					this._clip = null;
				}

				// Token: 0x04001D5B RID: 7515
				[CharacterAnimationController.KeyAttribute]
				[SerializeField]
				private string _key = "CharacterBody";

				// Token: 0x04001D5C RID: 7516
				[SerializeField]
				private AnimationClip _clip;
			}
		}

		// Token: 0x020006B8 RID: 1720
		public class Parameter
		{
			// Token: 0x06002299 RID: 8857 RVA: 0x00067F80 File Offset: 0x00066180
			public void CopyFrom(CharacterAnimationController.Parameter parameter)
			{
				this.walk = parameter.walk;
				this.grounded = parameter.grounded;
				this.movementSpeed = parameter.movementSpeed;
				this.ySpeed = parameter.ySpeed;
				this.flipX = parameter.flipX;
			}

			// Token: 0x04001D60 RID: 7520
			public bool walk;

			// Token: 0x04001D61 RID: 7521
			public bool grounded;

			// Token: 0x04001D62 RID: 7522
			public float movementSpeed;

			// Token: 0x04001D63 RID: 7523
			public float ySpeed;

			// Token: 0x04001D64 RID: 7524
			public bool flipX;
		}
	}
}
