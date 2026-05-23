using System.Collections.Generic;
using UnityEngine;

public class Electrician : MonoBehaviour
{
    public static Electrician Instance;

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private GameObject trigger;

    [SerializeField]
    private float speed;
    
    private Vector3 defaultPosition;
    private List<Vector3> addresses = new List<Vector3>();
    private bool atHome;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        defaultPosition = transform.position;
        atHome = true;
    }

    private void Update()
    {
        if(addresses.Count > 0)
            FollowAddresses();
        else if (!atHome)
            GoHome();
    }

    private void FollowAddresses()
    {
        if((addresses[0] - transform.position).magnitude > 0.5f)
        {
            if (body.linearVelocity.magnitude <= 0 || (int)Time.time % 2 == 0)
            {
                body.linearVelocity = (addresses[0] - transform.position).normalized * speed;
                trigger.SetActive(false);
            }
        }
        else
        {
            trigger.SetActive(true);
            body.linearVelocity = Vector2.zero;
            addresses.RemoveAt(0);
        }
    }

    private void GoHome()
    {
        if((defaultPosition - transform.position).magnitude > 0.5f)
        {
            if (body.linearVelocity.magnitude <= 0 || (int)Time.time % 2 == 0)
            {
                body.linearVelocity = (defaultPosition - transform.position).normalized * speed;
                trigger.SetActive(false);
            }
        }
        else
        {
            body.linearVelocity = Vector2.zero;
            atHome = true;
        }
    }

    public void CallElectrician(Vector3 newAddress)
    {
        addresses.Add(newAddress);
        atHome = false;
    }
}
