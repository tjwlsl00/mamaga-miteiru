using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    #region 
    [SerializeField] float moveSpeed;
    // 똥 소환 
    [SerializeField] float timer = 0;
    public GameObject poopPrefab;
    // 웨이 포인트 
    private List<Transform> waypoints;
    private int currentWayPointIndex = 0;
    #endregion

    public void SetRoute(List<Transform> routeToFollow)
    {
        this.waypoints = routeToFollow;
        this.currentWayPointIndex = 0;
    }

    void Update()
    {
        MoveToRoute();

        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            Pooping();
            timer = 0;
        }
    }

    void MoveToRoute()
    {
        if (waypoints == null || waypoints.Count == 0 || currentWayPointIndex >= waypoints.Count) return;

        Transform targetWayPoint = waypoints[currentWayPointIndex];

        // 루튼 안에 있는 타겟 Transform 위치로 이동 
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWayPoint.position) < 0.1f)
        {
            currentWayPointIndex++;

            // 경로 끝까지 움직였으면 강아지 삭제 
            if (currentWayPointIndex >= waypoints.Count)
            {
                Destroy(gameObject);
            }
        }
    }

    void Pooping()
    {
        Instantiate(poopPrefab, transform.position, poopPrefab.transform.rotation);
    }
}
