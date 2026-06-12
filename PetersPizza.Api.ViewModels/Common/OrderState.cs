namespace PetersPizza.Api.ViewModels.Common;

public enum OrderState
{
    Undefined,
    WaitingToAccept,
    Preparing,
    ReadyToPickUp,
    Done
}