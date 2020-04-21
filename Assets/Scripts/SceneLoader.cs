using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public int m_targetSceneIndex = 1;

	public void LoadTargetScene()
	{
		SceneManager.LoadScene( m_targetSceneIndex );
	}

	public void LoadNextScene()
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
	}
}
