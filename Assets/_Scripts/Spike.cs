using UnityEngine;

public class Spike : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerRotate>().AttackPlayer(transform);
        }
    }
}
