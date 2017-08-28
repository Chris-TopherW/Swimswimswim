using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmCycler : MonoBehaviour {
    public GameObject palmPrefab;
    public int numTrees;
    public int treeSpacing;
    public int visibleDistance;
    private GameObject player;
    private GameObject[] palmGroups;
    private int nextTreeZ;
    private int closestTreeIndex;
	// Use this for initialization

    //StartJobs: Instantiate prefabs, set z distance correctly
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        palmGroups = new GameObject[numTrees];
        for (int i = 0; i < palmGroups.Length; i++)
        {
            palmGroups[i] = Instantiate(palmPrefab,
                                        new Vector3(0, 0, i * treeSpacing),
                                        Quaternion.identity);
            if (i * treeSpacing > visibleDistance)
            {
                palmGroups[i].GetComponent<PalmFadeIn>().SetInvisible();
            }
        }

        nextTreeZ = palmGroups.Length * treeSpacing;
        closestTreeIndex = 0;

    }
	
	// Update is called once per frame
    // Check transform of nearest prefab to player
    // If z is behind player by 5 set transparency to 1 and place at next Z
	void Update () {
		foreach (GameObject palm in palmGroups)
        {
            if (palm != null)
            {
                PalmFadeIn palmFade = palm.GetComponent<PalmFadeIn>();
                if (player.transform.position.z - palm.transform.position.z > 10)
                {
                    palmFade.SetInvisible();
                    palm.transform.position = new Vector3(0, 0, nextTreeZ);
                    nextTreeZ += treeSpacing;
                }
                else if (palm.transform.position.z - player.transform.position.z < visibleDistance && !palmFade.visible)
                {
                    palmFade.SetVisible();
                }
            }
        }
	}
}
