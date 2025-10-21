using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GomiSpawnManager : MonoBehaviour
{
    #region 내부
    public List<Transform> SpawnPoints = new List<Transform>();
    public GameObject gomiPrefab;
    [SerializeField]
    private float spawnInterval = 10f;
    private float overlapCheckRadius = 0.5f;
    private LayerMask collisionLayer;
    private List<Transform> availableSpawnPoints = new List<Transform>();
    #endregion

    void Awake()
    {
        initializeAvailableSpawnPoints();
    }

    void Start()
    {
        if (gomiPrefab != null && SpawnPoints != null && SpawnPoints.Count > 0)
        {
            StartCoroutine(GomiRoutine());
        }
    }

    void Update()
    {
        // 게임 승리 / 오버 시 코루틴 멈춤
        if (GameManager.Instance.currentGameDirection == GameManager.GameDirection.Victroy || GameManager.Instance.currentGameDirection == GameManager.GameDirection.GameOver)
        {
            StopAllCoroutines();
        }
    }

    void initializeAvailableSpawnPoints()
    {
        availableSpawnPoints.Clear();
        foreach (Transform point in SpawnPoints)
        {
            availableSpawnPoints.Add(point);
        }
    }

    IEnumerator GomiRoutine()
    {
        while(GameManager.Instance.currentGameDirection == GameManager.GameDirection.Playing)
        {
            SpawnGomi();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGomi()
    {
        int spawnCount = 5;

        if (spawnCount > SpawnPoints.Count)
        {
            spawnCount = SpawnPoints.Count;
        }

        List<Transform> currentAvailablePoints = new List<Transform>();

        foreach (Transform spawnPoint in SpawnPoints)
        {
            bool isOccupied = Physics.CheckSphere(spawnPoint.position, overlapCheckRadius, collisionLayer);
            if (!isOccupied)
            {
                currentAvailablePoints.Add(spawnPoint);
            }
        }

        if (spawnCount > currentAvailablePoints.Count)
        {
            spawnCount = currentAvailablePoints.Count;
        }

        ShuffleList(currentAvailablePoints);

        for (int i = 0; i < spawnCount; i++)
        {
            if (i >= currentAvailablePoints.Count) break;
            Transform randomSpawnPoint = currentAvailablePoints[i];

            GameObject spawnedObject = Instantiate(gomiPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

            spawnedObject.transform.parent = transform;
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Gizmo
    void OnDrawGizmosSelected()
    {
        if (SpawnPoints == null) return;
        Gizmos.color = Color.cyan;
        foreach (Transform SpawnPoint in SpawnPoints)
        {
            if (SpawnPoint != null)
            {
                Gizmos.DrawWireSphere(SpawnPoint.position, overlapCheckRadius);
            }
        }
    }

}