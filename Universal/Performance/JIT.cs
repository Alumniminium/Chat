using System.Reflection;
using System.Threading.Tasks;

namespace Universal.Performance
{
    public static class JIT
    {
        public static void PreJIT()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                {
                    System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
                }
            }

        }
        public static async Task PreJIT_Async() => await Task.Run(PreJIT);
    }
}
