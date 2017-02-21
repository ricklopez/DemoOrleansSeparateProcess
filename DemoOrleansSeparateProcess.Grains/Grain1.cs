using System.Threading.Tasks;
using Orleans;
using DemoOrleansSeparateProcess.Interfaces;

namespace DemoOrleansSeparateProcess.Grains
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class Grain1 : Grain, IGrain1
    {
        public Task<string> SayHello(string greeting)
        {
            return Task.FromResult("You said: '" + greeting + "', I say: Hello!");
        }
    }
}
