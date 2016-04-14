using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebUserAddressCollection:CollectionBase,IList<WebUserAddress>
	{
		public WebUserAddressCollection ()
		{
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf (WebUserAddress item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebUserAddress item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebUserAddress item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebUserAddress item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebUserAddress[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebUserAddress item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebUserAddress> IEnumerable<WebUserAddress>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebUserAddress;
			}
		}

		public WebUserAddress this [int index] {
			get {
				return this.InnerList [index] as WebUserAddress;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

