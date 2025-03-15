using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWS : MonoBehaviour
{


    Vector3 StartPosition = new Vector3(1000,0,0);
    Vector3 CenterPosition = new Vector3(0, 0, 0);
    Vector3 EndPosition = new Vector3(-1000, 0, 0);



    private void Start()
    {
        StartCoroutine(Right_to_Center());
    }


    /// <summary>
    /// NEWS“ü‚è‚Å‚·
    /// </summary>
    /// <returns></returns>
    IEnumerator Right_to_Center()
    {
        float elapsedTime = 0f;
        float duration = 0.3f;
        float t;

        while (elapsedTime <duration)
        {
            t = elapsedTime / duration;


            transform.localPosition = Vector3.Lerp(StartPosition, CenterPosition, t*t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = CenterPosition;


    }



    /// <summary>
    /// NEWS‰ðŽU
    /// </summary>
    IEnumerator Center_to_Left()
    {

        float elapsedTime = 0f;
        float duration = 0.3f;
        float t;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;


            transform.localPosition = Vector3.Lerp(CenterPosition, EndPosition, t*t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = EndPosition;

        Destroy(this.gameObject);
    }



    /// <summary>
    /// News‚ð‰ðŽU‚µ‚Ü‚·
    /// </summary>
    public void Breakup()
    {
        StartCoroutine(Center_to_Left());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
