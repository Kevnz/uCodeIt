
namespace uCodeIt.Strategies
{
    public static class ExecutionStrategyFactory {

        private static IExecutionStrategy _strategy = null;

        public static IExecutionStrategy Current
        {
            get
            {
                return _strategy == null ? _strategy = new AppStartupExecutionStrategy() : _strategy;
            }
            set
            {
                _strategy = value;
            }
        }
    }
}
