using UnityEngine;
using System.Collections;

namespace BomberChap
{
	public class SplitScreenView : MonoBehaviour 
	{
		[SerializeField]
		private int m_maxVerticalDistanceInTiles;
		[SerializeField]
		private float m_splitSpacing;
		[SerializeField]
		private AnimationCurve m_spacingDistribution;
		[SerializeField]
		private Camera m_splitViewCamera;
		[SerializeField]
		private SplitScreenCameraController m_cameraOneController;
		[SerializeField]
		private SplitScreenCameraController m_cameraTwoController;
		[SerializeField]
		private Material m_cameraOneMaterial;
		[SerializeField]
		private Material m_cameraTwoMaterial;
		[SerializeField]
		private MeshFilter m_meshFilter;

		private Transform m_playerOne;
		private Transform m_playerTwo;
		private RenderTexture m_cameraOneTexture;
		private RenderTexture m_cameraTwoTexture;
		private Vector3[] m_vertices;
		private Vector2[] m_uvs;
		private Mesh m_mesh;
		private Vector3 m_cameraOneTargetPos;
		private Vector3 m_cameraTwoTargetPos;

		public Camera CameraOne
		{
			get { return m_cameraOneController.Camera; }
		}

		public Camera CameraTwo
		{
			get { return m_cameraTwoController.Camera; }
		}

		public void SetPlayerOne(Transform playerOne)
		{
			m_playerOne = playerOne;
			m_cameraOneController.SetTarget(playerOne);
		}

		public void SetPlayerTwo(Transform playerTwo)
		{
			m_playerTwo = playerTwo;
			m_cameraTwoController.SetTarget(playerTwo);
		}

		public void Initialize()
		{
			InitializeRenderTextures();
			InitializeMesh();
		}

		private void InitializeRenderTextures()
		{
			m_cameraOneTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			m_cameraOneMaterial.mainTexture = m_cameraOneTexture;
			m_cameraOneController.Camera.targetTexture = m_cameraOneTexture;
			
			m_cameraTwoTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			m_cameraTwoMaterial.mainTexture = m_cameraTwoTexture;
			m_cameraTwoController.Camera.targetTexture = m_cameraTwoTexture;
		}
		
		private void InitializeMesh()
		{
			m_vertices = new Vector3[8];
			m_uvs = new Vector2[8];
			
			m_mesh = new Mesh();
			m_mesh.name = "SplitScreenViewMesh";
			m_mesh.vertices = m_vertices;
			m_mesh.uv = m_uvs;
			m_mesh.subMeshCount = 2;
			m_mesh.SetTriangles(new int[] { 0, 1, 3, 3, 1, 2 }, 0);
			m_mesh.SetTriangles(new int[] { 4, 5, 7, 7, 5, 6 }, 1);
			m_mesh.RecalculateBounds();
			m_mesh.RecalculateNormals();
			m_meshFilter.sharedMesh = m_mesh;
		}

		private void OnDestroy()
		{
			if(m_cameraOneTexture != null)
				RenderTexture.Destroy(m_cameraOneTexture);
			if(m_cameraTwoTexture != null)
				RenderTexture.Destroy(m_cameraTwoTexture);
			if(m_mesh != null)
			{
				m_meshFilter.sharedMesh = null;
				Mesh.Destroy(m_mesh);
			}
		}

		private void Update()
		{
			float viewRatio = CalculateViewRatio();

			Vector2 camOneOffset = m_cameraOneController.Offset;
			Vector2 camTwoOffset = m_cameraTwoController.Offset;

			camOneOffset.x = m_cameraOneController.Camera.orthographicSize * m_cameraOneController.Camera.aspect / 2.0f;
			camOneOffset.y = -viewRatio * m_cameraOneController.Camera.orthographicSize / 2.0f;

			camTwoOffset.x = -m_cameraTwoController.Camera.orthographicSize * m_cameraTwoController.Camera.aspect / 2.0f;
			camTwoOffset.y = viewRatio * m_cameraTwoController.Camera.orthographicSize / 2.0f;

			m_cameraOneController.Offset = camOneOffset;
			m_cameraTwoController.Offset = camTwoOffset;

			UpdateMesh();
		}

		private void UpdateMesh()
		{
			float dy = m_splitViewCamera.orthographicSize;
			float dx = m_splitViewCamera.orthographicSize * m_splitViewCamera.aspect;
			float r = CalculateViewRatio();
			float s = dx * m_splitSpacing * m_spacingDistribution.Evaluate(Mathf.Max(Mathf.Abs(r), 0.1f));
			float s2 = 0.5f * m_splitSpacing * m_spacingDistribution.Evaluate(Mathf.Max(Mathf.Abs(r), 0.1f));

			m_vertices[0].Set(-dx, dy, 0);
			m_vertices[1].Set(Mathf.Max(dx * r - s, -dx), dy, 0);
			m_vertices[2].Set(Mathf.Max(-dx * r - s, -dx), -dy, 0);
			m_vertices[3].Set(-dx, -dy, 0);

			m_vertices[4].Set(Mathf.Min(dx * r + s, dx), dy, 0);
			m_vertices[5].Set(dx, dy, 0);
			m_vertices[6].Set(dx, -dy, 0);
			m_vertices[7].Set(Mathf.Min(-dx * r + s, dx), -dy, 0);

			m_uvs[0].Set(0.0f, 1.0f);
			m_uvs[1].Set(Mathf.Max(0.5f + (0.5f * r) - s2, 0.0f), 1.0f);
			m_uvs[2].Set(Mathf.Max(0.5f + (-0.5f * r) - s2, 0.0f), 0.0f);
			m_uvs[3].Set(0.0f, 0.0f);

			m_uvs[4].Set(Mathf.Min(0.5f + (0.5f * r) + s2, 1.0f), 1.0f);
			m_uvs[5].Set(1.0f, 1.0f);
			m_uvs[6].Set(1.0f, 0.0f);
			m_uvs[7].Set(Mathf.Min(0.5f + (-0.5f * r) + s2, 1.0f), 0.0f);

			m_mesh.vertices = m_vertices;
			m_mesh.uv = m_uvs;
			m_mesh.RecalculateBounds();
		}

		private float CalculateViewRatio()
		{
			Level currentLevel = LevelManager.GetLoadedLevel();
			float maxVerticalDistance = m_maxVerticalDistanceInTiles * Mathf.Min(currentLevel.TileWidth, currentLevel.TileHeight) * currentLevel.PixelToUnit;
			float absVerticalDistance = Mathf.Abs(m_playerOne.position.y - m_playerTwo.position.y);
			float sign = Mathf.Sign(m_playerOne.position.y - m_playerTwo.position.y);

			return sign * Mathf.Min((absVerticalDistance / maxVerticalDistance), 1.0f);
		}
	}
}
