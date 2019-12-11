using System.Collections;
using System.Collections.Generic;
using TRGames.ProMiner.Gameplay;
using UnityEngine;

public class ParticleFireLong : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.GetComponent<Ground>() != null)
        {
            if (other.gameObject.GetComponent<Ground>().NotDestroyeble)
                return;
            else
                other.gameObject.GetComponent<Ground>().Destroy();
        }
        else if (other.gameObject.GetComponent<TNTController>() != null)
            other.gameObject.GetComponent<TNTController>().Destroy();
        else if (other.gameObject.GetComponent<Crystal>() != null)
            other.gameObject.GetComponent<Crystal>().Destroy();
    }
}
