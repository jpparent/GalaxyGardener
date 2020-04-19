using System.Collections;
using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class GrowBehavior : MonoBehaviour
{
	public Sprite[] m_evolutionSprites;
	public int m_startingSpriteIndex = 0;

	public int m_foodAmount = 2;
	public float m_flickerWaitTime = 0.5f;

	private int m_currentSpriteIndex = 0;
	private DayManager m_gameManager;
	private bool m_isFullyGrown = false;
	private SpriteRenderer m_spriteRenderer;

	private IEnumerator m_readyToHarvestCoroutine;


	// Start is called before the first frame update
	private void Start()
	{
		m_currentSpriteIndex = m_startingSpriteIndex;
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_spriteRenderer.sprite = m_evolutionSprites[m_currentSpriteIndex];

		m_gameManager = GameObject.FindObjectOfType<DayManager>();
		m_gameManager.BeginDayEvent += OnNewDayEvent;
	}

	private void OnNewDayEvent(int day)
	{
		if ( !m_isFullyGrown )
		{
			Grow();
		}
	}

	private void Grow()
	{
		m_currentSpriteIndex++;
		GetComponent<SpriteRenderer>().sprite = m_evolutionSprites[m_currentSpriteIndex];
		if ( m_evolutionSprites.Length == m_currentSpriteIndex + 1 )
		{
			m_isFullyGrown = true;
		}
	}

	// Update is called once per frame
	private void Update()
	{
		if ( m_isFullyGrown && m_readyToHarvestCoroutine == null )
		{
			m_readyToHarvestCoroutine = ReadyToHarvestFeedback();
			StartCoroutine( m_readyToHarvestCoroutine );
		}
	}

	public bool IsFullyGrown()
	{
		return m_isFullyGrown;
	}

	private IEnumerator ReadyToHarvestFeedback()
	{
		int sign = -1;
		while ( true )
		{
			sign *= -1;
			transform.Rotate( 0f, 0f, 10f * sign );
			yield return new WaitForSeconds( m_flickerWaitTime );
		}
	}

	public int Harvest()
	{
		// do some cleaning before deleting
		StopCoroutine( m_readyToHarvestCoroutine );

		Destroy( this.gameObject );

		return m_foodAmount;
	}
}
