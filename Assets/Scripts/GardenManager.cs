using System;
using System.Collections.Generic;
using TMPro;
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

	public GameObject m_gameOverCanvasObjectRef;
	public TMP_Text m_gameOverRecapText;

	public GameObject m_cropReference;
	public Tilemap m_tilemap;
	public GameObject highlightTile;

	public int m_startingHealth = 3;

	public bool IsStomachFull => m_stomachPoints == MAX_STOMACH_POINTS;
	public bool IsStomachEmpty => m_stomachPoints == 0;
	public bool IsHealthFull => m_healthPoints == MAX_HEALTH_POINTS;
	public bool IsReservFull => m_foodReserve == MAX_FOOD_RESERVE;

	private Dictionary<string, GameObject> m_positionCropsList = new Dictionary<string, GameObject>();
	private Vector3Int m_hoveredCellGridPos;
	private Vector3 m_hoveredCellCenterPos;

	private int m_foodReserve = 0;
	private DayManager m_dayManagerRef;
	private int m_healthPoints;
	private int m_stomachPoints;

	private int m_plantedSeedsCount = 0;
	private int m_harvestedCropsCount = 0;

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

		if ( day == DAYS_TO_SURVIVE )
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
		m_gameOverCanvasObjectRef?.SetActive( true );
		m_gameOverRecapText?.SetText( "You survived {0} days by planting {1} seeds and harvesting {2} crops... in space!\n\nWell done. Now is the time for a well deserved rest. Thanks for playing this incomplete game !", m_dayManagerRef.GetCurrentDay(), m_plantedSeedsCount, m_harvestedCropsCount );
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
					PlantSeeds();
				}
				else
				{
					var selectedObject = m_positionCropsList[m_hoveredCellGridPos.ToString()];
					var selectedCrop = selectedObject.GetComponent<GrowBehavior>();
					if ( selectedCrop.IsFullyGrown() )
					{
						HarvestCrop( selectedCrop );
					}
				}
			}
		}
	}

	// feed the stomach first then put leftover in food reserve
	private void HarvestCrop( GrowBehavior selectedCrop )
	{
		int foodToDistribute = selectedCrop.Harvest();

		if ( !IsStomachFull )
		{
			int stomachPointsAvailable = MAX_STOMACH_POINTS - m_stomachPoints;
			int stomachPointsToAdd = foodToDistribute >= stomachPointsAvailable ? stomachPointsAvailable : foodToDistribute;
			AddStomachPoints( stomachPointsToAdd );

			foodToDistribute -= stomachPointsToAdd;
		}

		if ( foodToDistribute > 0 && !IsReservFull )
		{
			int reservePointsAvailable = MAX_FOOD_RESERVE - m_foodReserve;
			int reservePointsToAdd = foodToDistribute >= reservePointsAvailable ? reservePointsAvailable : foodToDistribute;
			AddReservePoints( reservePointsToAdd );
		}

		m_positionCropsList.Remove( m_hoveredCellGridPos.ToString() );
		++m_harvestedCropsCount;
	}

	private void AddStomachPoints( int stomachPointsToAdd )
	{
		m_stomachPoints += stomachPointsToAdd;
		StomachPointsChangeEvent?.Invoke( m_stomachPoints );
	}

	private void PlantSeeds()
	{
		var newCrop = Instantiate( m_cropReference, m_hoveredCellCenterPos, Quaternion.identity );
		m_positionCropsList.Add( m_hoveredCellGridPos.ToString(), newCrop );

		++m_plantedSeedsCount;
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

	// TODO: make into a coroutine so we can see what's happening
	private void FeedOnReserve()
	{
		while ( m_foodReserve > 0 && !IsStomachFull )
		{
			AddReservePoints( -1 );
			AddStomachPoints( 1 );
		}
	}

	private void AddReservePoints( int units )
	{
		m_foodReserve += (units);
		FoodReserveChangeEvent?.Invoke( m_foodReserve );
	}
}