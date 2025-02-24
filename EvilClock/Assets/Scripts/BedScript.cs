using UnityEngine;

public class BedScript : MonoBehaviour
{

    public LogicScript logic;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.GameWin();
        Debug.Log("OnCollisionEnter2D");
    }
}
