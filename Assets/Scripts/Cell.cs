using Godot;
using System;

public partial class Cell : Area2D
{


	private Vector2I arrayPosition;
	public Vector2I ArrayPosition { set { arrayPosition = value; } }
	private bool isBomb;
	public bool IsBomb { get { return isBomb; } set { isBomb = value; } }
	private bool isRevealed = false;
	private bool isFlagged = false;
	private bool wasDetonated = false;
	private bool isTargeted = false;
	private int bombsAround;
	public int BombsAround { set { bombsAround = value; } }
	public AnimatedSprite2D animatedSprite;
	GameManager gameManager;

	

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		gameManager = GetNode<GameManager>("../../GameManager");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{



	}

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
		
        if (@event is InputEventMouseButton eventMouseButton)
		{
			if (Input.IsActionJustPressed("revealCell"))
			{
				RevealCell();
				
			} 
			else if (Input.IsActionJustPressed("flagCell"))
			{
				FlagCell();
			} 

			

		}
    }

    public override void _MouseEnter()
    {
        isTargeted = true;
		UpdateSprite();
    }

    public override void _MouseExit()
    {
        isTargeted = false;
		UpdateSprite();
    }

    public void RevealCell()
	{
		if (!isFlagged) // flagged can't be revealed
		if (!isRevealed) // if not revealed yet
		{
			isRevealed = true;

			gameManager.DecreaseUnrevealedCellCount();

			

			if(isBomb) // unrevealed bomb
			{
				isRevealed = true;
				if (!gameManager.HasGameEnded) // checks if it's the first bomb
				{ 
					wasDetonated = true;
					gameManager.EndGame();
				}
			}
			else if (bombsAround != 0) // checks if it's not 0 cell
			{
				// isRevealed = true;
				
			}
			else // if it has 0 bombs around it reveals all nearby cells
			{
				RevealAdjacent();
			}
			
		}
		else if (bombsAround != 0) // if you click on a no 0 cell and the flags around number matches bombs around number, it reveals adjacent cells
		{
			
			if (FlagsAround() >= bombsAround) RevealAdjacent();

		}

		UpdateSprite();
		

	}

	

	public void FlagCell()
	{
		if (!isRevealed)
		{
			if (isFlagged == false) // if is flagged then unflag
			{
				isFlagged = true;
				gameManager.DecreaseBombCount();
			}
			else
			{
				isFlagged = false; // if is unflagged then flag
				
				gameManager.IncreaseBombCount();
			}
		}

		UpdateSprite();
		
		
	}


	public void RevealAdjacent(){

		
		
		for (int i = -1; i < 2; i++)
					{
						for (int j = -1; j < 2; j++)
						{
							if (!(arrayPosition.X + i < 0 || arrayPosition.Y + j < 0 || arrayPosition.X + i > gameManager.BoardSize.X - 1 || arrayPosition.Y + j > gameManager.BoardSize.Y - 1 ))
								if (!gameManager.CellArray[arrayPosition.X + i, arrayPosition.Y + j].isRevealed)
								{
									gameManager.CellArray[arrayPosition.X + i, arrayPosition.Y + j].RevealCell();
								}
									
						}
					}

	}

	public int FlagsAround(){

		int flagsAround = 0;
			for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						if (!(arrayPosition.X + i < 0 || arrayPosition.Y + j < 0 || arrayPosition.X + i > gameManager.BoardSize.X - 1 || arrayPosition.Y + j > gameManager.BoardSize.Y - 1 ))
							if (gameManager.CellArray[arrayPosition.X + i, arrayPosition.Y + j].isFlagged)
							{
								flagsAround++;
							}
					}
				}
		return flagsAround;

	}

	public void UpdateSprite()
	{
		if(isRevealed)
		{
			if(isBomb)
			{
				if (wasDetonated) animatedSprite.Play("detonated_bomb");
				else animatedSprite.Play("bomb");
			}
			else
			{
				animatedSprite.Play(bombsAround.ToString());
			}
		} else if (isFlagged) 
		{
			animatedSprite.Play("flagged");
		}
		else 
		{
			if (isTargeted) animatedSprite.Play("targeted_unrevealed");
			else animatedSprite.Play("unrevealed");
		}
	}


}
