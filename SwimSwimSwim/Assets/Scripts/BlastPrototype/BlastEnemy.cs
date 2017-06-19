using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEnemy : MonoBehaviour
{
    public int hitPoints = 1;
    public int scorePoints = 500;
    public float speed = 0.025f;
    private Renderer rend;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
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

        float zDistance = float.PositiveInfinity;
        foreach (BlastZone z in zones)
        {
            float d = Vector2.Distance(transform.position.xz(), z.transform.position.xz()) - z.zoneScale/2;
            if (d < zDistance)
            {
                zDistance = d;
            }
        }
        return zDistance;
    }

    void UpdatePosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.z -= speed;
        this.transform.position = newPosition;
    }

    void UpdateMaterial()
    {
        float zoneDistance = Mathf.Clamp(getDistanceToZone(),-0.5f,0.5f);
        rend.material.SetFloat(Shader.PropertyToID("_GlitchAmount"), 0.5f-zoneDistance);
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
