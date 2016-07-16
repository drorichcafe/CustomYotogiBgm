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
	[PluginFilter("CM3D2x64"), PluginFilter("CM3D2x86"), PluginName("CustomYotogiBgm"), PluginVersion("0.0.0.1")]
	public class CustomYotogiBgm : PluginBase
	{
		public class File
		{
			public string Name = "";
			public bool Loop = false;
		}

		public class Item
		{
			public string SkillCategory = "";
			public string SkillName = "";
			public bool WaitCommand = false;
			public string CommandName = "";
			public string CommandType = "";
			public string ExcitementStatus = "";
			public string StageName = "";

			[XmlArrayItem(typeof(File))]
			public List<File> Files = new List<File>();
		}

		public class Config
		{
			[XmlArrayItem(typeof(Item))]
			public List<Item> Items = new List<Item>();
		}

		private bool m_stateChanged = false;
		private YotogiManager.PlayingSkillData m_skill = null;
		private Yotogi.SkillData.Command.Data m_data = null;
		private YotogiCommandFactory.CommandCallback m_command = null;
		private YotogiCommandFactory.CommandCallback m_hooked = null;
		private string m_stageName = "";
		private Config m_config = null;

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public void LateUpdate()
		{
			var yotogi_manager = getYotogiManager();
			if (yotogi_manager == null || yotogi_manager.cur_call_screen_name != "Play") return;

			updateState();
			if (!m_stateChanged) return;
			m_stateChanged = false;

			var skill = getCurrentSkill();
			var excite = getExcitementStatus().ToString();

			Console.WriteLine("SkillCategory: " + (skill != null ? skill.skill_pair.base_data.category.ToString() : "-") + ", " +
				"SkillName: " + (skill != null ? skill.skill_pair.base_data.name : "-") + ", " +
				"CommandName: " + (m_data != null ? m_data.basic.name : "-") + ", " +
				"CommandType: " + (m_data != null ? m_data.basic.command_type.ToString() : "-") + ", " +
				"ExcitementStatus: " + excite + ", " +
				"StageName: " + (m_stageName.Length > 0 ? m_stageName : "-") + ";");

			loadConfig();
			if (m_config != null)
			{
				foreach (var item in m_config.Items)
				{
#if 0
					Console.WriteLine("ITEM = SkillCategory: " + item.SkillCategory + ", " +
						"SkillName: " + item.SkillName + ", " +
						"CommandName: " + item.CommandName + ", " +
						"CommandType: " + item.CommandType + ", " +
						"ExcitementStatus: " + item.ExcitementStatus + ", " +
						"StageName: " + item.StageName + ";");
#endif

					if (item.Files.Count == 0) continue;
					if (item.SkillCategory.Length > 0 && (skill == null || !skill.skill_pair.base_data.category.ToString().Contains(item.SkillCategory))) continue;
					if (item.SkillName.Length > 0 && (skill == null || !skill.skill_pair.base_data.name.Contains(item.SkillName))) continue;
					if (item.WaitCommand && m_data == null) continue;
					if (item.CommandName.Length > 0 && (m_data == null || !m_data.basic.name.Contains(item.CommandName))) continue;
					if (item.CommandType.Length > 0 && (m_data == null || !m_data.basic.command_type.ToString().Contains(item.CommandType))) continue;
					if (item.ExcitementStatus.Length > 0 && !excite.Contains(item.ExcitementStatus)) continue;
					if (item.StageName.Length > 0 && !m_stageName.Contains(item.StageName)) continue;

					File f = item.Files[UnityEngine.Random.Range(0, item.Files.Count)];
					GameMain.Instance.SoundMgr.PlayBGM(f.Name, 0.0f, f.Loop);
					return;
				}
			}

			var stageData = Yotogi.GetStageData(YotogiStageSelectManager.StagePrefab);
			if (stageData != null)
			{
				GameMain.Instance.SoundMgr.PlayBGM(stageData.bgm_file, 0.0f, true);
			}
		}

		private void updateState()
		{
			var play_manager = getPlayerManager();
			if (play_manager != null)
			{
				var command_factory = (YotogiCommandFactory)getPrivateMember(play_manager, "command_factory_");
				if (command_factory != null)
				{
					var command = (YotogiCommandFactory.CommandCallback)getPrivateMember(command_factory, "callback_func_");
					if (command != null && command != m_hooked)
					{
						m_command = command;
						m_hooked = new YotogiCommandFactory.CommandCallback(this.hookedCommandCallback);
						command_factory.SetCommandCallback(m_hooked);
						m_stateChanged = true;
					}
				}
			}

			var skill = getCurrentSkill();
			if (skill != null && m_skill != skill)
			{
				m_skill = skill;
				m_stateChanged = true;
			}

			if (m_stageName != YotogiStageSelectManager.StageName)
			{
				m_stageName = YotogiStageSelectManager.StageName;
				m_stateChanged = true;
			}
		}

		private void hookedCommandCallback(Yotogi.SkillData.Command.Data data)
		{
			if (m_command != null)
			{
				m_data = data;
				m_command(data);
				m_stateChanged = true;
			}
		}

		private YotogiManager.PlayingSkillData getCurrentSkill()
		{
			var play_manager = getPlayerManager();
			if (play_manager == null) return null;
			return (YotogiManager.PlayingSkillData)getPrivateMember(play_manager, "playing_skill_");
		}

		private Yotogi.ExcitementStatus getExcitementStatus()
		{
			var play_manager = getPlayerManager();
			if (play_manager == null) return Yotogi.ExcitementStatus.Small;
			var maidStatus = (MaidParam.StatusAccess)getPrivateMember(play_manager, "maid_status_");
			if (maidStatus == null) return Yotogi.ExcitementStatus.Small;
			return YotogiPlay.GetExcitementStatus(maidStatus.cur_excite);
		}

		private YotogiManager getYotogiManager()
		{
			var go = GameObject.Find("UI Root/YotogiManager");
			if (go == null) return null;
			return go.GetComponent<YotogiManager>();
		}

		private YotogiPlayManager getPlayerManager()
		{
			var yotogi_manager = getYotogiManager();
			if (yotogi_manager == null) return null;
			return (YotogiPlayManager)getPrivateMember(yotogi_manager, "play_mgr_");
		}

		private object getPrivateMember(object obj, string name)
		{
			return obj.GetType().InvokeMember(name,
				 System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetField,
				 null, obj, null);
		}

		private void loadConfig()
		{
			string fname = Path.Combine(this.DataPath, "CustomYotogiBgm.xml");
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
			StreamReader sr = new StreamReader(fname, new UTF8Encoding(true));
			m_config = (Config)serializer.Deserialize(sr);
			sr.Close();
		}
	}
}