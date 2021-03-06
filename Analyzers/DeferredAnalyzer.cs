﻿using Analyzers.Common;
using Securities;

namespace Analyzers
{
    public class DeferredAnalyzer : IAnalyzer
    {
        private readonly ISecurity _security;
        private readonly double _transactionFee;
        private readonly int _numDaysUntilSettlement;

        public DeferredAnalyzer(ISecurity security, double transactionFee, int numDaysUntilSettlement)
        {
            _security               = security;
            _transactionFee         = transactionFee;
            _numDaysUntilSettlement = numDaysUntilSettlement;
        }

        public void CalculatePerformanceResults(Parameters parameters, double startCapital, bool showOutput = false)
        {
            long totalTicks = _security.Prices[^1].Date.Ticks - _security.Prices[0].Date.Ticks;
            var state = new DeferredAnalyzerState(parameters, _transactionFee, startCapital, totalTicks, showOutput,
                _numDaysUntilSettlement);

            foreach (IPrice price in _security.Prices)
            {
                if (state.BollingerBand.IsCalculated)
                {
                    state.HandleOrder(price);
                    state.CreateOrder();
                }

                state.BollingerBand.Recalculate(price);
            }

            if (showOutput) state.ShowFinalBollingerBand();

            parameters.UpdateResults(state);
        }
    }
}
