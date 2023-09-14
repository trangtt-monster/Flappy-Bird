using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	Easy,
	Hard,
}

public class ObstacleSpawner : MonoBehaviour {

	[SerializeField] private float waitTime;
	[SerializeField] private GameObject[] obstaclePrefabs;
	[SerializeField] private GameObject obstaclePrefab;
	private float tempTime;
    private Difficulty[] difficulties = new Difficulty[]{ Difficulty.Easy, Difficulty.Easy, Difficulty.Easy, Difficulty.Hard, Difficulty.Hard };
	private int difficultyIndex;
	private Vector3 previousPosition;

	public float maxHardDistance = 3;
	public float minHardDistance = 1.5f;
	public float maxEasyDistance = 1.5f;
	public float minEasyDistance = 0;
	public float minY = -1.57f;
	public float maxY = 2.8f;


	void Start(){
		tempTime = waitTime - Time.deltaTime;
		difficultyIndex = 0;
		previousPosition = transform.position;
	}

	void LateUpdate () {
		if(GameManager.Instance.GameState()){
			tempTime += Time.deltaTime;
			if(tempTime > waitTime){
				// Wait for some time, create an obstacle, then set wait time to 0 and start again
				tempTime = 0;
				GameObject pipeClone = Instantiate(obstaclePrefab, previousPosition, transform.rotation);
				UpdatePositionSpawn();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.transform.parent != null){
			Destroy(col.gameObject.transform.parent.gameObject);
		}else{
			Destroy(col.gameObject);
		}
	}

	private void UpdatePositionSpawn()
	{
		if (difficultyIndex >= difficulties.Length) difficultyIndex = 0;
		var difficulty = difficulties[difficultyIndex];

		float minDistance = 0;
		float maxDistance = 0;
		if (difficulty == Difficulty.Hard)
		{
			minDistance = minHardDistance;
			maxDistance = maxHardDistance;
		}
		else if (difficulty == Difficulty.Easy)
		{
			minDistance = minEasyDistance;
			maxDistance = maxEasyDistance;
		}

		previousPosition.y = RandomInTwoRange(previousPosition.y + minDistance, Mathf.Min(previousPosition.y + maxDistance, maxY),
								Mathf.Max(previousPosition.y - maxDistance, minY), previousPosition.y - minDistance);

		difficultyIndex++;
	}

	private float RandomInTwoRange(float minRange1, float maxRange1, float minRange2, float maxRange2)
	{
		float minRange = 0;
		float maxRange = 0;
		if (minRange1 >= maxRange1)
		{
			minRange = minRange2;
			maxRange = maxRange2;
		}
		else if (minRange2 >= maxRange2)
		{
			minRange = minRange1;
			maxRange = maxRange1;
		}
		else
		{
			if (Random.value >= 0.5)
			{
				minRange = minRange2;
				maxRange = maxRange2;
			}
			else
			{
				minRange = minRange1;
				maxRange = maxRange1;
			}
		}

		return Random.Range(minRange, maxRange);
	}


}
