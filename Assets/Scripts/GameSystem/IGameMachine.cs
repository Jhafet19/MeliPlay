public interface IGameMachine
{
    public IGame CurrentGame { get; set; }
    void ChangeState(IGame game);
}
