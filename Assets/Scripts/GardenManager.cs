using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GardenManager : MonoBehaviour
{
	public static Vector3 INVALID_VECTOR = new Vector3( -99f, -99f, -99f );
	public int MAX_HEALTH_POINTS = 3;
	public int MAX_STOMACH_POINTS = 2;
	public int MAX_FOOD_RESERVE = 4;
	public int DAYS_TO_SURVIVE = 10;

	public UnityAction<int> HealthPointsChangeEvent;
	public UnityAction<int> StomachPointsChangeEvent;
	public UnityAction<int> FoodReserveChangeEvent;

	public GameObject m_cropReference;
	public Tilemap m_tilemap;
	public GameObject highlightTile;

	public int m_startingHealth = 3;

	public bool IsStomachFull { get => m_stomachPoints == MAX_STOMACH_POINTS; }
	public bool IsStomachEmpty { get => m_stomachPoints == 0; }
	public bool IsHealthFull { get => m_healthPoints == MAX_HEALTH_POINTS; }

	private Dictionary<string, GameObject> m_positionCropsList = new Dictionary<string, GameObject>();
	private Vector3Int m_hoveredCellGridPos;
	private Vector3 m_hoveredCellCenterPos;

	private int m_foodReserve = 0;
	private DayManager m_dayManagerRef;
	private int m_healthPoints;
	private int m_stomachPoints;

	// Start is called before the first frame update
	private void Start()
	{
		m_healthPoints = m_startingHealth;

		m_dayManagerRef = GameObject.FindObjectOfType<DayManager>();
		m_dayManagerRef.EndDayEvent += OnEndDay;
		m_dayManagerRef.BeginDayEvent += OnBeginDay;
	}

	private void OnBeginDay( int day )
	{
		UpdateHealth();

		if ( m_healthPoints <= 0 )
		{
			GameOver();
		}

		if (day == DAYS_TO_SURVIVE )
		{
			Win();
		}
	}

	private void Win()
	{
		throw new NotImplementedException();
	}

	private void OnEndDay( int day )
	{
		FeedOnReserve();
	}

	private void UpdateHealth()
	{
		if ( IsStomachFull && !IsHealthFull )
		{
			++m_healthPoints;
			HealthPointsChangeEvent?.Invoke( m_healthPoints );
		}
		else if ( IsStomachEmpty )
		{
			--m_healthPoints;
			HealthPointsChangeEvent?.Invoke( m_healthPoints );
		}

		m_stomachPoints = 0;
		StomachPointsChangeEvent?.Invoke( m_stomachPoints );
	}

	private void GameOver()
	{
		throw new NotImplementedException();
	}

	// Update is called once per frame
	private void Update()
	{
		UpdateHoveredTile();
		UpdateTileHighlight();

		if ( Input.GetButtonDown( "Fire1" ) )
		{
			if ( m_hoveredCellCenterPos != INVALID_VECTOR )
			{
				if ( !m_positionCropsList.ContainsKey( m_hoveredCellGridPos.ToString() ) )
				{
					var newCrop = Instantiate( m_cropReference, m_hoveredCellCenterPos, Quaternion.identity );
					m_positionCropsList.Add( m_hoveredCellGridPos.ToString(), newCrop );
				}
				else
				{
					var selectedObject = m_positionCropsList[m_hoveredCellGridPos.ToString()];
					var selectedCrop = selectedObject.GetComponent<GrowBehavior>();
					if ( selectedCrop.IsFullyGrown() )
					{
						m_foodReserve += selectedCrop.Harvest();
						m_positionCropsList.Remove( m_hoveredCellGridPos.ToString() );
					}
				}
			}

		}

	}

	private void UpdateTileHighlight()
	{
		highlightTile.transform.position = m_hoveredCellCenterPos;
	}

	private void UpdateHoveredTile()
	{
		Vector2 mouseWorlPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		m_hoveredCellGridPos = m_tilemap.WorldToCell( mouseWorlPos );


		m_hoveredCellCenterPos = m_tilemap.HasTile( m_hoveredCellGridPos ) ? m_tilemap.GetCellCenterWorld( m_hoveredCellGridPos ) : INVALID_VECTOR;
	}

	private void FeedOnReserve()
	{
		while (m_foodReserve > 0 && !IsStomachFull )
		{
			--m_foodReserve;
			++m_stomachPoints;

			FoodReserveChangeEvent?.Invoke( m_foodReserve );
			StomachPointsChangeEvent?.Invoke( m_stomachPoints );
		}
	}

}