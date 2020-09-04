using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Attributes")]
    public int health;
    public int moveSpeedX;
    public int moveSpeedY;

    [Header("Other")]
    public GameObject mark; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.right * moveSpeedX * Time.deltaTime); 
    }

    public void TakeDamage(int damage)
    {
        health -= damage; 
        if(health <= 0)
        {
            Die(); 
        }
    }

    public void ShowMark()
    {
        mark.SetActive(true); 
    }

    private void Die()
    {
        Destroy(gameObject); 
    }
}
