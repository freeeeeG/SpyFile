using System;
using UnityEngine;

// Token: 0x0200073A RID: 1850
[AddComponentMenu("KMonoBehaviour/scripts/TemperatureCookable")]
public class TemperatureCookable : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060032D5 RID: 13013 RVA: 0x0010DD99 File Offset: 0x0010BF99
	public void Sim1000ms(float dt)
	{
		if (this.element.Temperature > this.cookTemperature && this.cookedID != null)
		{
			this.Cook();
		}
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x0010DDBC File Offset: 0x0010BFBC
	private void Cook()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.cookedID), position);
		gameObject.SetActive(true);
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = this.element.Temperature;
		component2.Mass = this.element.Mass;
		base.gameObject.DeleteObject();
	}

	// Token: 0x04001E7F RID: 7807
	[MyCmpReq]
	private PrimaryElement element;

	// Token: 0x04001E80 RID: 7808
	public float cookTemperature = 273150f;

	// Token: 0x04001E81 RID: 7809
	public string cookedID;
}
