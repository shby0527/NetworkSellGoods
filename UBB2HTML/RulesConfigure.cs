using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UBB2HTML
{
	public class tagReguex
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UBB2HTML.tagReguex"/> class.
		/// </summary>
		/// <param name="Regex">Regex.</param>
		/// <param name="Rule">Rule.</param>
		public tagReguex (string Regex, string Rule)
		{
			this.Regex = Regex;
			this.Rule = Rule;
		}

		/// <summary>
		/// Gets or sets the regex.
		/// the regex is take a search
		/// Regex string
		/// </summary>
		/// <value>The regex.</value>
		public string Regex{ get; private set; }

		/// <summary>
		/// Gets or sets the rule.
		/// replace rules,How to replace
		/// and processed the Text
		/// </summary>
		/// <value>The rule.</value>
		public string Rule{ get; private set; }
	}
	/*
	 * the xml file format is
	 * <rules>
	 *     <version ver="ver" />
	 *     <rule>
	 *         <regex>the match regex</regex>
	 *         <replace>replace rules</replace>
	 *     </rule>
	 *     <rule>.....</rule>
	 *     ......
	 * </rules>
	 */
	[DebuggerDisplay("RulesCount={Count}")]
	public sealed class RulesConfigure:IDisposable,ICollection<tagReguex>
	{
		private Stream xmlFile = null;
		private bool isDisposed;
		private List<tagReguex> lstRules = null;
		//read all rules
		public static readonly string Version = "1.0";
		private static RulesConfigure instance = null;

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="path">Path.</param>
		public static RulesConfigure CreateInstance (string path)
		{
			if (instance != null)
				return instance;
			instance = new RulesConfigure (path);
			return instance;
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="file">File.</param>
		public static RulesConfigure CreateInstance (Stream file)
		{
			if (instance != null)
				return instance;
			instance = new RulesConfigure (file);
			return instance;
		}

		/// <summary>
		/// Gets the reg rules list.
		/// </summary>
		/// <value>The reg rules.</value>
		public IList<tagReguex> RegRules {
			get {
				if (this.lstRules != null)
					return this.lstRules;
				return null;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UBB2HTM.RulesConfigure"/> class.
		/// </summary>
		/// <param name="file">File.</param>
		private RulesConfigure (string file)
		{
			this.xmlFile = File.Open (file, FileMode.Open);
			this.isDisposed = false;
			this.Load ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UBB2HTM.RulesConfigure"/> class.
		/// </summary>
		/// <param name="file">File.</param>
		private RulesConfigure (Stream file)
		{
			this.xmlFile = file;
			this.isDisposed = false;
			this.Load ();
		}

		/// <summary>
		/// Load XML file,
		/// the XML file's Format is definded
		/// </summary>
		private void Load ()
		{
			XmlDocument xml = new XmlDocument ();
			using (XmlTextReader xmlreader = new XmlTextReader (this.xmlFile)) {
				xml.Load (xmlreader);
				XmlElement root = xml.DocumentElement;
				if (root.Name != "rules") {
					throw new BadXmlConfigureFile ();
				}
				XmlNodeList vernode = root.GetElementsByTagName ("version");
				if (vernode.Count != 1) {
					throw new BadXmlConfigureFile ("version Error", 
					                               "version", 
					                               "Could not Get Versin Element");
				}
				if (vernode [0].ParentNode.Name != "rules") {
					throw new BadXmlConfigureFile ("version not find",
					                               "version",
					                               "Missing version Element");
				}
				XmlAttributeCollection nodeAttributes = vernode [0].Attributes;
				if (nodeAttributes.Count != 1) {
					throw new BadXmlConfigureFile ("Version Error",
					                               vernode [0].Name,
					                               "Attribute Error");
				}
				XmlAttribute nodeAttr = nodeAttributes [0];
				if (nodeAttr.Name != "ver") {
					throw new BadXmlConfigureFile ("Attribute Error",
					                               vernode [0].Name,
					                               "Attribute Name Wrong");
				}
				if (nodeAttr.Value != RulesConfigure.Version) {
					throw new BadXmlConfigureFile ("Version is not be used",
					                               vernode [0].Name,
					                               "Version Error");
				}
				//where it all be checked
				XmlNodeList rules = root.GetElementsByTagName ("rule");
				this.lstRules = new List<tagReguex> ();
				foreach (XmlNode rule in rules) {
					tagReguex tmp = this.RulesProcess (rule);
					if (tmp == null)
						continue;
					this.lstRules.Add (tmp);
				}
			}

		}
		//every rule node processing method
		private tagReguex RulesProcess (XmlNode ruleNode)
		{
			//if the parent node is not rules element
			//we ignored it
			if (ruleNode.ParentNode.Name != "rules")
				return null;
			string Regex = "";
			string Rule = "";
			XmlNodeList child = ruleNode.ChildNodes;
			foreach (XmlNode r in child) {
				if (r.Name == "regex") {
					Regex = r.InnerText;
				} else if (r.Name == "replace") {
					Rule = r.InnerText;
				}
			}
			//if the rule is empty,we should return null
			if (Regex == "" || Rule == "")
				return null;
			return new tagReguex (Regex, Rule);
		}
		#region ICollection implementation
		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// read only collection,it do nothing
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (tagReguex item)
		{
			return;
		}

		/// <summary>
		/// read only collection,it do nothing
		/// </summary>
		public void Clear ()
		{
			return;
		}

		/// <Docs>The object to locate in the current collection.</Docs>
		/// <para>Determines whether the current collection contains a specific value.</para>
		/// <summary>
		/// Contains the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Contains (tagReguex item)
		{
			return this.lstRules.Contains (item);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="arrayIndex">Array index.</param>
		public void CopyTo (tagReguex[] array, int arrayIndex)
		{
			this.lstRules.CopyTo (array, arrayIndex);
		}

		/// <Docs>The item to remove from the current collection.</Docs>
		/// <para>Removes the first occurrence of an item from the current collection.</para>
		/// <summary>
		/// read only collection,it do nothing
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Remove (tagReguex item)
		{
			return false;
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return this.lstRules.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly {
			get {
				return true;
			}
		}
		#endregion
		#region IEnumerable implementation
		public IEnumerator<tagReguex> GetEnumerator ()
		{
			return this.lstRules.GetEnumerator ();
		}
		#endregion
		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.lstRules.GetEnumerator ();
		}
		#endregion
		#region IDisposable implementation
		public void Dispose ()
		{
			this.Dispose (true);
			GC.SuppressFinalize (this);
		}

		private void Dispose (bool isDisposing)
		{
			if (this.isDisposed)
				return;
			if (isDisposing) {

			}
			if (xmlFile != null) {
				xmlFile.Close ();
				xmlFile.Dispose ();
			}
			this.isDisposed = true;
		}

		~RulesConfigure ()
		{
			this.Dispose (false);
		}
		#endregion
	}
}

