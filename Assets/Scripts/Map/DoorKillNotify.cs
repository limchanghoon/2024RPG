using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKillNotify : MonoBehaviour
{
    [SerializeField] HPController_AI[] hPController_AIs;
    [SerializeField] Transform door;
    int goal;
    int count = 0;



    private void Start()
    {
        goal = hPController_AIs.Length;
        for (int i = 0;i< hPController_AIs.Length;i++)
        {
            hPController_AIs[i].onDie += CountUp;
        }
    }

    public void CountUp() 
    {
        ++count;
        if(count >= goal)
        {
            AchieveTheGoal();
        }
    }

    private void AchieveTheGoal()
    {
        StartCoroutine(DoorOpen());
    }

    private IEnumerator DoorOpen()
    {
        float timer = 0f;
        Vector3 startPos = new Vector3(0f, -1f, -0.2f);
        Vector3 endPos = new Vector3(0f, 3f, -0.2f);

        while(timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            door.localPosition = Vector3.Lerp(startPos, endPos, timer);
        }
    }
}
