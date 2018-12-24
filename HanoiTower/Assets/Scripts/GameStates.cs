public interface GameStates
{
    void OnStateEnter(Manager manager);
    void ExecuteState(int index);
}

public class FirstClickState : GameStates
{
    Manager mg;

    void GameStates.OnStateEnter(Manager manager)
    {
        mg = manager;
    }

    void GameStates.ExecuteState(int index)
    {
        if (!mg.isStackEmpty(index))
        {
            mg.GrabBlock(index);
            mg.ChangeState(new SecondClickState());
        }
    }
}

public class SecondClickState : GameStates
{
    Manager mg;

    void GameStates.OnStateEnter(Manager manager)
    {
        mg = manager;
    }

    void GameStates.ExecuteState(int index)
    {
        if (mg.CanPlace(index))
        {
            mg.PlaceBlock(index);
            if (!mg.CheckWin())
            {
                mg.ChangeState(new FirstClickState());
            }
            else
            {
                mg.Win();
                mg.ChangeState(new EmptyState());
            }
        }
    }
}

public class EmptyState : GameStates
{
    Manager mg;

    void GameStates.OnStateEnter(Manager manager)
    {
        mg = manager;
    }

    void GameStates.ExecuteState(int index)
    {
        
    }
}
