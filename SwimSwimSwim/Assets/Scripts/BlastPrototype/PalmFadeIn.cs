using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmFadeIn : MonoBehaviour {
    private Material[] materials;
    public bool visible = true;
	// Use this for initialization
	void Awake () {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        List<Material> m = new List<Material>();
        foreach(Renderer r in renderers)
        {
           foreach(Material mat in r.materials)
            {
                m.Add(mat);
            }
        }
        materials = m.ToArray();
	}
	
    public void SetVisible()
    {
        visible = true;
        StartCoroutine(FadeIn(0.5f));
    }

    public void SetInvisible()
    {
        visible = false;
        foreach (Material m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Transparency"), 1);
        }
    }

    IEnumerator FadeIn(float animTime)
    {
        float fadeAmount = 1;
        while (fadeAmount > 0)
        {
            fadeAmount -= Time.deltaTime / animTime;
            foreach (Material m in materials)
            {
                m.SetFloat(Shader.PropertyToID("_Transparency"), fadeAmount);
            }
            yield return null;
        }
    }
}
