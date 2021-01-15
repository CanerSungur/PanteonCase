/// <summary>
/// 
/// This code is found and altered to fit the projet's needs.
/// 
/// Code credit: Rodrigo Fernandez Diaz
/// For a detailed tutorial you can visit: http://codeartist.mx/tutorials/dynamic-texture-painting/
/// For any contact visit: http://codeartist.mx/
/// </summary>


using UnityEngine;

public class TexturePainter : MonoBehaviour
{
	[Header("Brush Setup")]
	public GameObject brushCursor;    // The cursor that overlaps the model.
	public GameObject brushContainer; // Our container for the brushes painted.
	public GameObject brushEntity;    // Prefab that is instantiated in front of canvas camera.

	[Header("Camera Setup")]
	public Camera secondCamera; // The camera that looks at the model.
	public Camera canvasCam;    // The camera that looks at the canvas.  

	[Header("Textures & Materials Setup")]
	public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes.
	public Material baseMaterial;       // The material of our base texture (Were we will save the painted texture).

	[Header("Brush Properties")]
	private float brushSize = 3.0f;
	private Color brushColor = Color.red;
	private int brushCounter = 0, MAX_BRUSH_COUNT = 50;

	[Header("Painter Object Properties")]
	public Transform brushPlaceholder;

	private bool saving = false; // Flag to check if we are saving the texture	

	void Update()
	{
		if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
			DoAction();

		UpdateBrushCursor ();
	}

	// The main action, instantiates a brush entity at the clicked position on the UV map
	void DoAction()
	{

		if (saving)
			return;

		Vector3 uvWorldPosition = Vector3.zero;

		if (HitTestUVPosition(ref uvWorldPosition))
		{
			GameObject brushObj;

			brushObj = Instantiate(brushEntity);                        // Paint a brush
			brushObj.GetComponent<SpriteRenderer>().color = brushColor; // Set the brush color

			brushObj.transform.parent = brushContainer.transform;  // Add the brush to our container to be wiped later.
			brushObj.transform.localPosition = uvWorldPosition;    // The position of the brush (in the UVMap).
			brushObj.transform.localScale = Vector3.one * brushSize; // Set the size of the brush.
		}

		brushCounter++; // Add to the max brushes.

		// If we reach the max brushes available, flatten the texture and clear the brushes.
		if (brushCounter >= MAX_BRUSH_COUNT) 
		{ 
			brushCursor.SetActive (false);
			saving = true;
			Invoke("SaveTexture", 0.1f);
		}
	}

	
	// To update at realtime the painting cursor on the mesh.
	void UpdateBrushCursor()
	{
		Vector3 uvWorldPosition = Vector3.zero;

		if (HitTestUVPosition(ref uvWorldPosition) && !saving)
		{
			brushCursor.SetActive(true);
			brushCursor.transform.position = uvWorldPosition + brushContainer.transform.position;
		}
		else 
			brushCursor.SetActive(false);
	}
	
	

	// Returns the position on the texuremap according to a hit in the mesh collider.
	bool HitTestUVPosition(ref Vector3 uvWorldPosition)
	{
		RaycastHit hit;
		Ray cursorRay;

			cursorRay = new Ray(brushPlaceholder.transform.position,  brushPlaceholder.transform.forward);
			Debug.DrawRay(brushPlaceholder.transform.position, brushPlaceholder.transform.forward, Color.red);

			if (Physics.Raycast(cursorRay, out hit, 200))
			{
				MeshCollider meshCollider = hit.collider as MeshCollider;

				if (meshCollider == null || meshCollider.sharedMesh == null)
					return false;

				Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
				uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize; // To center the UV on X
				uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize; // To center the UV on Y
				uvWorldPosition.z = 0.0f;

				return true;
			}
			else
				return false;
	}



	// Sets the base material with a our canvas texture, then removes all our brushes.
	void SaveTexture()
	{		
		brushCounter = 0;
		System.DateTime date = System.DateTime.Now;
		RenderTexture.active = canvasTexture;
		Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);		
		tex.ReadPixels (new Rect (0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
		tex.Apply ();
		RenderTexture.active = null;
		baseMaterial.mainTexture = tex;	// Put the painted texture as the base.
		
		foreach (Transform child in brushContainer.transform) // Clear brushes.
		{
			Destroy(child.gameObject);
		}

		//saving = false;

		Invoke ("ShowCursor", 0.1f);
	}


	// Show again the user cursor (To avoid saving it to the texture).
	void ShowCursor() => saving = false;

}





