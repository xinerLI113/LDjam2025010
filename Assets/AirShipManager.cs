using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipManager : MonoBehaviour
{
    [Header("飞船预制体列表")]
    public List<GameObject> airShipPrefabs = new List<GameObject>();
    
    [Header("Y轴随机范围")]
    public float yOffsetRange = 3f;
    public GameObject endPoint;
    public GameObject startPoint;
    public float airShipSpeed = 10f;
    public int airShipInterval = 10;
    
    private float timer = 0;
    private List<AirShipData> activeAirShips = new List<AirShipData>();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > airShipInterval && airShipPrefabs.Count > 0)
        {
            MakeAirShip();
            timer = 0;
        }

        
        for (int i = activeAirShips.Count - 1; i >= 0; i--)
        {
            AirShipData airShipData = activeAirShips[i];
            
            if (airShipData.airShip == null)
            {
                activeAirShips.RemoveAt(i);
                continue;
            }

            // 移动飞船
            airShipData.airShip.transform.position = Vector3.MoveTowards(
                airShipData.airShip.transform.position,
                airShipData.targetPosition,
                airShipSpeed * Time.deltaTime
            );

            // 到达终点之后销毁
            if (Vector3.Distance(airShipData.airShip.transform.position, airShipData.targetPosition) <= 0.01f)
            {
                Destroy(airShipData.airShip);
                activeAirShips.RemoveAt(i);
            }
        }
    }

    void MakeAirShip()
    {
        
        GameObject randomAirShipPrefab = airShipPrefabs[Random.Range(0, airShipPrefabs.Count)];
        
        float randomYOffset = Random.Range(-yOffsetRange, yOffsetRange);
        Vector3 spawnPosition = startPoint.transform.position;
        spawnPosition.y += randomYOffset;

        Vector3 targetPosition = endPoint.transform.position;
        targetPosition.y += randomYOffset;

        
        GameObject newAirShip = Instantiate(randomAirShipPrefab, spawnPosition, Quaternion.identity);
        
        
        AirShipData newData = new AirShipData
        {
            airShip = newAirShip,
            targetPosition = targetPosition
        };
        
        activeAirShips.Add(newData);
    }
}


[System.Serializable]
public class AirShipData
{
    public GameObject airShip;
    public Vector3 targetPosition;
}