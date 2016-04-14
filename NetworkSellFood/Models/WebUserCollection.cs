using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebUserCollection:CollectionBase,IList<WebUser>,ILimitData
	{
		public WebUserCollection ()
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

		public int IndexOf (WebUser item)
		{
			return base.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebUser item)
		{
			base.InnerList.Insert (index, item);
		}

		public void Add (WebUser item)
		{
			base.InnerList.Add (item);
		}

		public bool Contains (WebUser item)
		{
			return base.InnerList.Contains (item);
		}

		public void CopyTo (WebUser[] array, int arrayIndex)
		{
			base.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebUser item)
		{
			base.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebUser> IEnumerable<WebUser>.GetEnumerator ()
		{
			foreach (object i in base.InnerList) {
				yield return i as WebUser;
			}
		}

		public WebUser this [int index] {
			get {
				return base.InnerList [index] as WebUser;
			}
			set {
				base.InnerList [index] = value;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}
	}
}

