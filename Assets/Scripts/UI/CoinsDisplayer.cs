public class CoinsDisplayer : Displayer
{
    protected override void ShowValue(int count)
    {
        TextField.text = "My coins:" + count;
    }

    protected override void Subscribe()
    {
        Player.CoinsCountChanged += ShowValue;
    }
}