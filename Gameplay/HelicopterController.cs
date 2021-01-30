using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelicopterController : MonoBehaviour
{

    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float velocity = 1;
    [SerializeField]
    private UnityEvent afterLanding;
    [SerializeField]
    private Transform passenger;
    [SerializeField]
    private AudioSource audioSrc;

    private bool stillFlying = true;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = IsGrounded();
        if (grounded && stillFlying)
        {
            StartCoroutine(Fadeout());
            afterLanding.Invoke();
            stillFlying = false;
        } else if(!grounded)
        {
            transform.Translate(Vector3.down * Time.deltaTime * velocity);
            passenger.Translate(Vector3.down * Time.deltaTime * velocity);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, floorLayer);
    }

    IEnumerator Fadeout()
    {
        var initialVolume = audioSrc.volume;
        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= initialVolume / 12;
            yield return new WaitForSeconds(0.1f);
        }

        enabled = false;
    }
}
