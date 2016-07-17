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
	[PluginFilter("CM3D2x64"), PluginFilter("CM3D2x86"), PluginName("CustomYotogiBgmStatic"), PluginVersion("0.0.0.3")]
	public class CustomYotogiBgmStatic : PluginBase
	{
		public class StageTag
		{
			public string Name = "";
			[XmlArrayItem("File", typeof(string))]
			public List<string> Files = new List<string>();
		}

		public class SpecificTag
		{
			[XmlArrayItem("Stage", typeof(StageTag))]
			public List<StageTag> Stages = new List<StageTag>();
		}

		public class GeneralTag
		{
			[XmlArrayItem("File", typeof(string))]
			public List<string> Files = new List<string>();
		}

		public class Config
		{
			public SpecificTag Specific = new SpecificTag();
			public GeneralTag General = new GeneralTag();
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
				using (StreamReader sr = new StreamReader(fname, new UTF8Encoding(true)))
				{
					var config = (Config)serializer.Deserialize(sr);
					foreach (var stagePair in Yotogi.stage_data_list)
					{
						if (!applySpecific(config, stagePair.Key.ToString(), stagePair.Value))
						{
							// apply general setting
							setBgm(stagePair.Value, config.General.Files[UnityEngine.Random.Range(0, config.General.Files.Count)]);
						}
					}
				}
			}
		}

		private bool applySpecific(Config config, string stageName, Yotogi.StageData stageData)
		{
			foreach (var stage in config.Specific.Stages)
			{
				if (stage.Name == stageName)
				{
					setBgm(stageData, stage.Files[UnityEngine.Random.Range(0, stage.Files.Count) ]);
					return true;
				}
			}

			return false;
		}

		private void setBgm(Yotogi.StageData stage, string ogg)
		{
			var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetField;
			stage.GetType().InvokeMember("bgm_file", flags, null, stage, new object[] { ogg });
		}
	}
}