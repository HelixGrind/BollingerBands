using System;
using System.Collections.Generic;
using System.Linq;
using Optimize;
using Securities;
using Xunit;

namespace UnitTests.Optimize
{
    public class BollingerBandTests
    {
        [Fact]
        public void Recalculate_FirstPeriod()
        {
            var prices = GetPrices();

            var bollingerBand = new BollingerBand(14, 2.0);
            foreach (var price in prices) bollingerBand.Recalculate(price);

            Assert.True(bollingerBand.IsCalculated);

            const double expectedUpperBand = 165.61;
            Assert.Equal(expectedUpperBand, bollingerBand.UpperBandPrice, 2);

            const double expectedLowerBand = 163.70;
            Assert.Equal(expectedLowerBand, bollingerBand.LowerBandPrice, 2);
        }

        [Fact]
        public void Recalculate_AdditionalPeriod()
        {
            var prices = GetPrices();
            prices.Add(new Price(DateTime.Now, 0, 0, 0, 164.34));

            var bollingerBand = new BollingerBand(14, 2.0);
            foreach (var price in prices) bollingerBand.Recalculate(price);

            Assert.True(bollingerBand.IsCalculated);

            const double expectedUpperBand = 165.57;
            Assert.Equal(expectedUpperBand, bollingerBand.UpperBandPrice, 2);

            const double expectedLowerBand = 163.66;
            Assert.Equal(expectedLowerBand, bollingerBand.LowerBandPrice, 2);
        }

        [Fact]
        public void Recalculate_NotEnoughPeriods()
        {
            var bollingerBand = new BollingerBand(14, 2.0);
            Assert.False(bollingerBand.IsCalculated);

            bollingerBand.Recalculate(new Price(DateTime.Now, 0, 0, 0, 200));
            Assert.False(bollingerBand.IsCalculated);
        }

        private static List<IPrice> GetPrices()
        {
            var closePrices = new[] { 164.92, 164.92, 165.2, 165.07, 165.2, 165.26, 165.07, 164.41, 163.98, 163.81, 164.09, 164.37, 164.44, 164.4 };
            return closePrices.Select(closePrice => new Price(DateTime.Now, 0, 0, 0, closePrice)).Cast<IPrice>().ToList();
        }
    }
}