using System;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AED RID: 2797
[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06005624 RID: 22052 RVA: 0x001F5B84 File Offset: 0x001F3D84
	// (remove) Token: 0x06005625 RID: 22053 RVA: 0x001F5BBC File Offset: 0x001F3DBC
	public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

	// Token: 0x06005626 RID: 22054 RVA: 0x001F5BF1 File Offset: 0x001F3DF1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.OnClickInternal;
	}

	// Token: 0x06005627 RID: 22055 RVA: 0x001F5C10 File Offset: 0x001F3E10
	public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
	{
		if (this.asteroidData == null || newAsteroidData.beltPath != this.asteroidData.beltPath)
		{
			this.asteroidData = newAsteroidData;
			ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
			KAnimFile kanimFile;
			Assets.TryGetAnim(getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon, out kanimFile);
			if (kanimFile != null)
			{
				this.asteroidImage.gameObject.SetActive(false);
				this.animController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				this.animController.initialMode = KAnim.PlayMode.Loop;
				this.animController.initialAnim = "idle_loop";
				this.animController.gameObject.SetActive(true);
				if (this.animController.HasAnimation(this.animController.initialAnim))
				{
					this.animController.Play(this.animController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
					return;
				}
			}
			else
			{
				this.animController.gameObject.SetActive(false);
				this.asteroidImage.gameObject.SetActive(true);
				this.asteroidImage.sprite = this.asteroidData.sprite;
			}
		}
	}

	// Token: 0x06005628 RID: 22056 RVA: 0x001F5D50 File Offset: 0x001F3F50
	private void OnClickInternal()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Clicked asteroid belt",
			this.asteroidData.beltPath
		});
		this.OnClicked(this.asteroidData);
	}

	// Token: 0x040039E5 RID: 14821
	[SerializeField]
	private Image asteroidImage;

	// Token: 0x040039E6 RID: 14822
	[SerializeField]
	private KButton button;

	// Token: 0x040039E7 RID: 14823
	[SerializeField]
	private KBatchedAnimController animController;

	// Token: 0x040039E9 RID: 14825
	private ColonyDestinationAsteroidBeltData asteroidData;
}
