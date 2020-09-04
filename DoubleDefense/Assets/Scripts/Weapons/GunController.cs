using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Attributes")]
    public int damage;
    public float reloadTime;
    public float timeTillShot; 

    // -- touch and reloading 
    private float curReloadTime; 
    private Touch touch; 


    void Start()
    {
        
    }

    void Update()
    {
        curReloadTime -= Time.deltaTime; 

        // -- getting touch input 
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (curReloadTime < 0f)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // -- only shoot if touching enemy 
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                Enemy enemy = touchedCollider.gameObject.GetComponent<Enemy>(); 
                if (enemy)
                {
                    StartCoroutine(ShootAfterDelay(touchedCollider.gameObject));
                    curReloadTime = reloadTime; 
                }
            }
        }
    }

    private IEnumerator ShootAfterDelay(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().ShowMark(); 
        yield return new WaitForSeconds(timeTillShot);
        ShootEnemy(enemy); 
    }

    private void ShootEnemy(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().TakeDamage(damage); 
    }
}
