using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DogSpawnManager : MonoBehaviour
{
    #region 
    // 스폰 
    public List<Transform> SpawnPoints = new List<Transform>();
    private List<Transform> availableSpawnPoints = new List<Transform>();
    public List<Transform> routeParents; //각 루트 경로 부모 오브젝트 
    private LayerMask collisionLayer;
    private float timer = 0;
    [SerializeField] private float spawnInterval;
    // 개 프리팹 
    public GameObject DogPrefab;
    #endregion

    void Awake()
    {
        initializeAvailableSpawnPoints();
    }

    void Update()
    {
        if (GameManager.Instance.currentGameDirection != GameManager.GameDirection.Playing) return;

        // spawnInterval 시간 경과 마다 개 소환

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (DogPrefab != null && SpawnPoints != null && SpawnPoints.Count > 0)
            {
                SpawnDog();
            }
            timer = 0;
        }

    }

    void initializeAvailableSpawnPoints()
    {
        availableSpawnPoints.Clear();
        foreach (Transform SpawnPoint in SpawnPoints)
        {
            availableSpawnPoints.Add(SpawnPoint);
        }
    }

    void SpawnDog()
    {
        int spawnCount = 1;

        if (spawnCount > SpawnPoints.Count)
        {
            spawnCount = SpawnPoints.Count;
        }

        List<int> currentAvailablePoints = new List<int>();

        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            bool isOccupied = Physics.CheckSphere(SpawnPoints[i].position, 0.5f, collisionLayer);
            if (!isOccupied)
            {
                currentAvailablePoints.Add(i);
            }
        }

        // 소환 가능 위치 중 하나 랜덤 선택 
        int randomIndexInList = Random.Range(0, currentAvailablePoints.Count);
        int matchingIndex = currentAvailablePoints[randomIndexInList];

        // 선택된 번호 스폰 위치 / 경로 가져오기 
        Transform selectedSpawnPoint = SpawnPoints[matchingIndex];
        Transform selectedRouteParent = routeParents[matchingIndex];

        // 강아지 소환 
        GameObject spawnedObject = Instantiate(DogPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        spawnedObject.transform.parent = transform;

        // 웨이포인트(경로) 전달 
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform point in selectedRouteParent)
        {
            waypoints.Add(point);
        }

        Dog dog = spawnedObject.GetComponent<Dog>();
        dog.SetRoute(waypoints);
    }

    private void OnDrawGizmos()
    {
        if (SpawnPoints == null) return;
        Gizmos.color = Color.red;
        foreach (Transform SpawnPoint in SpawnPoints)
        {
            if (SpawnPoint != null)
            {
                Gizmos.DrawWireSphere(SpawnPoint.position, 0.5f);
            }
        }

        if (routeParents == null) return;
        Gizmos.color = Color.blue;
        foreach (Transform routeParent in routeParents)
        {
            if (routeParent != null)
            {
                Gizmos.DrawWireSphere(routeParent.position, 0.5f);
            }
        }

    }

}
