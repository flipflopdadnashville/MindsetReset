using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserCollIder : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            this.GetComponent<Rigidbody>().AddForce(0, 15, Random.Range(-1,-30), ForceMode.Impulse);
            audioSource.PlayOneShot(audioSource.clip, Random.Range(.1f, .5f));
        }
    }
}
