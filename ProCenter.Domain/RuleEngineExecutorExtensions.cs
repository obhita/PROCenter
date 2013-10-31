namespace ProCenter.Domain
{
    #region Using Statements

    using System.Runtime.CompilerServices;
    using Pillar.FluentRuleEngine;

    #endregion

    public static class RuleEngineExecutorExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Fors the calling method rule set.
        /// </summary>
        /// <param name="ruleEngineExecutor">The rule engine executor.</param>
        /// <param name="caller">The caller.</param>
        /// <returns>A rule engine executor.</returns>
        public static RuleEngineExecutor<T> ForCallingMethodRuleSet<T>(this RuleEngineExecutor<T> ruleEngineExecutor,
                                                                                       [CallerMemberName] string caller = null )
        {
            return ruleEngineExecutor.ForRuleSet ( caller + "RuleSet" );
        }

        #endregion
    }
}