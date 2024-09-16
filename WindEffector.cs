using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffector : LocationTrigger
{

    
    [SerializeField] private float Windstrength;
    [SerializeField] private Vector2 WindDir;
    [SerializeField] private float windStrengthXDamp;
    new private ParticleSystem particleSystem;
    private Movement movement;
    new private void Start()
    {
        base.Start();
        particleSystem = GetComponent<ParticleSystem>();
        var startSpeed = particleSystem.main;
        startSpeed.startSpeed = Windstrength / 500;
        startSpeed.startLifetime = area.size.y / (Windstrength / 500);
        var shape = particleSystem.shape;
        shape.radius = area.size.x/2;

    }

    public override void TriggerEvent(Collider2D other)
    {
        base.TriggerEvent(other);
        if(!movement && other.GetComponent<Movement>() != null)
        {
            movement = other.GetComponent<Movement>();
        }
    }

    public override void TriggeringEvent(Collider2D other)
    {
        base.TriggeringEvent(other);

        movement?.SetWindBool(true);

        if(WindDir.x == 0)
            other.GetComponent<Rigidbody2D>()?.AddForce(WindDir.normalized * Windstrength * Time.deltaTime);
        else
        {
            if(other.GetComponent<Movement>() != null && movement.GetXWindSpeed() < Windstrength)
            movement?.SetXWindSpeed(movement.GetXWindSpeed() + ((Windstrength * WindDir.normalized.x)/windStrengthXDamp) * Time.deltaTime);
            other.GetComponent<Rigidbody2D>()?.AddForce(WindDir.normalized * Windstrength * Time.deltaTime);
        }

    }

    public override void TriggeredEvent(Collider2D other)
    {
        other.GetComponent<Movement>()?.SetWindBool(false);
        base.TriggeredEvent(other);
    }
}
