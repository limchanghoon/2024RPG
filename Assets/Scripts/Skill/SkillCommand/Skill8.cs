using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill8 : MonoActiveSkill
{
    ThirdPersonController thirdPersonController;
    [SerializeField] float meshRefreshRate;

    [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] private Material testMat;

    [SerializeField] List<GameObject> objs = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        thirdPersonController = GameManager.Instance.playerObj.GetComponent<ThirdPersonController>();
        skinnedMeshRenderers = GameManager.Instance.playerObj.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public override void Execute()
    {
        if (!IsReady()) return;
        StartCoroutine(SpeedBuffCoroutine());
        ResetCooldown();
    }

    private IEnumerator SpeedBuffCoroutine()
    {
        int upSpeed = GameManager.Instance.skillManager.GetSkillDataByID(GetID()).GetDamageValue(0);
        thirdPersonController.SprintSpeed += upSpeed;
        yield return ActiveTrail(2f);
        thirdPersonController.SprintSpeed -= upSpeed;
    }

    IEnumerator ActiveTrail(float timeActive)
    {
        while(timeActive > 0f)
        {
            timeActive -= meshRefreshRate;

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject _obj = null;
                MeshRenderer mr = null;
                MeshFilter mf = null;
                bool needToGenerate = true;
                for (int j = 0; j < objs.Count; j++)
                {
                    if (objs[j].activeSelf == false)
                    {
                        needToGenerate = false;
                        _obj = objs[j];
                        mr = _obj.GetComponent<MeshRenderer>();
                        mf = _obj.GetComponent<MeshFilter>();
                        mr.material.SetFloat("_Alpha", 1f);
                        _obj.SetActive(true);
                        break;
                    }
                }
                if (needToGenerate)
                {
                    _obj = new GameObject();
                    objs.Add(_obj);

                    mr = _obj.AddComponent<MeshRenderer>();
                    mf = _obj.AddComponent<MeshFilter>();
                    _obj.AddComponent<DontDestroy>();
                    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    mr.material = testMat;
                }
                _obj.transform.SetPositionAndRotation(GameManager.Instance.playerObj.transform.position, GameManager.Instance.playerObj.transform.rotation);

                skinnedMeshRenderers[i].BakeMesh(mf.mesh);

                mf.mesh.SetTriangles(mf.mesh.triangles, 0);

                StartCoroutine(FadeCoroutine(mr.material, _obj));
            }

            yield return MyYieldCache.WaitForSeconds(meshRefreshRate);
        }
    }

    IEnumerator FadeCoroutine(Material mat, GameObject _obj)
    {
        float valueToAnimate = mat.GetFloat("_Alpha");

        while (valueToAnimate > 0f)
        {
            valueToAnimate -= 0.1f;
            mat.SetFloat("_Alpha", valueToAnimate);
            yield return MyYieldCache.WaitForSeconds(0.05f);
        }
        _obj.SetActive(false);
    }
}
