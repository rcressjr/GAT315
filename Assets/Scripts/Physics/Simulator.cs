using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] List<Force> forces;

	public List<Body> bodies { get; set; } = new List<Body>();
	Camera activeCamera;

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		forces.ForEach(force => force.ApplyForce(bodies));

        bodies.ForEach(body =>
		{
			Integrator.SemiImplicitEuler(body, Time.deltaTime);
		}); //using linq

		bodies.ForEach(body => body.acceleration = Vector2.zero);


        /*foreach (var body in bodies) // "old school"
        {
			Integrator.SemiImplicitEuler(body, Time.deltaTime);
        }*/
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}
}
