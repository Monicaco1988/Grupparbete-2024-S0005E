using Godot;

public partial class player_gui : Control
{

    Control Player1;
    Control Player2;
    Control Player3;
    Control Player4;
    int nrp;

    //placeholder for the Label in player_GUI to update the Label with the updated playerscore
    public Label scoreText;
    public Label scoreText2;
    public Label scoreText3;
    public Label scoreText4;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Player1 = GetNode<Control>("Player1");
        Player2 = GetNode<Control>("Player2");
        Player3 = GetNode<Control>("Player3");
        Player4 = GetNode<Control>("Player4");

        // gets the score from playermanager to the GUI

        scoreText = (Label)GetNode("Player1/Player1/Position1");
        scoreText.Text = "WINS: " + player_manager.instance.player1Score;

        scoreText2 = (Label)GetNode("Player2/Player2/Position2");
        scoreText2.Text = "WINS: " + player_manager.instance.player2Score;

        scoreText3 = (Label)GetNode("Player3/Player3/Position3");
        scoreText3.Text = "WINS: " + player_manager.instance.player3Score;

        scoreText4 = (Label)GetNode("Player4/Player4/Position4");
        scoreText4.Text = "WINS: " + player_manager.instance.player4Score;

        hidePlayers();
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //adds score on the players GUI
        scoreText.Text = "WINS: " + player_manager.instance.player1Score;

        scoreText2.Text = "WINS: " + player_manager.instance.player2Score;

        scoreText3.Text = "WINS: " + player_manager.instance.player3Score;

        scoreText4.Text = "WINS: " + player_manager.instance.player4Score;

        nrp = player_manager.instance.GetPNum();
        showPlayers();
    }

    public void hidePlayers() 
    {
        Player1.Visible = false;
        Player2.Visible = false;
        Player3.Visible = false;
        Player4.Visible = false;
    }
    public void showPlayers()
    {
        if (nrp == 1)
        {
            Player1.Visible = true;
        }
        if (nrp == 2)
        {
            Player1.Visible = true;
            Player2.Visible = true;
        }

        if (nrp == 3)
        {
            Player1.Visible = true;
            Player2.Visible = true;
            Player3.Visible = true;
        }
        if (nrp == 4)
        {
            Player1.Visible = true;
            Player2.Visible = true;
            Player3.Visible = true;
            Player4.Visible = true;
        }
    }
}
