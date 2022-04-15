using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData fps;
	[SerializeField] List<Force> forces;

	public List<Body> bodies { get; set; } = new List<Body>();
	public float fixedDeltaTime => 1.0f / fixedFPS.value;

	Camera activeCamera;

	float timeAccumulator = 0;

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		fps.value = (1.0f / Time.deltaTime).ToString("F2");

		timeAccumulator += Time.deltaTime;

		forces.ForEach(force => force.ApplyForce(bodies));

		while (timeAccumulator >= fixedDeltaTime)
		{
			bodies.ForEach(body => body.shape.color = Color.black);
			Collision.CreateContacts(bodies, out var contacts);
			contacts.ForEach(contact => { contact.bodyA.shape.color = Color.red; contact.bodyB.shape.color = Color.blue; });

			bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime));
			timeAccumulator -= fixedDeltaTime;
		}
		bodies.ForEach(body => body.acceleration = Vector2.zero);
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}

	public Body GetScreenToBody(Vector3 screen)
    {
		Body body = null;

		Ray ray = activeCamera.ScreenPointToRay(screen);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

		if (hit.collider)
        {
			hit.collider.gameObject.TryGetComponent<Body>(out body);
        }

		return body;
    }
}
