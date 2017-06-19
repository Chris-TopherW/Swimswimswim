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

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        maxHP = hitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateMaterial();
        if (hitPoints <= 0)
        {

            BlastManager.Instance.IncreaseScore(scorePoints);

            Destroy(gameObject);
        }
        if (gameObject.transform.position.z <= -6.0f)
        {
            BlastManager.Instance.IncreasePollution(1);
            Destroy(gameObject);
        }
    }

    float getDistanceToZone()
    {
        List<BlastZone> zones = BlastManager.Instance.BlastZones;
        zonesIn = 0;
        float zDistance = float.PositiveInfinity;
        foreach (BlastZone z in zones)
        {
            float d = Vector2.Distance(transform.position.xz(), z.transform.position.xz()) - z.zoneScale/2;
            if (d <= 0)
            {
                zonesIn++;
            }
            if (d < zDistance)
            {
                zDistance = d;
            }
        }
        return zDistance;
    }

    void UpdatePosition()
    {
        Quaternion newRotation = transform.rotation;
        newRotation.y += .005f;
        this.transform.rotation = newRotation;
    }

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

    IEnumerator Cleanup(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
