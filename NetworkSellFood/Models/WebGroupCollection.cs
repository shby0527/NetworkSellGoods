using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebGroupCollection:CollectionBase,IList<WebUserGroup>
	{
		public WebGroupCollection ()
		{
		}

		public int IndexOf (WebUserGroup item)
		{
			return base.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebUserGroup item)
		{
			base.InnerList.Insert (index, item);
		}

		public void Add (WebUserGroup item)
		{
			base.InnerList.Add (item);
		}

		public bool Contains (WebUserGroup item)
		{
			return base.InnerList.Contains (item);
		}

		public void CopyTo (WebUserGroup[] array, int arrayIndex)
		{
			base.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebUserGroup item)
		{
			base.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebUserGroup> IEnumerable<WebUserGroup>.GetEnumerator ()
		{
			foreach (object i in base.InnerList) {
				WebUserGroup t = i as WebUserGroup;
				yield return t;
			}
		}

		public WebUserGroup this [int index] {
			get {
				return base.InnerList [index] as WebUserGroup;
			}
			set {
				base.InnerList [index] = value;
			}
		}

		public bool IsReadOnly
		{
			get{
				return false;
			}
		}
	}
}

