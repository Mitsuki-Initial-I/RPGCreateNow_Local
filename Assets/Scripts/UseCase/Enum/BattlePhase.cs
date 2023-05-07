namespace RPGCreateNow_Local.UseCase
{
    public enum BattlePhase
    {
        Start,
        Start_Wait,
        Select,
        Select_Wait,
        Battle_PhaseONE_Start,
        Battle_PhaseONE_Wait,
        Battle_PhaseONE_EndCheck,
        Battle_PhaseTWO_Start,
        Battle_PhaseTWO_Wait,
        Battle_PhaseTWO_EndCheck,
        End,
        Result,
        SceneChange,
    }
}