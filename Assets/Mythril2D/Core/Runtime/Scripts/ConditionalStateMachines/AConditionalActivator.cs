namespace Gyvr.Mythril2D
{
    public abstract class AConditionalActivator : AConditionalStateMachine
    {
        protected override void OnConditionMet() => Activate(true);
        protected override void OnConditionNotMet() => Activate(false);

        protected abstract void Activate(bool state);
    }
}
