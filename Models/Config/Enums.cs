namespace NotasProject.Models.Config
{ 
    public enum PersistedState
    {
        OK,
        KO
    }
    public enum ConfirmationState
    {
        OK,
        DataKO,
        Outdated,
        ConnectionProblem,
        AlreadyConfirmedOrNonExist
    }
}