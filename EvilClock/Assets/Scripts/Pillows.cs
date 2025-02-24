using Unity.VisualScripting;
using UnityEngine;
using System;

public class Pillows : MonoBehaviour, IItem
{

    public static event Action<int> onPillowCollect;
    public int worth = 5;
    public void Collect()
    {
        onPillowCollect.Invoke(worth);
        Destroy(gameObject);
    }

    
}
