using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using ParallelPerformance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParallelPerformance
{
    public class ApiParallelOperations
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [Benchmark]
        public async Task<List<int>> NormalForApi()
        {
            var list = new List<int>();
            var tasks = Enumerable.Range(0, 1000).Select(_ => new Func<Task<int>>(() => GetData(httpClient))).ToList();
            foreach (var item in tasks)
            {
                list.Add(await item());
            }
            return list;
        }

        [Benchmark]
        public List<int> ParallelApi()
        {
            var list = new List<int>();
            var tasks = Enumerable.Range(0, 1000).Select(_ => new Func<int>(() => GetData(httpClient).GetAwaiter().GetResult())).ToList();
            Parallel.For(0, tasks.Count, i =>
            {
                list.Add(tasks[i]());
            });
            return list;
        }

        [Benchmark]
        public List<int> ParallelApiWithMaxDegreeOfParallelism()
        {
            var list = new List<int>();
            var tasks = Enumerable.Range(0, 1000).Select(_ => new Func<int>(() => GetData(httpClient).GetAwaiter().GetResult())).ToList();
            Parallel.For(0, tasks.Count, new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4
            }, i =>
            {
                list.Add(tasks[i]());
            });
            return list;
        }

        public async Task<int> GetData(HttpClient http)
        {
            List<UserModel> users = new List<UserModel>();
            string url = "https://localhost:44363/api/users";
            var res = await http.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                var stringResponse = await res.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<UserModel>>(stringResponse);
            }
            return users.Count;
        }
    }
}
