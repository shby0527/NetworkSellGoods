using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{

	public class WebIndentWithReplay
	{
		public WebUserIndent Indent{ get; set; }

		public WebIndentRepCollection Replay{ get; set; }
	}

	public class WebIndentRepCollection:CollectionBase,IList<WebIndentReplay>,ILimitData
	{
		public WebIndentRepCollection ()
		{
		}

		public uint Page {
			get;
			set;
		}

		public uint PageCount {
			get;
			set;
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf (WebIndentReplay item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebIndentReplay item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebIndentReplay item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebIndentReplay item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebIndentReplay[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebIndentReplay item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebIndentReplay> IEnumerable<WebIndentReplay>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebIndentReplay;
			}
		}

		public WebIndentReplay this [int index] {
			get {
				return this.InnerList [index] as WebIndentReplay;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

