﻿using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using Domain;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> Currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        public static void Main()
        {
            try
            {
                var serviceCollection = new ServiceCollection()
                    .AddHttpClient()
                    .AddSingleton<ICzechNationalBankService, CzechNationalBankService>()
                    .AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
                
                var serviceProvider = serviceCollection
                    .BuildServiceProvider();

                var rates = serviceProvider
                    .GetRequiredService<IExchangeRateProvider>()
                    .GetExchangeRates(Currencies)
                    .ToList();

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
