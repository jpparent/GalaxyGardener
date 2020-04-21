using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
	public int m_nextSceneToLoad = 1;

	public void LoadNextScene()
	{
		SceneManager.LoadScene( m_nextSceneToLoad );
	}
}
