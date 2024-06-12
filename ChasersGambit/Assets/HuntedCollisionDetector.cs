using System;
using UnityEngine;

public class HunterCollisionDetector : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a power-up
        if (collision.collider.CompareTag("PowerUp"))
        {
            // Notify the CollisionManager of the collision
            Debug.Log(this.gameObject.tag);
            CollisionManager.Instance.OnHuntedCollisionWithPowerUp(collision.collider.gameObject, this.gameObject);   
        }
        else if (collision.collider.CompareTag("Hunter"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithHunter();
        }
        else if (collision.collider.CompareTag("Exit"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithExit();
        }
    }
}