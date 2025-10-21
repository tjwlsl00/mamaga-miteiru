using UnityEngine;

public class poop : MonoBehaviour
{
    private float timer = 0f;
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 15f)
        {
            Destroy(gameObject);
            timer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
