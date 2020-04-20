using UnityEngine;
using UnityEngine.UI;

public class UIHealthUpdater : MonoBehaviour
{
	public Image[] m_hearths;

	public Color m_lostHPTint = Color.gray;

	private Color m_originalTint;

	// Start is called before the first frame update
	private void Start()
	{
		m_originalTint = m_hearths.Length > 0 ? m_hearths[0].color : Color.white;
	}

	public void UpdateUI( int hp )
	{
		for ( int i = 0; i < m_hearths.Length; i++ )
		{
			m_hearths[i].color = i < hp ? m_originalTint : m_lostHPTint;
		}
	}

}
