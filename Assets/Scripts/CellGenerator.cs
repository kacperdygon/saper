using Godot;
using System;
using System.Threading;

public partial class CellGenerator : Node
{

	[Export] ProgressBar progressBar;
	[Export] Label progressLabel;
	[Export] Node2D board;
	[Export] GameManager gameManager;
	[Export] PanelContainer progressPanel;
	PackedScene packedCell;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		packedCell = GD.Load<PackedScene>("res://Assets/Scenes/cell.tscn");
		Thread thread = new(GenerateBoard);
		thread.Start();

	}

	public void UpdateProgressBar(float progress)
	{
		progressBar.Value = progress;
	}

	public void UpdateProgressLabel(string text)
	{

		progressLabel.Text = text;
	}

	public void HideProgressPanel()
	{
		progressPanel.Visible = false;
	}

	public void GenerateBoard()
	{

		// GenerateCells();
		// GenerateBombs();
		// AssignBombAroundNumbers();
		// QueueFree();

		CallDeferred(MethodName.GenerateCells);
		CallDeferred(MethodName.GenerateBombs);
		CallDeferred(MethodName.AssignBombAroundNumbers);
		CallDeferred(MethodName.HideProgressPanel);
		// QueueFree();

	}

	private void GenerateCells()
	{

		UpdateProgressLabel("Generating cells...");

		Vector2I currentCellPosition = new(0, 0);

		int cellNumber = gameManager.BoardSize.X * gameManager.BoardSize.Y;

		for (int i = 0; i < cellNumber; i++)
		{
			Cell currentCell = packedCell.Instantiate() as Cell;
			currentCell.Position = new Vector2(currentCellPosition.X * GameManager.GetPixelsPerCell(), currentCellPosition.Y * GameManager.GetPixelsPerCell());
			currentCell.ArrayPosition = new Vector2I(currentCellPosition.X, currentCellPosition.Y);

			board.AddChild(currentCell);

			gameManager.AddCellToArray(currentCell, currentCellPosition.X, currentCellPosition.Y);

			if (currentCellPosition.X == gameManager.BoardSize.X - 1)
			{
				currentCellPosition.X = 0;
				currentCellPosition.Y++;
			}
			else
			{
				currentCellPosition.X++;
			}

			UpdateProgressBar(i / cellNumber * 100);



		}

	}

	public void GenerateBombs()
	{

		UpdateProgressLabel("Generating bombs...");

		Random randomNumberGenerator = new();

		int bombAmount = gameManager.BombAmount;

		for (int i = 0; i < bombAmount; i++)
		{

			int randomX;
			int randomY;
			while (true)
			{
				randomX = randomNumberGenerator.Next(0, gameManager.BoardSize.X);
				randomY = randomNumberGenerator.Next(0, gameManager.BoardSize.Y);

				Cell rolledCell = gameManager.CellArray[randomX, randomY];

				if (!rolledCell.IsBomb)
				{
					rolledCell.IsBomb = true;
					break;
				}
			}

			UpdateProgressBar(i / bombAmount * 100);

		}

	}

	public void AssignBombAroundNumbers()
	{

		UpdateProgressLabel("Assigning numbers...");

		int boardSizeX = gameManager.BoardSize.X;
		int boardSizeY = gameManager.BoardSize.Y;

		int cellsIterated = 0;
		int cellsTotalNumber = boardSizeX * boardSizeY;

		for (int i = 0; i < gameManager.BoardSize.X; i++)
		{

			for (int j = 0; j < gameManager.BoardSize.Y; j++)
			{

				Cell currentCell = gameManager.CellArray[i, j];

				int bombsAround = 0;

				for (int k = -1; k < 2; k++)
				{
					for (int l = -1; l < 2; l++)
					{
						if (!(i + k < 0 || j + l < 0 || i + k > gameManager.BoardSize.X - 1 || j + l > gameManager.BoardSize.Y - 1))
							// if (!(i - 1 < 0 || j - 1 < 0 || i + 1 > gameManager.BoardSize.X - 1 || j + 1 > gameManager.BoardSize.Y - 1) )
							if (gameManager.CellArray[i + k, j + l].IsBomb)
								bombsAround++;
					}
				}

				currentCell.BombsAround = bombsAround;

				cellsIterated++;

				UpdateProgressBar(cellsIterated / cellsTotalNumber);


			}

		}

	}


}
