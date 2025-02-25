using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ia : MonoBehaviour
{
    [SerializeField] private GameManagerIanulle gameManagerReference;
    [SerializeField] private int waitForIaPlay;

    public void IaTurn()
    {
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        yield return new WaitForSeconds(waitForIaPlay);
        gameManagerReference.AddToken(Random.Range(0,7));
    }
}
