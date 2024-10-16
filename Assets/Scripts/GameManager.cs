using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class GameManager : Node
{

	const int PIXELSPERCELL = 16;
	
	private Vector2I boardSize = new(5,5);
	public Vector2I BoardSize { get { return boardSize; } set { boardSize = value; } }
	private int bombAmount = 3;
	private int bombsLeft;
	private int unrevealedCellsLeft;
	public int BombAmount { get { return bombAmount; } set { bombAmount = value; } }
	// private bool isGamePaused = false;
	// public bool IsGamePaused { get { return isGamePaused; } }
	private bool hasGameEnded = false;
	public bool HasGameEnded { get { return hasGameEnded; } }
	private bool hasPlayerLost;

	private Cell[,] cellArray;
	public Cell[,] CellArray { get { return cellArray; } }

	private Label bombsLeftLabel;
	private Label unrevealedCellsLeftLabel;
	private PanelContainer gameOverPanel;


	public override void _Ready()
	{
		GenerateGame();
	}

	public void GenerateGame()
	{
		cellArray = new Cell[boardSize.X,boardSize.Y];
		bombsLeft = bombAmount;
		unrevealedCellsLeft = boardSize.X * boardSize.Y;

		bombsLeftLabel = GetNode<Label>("%BombAmount");
		unrevealedCellsLeftLabel = GetNode<Label>("%UnrevealedCellAmount");
		gameOverPanel = GetNode<PanelContainer>("%GameOverPanel");

		UpdateUI();
	}

	public static int GetPixelsPerCell()
	{
		return PIXELSPERCELL;
	}

	public void EndGame()
	{
		// isGamePaused = true;
		hasGameEnded = true;

		for (int i = 0; i < boardSize.X; i++)
		{
			
			for (int j = 0; j < boardSize.Y; j++)
			{

				cellArray[i,j].RevealCell();

			}
		}

		if (bombsLeft == 0 && unrevealedCellsLeft == 0) hasPlayerLost = false;
		else hasPlayerLost = true;

		UpdateUI();

	}

	public void AddCellToArray(Cell cell, int posX, int posY)
	{
		cellArray[posX, posY] = cell;
	}


	public void DecreaseBombCount()
	{
		bombsLeft--;
		unrevealedCellsLeft--;
		UpdateUI();
		if (bombsLeft == 0 && unrevealedCellsLeft == 0) EndGame();
	}

	public void IncreaseBombCount()
	{
		bombsLeft++;
		unrevealedCellsLeft++;
		UpdateUI();
	}

	public void DecreaseUnrevealedCellCount()
	{
		if (!hasGameEnded)
		unrevealedCellsLeft--;
		UpdateUI();
		if (bombsLeft == 0 && unrevealedCellsLeft == 0) EndGame();
	}

	private void UpdateUI()
	{
		bombsLeftLabel.Text = bombsLeft.ToString();
		unrevealedCellsLeftLabel.Text = unrevealedCellsLeft.ToString();
		if (hasGameEnded)
		{
			gameOverPanel.Visible = true;
			Label gameConclusionLabel = gameOverPanel.GetNode<MarginContainer>("MarginContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<Label>("GameConclusionLabel");
			if (hasPlayerLost) gameConclusionLabel.Text = "You lose!";
			else gameConclusionLabel.Text = "You win!";
		}
	}

}
