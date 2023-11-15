using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B21 RID: 2849
public class AwardSceneUIController : MonoBehaviour
{
	// Token: 0x060039AB RID: 14763 RVA: 0x0011181C File Offset: 0x0010FC1C
	public void SetData(SceneDirectoryData.SceneDirectoryEntry _scene)
	{
		this.m_sceneImage.sprite = _scene.SceneVarients[0].Screenshot;
		this.m_name.text = _scene.Label;
	}

	// Token: 0x04002E66 RID: 11878
	[SerializeField]
	private Image m_sceneImage;

	// Token: 0x04002E67 RID: 11879
	[SerializeField]
	private Text m_name;
}
