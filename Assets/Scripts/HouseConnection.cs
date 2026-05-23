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

    private void FixedUpdate()
    {
        if (activationStarted)
        {
            if(currentActivationTimer > 0)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b,
                    sprite.color.a + spriteTransparencyDelta);
                currentActivationTimer -= Time.fixedDeltaTime;
            } 
            else
            {
                house1.Charge();
                house2.Charge();
                activationStarted = false;
                sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 1f);
            }
        }
    }

    public void TryToActivate()
    {
        if (house1.isActive || house2.isActive)
            Activate();
    }
    
    private void Activate()
    {
        spriteTransparencyDelta = 1f * Time.fixedDeltaTime / activationTime;
        currentActivationTimer = activationTime;
        activationStarted = true;
    }

    public void Deactivate()
    {
        activationStarted = false;
        currentActivationTimer = 0f;
        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, 0f);
    }
}
