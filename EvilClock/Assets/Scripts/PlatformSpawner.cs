using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject[] platforms;
    int numberOfPlatforms;
    public int randCycleTime;
    bool platformStatus = false;

    private void Start()
    {
        numberOfPlatforms = platforms.Length;
        StartCoroutine(startPlatforms());
        
    }

    private void Update()
    {
        
    }

    private IEnumerator startPlatforms()
    {
        

        while (true) 
        {
            randCycleTime = Random.Range(3, 6);
            int randPlatform = Random.Range(0, numberOfPlatforms);

            GameObject platform = platforms[randPlatform];
            platformStatus = !platform.activeInHierarchy;
            platform.SetActive(platformStatus);
            yield return new WaitForSeconds(randCycleTime);
        }
        
        
    }

    //private IEnumerator managePlatform(GameObject platform)
    //{
    //    while (true)
    //    {
    //        platform.SetActive(true);
    //        yield return new WaitForSeconds(randCycleTime);
    //        platform.SetActive(false);
    //        yield return new WaitForSeconds(randCycleTime);
    //    }
    //}
        


}
