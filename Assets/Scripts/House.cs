using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;
    
    [SerializeField]
    private float hideTime;

    [SerializeField]
    private List<HouseConnection> connections;

    private float currentHideTimer;
    private float spriteTransparencyDelta;
    
    [SerializeField]
    private bool charged, hidden;
    private bool hidingStatusChangeStarted;

    [SerializeField]
    private float chanceOfBeingSpotted;

    
    private void FixedUpdate()
    {
        TryToGetSpotted();
        ProcessHidingTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Trigger"))
            if (!charged)
            {
                TryToStartCharging();
            }
            else
            {
                ChangeHiddenStatus();
            }
        else
            if(other.gameObject.name.Equals("Electrician") && !hidden)
                Deactivate();
    }

    private void TryToGetSpotted()
    {
        if(isActive)
            if (Random.Range(0f,100f) <= chanceOfBeingSpotted)
                Electrician.Instance.CallElectrician(transform.position);
    }

    private void ProcessHidingTimer()
    {
        if (hidingStatusChangeStarted)
        {
            if(currentHideTimer > 0)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b,
                    sprite.color.a + spriteTransparencyDelta);
                currentHideTimer -= Time.fixedDeltaTime;
            } 
            else
            {
                hidden = !hidden;
                sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b,
                    hidden?0.5f:1f);
                hidingStatusChangeStarted = false;
            }
        }
    }

    private void TryToStartCharging()
    {
        foreach (var connection in connections)
        {
            connection.TryToActivate();
        }
    }

    private void ChangeHiddenStatus()
    {
        if (hidingStatusChangeStarted) return;
        spriteTransparencyDelta = (hidden?1f:-1f) * 0.5f * Time.fixedDeltaTime / hideTime;
        currentHideTimer = hideTime;
        hidingStatusChangeStarted = true;
    }

    public void Charge()
    {//todo: make either this or corresponding method in Connection to be called via events 
        if (!charged)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 1f);
            charged = true;
        }
    }

    private void Deactivate()
    {
        foreach (var connection in connections)
            connection.Deactivate();
        hidden = false;
        currentHideTimer = 0f;
        hidingStatusChangeStarted = false;
        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 0f);
        charged = false;
    }

    public bool isCharged => charged;
    public bool isActive => !hidden && charged;
}
