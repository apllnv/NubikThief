using System;

public static class EventBus
{
    public static Action OnChangeGravityDirection;
    public static Action OnPlayerChangeDirection;

    public static Action<string, bool> OnAnimatorSetBool;

    public static Action<System.Object> OnAddHealth;
    public static Action<System.Object> OnTakeDamage;
    public static Action<System.Object> OnDead;
}