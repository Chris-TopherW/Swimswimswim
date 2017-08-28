using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEnemy : MonoBehaviour
{
    public int hitPoints = 1;
    private int maxHP = 1;
    public int scorePoints = 500;
    public float speed = 0f;
    private int zonesIn = 0;
    private Renderer rend;
    private float rot = 0;
    private bool destroyed = false;
    private float fadeInAmt = 1;
    private float explAmt = 0;
    private Material dynamicMaterial;
    [Range(0.0f, 2.0f)]
    public float explodeTime = 1;
    [Range(0.0f, 2.0f)]
    public float fadeoutTime = 1;
    [Range(0.1f, 1.0f)]
    public float fadeLength = 1;
    //TODO: NO HP, score multiplier in overlapping zones instead
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        dynamicMaterial = rend.material;
        dynamicMaterial.SetFloat(Shader.PropertyToID("_Transparency"), 1);
        StartCoroutine(FadeIn(1f));
        maxHP = hitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateMaterial();
        if (hitPoints <= 0 && !destroyed)
        {

            BlastManager.Instance.IncreaseScore(scorePoints);
            StartCoroutine(Explode(explodeTime));
            destroyed = true;
        }
        if (gameObject.transform.position.z <= -6.0f)
        {
            BlastManager.Instance.IncreasePollution(1);
            Destroy(gameObject);
        }
    }


    float getDistanceToZone()
    {

        float zDistance = float.PositiveInfinity;
        if (BlastManager.Instance.BlastZones != null)
        {
            List<BlastZone> zones = BlastManager.Instance.BlastZones;
            zonesIn = 0;
            foreach (BlastZone z in zones)
            {
                float d = Vector2.Distance(transform.position.xz(), z.transform.position.xz()) - z.zoneScale / 2;
                if (d <= 0)
                {
                    zonesIn++;
                }
                if (d < zDistance)
                {
                    zDistance = d;
                }
            }

        }
        return zDistance;
    }

    void UpdatePosition()
    {
        transform.Rotate(0, 360 / 10 * Time.deltaTime, 0);
    }

    //TODO: Clean dis up
    void UpdateMaterial()
    {
        float zoneDistance = Mathf.Clamp(getDistanceToZone(),-0.5f,0.5f);
        float glitchAmt;
        if (hitPoints > 0)
        {
            glitchAmt = zonesIn /(float)hitPoints;
        } else
        {
            glitchAmt = 1;
        }
        rend.material.SetFloat(Shader.PropertyToID("_GlitchAmount"), (1 - (hitPoints-zonesIn)/(float)maxHP));
    }

    public void Hit()
    {
        DoDamage(zonesIn);
    }

    //Damage to enemy
    public void DoDamage(int damage)
    {
        hitPoints -= damage;
    }

    IEnumerator ExplodeLoop(float animTime)
    {
        while (true)
        {
            StartCoroutine(Explode(animTime));
            yield return new WaitForSeconds(animTime);
        }

    }


    IEnumerator Explode(float animTime)
    {
        explAmt = 0;
        float transAmount = 0;
        while (explAmt < 2)
        {
            //   this.gameObject.transform.localScale *= 1.025f;
            explAmt += Time.deltaTime * 2 / animTime;
            transAmount = Mathf.Clamp((explAmt - fadeoutTime) / fadeLength, 0,1);
            if (explAmt > 2) explAmt = 2;
            dynamicMaterial.SetFloat(Shader.PropertyToID("_ExplodeAmount"), explAmt);
            dynamicMaterial.SetFloat(Shader.PropertyToID("_Transparency"), transAmount);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator FadeIn(float animTime)
    {
        for (int i = 0; i <= 20; i++)
        {
            fadeInAmt = 1 - i / 20.0f;
            dynamicMaterial.SetFloat(Shader.PropertyToID("_Transparency"), fadeInAmt);
            yield return new WaitForSeconds(animTime / 20.0f);
        }
    }
}
