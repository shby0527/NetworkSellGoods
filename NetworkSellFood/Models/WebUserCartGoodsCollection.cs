using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebUserCartGoodsCollection:CollectionBase,IList<WebUserCart>,ILimitData
	{
		public WebUserCartGoodsCollection ()
		{
		}

		public uint Page {
			get ;
			set ;
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

		public int IndexOf (WebUserCart item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebUserCart item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebUserCart item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebUserCart item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebUserCart[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebUserCart item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebUserCart> IEnumerable<WebUserCart>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebUserCart;
			}
		}

		public WebUserCart this [int index] {
			get {
				return this.InnerList [index] as WebUserCart;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

