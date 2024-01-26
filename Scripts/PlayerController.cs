using Godot;

public partial class PlayerController : CharacterBody2D
{
	[Export]
	private float gravity = 20;
	[Export]
	private float fallingGravity = 25f;
	[Export]
	private float fallingGravityMul = 1f;
	[Export]
	private float maxFallSpeed = 200;
	[Export]
	private float jumpForce = 300;
	[Export]
	private float maxSpeed = 80;
	[Export]
	private AnimationPlayer animationPlayer;

	private float jumpPressRememberTime = 0.1f, jumpPressRemember = 0;
	private float groundRememberTime = 0.1f, groundRemember = 0;
	private float originalGravity;

	public override void _Ready()
	{
		originalGravity = gravity;
	}

	public override void _Process(double delta)
	{
		Velocity += new Vector2(0, gravity * (float)delta);
		if (Velocity.Y > maxFallSpeed)
			Velocity = new Vector2(Velocity.X, maxFallSpeed);

		Jump(delta);
		HandleCollisions(delta);

		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement();
	}

	private void Movement()
	{
		if (Input.IsActionPressed("Right"))
		{
			Velocity = new Vector2(maxSpeed, Velocity.Y);
			animationPlayer.Play("walk");
		}
		else if (Input.IsActionPressed("Left"))
		{
			Velocity = new Vector2(-maxSpeed, Velocity.Y);
			animationPlayer.Play("walk");
		}
		else
		{
			Velocity = new Vector2(0, Velocity.Y);
			animationPlayer.Play("idle");
		}
	}

	private void Jump(double dt)
	{
		float delta = (float)dt;

		// making it so that when we walk off a cliff, and then press jump after like 0.1f secs, then we can jump instead of ignoring it
		groundRemember -= delta;
		if (IsOnFloor())
		{
			groundRemember = groundRememberTime;
			gravity = originalGravity;
		}

		jumpPressRemember -= delta;
		if (Input.IsActionJustPressed("Jump"))
		{
			jumpPressRemember = jumpPressRememberTime;
		}
		// jumping
		if ((jumpPressRemember > 0) && (groundRemember > 0))
		{
			jumpPressRemember = 0;
			groundRemember = 0;
			Velocity = new Vector2(Velocity.X, -jumpForce);
		}
	   
		// making it so that gravity when falling down is more than gravity when going up, just to make the movement feel better
		if (Velocity.Y < 0 && gravity < fallingGravity)
			gravity *= fallingGravityMul;
	}

	private void HandleCollisions(double delta)
	{

	}
}
