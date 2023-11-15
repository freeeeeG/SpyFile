using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
	// Token: 0x0200005F RID: 95
	public class ETFXEffectController : MonoBehaviour
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00005FF0 File Offset: 0x000041F0
		private void Awake()
		{
			this.effectNameText = GameObject.Find("EffectName").GetComponent<Text>();
			this.effectIndexText = GameObject.Find("EffectIndex").GetComponent<Text>();
			this.etfxMouseOrbit = Camera.main.GetComponent<ETFXMouseOrbit>();
			this.etfxMouseOrbit.etfxEffectController = this;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006043 File Offset: 0x00004243
		private void Start()
		{
			this.etfxMouseOrbit = Camera.main.GetComponent<ETFXMouseOrbit>();
			this.etfxMouseOrbit.etfxEffectController = this;
			base.Invoke("InitializeLoop", this.startDelay);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006072 File Offset: 0x00004272
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				this.NextEffect();
			}
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.PreviousEffect();
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000060AA File Offset: 0x000042AA
		private void FixedUpdate()
		{
			if (this.autoRotation)
			{
				this.etfxMouseOrbit.SetAutoRotationSpeed(this.autoRotationSpeed);
				if (!this.etfxMouseOrbit.isAutoRotating)
				{
					this.etfxMouseOrbit.InitializeAutoRotation();
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000060DD File Offset: 0x000042DD
		public void InitializeLoop()
		{
			base.StartCoroutine(this.EffectLoop());
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000060EC File Offset: 0x000042EC
		public void NextEffect()
		{
			if (this.effectIndex < this.effects.Length - 1)
			{
				this.effectIndex++;
			}
			else
			{
				this.effectIndex = 0;
			}
			this.CleanCurrentEffect();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000611D File Offset: 0x0000431D
		public void PreviousEffect()
		{
			if (this.effectIndex > 0)
			{
				this.effectIndex--;
			}
			else
			{
				this.effectIndex = this.effects.Length - 1;
			}
			this.CleanCurrentEffect();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000614E File Offset: 0x0000434E
		private void CleanCurrentEffect()
		{
			base.StopAllCoroutines();
			if (this.currentEffect != null)
			{
				Object.Destroy(this.currentEffect);
			}
			base.StartCoroutine(this.EffectLoop());
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000617C File Offset: 0x0000437C
		private IEnumerator EffectLoop()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.effects[this.effectIndex], base.transform.position, Quaternion.identity);
			this.currentEffect = gameObject;
			if (this.disableLights && gameObject.GetComponent<Light>())
			{
				gameObject.GetComponent<Light>().enabled = false;
			}
			if (this.disableSound && gameObject.GetComponent<AudioSource>())
			{
				gameObject.GetComponent<AudioSource>().enabled = false;
			}
			this.effectNameText.text = this.effects[this.effectIndex].name;
			this.effectIndexText.text = (this.effectIndex + 1).ToString() + " of " + this.effects.Length.ToString();
			ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
			for (;;)
			{
				yield return new WaitForSeconds(particleSystem.main.duration + this.respawnDelay);
				if (!this.slideshowMode)
				{
					if (!particleSystem.main.loop)
					{
						this.currentEffect.SetActive(false);
						this.currentEffect.SetActive(true);
					}
				}
				else
				{
					if (particleSystem.main.loop)
					{
						yield return new WaitForSeconds(this.respawnDelay);
					}
					this.NextEffect();
				}
			}
			yield break;
		}

		// Token: 0x04000129 RID: 297
		public GameObject[] effects;

		// Token: 0x0400012A RID: 298
		private int effectIndex;

		// Token: 0x0400012B RID: 299
		[Space(10f)]
		[Header("Spawn Settings")]
		public bool disableLights = true;

		// Token: 0x0400012C RID: 300
		public bool disableSound = true;

		// Token: 0x0400012D RID: 301
		public float startDelay = 0.2f;

		// Token: 0x0400012E RID: 302
		public float respawnDelay = 0.5f;

		// Token: 0x0400012F RID: 303
		public bool slideshowMode;

		// Token: 0x04000130 RID: 304
		public bool autoRotation;

		// Token: 0x04000131 RID: 305
		[Range(0.001f, 0.5f)]
		public float autoRotationSpeed = 0.1f;

		// Token: 0x04000132 RID: 306
		private GameObject currentEffect;

		// Token: 0x04000133 RID: 307
		private Text effectNameText;

		// Token: 0x04000134 RID: 308
		private Text effectIndexText;

		// Token: 0x04000135 RID: 309
		private ETFXMouseOrbit etfxMouseOrbit;
	}
}
