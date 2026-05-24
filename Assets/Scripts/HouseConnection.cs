using UnityEngine;

public class HouseConnection : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;
    
    [SerializeField]
    private float activationTime;
    
    [SerializeField]
    private House house1, house2;

    private float currentActivationTimer;
    private float spriteTransparencyDelta;
    
    private bool activationStarted;

    private void Awake()
    {
        house1.OnChargeRequested += TryToActivate;
        house1.OnCharged += Activate;
        house1.OnHidden += StopActivation;
        house1.OnDeactivated += Deactivate;
        house2.OnChargeRequested += TryToActivate;
        house2.OnCharged += Activate;
        house2.OnHidden += StopActivation;
        house2.OnDeactivated += Deactivate;
    }

    private void OnDestroy()
    {
        house1.OnChargeRequested -= TryToActivate;
        house1.OnCharged -= Activate;
        house1.OnHidden -= StopActivation;
        house1.OnDeactivated -= Deactivate;
        house2.OnChargeRequested -= TryToActivate;
        house2.OnCharged -= Activate;
        house2.OnHidden -= StopActivation;
        house2.OnDeactivated -= Deactivate;
    }

    private void FixedUpdate()
    {
        if (activationStarted)
        {
            if(currentActivationTimer > 0)
            {
                SetTransparency(sprite.color.a + spriteTransparencyDelta);
                currentActivationTimer -= Time.fixedDeltaTime;
            } 
            else
            {
                house1.Charge();
                house2.Charge();
                activationStarted = false;
                SetTransparency(1f);
            }
        }
    }

    private void TryToActivate()
    {
        if (house1.isActive || house2.isActive)
            InitializeActivation();
    }
    
    private void InitializeActivation()
    {
        spriteTransparencyDelta = 1f * Time.fixedDeltaTime / activationTime;
        currentActivationTimer = activationTime;
        activationStarted = true;
    }
    
    private void StopActivation()
    {
        if (!activationStarted) return;
        currentActivationTimer = 0;
        activationStarted = false;
        SetTransparency(0f);
    }

    private void Activate()
    {
        if (!(house1.isCharged && house2.isCharged)) return;
        activationStarted = false;
        currentActivationTimer = 0f;
        SetTransparency(1f);
    }

    private void Deactivate()
    {
        activationStarted = false;
        currentActivationTimer = 0f;
        SetTransparency(0f);
    }

    private void SetTransparency(float a)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, a);
    }
}
