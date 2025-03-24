using System.Collections;
using UnityEngine;

public class AligatorBehaviour : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private Renderer objRender;
    IsAligatorOpenMouth mouth;
  
    private int timer = 3;
    private void Start()
    {
        if(objRender == null)
        {
            objRender = GetComponent<Renderer>();
        }
        mouth = GetComponent<IsAligatorOpenMouth>();
        StartCoroutine(timeCoroutine());
    }

    IEnumerator timeCoroutine()
    {
        int index = 0;
        bool isMouthOpen = true;
        while(true)
        {
            objRender.material = materials[index];
            index = (index + 1) % materials.Length;
            isMouthOpen = !isMouthOpen;
            mouth.enabled = isMouthOpen;
            yield return new WaitForSeconds(2f);
        }
    }
}
