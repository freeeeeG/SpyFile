using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000660 RID: 1632
[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
	// Token: 0x06002B0B RID: 11019 RVA: 0x000E5BFC File Offset: 0x000E3DFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MonumentParts.Add(this);
		if (!string.IsNullOrEmpty(this.chosenState))
		{
			this.SetState(this.chosenState);
		}
		this.UpdateMonumentDecor();
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000E5C30 File Offset: 0x000E3E30
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (Db.GetMonumentParts().TryGet(this.chosenState) == null)
		{
			string id = "";
			if (this.part == MonumentPartResource.Part.Bottom)
			{
				id = "bottom_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Middle)
			{
				id = "mid_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Top)
			{
				id = "top_" + this.chosenState;
			}
			if (Db.GetMonumentParts().TryGet(id) != null)
			{
				this.chosenState = id;
			}
		}
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x000E5CBA File Offset: 0x000E3EBA
	protected override void OnCleanUp()
	{
		Components.MonumentParts.Remove(this);
		this.RemoveMonumentPiece();
		base.OnCleanUp();
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x000E5CD4 File Offset: 0x000E3ED4
	public void SetState(string state)
	{
		MonumentPartResource monumentPartResource = Db.GetMonumentParts().Get(state);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.SwapAnims(new KAnimFile[]
		{
			monumentPartResource.AnimFile
		});
		component.Play(monumentPartResource.State, KAnim.PlayMode.Once, 1f, 0f);
		this.chosenState = state;
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x000E5D2C File Offset: 0x000E3F2C
	public bool IsMonumentCompleted()
	{
		bool flag = this.GetMonumentPart(MonumentPartResource.Part.Top) != null;
		bool flag2 = this.GetMonumentPart(MonumentPartResource.Part.Middle) != null;
		bool flag3 = this.GetMonumentPart(MonumentPartResource.Part.Bottom) != null;
		return flag && flag3 && flag2;
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x000E5D68 File Offset: 0x000E3F68
	public void UpdateMonumentDecor()
	{
		GameObject monumentPart = this.GetMonumentPart(MonumentPartResource.Part.Middle);
		if (this.IsMonumentCompleted())
		{
			monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject != monumentPart)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
				}
			}
		}
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x000E5DF4 File Offset: 0x000E3FF4
	public void RemoveMonumentPiece()
	{
		if (this.IsMonumentCompleted())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject.GetComponent<MonumentPart>() != this)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
				}
			}
		}
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x000E5E6C File Offset: 0x000E406C
	private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			MonumentPart component = gameObject.GetComponent<MonumentPart>();
			if (!(component == null) && component.part == requestPart)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x04001939 RID: 6457
	public MonumentPartResource.Part part;

	// Token: 0x0400193A RID: 6458
	public string stateUISymbol;

	// Token: 0x0400193B RID: 6459
	[Serialize]
	private string chosenState;
}
