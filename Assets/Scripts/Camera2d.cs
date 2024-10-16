using Godot;
using System;

public partial class Camera2d : Camera2D
{

	const int SPEEDED_UP_CAMERA_MULTIPLIER = 5;

	private float zoomMultiplier = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameManager = GetNode<GameManager>("%GameManager");
		Position = new Vector2(gameManager.BoardSize.X * GameManager.GetPixelsPerCell() / 2,  gameManager.BoardSize.Y * GameManager.GetPixelsPerCell() / 2);
		// PositionSmoothingEnabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MoveCamera();

	}

	private void MoveCamera()
	{
		var cameraMovement = Input.GetVector("moveCameraLeft","moveCameraRight","moveCameraUp","moveCameraDown");
		
		if (Input.IsActionPressed("speedUpCameraMove")) Position += cameraMovement * new Vector2(2 - Zoom.X / 5, 2 - Zoom.Y / 5) * SPEEDED_UP_CAMERA_MULTIPLIER;
		else Position += cameraMovement * new Vector2(2 - Zoom.X / 5, 2 - Zoom.Y / 5);
		

		if (Input.IsActionJustReleased("zoomCameraIn"))
		{
			if (Zoom.X + zoomMultiplier * 1 < 10)
			Zoom += new Vector2(1 * zoomMultiplier, 1 * zoomMultiplier);
		}
		else if (Input.IsActionJustReleased("zoomCameraOut"))
		{
			if (Zoom.X - zoomMultiplier * 1 > 0)
			Zoom -= new Vector2(1 * zoomMultiplier, 1 * zoomMultiplier);
		}
	}


}
