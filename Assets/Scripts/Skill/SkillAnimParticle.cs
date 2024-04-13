using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimParticle : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        gameObject.SetActive(false);
    }

    private IEnumerator OnParticleSystemStopped()
    {
        transform.position = new Vector3(-99999, -99999, -99999);
        yield return MyYieldCache.WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
