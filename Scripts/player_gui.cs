using Godot;

public partial class player_gui : Control
{
    Control Player1;
    Control Player2;
    Control Player3;
    Control Player4;
    int nrp;

    public Label scoreText;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Player1 = GetNode<Control>("Player1");
        Player2 = GetNode<Control>("Player2");
        Player3 = GetNode<Control>("Player3");
        Player4 = GetNode<Control>("Player4");

        scoreText = (Label)GetNode("Player1/Player1/Position1");
        Control score = GetNode<Control>("/root/GameManager/PlayerManager");
        //scoreText.Text = "WINS: " + score.Score()

        hidePlayers();
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
