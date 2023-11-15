using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class BackgroundParticle : MonoBehaviour
{
	// Token: 0x06000286 RID: 646 RVA: 0x0000F2C6 File Offset: 0x0000D4C6
	private void Awake()
	{
		BackgroundParticle.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000F2DC File Offset: 0x0000D4DC
	private void Start()
	{
		this.ApplySetting();
		for (int i = 0; i < this.partRenders.Length; i++)
		{
			this.blooms[i].InitMat(this.partRenders[i], ResourceLibrary.Inst.GlowBackStars, false);
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000F322 File Offset: 0x0000D522
	public void ApplySetting()
	{
		base.gameObject.SetActive(Setting.Inst.Option_BackgroundParticle);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000F33C File Offset: 0x0000D53C
	public void UpdateShape(EnumLevelType levelShape)
	{
		EnumShapeType shape;
		switch (levelShape)
		{
		case EnumLevelType.TRIANGLE:
			shape = EnumShapeType.TRIANGLE;
			break;
		case EnumLevelType.CIRCLE:
			shape = EnumShapeType.CIRCLE;
			break;
		case EnumLevelType.SQUARE:
			shape = EnumShapeType.SQUARE;
			break;
		default:
			shape = (EnumShapeType)Random.Range(0, 3);
			break;
		}
		Sprite sprite_Shape = ResourceLibrary.Inst.GetSprite_Shape(shape, true);
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].textureSheetAnimation.SetSprite(0, sprite_Shape);
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000F3A8 File Offset: 0x0000D5A8
	private void LateUpdate()
	{
		float d = Camera.main.orthographicSize / 24f;
		base.transform.localScale = Vector3.one * d;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000F3DC File Offset: 0x0000D5DC
	private void FixedUpdate()
	{
		if (this.transRotate != null)
		{
			this.transRotate.Rotate(0f, 0f, Time.fixedDeltaTime * this.rotateSpd);
		}
	}

	// Token: 0x04000256 RID: 598
	public static BackgroundParticle inst;

	// Token: 0x04000257 RID: 599
	[SerializeField]
	private Transform transRotate;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	private float rotateSpd = 1f;

	// Token: 0x04000259 RID: 601
	[SerializeField]
	private ParticleSystemRenderer[] partRenders;

	// Token: 0x0400025A RID: 602
	[SerializeField]
	private BloomMaterialControl[] blooms;
}
