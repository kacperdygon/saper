using Godot;
using System;

public partial class MenuManager : Node
{
	// Called when the node enters the scene tree for the first time.

	SpinBox bombAmountSpinBox;
	SpinBox horizontalBoardSizeSpinBox;
	SpinBox verticalBoardSizeSpinBox;
	Button startButton;

	public override void _Ready()
	{

		bombAmountSpinBox = GetNode<SpinBox>("%BombAmountSpinBox");
		horizontalBoardSizeSpinBox = GetNode<SpinBox>("%HorizontalBoardSizeSpinBox");
		verticalBoardSizeSpinBox = GetNode<SpinBox>("%VerticalBoardSizeSpinBox");
		startButton = GetNode<Button>("%StartButton");
		startButton.Pressed += () => StartGame();
		horizontalBoardSizeSpinBox.ValueChanged += (value) => ChangeMaxBombAmount();
		verticalBoardSizeSpinBox.ValueChanged += (value) => ChangeMaxBombAmount();

	}

	public override void _Process(double delta)
	{



	}

	public void StartGame()
	{

		Node2D game = GD.Load<PackedScene>("res://Assets/Scenes/game.tscn").Instantiate() as Node2D;
		GameManager gameManager = game.GetNode<GameManager>("%GameManager");
		int bombAmount = (int)bombAmountSpinBox.Value;
		Vector2I boardSize = new((int)horizontalBoardSizeSpinBox.Value, (int)verticalBoardSizeSpinBox.Value);
		gameManager.BombAmount = bombAmount;
		gameManager.BoardSize = boardSize;
		gameManager.GenerateGame();
		AddChild(game);
		GetNode<Control>("%MenuUI").Visible = false;


	}

	private void ChangeMaxBombAmount()
	{
		bombAmountSpinBox.MaxValue = horizontalBoardSizeSpinBox.Value * verticalBoardSizeSpinBox.Value;
		Console.WriteLine(bombAmountSpinBox.MaxValue);

	}

}
