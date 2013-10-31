namespace ProCenter.Common
{
    using Pillar.Common.Bootstrapper;

    public interface IOrderedBootstrapperTask : IBootstrapperTask
    {
        int Order { get; }
    }
}
