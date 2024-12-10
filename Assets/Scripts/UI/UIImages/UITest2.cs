using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Data;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public class UITest2 : UIBehaviour
{

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        string json = JsonConvert.SerializeObject(
          new Dictionary<string, string> {
                { "123", "123" },{ "456", "456" }, });
        string path = Application.persistentDataPath + "/saveFile.json";
        File.WriteAllText(path, json);

        Debug.Log($"UITest2 json{json}");
        text.text = json;

        var jsonStr = @"{
                           'DisplayName': '新一代算法模型',
                           'CustomerType': 1,
                           'Report': {
                             'TotalCustomerCount': 1000,
                             'TotalTradeCount': 50
                           },
                           'CustomerIDHash': [1,2,3,4,5]
                         }";


        var dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonStr);

        var report = dict["Report"] as JObject;
        var totalCustomerCount = report["TotalCustomerCount"];

        //Console.WriteLine($"totalCustomerCount={totalCustomerCount}");

        text.text += $"totalCustomerCount={totalCustomerCount}";

        var arr = dict["CustomerIDHash"] as JArray;
        var list = arr.Select(m => m.Value<int>()).ToList();

        //Console.WriteLine($"list={string.Join(",", list)}");

        text.text += $"list={string.Join(",", list)}";


    }
}
