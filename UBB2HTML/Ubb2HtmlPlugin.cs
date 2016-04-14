using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AbTextProcess;
using PluginLoader.PluginAttribute;
using PluginLoader.Configure;

namespace UBB2HTML
{
	[DingerInfo(Address = "null",EMail = "umi@hayama.cf",Name = "umi",Phone = "null")]
	[PluginExtraInfo(Commit = "Prority is 0",Company = "null",ReleaseTime = "2015.11",UpdateTime = "2015.11")]
	[PluginInfo("F56ED1A54003B1996960D82186DC10DA9849FED0CE0844396507EE8502C962DD",0,Author = "umi",Name = "Ubb2HtmlPlugin",Version = "1.0.0")]
	public class Ubb2HtmlPlugin : AbTextProc
	{
		private RulesConfigure m_allRegex;
		private string RegexConfigFile;

		public Ubb2HtmlPlugin ()
		{
			this.m_allRegex = null;
			this.RegexConfigFile = "";
		}

		/// <summary>
		/// we should add all regex to the list
		/// from a config file
		/// </summary>
		public override bool Loading ()
		{
			ConfigureManager cfg = new ConfigureManager (this);
			if (!cfg.IsConfigKeyExists ("RegexFile")) {
				cfg ["RegexFile"] = "Regex.xml";
				cfg.SaveAllConfig ();
			}
			this.RegexConfigFile = cfg.ConfigDirectory + "/" + cfg ["RegexFile"];
			this.LoadRules ();
			return base.Loading ();
		}

		/// <summary>
		/// Loads the rules from the RegexFile.
		/// this plugin's configure directory
		/// Regex.xml
		/// </summary>
		private void LoadRules ()
		{
			using (Stream stream = File.Open(this.RegexConfigFile,FileMode.Open)) {
				this.m_allRegex = RulesConfigure.CreateInstance (stream);
			}
		}

		public override bool UnLoading ()
		{
			this.m_allRegex.Dispose ();
			return base.UnLoading ();
		}
		#region implemented abstract members of AbTextProc
		protected override string Processing ()
		{
			if (this.InputText == null)
				return null;
			string tmp = this.InputText;
			foreach (var i in this.m_allRegex.RegRules) {
				Regex reg = new Regex (i.Regex, RegexOptions.Multiline);
				tmp = reg.Replace (tmp, match =>
				{
					string[] args = new string[match.Groups.Count - 1];
					for (int j = 1; j<match.Groups.Count; j++) {
						args [j - 1] = match.Groups [j].Value;
					}
					return string.Format (i.Rule, args);
				});
			}
			return tmp;
		}
		#endregion
	}
}

