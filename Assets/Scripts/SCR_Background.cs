// class: SCR_Background
using UnityEngine;

public class SCR_Background : MonoBehaviour
{
	
	public float moveSpeed;

	public float positionRange;

	private Material matBackground;

	public void Start()
	{
		matBackground = GetComponent<MeshRenderer>().material;
		float y = UnityEngine.Random.Range(base.transform.position.y - positionRange, base.transform.position.y);
		base.transform.position = new Vector3(base.transform.position.x, y, base.transform.position.z);
	}

	public void Move(float v)
	{
		matBackground.mainTextureOffset = new Vector2(matBackground.mainTextureOffset.x + moveSpeed * v, matBackground.mainTextureOffset.y);
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(matBackground);
	}
}
