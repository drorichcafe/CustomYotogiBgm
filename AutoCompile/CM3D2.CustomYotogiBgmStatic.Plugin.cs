using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.CustomYotogiBgm
{
	[PluginFilter("CM3D2x64"), PluginFilter("CM3D2x86"), PluginName("CustomYotogiBgmStatic"), PluginVersion("0.0.0.2")]
	public class CustomYotogiBgmStatic : PluginBase
	{
		public class Item
		{
			public string Stage = "";
			public string Bgm = "";
		}

		public class Config
		{
			[XmlArrayItem(typeof(Item))]
			public List<Item> Items = new List<Item>();
		}

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public void OnLevelWasLoaded(int level)
		{
			if (level == 14)
			{
				var fname = Path.Combine(this.DataPath, "CustomYotogiBgmStatic.xml");
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
				StreamReader sr = new StreamReader(fname, new UTF8Encoding(true));
				var config = (Config)serializer.Deserialize(sr);
				sr.Close();

				foreach (var item in config.Items)
				{
					foreach (var stagePair in Yotogi.stage_data_list)
					{
						if (item.Stage == stagePair.Key.ToString())
						{
							var stage = stagePair.Value;
							var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetField;
							stage.GetType().InvokeMember("bgm_file", flags, null, stage, new object[] { item.Bgm });
							break;
						}
					}
				}
			}
		}
	}
}