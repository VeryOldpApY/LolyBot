using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Discord_LolyBot
{
    internal class AdventOfCode
    {     
        public static async Task<string> GetStats()
        {
            HttpClient web = new()
            {
                DefaultRequestHeaders =
                {
                    { "cookie", "session=53616c7465645f5ff2d812671d22925b3af6a16f428564b0714d0a5e57c6ef69a32a883b800e1dc0ae17885de5bc6972bc3cb1377609919d296b039631a359b2" }
                }
            };
            var response = await web.GetStringAsync("https://adventofcode.com/2022/leaderboard/private/view/1095630.json");
            var data = (JObject?)JsonConvert.DeserializeObject(response);

            if(data == null)
            {
                return "Error";
            }
            else
            {
                DataTable dataTable = new()
                {
                    Columns = { { "Name", typeof(string) }, { "Points", typeof(int) } }
                };
                foreach(var line in data["members"])
                {
                    dataTable.Rows.Add((string)data["members"][line.Path.Split('.')[1]]["name"], (int)data["members"][line.Path.Split('.')[1]]["local_score"]);
                }

                DataRow[] sortedrows = dataTable.Select("", sort: "Points DESC");

                string print = "";
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if(i == 0)
                    {
                        print += "🥇 : ";
                    }
                    else if(i == 1)
                    {
                        print += "🥈 : ";
                    }
                    else if(i == 2)
                    {
                        print += "🥉 : ";
                    }
                    else
                    {
                        print += i + 1 + " : ";
                    }
                    print += sortedrows[i][0] + " : " + sortedrows[i][1] + "\n";
                }
                return print;
            }
        }
    }
}
