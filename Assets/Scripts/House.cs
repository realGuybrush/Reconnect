using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class House : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;
    
    [SerializeField]
    private float hideTime;

    private float currentHideTimer;
    private float spriteTransparencyDelta;
    
    [SerializeField]
    private bool charged, hidden;
    private bool hidingStatusChangeStarted;

    [SerializeField]
    private float chanceOfBeingSpotted;
    
    public event Action OnChargeRequested = delegate { };
    public event Action OnCharged = delegate { };
    public event Action OnHidden = delegate { };
    public event Action OnDeactivated = delegate { };

    
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
                OnChargeRequested.Invoke();
            }
            else
            {
                ChangeHiddenStatus();
            }
        else
            if(other.gameObject.name.Equals("Electrician") && !hidden)
                Deactivate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
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
                OnHidden.Invoke();
            }
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
    {
        if (!charged)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 1f);
            charged = true;
            OnCharged.Invoke();
        }
    }

    private void Deactivate()
    {
        hidden = false;
        currentHideTimer = 0f;
        hidingStatusChangeStarted = false;
        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 0f);
        charged = false;
        OnDeactivated.Invoke();
    }
    public bool isActive => !hidden && charged;
    public bool isCharged => charged;
}
