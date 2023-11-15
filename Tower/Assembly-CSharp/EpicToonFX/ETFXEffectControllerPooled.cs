using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
	// Token: 0x02000060 RID: 96
	public class ETFXEffectControllerPooled : MonoBehaviour
	{
		// Token: 0x06000132 RID: 306 RVA: 0x000061C4 File Offset: 0x000043C4
		private void Awake()
		{
			this.effectNameText = GameObject.Find("EffectName").GetComponent<Text>();
			this.effectIndexText = GameObject.Find("EffectIndex").GetComponent<Text>();
			this.etfxMouseOrbit = Camera.main.GetComponent<ETFXMouseOrbit>();
			this.etfxMouseOrbit.etfxEffectControllerPooled = this;
			this.effectsPool = new List<GameObject>();
			for (int i = 0; i < this.effects.Length; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.effects[i], base.transform.position, Quaternion.identity);
				gameObject.transform.parent = base.transform;
				this.effectsPool.Add(gameObject);
				gameObject.SetActive(false);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006277 File Offset: 0x00004477
		private void Start()
		{
			base.Invoke("InitializeLoop", this.startDelay);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000628A File Offset: 0x0000448A
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

		// Token: 0x06000135 RID: 309 RVA: 0x000062C2 File Offset: 0x000044C2
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

		// Token: 0x06000136 RID: 310 RVA: 0x000062F5 File Offset: 0x000044F5
		public void InitializeLoop()
		{
			base.StartCoroutine(this.EffectLoop());
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006304 File Offset: 0x00004504
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

		// Token: 0x06000138 RID: 312 RVA: 0x00006335 File Offset: 0x00004535
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

		// Token: 0x06000139 RID: 313 RVA: 0x00006366 File Offset: 0x00004566
		private void CleanCurrentEffect()
		{
			base.StopAllCoroutines();
			if (this.currentEffect != null)
			{
				this.currentEffect.SetActive(false);
			}
			base.StartCoroutine(this.EffectLoop());
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006395 File Offset: 0x00004595
		private IEnumerator EffectLoop()
		{
			this.currentEffect = this.effectsPool[this.effectIndex];
			this.currentEffect.SetActive(true);
			if (this.disableLights && this.currentEffect.GetComponent<Light>())
			{
				this.currentEffect.GetComponent<Light>().enabled = false;
			}
			if (this.disableSound && this.currentEffect.GetComponent<AudioSource>())
			{
				this.currentEffect.GetComponent<AudioSource>().enabled = false;
			}
			this.effectNameText.text = this.effects[this.effectIndex].name;
			this.effectIndexText.text = (this.effectIndex + 1).ToString() + " of " + this.effects.Length.ToString();
			ParticleSystem particleSystem = this.currentEffect.GetComponent<ParticleSystem>();
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

		// Token: 0x04000136 RID: 310
		public GameObject[] effects;

		// Token: 0x04000137 RID: 311
		private List<GameObject> effectsPool;

		// Token: 0x04000138 RID: 312
		private int effectIndex;

		// Token: 0x04000139 RID: 313
		[Space(10f)]
		[Header("Spawn Settings")]
		public bool disableLights = true;

		// Token: 0x0400013A RID: 314
		public bool disableSound = true;

		// Token: 0x0400013B RID: 315
		public float startDelay = 0.2f;

		// Token: 0x0400013C RID: 316
		public float respawnDelay = 0.5f;

		// Token: 0x0400013D RID: 317
		public bool slideshowMode;

		// Token: 0x0400013E RID: 318
		public bool autoRotation;

		// Token: 0x0400013F RID: 319
		[Range(0.001f, 0.5f)]
		public float autoRotationSpeed = 0.1f;

		// Token: 0x04000140 RID: 320
		private GameObject currentEffect;

		// Token: 0x04000141 RID: 321
		private Text effectNameText;

		// Token: 0x04000142 RID: 322
		private Text effectIndexText;

		// Token: 0x04000143 RID: 323
		private ETFXMouseOrbit etfxMouseOrbit;
	}
}
