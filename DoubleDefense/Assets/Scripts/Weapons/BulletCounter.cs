using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BulletCounter : MonoBehaviour
{
    public Image[] bullets; 

    public void SetBulletNumber(int shotsLeft)
    {
        for(int i = 0; i < bullets.Length; i++)
        {
            if(i > shotsLeft - 1)
            {
                bullets[i].gameObject.SetActive(false); 
            }
            else
            {
                bullets[i].gameObject.SetActive(true);
            }
        }
    }
}
