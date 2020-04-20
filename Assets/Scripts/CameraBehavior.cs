using UnityEngine;
using UnityEngine.U2D;

[RequireComponent( typeof( Camera ), typeof( PixelPerfectCamera ) )]
public class CameraBehavior : MonoBehaviour
{
	[SerializeField]
	private readonly float m_zoomLevelMax = 10f;

	private Camera m_camera;
	private PixelPerfectCamera m_pixelCamera;

	private int m_zoomLevel = 1;
	private readonly int m_zoomLevelMin = 1;
	private int m_originalAssetsPPU;
	private Vector3 m_originalCameraPosition;

	private float m_screenCenterX;
	private float m_screenCenterY;

	private void Awake()
	{
		m_screenCenterX = Screen.width *0.5f;
		m_screenCenterY = Screen.height * 0.5f;
	}

	// Start is called before the first frame update
	private void Start()
	{
		m_camera = GetComponent<Camera>();
		m_originalCameraPosition = m_camera.transform.position;

		m_pixelCamera = GetComponent<PixelPerfectCamera>();
		m_originalAssetsPPU = m_pixelCamera.assetsPPU;
	}

	// Update is called once per frame
	private void Update()
	{
		if ( Input.mouseScrollDelta.y != 0 )
		{
			float scrollDelta = Input.mouseScrollDelta.y;
			m_zoomLevel = Mathf.RoundToInt( Mathf.Max( m_zoomLevelMin, Mathf.Min( m_zoomLevel + scrollDelta, m_zoomLevelMax ) ) );

			int ppuDelta = (m_zoomLevel - 1) * 10;
			m_pixelCamera.assetsPPU = Mathf.FloorToInt( m_originalAssetsPPU + ppuDelta );

			if (m_zoomLevel == 1 )
			{
				m_camera.transform.position = m_originalCameraPosition;
			}
			else
			{
				// move the camera in order to center the camera on the mouse position
				// calculate the delta between mouse pos and screen center and move the camera by that amount
				// NOTE: still somw brain flexing needed as the camera does not "focus" on the zoomed position
				Vector3 delta = new Vector2( Input.mousePosition.x - m_screenCenterX, Input.mousePosition.y - m_screenCenterY );
				m_camera.transform.position = m_originalCameraPosition + delta / m_pixelCamera.assetsPPU;
			}
		}
	}
}