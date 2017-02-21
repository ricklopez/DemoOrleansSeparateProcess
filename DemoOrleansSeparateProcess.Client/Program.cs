
using System;
using System.Threading;
using System.Threading.Tasks;
using DemoOrleansSeparateProcess.Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;

namespace DemoOrleansSeparateProcess.Client
{
    public class Program
    {
        static int Main(string[] args)
        {
            var config = ClientConfiguration.LocalhostSilo();
            try
            {
                InitializeWithRetries(config, initializeAttemptsBeforeFailing: 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Orleans client initialization failed failed due to {ex}");

                Console.ReadLine();
                return 1;
            }

            DoClientWork().Wait();
            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();
            return 0;
        }

        private static void InitializeWithRetries(ClientConfiguration config, int initializeAttemptsBeforeFailing)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }

        private static async Task DoClientWork()
        {
            // example of calling grains from the initialized client
            var friend = GrainClient.GrainFactory.GetGrain<IGrain1>(Guid.NewGuid());
            var response = await friend.SayHello("Good morning, my friend!");
            Console.WriteLine("\n\n{0}\n\n", response);
            Console.WriteLine(friend.SayHello("First").Result);
            Console.WriteLine(friend.SayHello("Second").Result);
            Console.WriteLine(friend.SayHello("Third").Result);
            Console.WriteLine(friend.SayHello("Fourth").Result);
        }
    }
}
