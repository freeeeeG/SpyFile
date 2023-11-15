using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005B7 RID: 1463
public class TriggerAttachedSpawn : MonoBehaviour
{
	// Token: 0x06001BCF RID: 7119 RVA: 0x00088254 File Offset: 0x00086654
	private void Awake()
	{
		if (this.m_attachmentPrefabs.Length > 0)
		{
			SpawnableEntityCollection spawnableEntityCollection = base.gameObject.AddComponent<SpawnableEntityCollection>();
			for (int i = 0; i < this.m_attachmentPrefabs.Length; i++)
			{
				GameObject attachmentPrefab = this.m_attachmentPrefabs[i].AttachmentPrefab;
				spawnableEntityCollection.RegisterSpawnable(attachmentPrefab, null);
			}
		}
	}

	// Token: 0x040015D1 RID: 5585
	[SerializeField]
	public TriggerAttachedSpawn.WeightedPrefab[] m_attachmentPrefabs = new TriggerAttachedSpawn.WeightedPrefab[0];

	// Token: 0x040015D2 RID: 5586
	[SerializeField]
	public Transform m_gridPointSelector;

	// Token: 0x040015D3 RID: 5587
	[SerializeField]
	public bool m_spawnInOrder;

	// Token: 0x040015D4 RID: 5588
	[SerializeField]
	public string m_trigger;

	// Token: 0x040015D5 RID: 5589
	[SerializeField]
	public int m_maxNumber = 50;

	// Token: 0x020005B8 RID: 1464
	[Serializable]
	public class WeightedPrefab : IWeight
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000882BD File Offset: 0x000866BD
		public float Weight
		{
			get
			{
				return this.m_weight;
			}
		}

		// Token: 0x040015D6 RID: 5590
		public GameObject AttachmentPrefab;

		// Token: 0x040015D7 RID: 5591
		[SerializeField]
		private float m_weight = 1f;
	}
}
