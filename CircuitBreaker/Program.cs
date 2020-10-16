using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CircuitBreaker
{
    class Program
    {
        private static AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

        static async Task Main(string[] args)
        {
            _circuitBreakerPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                                          .CircuitBreakerAsync(1, TimeSpan.FromSeconds(10), OnBreak, OnReset, OnHalfOpen);

            var apiClient = new HttpClient();
            int count = 0;

            while (true)
            {
                count++;
                Console.WriteLine($"Start calling to web API");
                Console.WriteLine("\n");
                Console.WriteLine("------------------------------------------------------------------------------------");

                //start calling web API
                var apiResponse = new HttpResponseMessage();

                try
                {
                    apiResponse = await _circuitBreakerPolicy.ExecuteAsync(
                        () => apiClient.GetAsync("https://localhost:5001/api/v1/quote/test", HttpCompletionOption.ResponseContentRead));
                    var json = await apiResponse.Content.ReadAsStringAsync();
                    //End

                    Console.WriteLine($"Http status code : {apiResponse.StatusCode}");
                    Console.WriteLine("\n");
                    Console.WriteLine($"Response: {json}");
                    Console.WriteLine("\n");
                    Console.WriteLine($"{count}. End calling to web API");
                    Console.WriteLine("\n");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.WriteLine("Type any key and press ENTER to make new call to API");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }

        }

        private static void OnHalfOpen()
        {
            Console.WriteLine("Connection half open - Circuit Break State is HALF-OPEN");
        }

        private static void OnReset(Context context)
        {
            Console.WriteLine("Connection reset - Circuit Break State is CLOSED");
        }

        private static void OnBreak(DelegateResult<HttpResponseMessage> delegateResult, TimeSpan timeSpan, Context context)
        {
            Console.WriteLine("Connection is Closed - Circuit Break State is OPEN");
        }
    }
}
