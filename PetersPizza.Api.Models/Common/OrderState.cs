namespace PetersPizza.Api.Models.Common;

public enum OrderState
{
    Undefined,
    WaitingToAccept,
    Preparing,
    ReadyToPickUp,
    Done
}