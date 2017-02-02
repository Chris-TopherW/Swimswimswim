using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    
    public void Disable()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

}
