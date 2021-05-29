using System;
using System.Threading.Tasks;

namespace RabbitMQCore
{
    public class Class1
    {
        public void StartStep()
        {
            for (int i = 0; i < 10; i++)
            {
                GetData(i);
            }
        }

        private Task<string> GetData(int index)
        {
            var task = Task.Run(() =>
            {
                for (long i = 0; i < 1000000000000; i++)
                {
                    Console.WriteLine($"GetData {index} {i}");
                }
                return "";

            });
            return task;
        }
    }
}
